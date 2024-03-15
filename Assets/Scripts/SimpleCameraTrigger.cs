using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SimpleCameraTrigger : MonoBehaviour
{
    public GameObject cameraToManage;


    private void Start()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cameraToManage.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cameraToManage.SetActive(false);
        }
    }
}

