using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour
{
    [SerializeField]
    Color deactivatedColor;
    [SerializeField]
    Color activatedColor;
    [SerializeField]
    GameObject shine;

    bool activated = false;
    // Start is called before the first frame update
    void Start()
    {
        foreach(SpriteRenderer s in GetComponentsInChildren<SpriteRenderer>())
            s.color = deactivatedColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate()
    {
        foreach (SpriteRenderer s in GetComponentsInChildren<SpriteRenderer>())
            s.color = activatedColor;

        shine.SetActive(true);
        activated = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerLogic>() == null)
            return;

        if(activated)
        {
            //gameObject.SetActive(false);
            //GameManager.instance.NextLevel();
            //StartCoroutine(GameManager.instance.NextLevel());
            GameManager.instance.StartNextLevelCoroutine();
            StaticPlayerInput.input.Disable();
        }

    }
}
