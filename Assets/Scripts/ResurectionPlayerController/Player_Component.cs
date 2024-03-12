using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player_Component : MonoBehaviour
{

    public abstract void UpdatePlayer(PlayerController player, CharacterController controller);

}
