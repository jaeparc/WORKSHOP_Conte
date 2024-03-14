using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class Player_Hands : Player_Component
{

    public GameObject GrabHand, WhatsGrabbed;

    public bool IsGrabbing;

    public GameObject torch;


    private bool canDrop;
    public override void UpdatePlayer(PlayerController player, CharacterController controller)
    {
       /* if (IsGrabbing && WhatsGrabbed != false)
        {

            ItemHold(WhatsGrabbed);
            if (canDrop == false)
            {
                Invoke(nameof(CanDrop), 0.5f);
            }
            if (Input.GetButtonDown("Fire1") && canDrop == true)
            {
                DropItem();
            }
        }*/

        
    }
    /*public void GrabItem(GameObject itemGrabbed)
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
    void CanDrop()
    {
        canDrop = true;
    }*/


    public void ItemTake()
    {
        torch.SetActive(true);
    }

    public void ItemPut( )
    {
        torch.SetActive(false);
    }


}
