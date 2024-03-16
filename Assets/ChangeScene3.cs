using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene3 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetButtonDown("Fire2"))
        {
            SceneManager.LoadScene("BACKUP maxime intérieur temple 1", LoadSceneMode.Additive);
        }
    }
}
