using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class ObjectManager : MonoBehaviour
{
    [SerializeField]
    List<InteractableObject> objectsForThisLevel;


    List<InteractableObject> objectList;
    Dictionary<InteractableObject, int> remainingItems;
    Dictionary<InteractableObject, InventoryItem> inventoryUI;

    List<IObserver> observers;
    [SerializeField]
    SpriteRenderer preview;
    [SerializeField]
    Color invalidColor;
    [SerializeField]
    Color validColor;

    public int objectIndex = 0;

    [SerializeField]
    LayerMask groundMask;

    Transform nextParent;

    [SerializeField]
    Transform inventoryList;
    [SerializeField]
    GameObject inventoryItem;

    // Start is called before the first frame update
    void Awake()
    {
        observers = new List<IObserver>();
        remainingItems = new Dictionary<InteractableObject, int>();
        objectList = new List<InteractableObject>();
        inventoryUI = new Dictionary<InteractableObject, InventoryItem>();

        foreach(InteractableObject o in objectsForThisLevel)
        {
            if (remainingItems.ContainsKey(o))
            {
                remainingItems[o]++;
            }
            else
            {
                remainingItems.Add(o, 1);

                GameObject i = Instantiate(inventoryItem, inventoryList);
                i.GetComponent<Image>().sprite = o.GetComponentInChildren<SpriteRenderer>().sprite;

                inventoryUI.Add(o, i.GetComponent<InventoryItem>());
            }

            if (!objectList.Contains(o))
                objectList.Add(o);

        }

        UpdateCount();

        StaticPlayerInput.input.Player.SwitchItem.performed += Scroll;
        StaticPlayerInput.input.Player.SpawnItem.performed += AttemptSpawnItem;

        UpdatePreview();
    }

    // Update is called once per frame
    void Update()
    {
        if (!CanSpawn())
        {
            preview.color = invalidColor;
            return;
        }

        preview.color = validColor;
    }
    private void OnDisable()
    {
    }

    public Sprite GetCurrentObjectSprite()
    {
        return objectList[objectIndex].GetComponentInChildren<SpriteRenderer>().sprite;
    }

    public void AddObserver(IObserver observer)
    {
        observers.Add(observer);
    }

    void Scroll(InputAction.CallbackContext ctx)
    {
        Debug.Log("Scroll");

        float v = ctx.ReadValue<float>();
        Debug.Log(v);

        var num = (int)Mathf.Sign(v);

        //Debug.Log(num);

        objectIndex += num;
        

        if (objectIndex >= objectList.Count)
            objectIndex = 0;

        if (objectIndex < 0)
            objectIndex = objectList.Count - 1;

        UpdatePreview();
    }

    void UpdatePreview()
    {
        if (preview == null)
            return;

        preview.sprite = objectList[objectIndex].GetComponentInChildren<SpriteRenderer>()?.sprite;
    }

    void UpdateCount()
    {
        bool empty = true;
        foreach(KeyValuePair<InteractableObject, InventoryItem> kvp in inventoryUI)
        {
            var io = kvp.Key;
            int count = remainingItems[io];

            kvp.Value.SetText(count.ToString());

            if (count > 0)
                empty = false;
        }

        if(empty)
        {
            UsedAllItems();
        }
    }

    void AttemptSpawnItem(InputAction.CallbackContext ctx)
    {
        var pos = FindObjectOfType<ObjectPlacer>().transform.position;
        pos.z = 0;

        if (!CanSpawn())
            return;
        
        InteractableObject i = Instantiate(objectList[objectIndex], pos, Quaternion.identity) ;
        if (nextParent != null)
        {
            i.transform.parent = nextParent;
            Vector2 updated_pos = i.transform.position;
            updated_pos.x = nextParent.transform.position.x;
            i.transform.position = updated_pos;
        }
        remainingItems[objectList[objectIndex]]--;

        UpdateCount();
    }

    bool CanSpawn()
    {
        if (preview == null)
            return false;

        if (EventSystem.current.IsPointerOverGameObject())
            return false;

        nextParent = null;

        if(remainingItems[objectList[objectIndex]] <= 0)
        {
            return false;
        }

        string obj_name = objectList[objectIndex].name;

        if (obj_name.Contains("Ground Tile"))
        {
            if (OverlappingGround())
                return false;
            return true;
        }

        if (obj_name.Contains("Bouncer"))
        {
            if (OverlappingGround())
                return false;
            return true;
        }

        if (obj_name.Contains("Dropper"))
        {
            if (OverlappingGround())
                return false;
            return true;
        }

        if (obj_name.Contains("Saw"))
        {
            

            if (OverlappingGround())
                return false;

            float dist = 1.5f;
            RaycastHit2D hitR = Physics2D.Raycast(preview.transform.position, Vector2.right, dist, groundMask);
            RaycastHit2D hitL = Physics2D.Raycast(preview.transform.position, Vector2.left, dist, groundMask);
            RaycastHit2D hitU = Physics2D.Raycast(preview.transform.position, Vector2.up, dist, groundMask);
            RaycastHit2D hitD = Physics2D.Raycast(preview.transform.position, Vector2.down, dist, groundMask);

            int hits = 0;

            if (hitR)
                hits++;
            if (hitL)
                hits++;
            if (hitU)
                hits++;
            if (hitD)
                hits++;

            if (hits >= 2)
                return true;
            return false;
        }

        if(obj_name.Contains("Laser"))
        {
            if (preview.transform.position.y < FindObjectOfType<PlayerLogic>().transform.position.y + 1)
                return false;

            if (OverlappingGround())
                return false;

            Debug.DrawLine(preview.transform.position, (Vector2)preview.transform.position + Vector2.up);
            RaycastHit2D hit = Physics2D.Raycast((Vector2)preview.transform.position + Vector2.up * 0.5f, Vector2.up, 0.3f, groundMask);
            if (!hit)
            {
                return false;
            }

            var ground = hit.transform.gameObject;


            if (ground.GetComponentInChildren<Laser>() != null)
                return false;

            nextParent = ground.transform;

            return true;
        }

        if(obj_name.Contains("Mover H"))
        {
            var ground = OverlappingGround();
            if (ground == null)
                ground = OverlappingSaw();
            if (ground == null)
                ground = OverlappingBouncer();

            if (ground == null)
                return false;

            nextParent = ground.transform;
            return true;
        }

        if (obj_name.Contains("Mover V"))
        {
            var ground = OverlappingGround();
            if (ground == null)
                ground = OverlappingSaw();
            if (ground == null)
                ground = OverlappingBouncer();

            if (ground == null)
                return false;

            nextParent = ground.transform;
            return true;
        }

        if (obj_name.Contains("Rotator"))
        {
            var ground = OverlappingGround();
            if (ground == null)
                ground = OverlappingLaser();
            if (ground == null)
                return false;

            nextParent = ground.transform;
            return true;
        }

        return false;
    }

    GameObject OverlappingGround()
    {
        if (preview == null)
            return null;

        var coll = preview.GetComponent<BoxCollider2D>();
        List<Collider2D> colliders = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D();
        if(coll.Overlap(filter, colliders) > 0)
        {
            foreach(Collider2D c in colliders)
            {
                if (c.name.Contains("Ground") || c.name.Contains("Dropper") || c.name.Contains("Bouncer"))
                    return c.gameObject;
            }
        }
        return null;
    }

    GameObject OverlappingSaw()
    {
        
        var coll = preview.GetComponent<BoxCollider2D>();
        List<Collider2D> colliders = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D();
        if (coll.Overlap(filter, colliders) > 0)
        {
            foreach (Collider2D c in colliders)
            {
                if (c.name.Contains("Saw"))
                {
                    Debug.Log("Overlapping Saw");
                    return c.gameObject;
                }
            }
        }
        //Debug.Log("No overlap saw");
        return null;
    }
    GameObject OverlappingBouncer()
    {
        var coll = preview.GetComponent<BoxCollider2D>();
        List<Collider2D> colliders = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D();
        if (coll.Overlap(filter, colliders) > 0)
        {
            foreach (Collider2D c in colliders)
            {
                if (c.name.Contains("Bouncer"))
                {
                    Debug.Log("Overlapping Bouncer");
                    return c.gameObject;
                }
            }
        }
        //Debug.Log("No overlap saw");
        return null;
    }

    GameObject OverlappingLaser()
    {
        var coll = preview.GetComponent<BoxCollider2D>();
        List<Collider2D> colliders = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D();
        if (coll.Overlap(filter, colliders) > 0)
        {
            foreach (Collider2D c in colliders)
            {
                if (c.name.Contains("Laser"))
                {
                    return c.gameObject;
                }
            }
        }
        return null;
    }

    void UsedAllItems()
    {
        FindObjectOfType<Key>().Activate();
    }
}
