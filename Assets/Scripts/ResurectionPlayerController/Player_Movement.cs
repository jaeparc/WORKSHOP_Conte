using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : Player_Component
{
    public Player_MovementBehaviour Walk, Ledge, Pole;


    private Player_MovementType type;
    public Player_MovementType Type
    { 
        get => type;
        set
        {
            type = value;
            switch (type)
            {
                case Player_MovementType.Walk:
                    currentBehaviour = Walk;
                    break;
                case Player_MovementType.Ledge:
                    currentBehaviour = Ledge;
                    break;
                case Player_MovementType.Pole:
                    currentBehaviour = Pole;
                    break;
            }
        }
    }


    private Player_MovementBehaviour currentBehaviour;

    public override void UpdatePlayer(PlayerController player, CharacterController controller)
    {
        currentBehaviour.UpdateMovement(this, controller,player);
    }
}
