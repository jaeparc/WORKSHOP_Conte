using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    public GameObject cameraToManage;

    private OtherCameraManager cameraManager;

    private void Start()
    {
        cameraManager = FindObjectOfType<OtherCameraManager>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cameraManager.EnterCollider(other, cameraToManage);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cameraManager.ExitCollider(other, cameraToManage);
        }
    }
}

