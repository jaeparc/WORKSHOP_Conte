using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestOpenDoors : MonoBehaviour
{
    bool triggerStay = false;
    public GameObject door1, door2;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && triggerStay)
        {
            door1.SetActive(false);
            door2.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Button"))
        {
            triggerStay = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Button"))
        {
            triggerStay = false;
        }
    }
}
