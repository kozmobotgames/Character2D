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

    public Transform groundCheck;

    public bool isGrounded;

    [SerializeField]
    private float moveSpeed = 300f;

    [SerializeField]
    private float jumpSpeed = 6f;

    public bool canDoubleJump;

    private enum StateAnimation { idle, run, jump, falling }

    [SerializeField]
    private float fallingThreshold;

    //the respawn position of the player
    public Vector3 respawnPosition;

    void Awake()
    {
        controls = new InputSystem();
        controls.Enable();

        controls.Land.Movement.performed += ctx =>
        {
            direction = ctx.ReadValue<float>();
        };

        controls.Land.Jump.performed += ctx => OnJump();
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

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 3f, groundLayers);

        if (isGrounded && rb.velocity.y <= 0.01f)
        {
            canDoubleJump = true;
        }

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

        if (transform.position.y < fallingThreshold)
        {
            transform.position = new Vector3(respawnPosition.x, respawnPosition.y, respawnPosition.z);
            GameManager.lives -= 1;
            Debug.Log(GameManager.lives + " lives left!");
        }

    }

    void OnJump()
    {
        if (isGrounded)
        {
            Jump();
        }
        else if (canDoubleJump)
        {
            Jump();
            canDoubleJump = false;
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
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
