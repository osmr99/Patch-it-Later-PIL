using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public Vector2 moveDirection = Vector2.zero;

    Rigidbody2D rb;
    float flipTimer = 0;

    protected float rotSpeed = 0;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        flipTimer += Time.deltaTime;

        if (rb != null)
            rb.linearVelocity = moveDirection;

        if (rotSpeed != 0)
            transform.Rotate(Vector3.forward, rotSpeed * Time.deltaTime);
    }

    public void SetMovementDirection(Vector2 vec)
    {
        if (moveDirection.x == 0)
            moveDirection.x = vec.x;

        if (moveDirection.y == 0)
            moveDirection.y = vec.y;
    }

    public void SwitchDirection()
    {
        if (flipTimer <= 0.5f) // make sure it doesn't keep reversing over and over
            return;

        moveDirection *= -1;
        flipTimer = 0;
    }

    public void SetRotation(float speed)
    {
        rotSpeed = speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("COLLISION");
        if (collision.gameObject.GetComponent<InteractableObject>() != null)
            SwitchDirection();
    }
}
