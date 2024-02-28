using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpForce = 5.0f;
    public float gravity = -20f;
    private CharacterController controller;
    private Vector3 velocity;
    private float groundSnapForce = -2f;
    public bool isGrounded;
    public float maxSlopeAngle = 45f;
    public Transform CheckGround1, CheckGround2, CheckGround3, CheckGround4, CheckGround5, CheckGround6, CheckGround7, CheckGround8;
    public bool isSliding;

    public float CoyotteTime, CoyotteTimeCounter;
    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    public void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = groundSnapForce;
        }

        Walk();
        SlopeManagement();

        SetCoyotteTime();

        if (Input.GetButtonDown("Fire1") && CoyotteTimeCounter > 0 && isSliding == false)
        {
            Jump();
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
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



        Vector3 moveDirection = (cameraForward * moveZ + cameraRight * moveX).normalized * speed ;

        float yVelocity = velocity.y;
        velocity = moveDirection;
        velocity.y = yVelocity;

        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, speed * Time.deltaTime * 200);
        }

    }

    void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        CoyotteTimeCounter = 0f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(CheckGround1.position, CheckGround1.position + Vector3.down);
        Gizmos.DrawLine(CheckGround2.position, CheckGround2.position + Vector3.down);
        Gizmos.DrawLine(CheckGround3.position, CheckGround3.position + Vector3.down);
        Gizmos.DrawLine(CheckGround4.position, CheckGround4.position + Vector3.down);
        Gizmos.DrawLine(CheckGround5.position, CheckGround5.position + Vector3.down);
        Gizmos.DrawLine(CheckGround6.position, CheckGround6.position + Vector3.down);
        Gizmos.DrawLine(CheckGround7.position, CheckGround7.position + Vector3.down);
        Gizmos.DrawLine(CheckGround8.position, CheckGround8.position + Vector3.down);

    }

    void SlopeManagement()
    {

        if (isGrounded)
        {
            RaycastHit hit;
            if (Physics.Raycast(CheckGround1.transform.position + Vector3.up * 0.1f, Vector3.down, out hit, 3f))
            {
                Vector3 groundNormal = hit.normal;
                float slopeAngle = Vector3.Angle(groundNormal, Vector3.up);
                
                isSliding = slopeAngle >= maxSlopeAngle;
                if (isSliding)
                {
                    Vector3 slopeDirection = Vector3.Cross(groundNormal, Vector3.up);
                    slopeDirection = Vector3.Cross(slopeDirection, groundNormal).normalized;
                    
                    Debug.Log(slopeDirection);
                    velocity += slopeDirection * -speed * 2;
                }
            }
        }

        
    }
    
    void SetCoyotteTime()
    {
        if (isGrounded == true)
        {
            CoyotteTimeCounter = CoyotteTime;
        }
        else
        {
            CoyotteTimeCounter -= Time.deltaTime;
        }
        
    }
}
