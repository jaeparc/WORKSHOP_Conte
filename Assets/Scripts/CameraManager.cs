using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public GameObject[] cameras;
    
    public void ResetAllCameras()
    {
        foreach(var c in cameras)
        {
            c.GetComponent<CinemachineVirtualCamera>().Priority = 10;
        }
    }

    public void SetMasterCamera(int cam)
    {
        cameras[cam].GetComponent<CinemachineVirtualCamera>().Priority = 15;
    }
}
