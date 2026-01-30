using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int speed =5;
    public int jumpForce = 200;
    bool isGrounded = true;
    Rigidbody2D rb;

    float horizontal_input;
    float jump_input;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontal_input = Input.GetAxis("Horizontal");
        jump_input = Input.GetAxis("Jump");

    }


    void FixedUpdate()
    {
        rb.linearVelocityX = speed * horizontal_input;
        if(jump_input > 0 && isGrounded)
        {
            rb.AddForceY(jumpForce);
            isGrounded = false;

        }

    }


    void OnColiisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
        
    }

}
