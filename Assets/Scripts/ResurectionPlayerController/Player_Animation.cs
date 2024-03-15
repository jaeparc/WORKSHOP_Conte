using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Animation : Player_Component
{

    public Animator anim;

    public bool jumpDone, LedgeGrab;

    public override void UpdatePlayer(PlayerController player, CharacterController controller)
    {
        /*        bool canJump = true;
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


                if (canJump && Input.GetButtonDown("Fire1")) 
                {
                    anim.SetBool("Jump", true);
                }

                if (controller.isGrounded)
                {
                    anim.SetBool("Jump", false);
                }

                anim.SetBool("isGrounded", controller.isGrounded);
            */

        anim.SetFloat("XZvelocity", Mathf.Abs(player.velocity.x + player.velocity.z));
        anim.SetFloat("Yvelocity", player.velocity.y);
        anim.SetBool("isGrounded", player.isGrounded);

        anim.SetBool("Jump", jumpDone);

        anim.SetBool("IsLedgeGrabbing", LedgeGrab);


        

    }
}

