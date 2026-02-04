using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer sprite;
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
        sprite.color = deactivatedColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate()
    {
        sprite.color = activatedColor;
        shine.SetActive(true);
        activated = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerLogic>() == null)
            return;

        if(activated)
        {
            FindObjectOfType<Lock>().Activate();
            gameObject.SetActive(false);
        }

    }
}
