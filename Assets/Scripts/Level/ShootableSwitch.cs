using UnityEngine;

// A switch the player shoots to open a blocking wall.
public class ShootableSwitch : MonoBehaviour
{
    public GameObject wallToDestroy;    // the wall that blocks the path
    public bool destroyBullet = true;   // should the bullet disappear on impact?

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("bullet"))
        {
            ActivateSwitch(other.gameObject);
        }
    }

    void ActivateSwitch(GameObject bullet)
    {
        if (wallToDestroy != null)
        {
            wallToDestroy.SetActive(false);
        }

        if (destroyBullet)
        {
            Destroy(bullet);
        }
    }
}
