using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestOpenDoors : MonoBehaviour
{
    public GameObject door1, door2;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Button"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                door1.SetActive(false);
                door2.SetActive(false);
            }
        }
    }
}
