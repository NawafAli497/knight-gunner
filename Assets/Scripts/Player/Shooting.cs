using UnityEngine;

public class Shooting : MonoBehaviour
{
    [Header("Setup")]
    public GameObject bullet;
    
    [Header("Trajectory Limits (Bullet Path)")]
    public float minAngle = -45f;
    public float maxAngle = 45f;
    public float minVisualAngle = -45f;
    public float maxVisualAngle = 45f;
    
    public float barrelOffset = 1.0f; 
    
    private float spriteAngularOffset = 0f; 

    private float verticalPivotOffset = 0f; 

    // References
    private ChargeJump playerScript;
    private Collider2D playerCollider;
    private SpriteRenderer gunSprite;
    private Rigidbody2D playerRb;

    void Awake()
    {
        playerScript = GetComponentInParent<ChargeJump>();
        playerCollider = GetComponentInParent<Collider2D>();
        playerRb = GetComponentInParent<Rigidbody2D>();
        gunSprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (playerScript == null) return;

        // ------------------------------------
        // 1. DETERMINE FACING DIRECTION
        // ------------------------------------
        bool shouldFaceLeft = false;
        
        bool isMovingUp = playerRb.linearVelocity.y > 0.1f;
        bool isAirborne = !playerScript.isGrounded || isMovingUp;
        float currentSize = Mathf.Abs(playerScript.transform.localScale.x);

        // Priority logic for facing direction (Charge/Jump takes precedence)
        if (playerScript.isCharging)
        {
            float inputX = Input.GetAxisRaw("Horizontal");
            if (inputX < -0.01f) shouldFaceLeft = true;
            else if (inputX > 0.01f) shouldFaceLeft = false;
            else shouldFaceLeft = (playerScript.transform.localScale.x < 0);
        }
        else if (isAirborne)
        {
            if (playerRb.linearVelocity.x < -0.1f) shouldFaceLeft = true;
            else if (playerRb.linearVelocity.x > 0.1f) shouldFaceLeft = false;
            else shouldFaceLeft = (playerScript.transform.localScale.x < 0);
        }
        else
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (mouseWorld.x < playerScript.transform.position.x) shouldFaceLeft = true;
            else shouldFaceLeft = false;
        }

        // Apply Flip
        float facingMult = shouldFaceLeft ? -1f : 1f;
        playerScript.transform.localScale = new Vector3(currentSize * facingMult, currentSize, 1);


        // ------------------------------------
        // 2. VISIBILITY (Hide Gun)
        // ------------------------------------
        if (playerScript.isCharging || isAirborne)
        {
            gunSprite.enabled = false;
            return; 
        }
        else
        {
            gunSprite.enabled = true;
        }


        // ------------------------------------
        // 3. AIMING LOGIC (Clamping and Visual Sync)
        // ------------------------------------
        
        // --- Height Correction (Vertical Alignment) ---
        transform.localPosition = new Vector3(transform.localPosition.x, verticalPivotOffset, transform.localPosition.z);

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = (mousePos - transform.position);

        // 1. Calculate the RAW angle to the mouse
        float rawLookAngle = Mathf.Atan2(dir.y, Mathf.Abs(dir.x)) * Mathf.Rad2Deg;
        
        // 2. CLAMPED TRAJECTORY ANGLE (Used for bullet path)
        float clampedTrajectoryAngle = Mathf.Clamp(rawLookAngle, minAngle, maxAngle);
        
        // 3. CLAMPED VISUAL ANGLE (Used for gun's rotation limit)
        float clampedVisualAngle = Mathf.Clamp(rawLookAngle, minVisualAngle, maxVisualAngle);

        // 4. Final visual angle includes the CLAMPED visual limit PLUS the sprite offset.
        float finalVisualAngle = clampedVisualAngle + spriteAngularOffset;

        // Rotate Gun using the final visual angle
        transform.localRotation = Quaternion.Euler(0, 0, finalVisualAngle);


        // ------------------------------------
        // 4. SHOOTING LOGIC
        // ------------------------------------
        if (Input.GetMouseButtonDown(0))
        {
            // A. Convert the CLAMPED TRAJECTORY angle to a vector (Bullet path)
            float radians = clampedTrajectoryAngle * Mathf.Deg2Rad;
            Vector2 trajectoryVector = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));

            // B. FORCE the X direction based on facing (FIXES SHOOTING DIRECTION)
            if (shouldFaceLeft)
            {
                trajectoryVector.x = -Mathf.Abs(trajectoryVector.x);
            }
            else
            {
                trajectoryVector.x = Mathf.Abs(trajectoryVector.x);
            }

            // C. Calculate Spawn Position (Pivot + Trajectory Vector * Barrel Length)
            Vector3 spawnPos = transform.position + (Vector3)(trajectoryVector * barrelOffset);

            // D. Spawn Bullet
            GameObject bulletClone = Instantiate(bullet, spawnPos, Quaternion.identity);

            // E. Apply Velocity and Rotation
            Bullet bScript = bulletClone.GetComponent<Bullet>();
            Rigidbody2D rb = bulletClone.GetComponent<Rigidbody2D>();

            if (rb != null && bScript != null)
            {
                rb.linearVelocity = trajectoryVector * bScript.speed;
                
                // Rotate bullet sprite to match the vector
                float bulletAngle = Mathf.Atan2(trajectoryVector.y, trajectoryVector.x) * Mathf.Rad2Deg;
                bulletClone.transform.rotation = Quaternion.Euler(0, 0, bulletAngle);
            }

            Collider2D bulletCol = bulletClone.GetComponent<Collider2D>();
            if (bulletCol != null && playerCollider != null)
            {
                Physics2D.IgnoreCollision(bulletCol, playerCollider);
            }
        }
    }
}