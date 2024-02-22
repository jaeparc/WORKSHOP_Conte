using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitchTrigger : MonoBehaviour
{
    private CameraManager camManager;
    public int selectCamera;    
    // Start is called before the first frame update
    void Start()
    {
        camManager = GameObject.Find("Managers").GetComponent<CameraManager>();   
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            camManager.ResetAllCameras();
            camManager.SetMasterCamera(selectCamera);
        }
    }
}
