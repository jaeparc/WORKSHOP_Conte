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

        // Get the main camera's forward and right vectors, ignoring the y component for flat movement
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        // Calculate movement direction relative to the camera's orientation
        Vector3 moveDirection = (cameraForward * moveZ + cameraRight * moveX).normalized;

        // Apply movement if there is input
        if (moveDirection.magnitude > 0.1f)
        {
            Vector3 targetMovement = moveDirection * speed * Time.deltaTime;
            Vector3 newPosition = rb.position + targetMovement;
            rb.MovePosition(newPosition);

            // Rotate the player to face the direction of movement
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            rb.rotation = Quaternion.RotateTowards(rb.rotation, toRotation, speed * Time.deltaTime * 20); // Adjust the rotation speed as needed
        }
    }


    void Jump()
    {
        // Apply an upward force for jumping
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
