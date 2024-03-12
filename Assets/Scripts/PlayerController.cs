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
    public Vector3 velocity;
    private float groundSnapForce = -2f;
    public bool isGrounded;
    public float maxSlopeAngle = 45f;
    public bool isSliding;

    public float CoyotteTime, CoyotteTimeCounter;

    public float JumpBufferTime, JumpBufferCounter;

    public Transform[] CheckGround;

    public float speedModifier;

    public AnimationCurve SpeedCurve;

/*    public bool Crouch;*/

    public bool Hanging, CanMove;

    public bool onPole;

    public Transform Pole;

    public GameObject ThePole;

    public Player_Hands hands;
    public Player_Animation anim;

    public Player_Movement movement;
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
        movement.Type = Player_MovementType.Walk;
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

        speedModifier = Mathf.Abs(Vector3.Dot(GroundNormal().normalized, Vector3.down));

        GrabLedge();

        if (CanMove == true)
        {
            Acceleration();


        }


        
        movement.UpdatePlayer(this, controller);

        if (JumpBufferCounter > 0 && CoyotteTimeCounter > 0 && isSliding == false)
        {
            Jump();



        }

        if ((onPole || Hanging) && Input.GetButtonDown("Fire1"))
        {

            Jump();
        }


        hands.UpdatePlayer(this, controller);

        


        anim.UpdatePlayer(this, controller);



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
            hands.GrabItem(other.transform.gameObject);
        }
    }

   
    void Jump()
    {
       

        movement.Type = Player_MovementType.Walk;
        gravity = -80f;
        CoyotteTimeCounter = 1;
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
                    movement.Type = Player_MovementType.Ledge;

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






    void GrabPole(GameObject other)
    {

        Vector3 targetPosition = new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z);
        Vector3 offset = targetPosition - transform.position;

        controller.Move(offset);

        velocity = Vector3.zero;
        gravity = 0;
        CanMove = false;

        Pole = other.transform;
        movement.Type = Player_MovementType.Pole;
        onPole = true;
    }

}