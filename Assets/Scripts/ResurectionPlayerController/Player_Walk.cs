using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Walk : Player_MovementBehaviour
{
    public AnimationCurve SpeedCurve;

    public float Speed;

    public override void UpdateMovement(Player_Movement movement, CharacterController controller, PlayerController player)
    {

        Speed = player.speed;

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
        if (controller.isGrounded)
        {
            walkSpeed = SpeedCurve.Evaluate(player.speedModifier);

            moveDirection = (cameraForward * moveZ + cameraRight * moveX).normalized * Speed * walkSpeed;

            float yVelocity = player.velocity.y;
            player.velocity = moveDirection;
            player.velocity.y = yVelocity;
        }
        else // Adjusted air control
        {
            Vector3 inputDirection = (cameraForward * moveZ + cameraRight * moveX).normalized;
            float airControlFactor = 0.025f; // Further reduced for less air control

            // Apply a damping effect based on the difference between current and desired direction
            Vector3 desiredDirection = inputDirection * Speed;
            Vector3 velocityChange = (desiredDirection - new Vector3(player.velocity.x, 0, player.velocity.z)) * airControlFactor;

            // Evaluate the current velocity to apply changes
            if (player.velocity.x * inputDirection.x < 0 || player.velocity.z * inputDirection.z < 0)
            {
                // If the player is trying to change direction, allow a bit more control
                velocityChange *= 2;
            }

            player.velocity.x += velocityChange.x;
            player.velocity.z += velocityChange.z;

            // Clamp the horizontal velocity to ensure it doesn't exceed maximum air Speed
            Vector3 horizontalVelocity = new Vector3(player.velocity.x, 0, player.velocity.z);
            if (horizontalVelocity.magnitude > Speed)
            {
                horizontalVelocity = horizontalVelocity.normalized * Speed;
                player.velocity.x = horizontalVelocity.x;
                player.velocity.z = horizontalVelocity.z;
            }
        }





        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, Speed * Time.deltaTime * 200);
        }


    }
}

