using UnityEngine;

public class ShootableSwitch : MonoBehaviour
{
    
    public GameObject wallToDestroy; // The wall that blocks the path

    
    public bool destroyBullet = true; // Should the bullet disappear on impact?

    // This function runs when something enters the switch's trigger area
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object hitting us is tagged as "Bullet"
        if (other.CompareTag("bullet"))
        {
            ActivateSwitch(other.gameObject);
        }
    }

    void ActivateSwitch(GameObject bullet)
    {
        // 1. Make the wall disappear
        if (wallToDestroy != null)
        {
            // SetActive(false) hides it. Use Destroy(wallToDestroy) to delete it forever.
            wallToDestroy.SetActive(false); 
        }

        // 2. Destroy the bullet so it doesn't fly through
        if (destroyBullet)
        {
            Destroy(bullet);
        }

        // Optional: You could add a sound effect or animation here later
        Debug.Log("Switch Activated!"); 
        
        // Optional: Destroy the switch itself so it can't be shot again?
        // Destroy(gameObject); 
    }
}