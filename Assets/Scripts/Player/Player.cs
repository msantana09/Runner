using UnityEngine;

public class Player : MonoBehaviour
{
    /*modified version of Standard Assets : UnityStandardAssets._2D. PlatformerCharacter2D
    */
    protected Animator animator;            // Reference to the player's animator component.
    protected Rigidbody2D rigidBody2D;
    [SerializeField] public float maxSpeed = 10f;                    // The fastest the player can travel in the x axis.
    [SerializeField] public float jumpForce = 400f;                  // Amount of force added when the player jumps.
    [Range(0, 1)] [SerializeField] public float crouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [SerializeField] public bool airControl = false;                 // Whether or not a player can steer while jumping;
    [SerializeField] public LayerMask groundLayer;                  // A mask determining what is ground to the character
    [SerializeField] public bool constantFowardMotion = true;

    protected Transform groundCheck;    // A position marking where to check if the player is grounded.
    protected const float groundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    //protected bool isGrounded;            // Whether or not the player is grounded.
    protected Transform ceilingCheck;   // A position marking where to check for ceilings
    protected const float ceilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
    protected bool isFacingRight = true;  // For determining which way the player is currently facing.
    protected bool doubleJumpAllowed = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidBody2D = GetComponent<Rigidbody2D>();

        //make the player movable
        gameObject.AddComponent<PlayerControl>();

        groundCheck = transform.Find("GroundCheck");
        ceilingCheck = transform.Find("CeilingCheck");
    }

    private void FixedUpdate()
    {
        animator.SetBool("isGrounded", IsGrounded());

        // Set the vertical animation
        animator.SetFloat("vSpeed", rigidBody2D.velocity.y);

    }

    private void UpdateHorizontalForce(float horizontalSpeed)
    {
        // Move the character
        if (Mathf.Abs(rigidBody2D.velocity.x) < maxSpeed)
        {
            if (rigidBody2D.velocity.x < (maxSpeed / 2))
            {
                rigidBody2D.AddForce(new Vector2(horizontalSpeed * maxSpeed * 10, 0));
            }
            else
            {
                rigidBody2D.AddForce(new Vector2(horizontalSpeed * maxSpeed * 5, 0));
            }
        }
    }

    private void UpdateVerticalForce()
    {
        if (IsGrounded())
        {
            // Add a vertical force to the player.
            if (rigidBody2D.velocity.x > 0)
            {
                //slowing down horizontal speed when jumps occur
                rigidBody2D.AddForce(new Vector2(-5f, jumpForce));
            }
            else
            {
                rigidBody2D.AddForce(new Vector2(0f, jumpForce));

            }
            doubleJumpAllowed = true;
        }
        else
        {
            if ((doubleJumpAllowed))
            {
                rigidBody2D.AddForce(new Vector2(0f, jumpForce));

                doubleJumpAllowed = false;
            }
        }

        gameObject.transform.parent = null;
    }

    private void UpdateAnimator(float horizontalSpeed)
    {
        // The Speed animator parameter is set to the absolute value of the horizontal input.
        animator.SetFloat("Speed", Mathf.Abs(horizontalSpeed));
        // If the input is moving the player right and the player is facing left...
        if (horizontalSpeed > 0 && !isFacingRight)
        {
            // ... flip the player.
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (horizontalSpeed < 0 && isFacingRight)
        {
            // ... flip the player.
            Flip();
        }
    }
    public void Move(float horizontalSpeed, bool crouchAttempted, bool jumpAttempted)
    {
        bool crouched = IsCrouching(crouchAttempted);
        // Set whether or not the character is crouching in the animator
        animator.SetBool("isCrouched", crouched);

        //only control the player if grounded or airControl is turned on
        if (IsGrounded() || airControl)
        {
            // Reduce the speed if crouching by the crouchSpeed multiplier
            horizontalSpeed = (crouched ? horizontalSpeed * crouchSpeed : horizontalSpeed);
 
            UpdateHorizontalForce(horizontalSpeed);

            UpdateAnimator(horizontalSpeed);
        }


        // Added to allow double jumps and removing parent of player
        if (jumpAttempted)
        {
            if (animator.GetBool("isGrounded"))
            {
                animator.SetBool("isGrounded", false);
            }
            UpdateVerticalForce();
        }
    }

    public bool IsGrounded()
    {
        bool grounded = false;
        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, groundLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                grounded = true;
                break;
            }
        }

        return grounded;

    }

    private bool IsCrouching(bool crouchAttempted)
    {
        bool isCrouched = crouchAttempted;
        // If crouching, check to see if the character can stand up
        if (!crouchAttempted && animator.GetBool("isCrouched"))
        {
            // If the character has a ceiling preventing them from standing up, keep them crouching
            if (Physics2D.OverlapCircle(ceilingCheck.position, ceilingRadius, groundLayer))
            {
                isCrouched = true;
            }
        }
        return isCrouched;
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        isFacingRight = !isFacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}