using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DroppingBoxTrigger : MonoBehaviour
{
    public GameObject associatedBox;
    public float distanceMagnet;


    void MagnetBox(){
        associatedBox.GetComponent<movingBox>().moving = false;
        associatedBox.transform.parent.position = new Vector3(transform.position.x,associatedBox.transform.parent.position.y,transform.position.z);
        associatedBox.GetComponent<movingBox>().moveable = false;
    }

    void OnTriggerStay(Collider other){
        if(other.CompareTag("Box")){
            Vector2 selfPos = new Vector2(transform.position.x,transform.position.y);
            Vector2 boxPos = new Vector2(other.transform.position.x,other.transform.position.y);
            float distance = Vector2.Distance(selfPos,boxPos);
            if(distance < distanceMagnet){
                associatedBox = other.transform.gameObject;
                MagnetBox();
                associatedBox.transform.parent.GetComponent<Animator>().SetBool("drop",true);
                Debug.Log("TRIGGERED");
            }
        }
    }

    void OnTriggerExit(Collider other){
        if(other.CompareTag("Box")){
            associatedBox.transform.parent.GetComponent<Animator>().SetBool("drop",false);
            associatedBox = null;
        }
    }
}
