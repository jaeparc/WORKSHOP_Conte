using UnityEngine;

public abstract class Player_MovementBehaviour : MonoBehaviour
{
    public abstract void UpdateMovement(Player_Movement movement, CharacterController controller, PlayerController player);
}






