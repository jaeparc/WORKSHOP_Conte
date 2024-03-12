using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_LedgeMovement : Player_MovementBehaviour
{
    public override void UpdateMovement(Player_Movement movement, CharacterController controller, PlayerController player)
    {
        float moveX = Input.GetAxis("Horizontal");
        Vector3 moveDirection = transform.right * moveX;
        float ledgeMoveSpeed = player.speed * 0.5f;
        controller.Move(moveDirection * ledgeMoveSpeed * Time.deltaTime);
    }
}
