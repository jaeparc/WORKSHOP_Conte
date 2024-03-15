using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deathZone : MonoBehaviour
{
    public GameObject fonduNoir;
    public Transform respawnPoint;

    void OnTriggerStay(Collider other){
        if(other.CompareTag("Player")){
            fonduNoir.SetActive(true);
            fonduNoir.GetComponent<fade>().respawnPoint = respawnPoint;
            fonduNoir.GetComponent<fade>().status = "respawn";
            fonduNoir.GetComponent<Animator>().SetTrigger("fadeIn");
        }
    }
}
