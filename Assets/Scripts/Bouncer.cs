using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncer : MonoBehaviour
{
    [SerializeField] Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.gameObject.GetComponent<PlayerLogic>();
        if(player != null)
        {
            
            var direction = (player.transform.position - transform.position);
            player.Bounce(direction.normalized * 20);
            animator.SetTrigger("bounce");

            //Debug.DrawLine(transform.position, transform.position + direction.normalized * 20, Color.magenta);
            //Debug.Break();
        }
    }
}
