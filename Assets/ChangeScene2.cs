using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene2 : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetButtonDown("Fire2"))
        {
            SceneManager.LoadScene("SceneGameplayExterieurTemple", LoadSceneMode.Additive);
        }
    }
}
