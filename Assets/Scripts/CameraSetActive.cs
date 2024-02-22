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

    private int collisionCount = 0;

    public bool IsNotColliding
    {
        get { return collisionCount == 0; }
    }

    private void OnTriggerEnter(Collider hit)
    {
        if (collisionCount < 1)
        {
            collisionCount++;
        }

        if (hit.tag == "Player")
        {
            Camera.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider hit)
    {
        
        if (collisionCount == 1)
        {
            collisionCount--;
        }
  
        if (IsNotColliding == true)
        {
            if (hit.tag == "Player")
            {
                Camera.SetActive(false);
            }
        }
        
    }
}
