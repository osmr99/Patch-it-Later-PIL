using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropper : MonoBehaviour
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

        if (player == null)
            return;

        var lineToPlayer = (player.transform.position - transform.position).normalized;

        var up = Vector3.up;

        var dot = Vector3.Dot(up, lineToPlayer);

        //Debug.Log("dot = " + dot);

        if (player != null && dot > 0.25f)
        {

            animator.SetTrigger("shake");
            StartCoroutine("BeginFallAfterDelay");
            //Debug.DrawLine(transform.position, transform.position + direction.normalized * 20, Color.magenta);
            //Debug.Break();
        }
    }

    IEnumerator BeginFallAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        GetComponent<Rigidbody2D>().gravityScale = 15;
        GetComponent<Rigidbody2D>().freezeRotation = true;
        gameObject.layer = LayerMask.NameToLayer("FallingBlock");
        /*var currentVel = GetComponent<Rigidbody2D>().velocity;
        currentVel += Vector2.down * 10;
        GetComponent<Rigidbody2D>().velocity = currentVel;*/
    }
}
