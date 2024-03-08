using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactDestroyScript : MonoBehaviour
{
    [SerializeField] GameObject objectToDestroy;
    [SerializeField] GameObject sparkle;
    [SerializeField] bool animationToPlay;
    bool interacted = false;


    void Control(){
        if(Input.GetKey(KeyCode.E) && !interacted)
            DestroyObject();
    }

    void DestroyObject(){
        objectToDestroy.GetComponent<dissolve>().TriggerDissolve();
        if(animationToPlay)
            GetComponent<Animator>().SetTrigger("Destroyed");
        interacted = true;
    }

    void animationDone(){
        sparkle.SetActive(false);
    }

    void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player") && !interacted)
            sparkle.SetActive(true);
    }

    void OnTriggerStay(Collider other){
        if(other.CompareTag("Player"))
            Control();
    }
    
    void OnTriggerExit(Collider other){
        if(other.CompareTag("Player"))
            sparkle.SetActive(false);
    }
}