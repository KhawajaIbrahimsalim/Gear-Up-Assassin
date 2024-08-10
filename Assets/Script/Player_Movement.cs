using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public float speed = 10.0f;
    public float jumpHeight = 2.0f;

    private bool isJumping = false;
    private Rigidbody rb;

    void Start()
    {
        // Get the Rigidbody component
        rb = this.GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();

        Jump();
    }

    void Move()
    {
        float Forward = Input.GetAxis("Vertical") * speed;
        float Right = Input.GetAxis("Horizontal") * speed;

        // Make it move 10 meters per second instead of 10 meters per frame...
        Forward *= Time.deltaTime;
        Right *= Time.deltaTime;

        // Move translation along the object's z-axis
        transform.Translate(Right, 0, Forward);
    }

    void Jump()
    {
        // Jump when the spacebar is pressed
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            rb.AddForce(new Vector3(0, jumpHeight, 0), ForceMode.Impulse);
            isJumping = true;
        }
    }

    // Detect collision with the ground
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }
}
