using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    InputSystem controls;

    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim;
    private SpriteRenderer spr;

    float direction = 0;

    [SerializeField]
    private LayerMask groundLayers;

    [SerializeField]
    private float moveSpeed = 300f;

    [SerializeField]
    private float jumpSpeed = 6f;

    private enum StateAnimation { idle, run, jump, falling }

    void Awake()
    {
        controls = new InputSystem();
        controls.Enable();

        controls.Land.Movement.performed += ctx =>
        {
            direction = ctx.ReadValue<float>();
        };

        controls.Land.Jump.performed += ctx => Jump();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        spr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //direction = Input.GetAxisRaw("Horizontal");
        /*if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }*/

        rb.velocity = new Vector2(direction * moveSpeed * Time.deltaTime, rb.velocity.y);

        StateAnimation state;

        if(direction > 0f)
        {
            //anim.SetBool("isRunning", true);
            state = StateAnimation.run;
            spr.flipX = false;
        }
        else if(direction < 0f)
        {
            state = StateAnimation.run;
            spr.flipX = true;
        }
        else
        {
            //anim.SetBool("isRunning", false);
            state = StateAnimation.idle;
        }

        if(rb.velocity.y > .1f)
        {
            //anim.SetBool("isJumping", true);
            state = StateAnimation.jump;
        }
        else if (rb.velocity.y < -.1f)
        {
            //anim.SetBool("isJumping", false);
            state = StateAnimation.falling;
        }

        anim.SetInteger("state", (int)state);
    }

    void Jump()
    {
        if (IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }
    }

    bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, groundLayers);
    }

    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }
}
