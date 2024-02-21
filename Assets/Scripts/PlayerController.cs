using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f; // Movement speed
    public float jumpForce = 5.0f; // Force applied for jumps

    private Rigidbody rb;
    private bool isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Ensure Rigidbody is not null; otherwise, add it
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
    }

    void Update()
    {
        GroundCheck();
        Walk();
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            /*Jump();*/
        }
    }

    void GroundCheck()
    {
        // Cast a ray downward to check for ground
        isGrounded = Physics.Raycast(transform.position, -Vector3.up, 1.1f);
    }

    void Walk()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(moveX, 0.0f, moveZ);

        // Apply movement using Rigidbody for physics interaction
        if (moveDirection.magnitude > 0.1f) // Check if there's significant movement
        {
            // Normalize moveDirection vector to ensure consistent speed
            moveDirection.Normalize();
            Vector3 targetMovement = moveDirection * speed * Time.deltaTime;
            Vector3 newPosition = rb.position + targetMovement;
            rb.MovePosition(newPosition);

            // Rotate the player to face the direction of movement
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            rb.rotation = Quaternion.RotateTowards(rb.rotation, toRotation, speed * Time.deltaTime * 50); // Adjust the rotation speed as needed
        }
    }


    void Jump()
    {
        // Apply an upward force for jumping
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
