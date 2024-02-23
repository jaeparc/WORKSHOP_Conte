using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpForce = 5.0f;
    public float gravity = -20f; // Increased gravity for better ground adherence
    private CharacterController controller;
    private Vector3 velocity;
    private float groundSnapForce = -2f; // Additional downward force to improve slope handling
    public bool isGrounded;
    public GameObject RayBas, RayHaut;
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            controller = gameObject.AddComponent<CharacterController>();
        }
    }

    public void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = groundSnapForce;
        }

        Walk();

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        //ClimbCheck();


    }


    void Walk()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection = (cameraForward * moveZ + cameraRight * moveX).normalized;

        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, speed * Time.deltaTime * 200);

            // Adjust movement based on slope
            if (isGrounded)
            {
                Vector3 slopeDirection = Vector3.ProjectOnPlane(moveDirection, Vector3.up);
                controller.Move(slopeDirection * speed * Time.deltaTime);
            }
            else
            {
                controller.Move(moveDirection * speed * Time.deltaTime);
            }
        }
    }

    void Jump()
    {
        // Apply a more significant jump force to counteract increased gravity
        velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
    }

    void ClimbCheck()
    {
        Vector3 RayBasStart = RayBas.transform.position;
        Vector3 RayHautStart = RayHaut.transform.position;
        Vector3 RayDirection = transform.forward;

        float MaxDistance = 0.1f;

        RaycastHit hit;

        RaycastHit hit2;
        float MaxDistance2 = 0.5f;

        if (Physics.Raycast(RayBasStart, RayDirection, out hit, MaxDistance))
        {
            if (Mathf.Abs(hit.normal.y) < 0.5f)
            {


                if (Physics.Raycast(RayHautStart, RayDirection, out hit2, MaxDistance2))
                {
                    
                }
                else
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y+0.5f,transform.position.z);
                }

            }
        }

        Debug.DrawRay(RayBasStart, RayDirection * MaxDistance, Color.red);
        Debug.DrawRay(RayHautStart, RayDirection * MaxDistance2, Color.red);
    }
}
