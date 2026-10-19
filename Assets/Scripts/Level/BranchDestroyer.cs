using UnityEngine;

// Removes a falling branch once it lands on the ground.
public class BranchDestroyer : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // The tag is defined as "Ground " (with a trailing space) in the Tag Manager.
        if (collision.gameObject.CompareTag("Ground "))
        {
            Destroy(gameObject);
        }
    }
}
