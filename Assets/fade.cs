using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fade : MonoBehaviour
{
    [HideInInspector] public string status;
    [HideInInspector] public Transform respawnPoint;

    public void AnimationFadeInDone(){
        switch(status){
            case "respawn":
                GameObject.FindWithTag("Player").GetComponent<CharacterController>().enabled = false;
                GameObject.FindWithTag("Player").transform.position = respawnPoint.position;
                GameObject.FindWithTag("Player").GetComponent<CharacterController>().enabled = true;
                GetComponent<Animator>().SetTrigger("fadeOut");
                break;
        }
    }

    public void AnimationFadeOutDone(){
        gameObject.SetActive(false);
    }
}
