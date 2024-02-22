using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetActive : MonoBehaviour
{
    public GameObject Camera;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider hit)
    {
        if (hit.tag == "Player")
        {
            Camera.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider hit)
    {
        if (hit.tag == "Player")
        {
            Camera.SetActive(false);
        }
    }
}
