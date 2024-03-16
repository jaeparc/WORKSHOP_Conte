using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxDetector : MonoBehaviour
{   
    public GameObject Sparkle;
    public Transform target;

    void Control(){
        if(Input.GetButton("Fire1")){
            transform.parent.GetComponent<movingBox>().SetDestination(target,transform);
        }
    }

    void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player") && !transform.parent.GetComponent<movingBox>().moving && transform.parent.GetComponent<movingBox>().moveable){
            Sparkle.SetActive(true);
        }
    }
    
    void OnTriggerStay(Collider other){
        if(other.CompareTag("Player") && !transform.parent.GetComponent<movingBox>().moving && transform.parent.GetComponent<movingBox>().moveable)
            Control();
        if(transform.parent.GetComponent<movingBox>().moving)
            Sparkle.SetActive(false);
    }
    
    void OnTriggerExit(Collider other){
        if(other.CompareTag("Player"))
            Sparkle.SetActive(false);
    }
}
