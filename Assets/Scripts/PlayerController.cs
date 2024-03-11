using JetBrains.Annotations;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.Rendering;

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

/*    public bool Crouch;*/

    public bool Hanging, CanMove;

    public bool onPole;

    public Transform Pole;

    public int movementMethod;

    public GameObject ThePole;

    public GameObject GrabHand,WhatsGrabbed;

    public bool IsGrabbing;


    private bool canDrop;
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
/*        Crouch = false;*/
        CanMove = true;
    }

    public void Update()
    {

        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = groundSnapForce;
        }

/*        if (Input.GetButtonDown("Fire2"))
        {
            Crouch = !Crouch;
        }*/

        GrabLedge();

        if (CanMove == true)
        {
            Acceleration();

/*            if (Input.GetButtonDown("Fire2"))
            {
                Crouch = !Crouch;
            }
*/


            /*            if (Crouch)
                        {
                            CrouchWalk();
                            transform.localScale = new Vector3(transform.localScale.x, 0.5f, transform.localScale.x);
                        }
                        else
                        {
                            Walk();
                            transform.localScale = new Vector3(transform.localScale.x, 1f, transform.localScale.x);

                        }*/

        }


        switch (movementMethod)
        {
            case 0:
                Walk();
                break;
            case 2:
                PoleMovement();
                break;
            case 3:
                LedgeMovement();
                break;
            default:
                Walk();
                break;
        }

        if (JumpBufferCounter > 0 && CoyotteTimeCounter > 0 && isSliding == false)
        {
            Jump();



        }

        if (onPole && Input.GetButtonDown("Fire1"))
        {

            Jump();
        }
        if (Hanging && Input.GetButtonDown("Fire1"))
        {

            Jump();
        }


        if (IsGrabbing && WhatsGrabbed != false)
        {

            ItemHold(WhatsGrabbed);
            if (canDrop == false)
            {
                Invoke("CanDrop", 0.5f);
            }
            if (Input.GetButtonDown("Fire2") && canDrop == true)
            {
                DropItem();
            }
        }

        

        speedModifier = Mathf.Abs(Vector3.Dot(GroundNormal().normalized, Vector3.down));

        AnimationCOntroller();



        SlopeManagement();

        SetCoyotteTime();
        SetJumpBuffer();



        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);


    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Pole") && Input.GetKeyDown(KeyCode.I) && onPole == false)
        {
            GrabPole(other.gameObject);
            ThePole = other.transform.gameObject;
        }
        if (other.CompareTag("Grabbable") && Input.GetButtonDown("Fire2"))
        {
            GrabItem(other.transform.gameObject);
        }
    }
    private void FixedUpdate()
    {




    }

/*    void CrouchWalk()
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

        float yVelocity = velocity.y;
        velocity = moveDirection;
        velocity.y = yVelocity;

        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, (speed / 2) * Time.deltaTime * 200);
        }
    }*/
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
        /*        if (Hanging)
                {
                    CanMove = true;
                    gravity = -80;
                    Hanging = false;
                    onPole = false;
                    velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
                    StartCoroutine(EnableCanMove(0.25f));
                }
                else if (onPole)
                {
                    gravity = -80;
                    onPole = false;
                    velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
                }
                else
                {
                    CoyotteTimeCounter = 1;
                    Crouch = false;
                    velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
                    CoyotteTimeCounter = 0f;
                    JumpBufferCounter = 0f;
                }
                */

        movementMethod = 0;
        gravity = -80f;
        CoyotteTimeCounter = 1;
