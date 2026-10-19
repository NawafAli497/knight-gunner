using UnityEngine;

public class ArmAim : MonoBehaviour
{
    [Header("Settings")]
    public float minAngle = -45f;
    public float maxAngle = 45f;
    
    // Set this to your Player's Scale (e.g. 4 or 5)
    public float characterSize = 1f;

    // References to the other parts
    private ChargeJump playerScript;
    private SpriteRenderer gunSprite;

    void Start()
    {
        // 1. Find the ChargeJump script on the Parent (The Knight)
        playerScript = GetComponentInParent<ChargeJump>();

        // 2. Find the SpriteRenderer on this object or its children
        // (This is the image of the gun/arm)
        gunSprite = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        // --- HIDING LOGIC ---
        // If we are Charging OR Not Grounded (Jumping/Falling)
        if (playerScript.isCharging || !playerScript.isGrounded)
        {
            // Hide the gun
            gunSprite.enabled = false;
            // Stop the rest of the function (don't aim invisible guns)
            return; 
        }
        else
        {
            // Show the gun
            gunSprite.enabled = true;
        }

        // --- AIMING LOGIC (Only runs if gun is visible) ---
        
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;

        // 1. FLIP LOGIC
        if (mousePos.x < transform.parent.position.x)
        {
            transform.parent.localScale = new Vector3(-characterSize, characterSize, 1);
        }
        else
        {
            transform.parent.localScale = new Vector3(characterSize, characterSize, 1);
        }

        // 2. ROTATION LOGIC
        float angle = Mathf.Atan2(direction.y, Mathf.Abs(direction.x)) * Mathf.Rad2Deg;

        // 3. CLAMP
        angle = Mathf.Clamp(angle, minAngle, maxAngle);

        // 4. APPLY
        transform.localRotation = Quaternion.Euler(0f, 0f, angle);
    }
}