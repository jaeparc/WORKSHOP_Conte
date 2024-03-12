using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_PoleMovement : Player_MovementBehaviour
{
    public override void UpdateMovement(Player_Movement movement, CharacterController controller, PlayerController player)
    {
        {

            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");


            Vector3 upMovement = moveZ * Vector3.up * Time.deltaTime * player.speed;
            controller.Move(upMovement);


            if (moveX != 0)
            {

                Vector3 directionToPole = player.ThePole.transform.position - transform.position;
                directionToPole.y = 0;


                Vector3 rightDirection = Quaternion.Euler(0, 90, 0) * directionToPole.normalized;

                Vector3 rotationMovement = rightDirection * moveX * Time.deltaTime * player.speed;
                controller.Move(rotationMovement);
            }

            Vector3 lookDirection = player.ThePole.transform.position - transform.position;
            lookDirection.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        }
    }
}
