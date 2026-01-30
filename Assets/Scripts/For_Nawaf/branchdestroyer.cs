using UnityEngine;

public class branchdestroyer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   private void OnCollisionEnter2D(Collision2D collision)
{
    // Check if the object we hit has the tag "Player"
    // (You can change "Player" to "Ground" or whatever you need)
    if (collision.gameObject.CompareTag("Ground "))
    {
        // Destroy the object this script is attached to (the WaterDrop)
        Destroy(gameObject);
    }
}
}
