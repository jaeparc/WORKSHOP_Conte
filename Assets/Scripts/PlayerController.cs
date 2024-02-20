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
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        // Apply movement using Rigidbody for physics interaction
        rb.MovePosition(rb.position + move * speed * Time.deltaTime);
    }

    void Jump()
    {
        // Apply an upward force for jumping
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