/*        Crouch = false;*/
        velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        CoyotteTimeCounter = 0f;
        JumpBufferCounter = 0f;
        Hanging = false;
        onPole = false;
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



    public void GrabLedge()
    {

        if (velocity.y < 0 && Hanging == false)
        {

            RaycastHit DownHit;
            Vector3 LineDownStart = (transform.position + Vector3.up * 1.5f) + transform.forward;
            Vector3 LineDownEnd = (transform.position + Vector3.up * 0.7f) + transform.forward * 1;
            Physics.Linecast(LineDownStart, LineDownEnd, out DownHit, LayerMask.GetMask("Ground"));
            Debug.DrawLine(LineDownStart, LineDownEnd);


            if (DownHit.collider != null)
            {

                RaycastHit ForwardHit;
                Vector3 LineForwardStart = new Vector3(transform.position.x, DownHit.point.y - 0.1f, transform.position.z);
                Vector3 LineForwardEnd = new Vector3(transform.position.x, DownHit.point.y - 0.1f, transform.position.z) + transform.forward * 1;
                Physics.Linecast(LineForwardStart, LineForwardEnd, out ForwardHit, LayerMask.GetMask("Ground"));
                Debug.DrawLine(LineForwardStart, LineForwardEnd);

                if (ForwardHit.collider != null)
                {
                    movementMethod = 3;

                    gravity = 0;
                    velocity = Vector3.zero;
                    Hanging = true;

                    Vector3 hangPos = new Vector3(ForwardHit.point.x, DownHit.point.y, ForwardHit.point.z);
                    Vector3 offset = transform.forward * -0.1f + transform.up * -1f;
                    hangPos += offset;

                    transform.position = hangPos;

                    transform.forward = -ForwardHit.normal;
                }
            }


        }
    }



    void PoleMovement()
    {
        {
            Debug.Log("poley");
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            // Moving up and down the pole
            Vector3 upMovement = moveZ * Vector3.up * Time.deltaTime * speed;
            controller.Move(upMovement);

            // Rotating around the pole
            if (moveX != 0)
            {
                // Calculate the direction from the player to the pole
                Vector3 directionToPole = ThePole.transform.position - transform.position;
                directionToPole.y = 0; // Ignore y-axis to ensure rotation is horizontal

                // Calculate the right vector relative to the direction to the pole, for clockwise/anti-clockwise rotation
                Vector3 rightDirection = Quaternion.Euler(0, 90, 0) * directionToPole.normalized;

                // Rotate around the pole by moving perpendicular to the direction to the pole
                Vector3 rotationMovement = rightDirection * moveX * Time.deltaTime * speed;
                controller.Move(rotationMovement);
            }

            // Optionally, ensure the player always faces the pole while moving around it
            Vector3 lookDirection = ThePole.transform.position - transform.position;
            lookDirection.y = 0; // Keep the rotation horizontal
            Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    public void LedgeMovement()
    {
        float moveX = Input.GetAxis("Horizontal"); // Get horizontal input

        // Assuming the player's forward direction is facing the wall, rightward movement will be the player's right vector, and leftward movement will be the opposite.
        Vector3 moveDirection = transform.right * moveX;

        // You might want to adjust the movement speed while on the ledge, for more precise control
        float ledgeMoveSpeed = speed * 0.5f; // Half the normal speed, for example

        // Apply the movement. Since you're using a CharacterController, you can directly use the Move function
        controller.Move(moveDirection * ledgeMoveSpeed * Time.deltaTime);

        // Optional: Adjust the player's rotation if necessary, depending on how you want them to face while moving along the ledge
        // For example, keep facing the wall or dynamically change facing direction based on movement direction
    }



    /*    public void PoleClimbing()
        {

            List<RaycastHit> allPoles = new List<RaycastHit>();
            RaycastHit hit;
            Collider[] hitCollider = Physics.OverlapSphere(transform.position, 3, 6);

            if (hitCollider!= null)
            {
                foreach (Collider pole in hitCollider)
                {
                    if (Physics.Raycast(transform.position, pole.transform.position, out hit))
                    {
                        if (hit.distance < 1 && !allPoles.Contains(hit))
                        {
                            allPoles.Add(hit);
                        }
                        else if (hit.distance >= 1 && allPoles.Contains(hit))
                        {
                            allPoles.Remove(hit);
                        }
                    }
                }
            }



            if (allPoles!=null)
            {
                if (allPoles.Count == 1)
                {
                    if (Input.GetButtonDown("Fire1"))
                    {
                        transform.position = allPoles[0].transform.position;
                    }
                }
                else
                {
                    for (int i = 0; i < allPoles.Count; i++)
                    {
                        if 
                    }
                }
            }

        }*/

    void GrabPole(GameObject other)
    {
        // Calculate the position offset
        Vector3 targetPosition = new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z);
        Vector3 offset = targetPosition - transform.position;

        // Move using CharacterController
        controller.Move(offset);

        velocity = Vector3.zero;
        gravity = 0;
        CanMove = false;

        Pole = other.transform;
        movementMethod = 2;
        onPole = true;
    }

    void GrabItem(GameObject itemGrabbed)
    {
        itemGrabbed.transform.position = GrabHand.transform.position;
        WhatsGrabbed = itemGrabbed;
        IsGrabbing = true;
        canDrop = false;


    }
    void ItemHold(GameObject itemGrabbed)
    {

        itemGrabbed.transform.position = GrabHand.transform.position;
    }
    void DropItem()
    {
        IsGrabbing = false;
        WhatsGrabbed.transform.position = new Vector3(WhatsGrabbed.transform.position.x, WhatsGrabbed.transform.position.y, WhatsGrabbed.transform.position.z);
        WhatsGrabbed = null;
        
    }

    private IEnumerator EnableCanMove(float WaitTime)
    {
        yield return new WaitForSeconds(WaitTime);
        CanMove = true;
    }

    void CanDrop()
    {
        canDrop = true;
    }
}