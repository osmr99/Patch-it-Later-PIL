using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class PlayerLogic : MonoBehaviour
{
    [SerializeField]
    float jumpForce = 5.0f;
    [SerializeField]
    float speed = 10.0f;
    [SerializeField]
    LayerMask groundMask;
    [SerializeField]
    CircleCollider2D groundCheck;
    bool grounded = false;
    [SerializeField]
    Animator animator;
    [SerializeField]
    Animator shaker;

    Rigidbody2D rb;

    /*[SerializeField]
    InputActionAsset controls;
    InputActionMap _actionMap;

    InputAction jumpAction;
    InputAction moveAction;*/

    float hInput = 0;

    [SerializeField]
    GameObject playerPieces;

    [SerializeField]
    ParticleSystem dustTrail;

    [SerializeField]
    GameObject jumpSound;

    [SerializeField]
    Transform killLine;

    bool prev_grounded = false;

    public enum PlayerStates // not complex enough to be worth a whole state system
    {
        normal,
        dead,
        goal,
        bounce
    }

    PlayerStates state = PlayerStates.normal;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        //input = new Ia_Player();

        StaticPlayerInput.input.Player.Jump.performed += Jump_performed;
        StaticPlayerInput.input.Player.Jump.started += Jump_started;
        StaticPlayerInput.input.Player.Jump.canceled += Jump_cancelled;

        StaticPlayerInput.input.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        hInput = StaticPlayerInput.input.Player.HorizontalMovement.ReadValue<float>();

        if (state == PlayerStates.normal)
        {
            animator.SetBool("grounded", grounded);
            animator.SetFloat("speed", rb.linearVelocity.x * Mathf.Abs(hInput));
        }

        if(grounded && !prev_grounded)
        {
            animator.SetTrigger("land");
            //dustTrail.Play();
        }

        if (transform.position.y < killLine.position.y)
            Death();

        if(Mathf.Abs(hInput) > 0 && grounded)
        {
            if (!dustTrail.isPlaying)
                dustTrail.Play();
        }

        else
        {
            if(dustTrail.isPlaying)
                dustTrail.Stop();
        }
    }

    private void LateUpdate()
    {
        prev_grounded = grounded;
    }

    private void OnDisable()
    {
        StaticPlayerInput.input.Disable();
    }

    private void FixedUpdate()
    {
        if (state != PlayerStates.normal)
            return;

        Vector2 additional_velocity = Vector2.zero;

        grounded = false;
        GetComponent<Collider2D>().sharedMaterial.friction = 0;

        var overlap = Physics2D.OverlapCircle(groundCheck.transform.position, groundCheck.radius, groundMask);
        if(overlap)
        {
            grounded = true;
            GetComponent<Collider2D>().sharedMaterial.friction = 1;

            Rigidbody2D other_rb = overlap.gameObject.GetComponent<Rigidbody2D>();

            if(other_rb != null)
            {
                additional_velocity = other_rb.linearVelocity;
                additional_velocity.y = 0;
            }
        }

        rb.linearVelocity = new Vector2(hInput *  speed, rb.linearVelocity.y) + additional_velocity;

        /*if (!grounded && dustTrail.isPlaying)
            dustTrail.Stop();*/
    }

    void Jump_performed(InputAction.CallbackContext ctx)
    {
        if (rb == null)
            return;

        if (!grounded)
            return;

        GameObject sound = Instantiate(jumpSound);
        Destroy(sound, 1.5f);

        //rb.AddForce(Vector2.up * jumpForce);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        animator.SetTrigger("jump");
    }

    void Jump_cancelled(InputAction.CallbackContext ctx)
    {
        if (state == PlayerStates.bounce)
            return;

        if (rb == null)
            return;

        if (rb.linearVelocity.y <= 0)
            return;

        rb.linearVelocity = Vector2.zero;
    }

    void Jump_started(InputAction.CallbackContext ctx)
    {

    }

    public void DeathByLaser()
    {
        Death();
    }

    public void DeathBySaw()
    {
        Death();
    }

    public void DeathByCrushed()
    {
        Death();
    }

    public void Death()
    {
        if (state == PlayerStates.dead)
            return;

        StartCoroutine(HandleDeathAnimations());
    }

    public void Bounce(Vector3 force)
    {
        state = PlayerStates.bounce;
        rb.AddForce(force, ForceMode2D.Impulse);
        StartCoroutine("StopBounce");
    }

    IEnumerator StopBounce()
    {
        yield return new WaitForSeconds(0.25f);
        state = PlayerStates.normal;
    }

    IEnumerator HandleDeathAnimations()
    {
        state = PlayerStates.dead;
        shaker.SetTrigger("shake");
        var vel = rb.linearVelocity;
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0;

        yield return new WaitForSeconds(0.2f);

        GameObject p = Instantiate(Resources.Load("Pop") as GameObject);
        p.transform.position = transform.position;

        GameManager.instance.ResetLevel();
        var g = Instantiate(playerPieces, transform.position, Quaternion.identity);

        foreach (Rigidbody2D r2d in g.GetComponentsInChildren<Rigidbody2D>())
            r2d.linearVelocity = -1 * vel;

        
        gameObject.SetActive(false);
    }
}
