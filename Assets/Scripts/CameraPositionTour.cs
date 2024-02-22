using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class CameraPositionTour : MonoBehaviour
{

    public GameObject cam;
    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cam.transform.position = new Vector3(17.5f * Mathf.Sin(-0.09f * other.transform.position.y ), other.transform.position.y, -17.5f * Mathf.Cos(0.09f * other.transform.position.y + 3.15f));
        }
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            cam.SetActive(true);
        }
    }


    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            cam.SetActive(true);
        }
    }
}

