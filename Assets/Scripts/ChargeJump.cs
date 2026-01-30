using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChargeJump : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    
    // movement
    public float moveSpeed = 5f;

    // jump
    public float minJumpForce = 5f;
    public float maxJumpForce = 25f;
    public float chargeSpeed = 10f;
    public float horizontalJumpMultiplier = 15f;
    
    public Slider chargeSlider;

    // ground check
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;
    public LayerMask wallLayer;

    private Animator animator;
    private Rigidbody2D rb;
    public bool isGrounded;
    public bool isCharging;
    private float currentJumpForce;
    private float chargeDirection;

    private bool isOnIce = false;

    public Vector2 wallCheckSize = new Vector2(0.1f, 1f); 
    public float wallCheckOffset = 0.5f;

    // ICE ONLY
    public float iceFriction = 0.985f;
    public float brakeStrength = 20f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        UpdateGroundedStatus();
        HandleGroundMovement();
        Moving();
        HandleJumpChargeStart();
        HandleJumpCharging();
        HandleJumpRelease();
    }

    void FixedUpdate()
    {
        HandleIceMomentum();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ice"))
            isOnIce = true;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ice"))
            isOnIce = false;
    }

    void Moving()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        bool hasInput = Mathf.Abs(inputX) > 0.01f;
        bool isTouchingWall = false;

        if (hasInput)
        {
            float facingDirection = inputX > 0 ? 1f : -1f;
            Vector2 boxCenter = (Vector2)transform.position + new Vector2(facingDirection * wallCheckOffset, 0f);
            
            isTouchingWall = Physics2D.OverlapBox(boxCenter, wallCheckSize, 0f, wallLayer);
            if(!isTouchingWall)
                isTouchingWall = Physics2D.OverlapBox(boxCenter, wallCheckSize, 0f, groundLayer);
        }

        if (isGrounded && !isCharging && hasInput && !isTouchingWall)
            animator.SetBool("isRunning", true);
        else
            animator.SetBool("isRunning", false);
    }

    void UpdateGroundedStatus()
    {
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        ) != null;

        animator.SetBool("isGround", isGrounded);
    }

    void HandleGroundMovement()
    {
        if (!isGrounded || rb.linearVelocity.y > 0f)
            return;

        // ICE OVERRIDES EVERYTHING
        if (isOnIce)
            return;

        float x = 0f;

        if (!isCharging)
        {
            x = Input.GetAxisRaw("Horizontal");
            animator.SetBool("isRunning", true);
        }

        if (Mathf.Approximately(x, 0f))
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            animator.SetBool("isRunning", false);
        }
        else
        {
            rb.linearVelocity = new Vector2(x * moveSpeed, rb.linearVelocity.y);
            animator.SetBool("isRunning", true);
        }
    }

    void HandleJumpChargeStart()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isCharge", true);
            isCharging = true;
            currentJumpForce = minJumpForce;
        }
    }

    void HandleJumpCharging()
    {
        if (!isCharging || !Input.GetKey(KeyCode.Space))
            return;

        currentJumpForce += chargeSpeed * Time.deltaTime;
        currentJumpForce = Mathf.Clamp(currentJumpForce, minJumpForce, maxJumpForce);

        if(chargeSlider != null)
            chargeSlider.value = currentJumpForce;
    }

    void HandleJumpRelease()
    {
        if (!isCharging || !Input.GetKeyUp(KeyCode.Space))
            return;
        
        animator.SetBool("isCharge", false);
        isCharging = false;

        // keep momentum on ice
        if (!isOnIce)
            rb.linearVelocity = Vector2.zero;
        else
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

        chargeDirection = Input.GetAxisRaw("Horizontal");
        
        float horizontalForce = chargeDirection * horizontalJumpMultiplier;
        Vector2 jumpForce = new Vector2(horizontalForce, currentJumpForce);

        rb.AddForce(jumpForce, ForceMode2D.Impulse);

        if(chargeSlider != null)
            chargeSlider.value = minJumpForce;
    }

    // FIXED ICE MOVEMENT
    void HandleIceMomentum()
    {
        if (!isOnIce)
            return;

        float inputX = Input.GetAxisRaw("Horizontal");
        float currentX = rb.linearVelocity.x;

        // A) Accelerate on ice when pressing movement
        if (Mathf.Abs(inputX) > 0.1f)
        {
            currentX += inputX * moveSpeed * 0.1f;
        }
        else
        {
            // B) No input → natural slow slide
            currentX *= iceFriction;
        }

        // C) Opposite direction = brake
        if (Mathf.Sign(inputX) != 0 && Mathf.Sign(inputX) != Mathf.Sign(currentX))
        {
            currentX += inputX * brakeStrength * Time.fixedDeltaTime;
        }

        rb.linearVelocity = new Vector2(currentX, rb.linearVelocity.y);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }

        Gizmos.color = Color.blue;
        Vector2 rightBoxCenter = (Vector2)transform.position + new Vector2(wallCheckOffset, 0f);
        Gizmos.DrawWireCube(rightBoxCenter, wallCheckSize);
        
        Vector2 leftBoxCenter = (Vector2)transform.position + new Vector2(-wallCheckOffset, 0f);
        Gizmos.DrawWireCube(leftBoxCenter, wallCheckSize);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Portal"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else if (collision.CompareTag("bottle"))
        {
            TimeLeftScript.timeLeft += 10f;
            collision.gameObject.SetActive(false);
        }
    }
}
