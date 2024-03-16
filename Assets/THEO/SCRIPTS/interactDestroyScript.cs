using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactDestroyScript : MonoBehaviour
{
    [SerializeField] List<GameObject> objectsToDestroy = new List<GameObject>();
    [SerializeField] GameObject sparkle;
    [SerializeField] bool animationToPlay;
    [SerializeField] GameObject colliderToDisable;
    [SerializeField] GameObject cameraToDisable;
    bool interacted = false;


    void Control(){
        if(Input.GetButtonDown("Fire1") && !interacted)
            DestroyObject();
    }

    void DestroyObject(){
        foreach(GameObject obj in objectsToDestroy){
            obj.GetComponent<dissolve>().TriggerDissolve();
        }
        if(animationToPlay)
            GetComponent<Animator>().SetTrigger("Destroyed");
        interacted = true;
    }

    void animationDone(){
        sparkle.SetActive(false);
        if (colliderToDisable != null) // Bug si le joueur arrive dans le collider avant la fin de l'animation en étant assez rapide (ce qui devrait être impossible)
        {
            colliderToDisable.SetActive(false);
            cameraToDisable.SetActive(false);
        }
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
