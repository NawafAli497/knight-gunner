using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 lastVelocity;
    public float speed = 50f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * speed;
    }

    void Update()
    {
        lastVelocity = rb.linearVelocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("ReflectWall"))
        {
            Vector2 normal = collision.contacts[0].normal;
            Vector2 newDirection = Vector2.Reflect(lastVelocity, normal).normalized;

            rb.linearVelocity = newDirection * speed;

            float angle = Mathf.Atan2(newDirection.y, newDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            GameObject enemyHit = collision.collider.gameObject;

            Destroy(enemyHit);

            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
