using JetBrains.Annotations;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float speedBase;
    public float jumpForce = 5.0f;
    public float gravity = -20f;
    private CharacterController controller;
    private Vector3 velocity;
    private float groundSnapForce = -2f;
    public bool isGrounded;
    public float maxSlopeAngle = 45f;
    public bool isSliding;

    public float CoyotteTime, CoyotteTimeCounter;

    public float JumpBufferTime, JumpBufferCounter;

    public Animator anim;

    public Transform[] CheckGround;

    float speedModifier;

    public AnimationCurve SpeedCurve;

    public bool Crouch;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (var item in CheckGround)
        {
            Gizmos.DrawLine(item.position, item.position + Vector3.down * 0.5f);
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + GroundNormal());
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        speedBase = speed;
        Crouch = false;
    }

    public void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = groundSnapForce;
        }

        if (Input.GetButtonDown("Fire2"))
        {
            Crouch = !Crouch;
        }

        speedModifier = Mathf.Abs(Vector3.Dot(GroundNormal().normalized, Vector3.down));

        AnimationCOntroller();
        Acceleration();

        if (Crouch)
        {
            CrouchWalk();
            transform.localScale = new Vector3(transform.localScale.x, 0.5f, transform.localScale.x);
        }
        else
        {
            Walk();
            transform.localScale = new Vector3(transform.localScale.x, 1f, transform.localScale.x);

        }
        SlopeManagement();

        SetCoyotteTime();
        SetJumpBuffer();

        if (JumpBufferCounter > 0 && CoyotteTimeCounter > 0 && isSliding == false)
        {
            Jump();

        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void CrouchWalk()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();



        float walkSpeed = 1;
        if (isGrounded)
        {
            walkSpeed = SpeedCurve.Evaluate(speedModifier);
        }

        if (!isGrounded)
        {

        }
        Vector3 moveDirection = (cameraForward * moveZ + cameraRight * moveX).normalized * (speed / 2) * walkSpeed;
        Debug.Log(speedModifier);

        float yVelocity = velocity.y;
        velocity = moveDirection;
        velocity.y = yVelocity;

        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, (speed / 2) * Time.deltaTime * 200);
        }
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

        Vector3 moveDirection = Vector3.zero;

        float walkSpeed = 1;
        if (isGrounded)
        {
            walkSpeed = SpeedCurve.Evaluate(speedModifier);

            moveDirection = (cameraForward * moveZ + cameraRight * moveX).normalized * speed * walkSpeed;

            float yVelocity = velocity.y;
            velocity = moveDirection;
            velocity.y = yVelocity;
        }
        else // Adjusted air control
        {
            Vector3 inputDirection = (cameraForward * moveZ + cameraRight * moveX).normalized;
            float airControlFactor = 0.05f; // Further reduced for less air control

            // Apply a damping effect based on the difference between current and desired direction
            Vector3 desiredDirection = inputDirection * speed;
            Vector3 velocityChange = (desiredDirection - new Vector3(velocity.x, 0, velocity.z)) * airControlFactor;

            // Evaluate the current velocity to apply changes
            if (velocity.x * inputDirection.x < 0 || velocity.z * inputDirection.z < 0)
            {
                // If the player is trying to change direction, allow a bit more control
                velocityChange *= 2;
            }

            velocity.x += velocityChange.x;
            velocity.z += velocityChange.z;

            // Clamp the horizontal velocity to ensure it doesn't exceed maximum air speed
            Vector3 horizontalVelocity = new Vector3(velocity.x, 0, velocity.z);
            if (horizontalVelocity.magnitude > speed)
            {
                horizontalVelocity = horizontalVelocity.normalized * speed;
                velocity.x = horizontalVelocity.x;
                velocity.z = horizontalVelocity.z;
            }
        }





        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, speed * Time.deltaTime * 200);
        }

    }

    void Jump()
    {
        Crouch = false;
        velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        CoyotteTimeCounter = 0f;
        JumpBufferCounter = 0f;
    }


    void SlopeManagement()
    {

        if (isGrounded)
        {
            Vector3 groundNormal = GroundNormal();
            if (groundNormal != Vector3.zero)
            {
                float slopeAngle = Vector3.Angle(groundNormal, Vector3.up);

                isSliding = slopeAngle >= maxSlopeAngle;
                if (isSliding)
                {
                    float slideSpeed = speedModifier * -speed * 4;

                    Vector3 slopeDirection = Vector3.Cross(groundNormal, Vector3.up);
                    slopeDirection = Vector3.Cross(slopeDirection, groundNormal).normalized;

                    Debug.Log(slopeDirection);
                    velocity += slopeDirection * slideSpeed;
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

    void SetJumpBuffer()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            JumpBufferCounter = JumpBufferTime;

        }
        else
        {
            JumpBufferCounter -= Time.deltaTime;
        }

    }

    void Acceleration()
    {

        if ((Input.GetAxis("Horizontal") >= 0.1f || Input.GetAxis("Horizontal") <= -0.1f || Input.GetAxis("Vertical") >= 0.1f || Input.GetAxis("Vertical") <= -0.1f) && speed <= 1.7f * speedBase)
        {
            speed += speedBase * 0.01f;
        }

        else if ((Mathf.Abs(Input.GetAxis("Horizontal")) < 0.1f && Mathf.Abs(Input.GetAxis("Vertical")) < 0.1f) && speed != speedBase)
        {
            speed = speedBase;
        }

    }


    void AnimationCOntroller()
    {
        bool canJump = true;
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 Movement = (moveZ * Vector3.up + moveX * Vector3.right);
        if (Movement != Vector3.zero)
        {
            anim.SetBool("IsMoving", true);
        }
        else
        {
            anim.SetBool("IsMoving", false);
        }


        if (canJump && Input.GetButtonDown("Fire1")) ;
        {
            anim.SetBool("Jump", true);
            canJump = false;
        }

        if (controller.isGrounded)
        {
            anim.SetBool("Jump", false);
            canJump = true;
        }

        anim.SetBool("isGrounded", controller.isGrounded);



    }

    public Vector3 GroundNormal()
    {

        List<Vector3> RaycastNormal = new List<Vector3>();

        //Recuperer les normale de tout les raycast qui touchent le sol
        foreach (Transform raycastOrigin in CheckGround)
        {

            RaycastHit hit;
            if (Physics.Raycast(raycastOrigin.transform.position + Vector3.up * 0.1f, Vector3.down, out hit, 0.5f))
            {
                Vector3 groundNormal = hit.normal;
                RaycastNormal.Add(groundNormal);
            }
        }

        //Calcule de la moyenne des normals recupere
        Vector3 averageNormal = Vector3.zero;

        foreach (Vector3 normal in RaycastNormal)
        {
            averageNormal += normal;
        }
        averageNormal /= RaycastNormal.Count;

        return averageNormal;
    }
}