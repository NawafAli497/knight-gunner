using UnityEngine;

public class ShowUIOnHit : MonoBehaviour
{
    public GameObject uiCanvas; // This will hold your Canvas

    void Start()
    {
        // 1. Hide the Canvas as soon as the game starts
        if (uiCanvas != null)
        {
            uiCanvas.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 2. Check if it is the Player who hit this object
        if (collision.gameObject.CompareTag("Player"))
        {
            // 3. Make the Canvas visible
            uiCanvas.SetActive(true);
            
            
        }
    }
}