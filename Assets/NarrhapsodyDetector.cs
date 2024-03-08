using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrhapsodyDetector : MonoBehaviour
{
    [SerializeField] NarrhapsodyScript narrhapsody;

    void OnTriggerEnter(Collider other){
        if(other.CompareTag("NarrativeTrigger")){
            narrhapsody.ActivateNarrativeElement(other.GetComponent<NarrativeTrigger>().ID);
        }
    }
}
