using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrhapsodyDetector : MonoBehaviour
{
    [SerializeField] NarrhapsodyScript narrhapsody;

    void OnTriggerEnter(Collider other){
        if(other.CompareTag("NarrativeTrigger") && other.GetComponent<NarrativeTrigger>().CSV){
            if((!narrhapsody.narrativeElements[other.GetComponent<NarrativeTrigger>().ID-2].triggered && other.GetComponent<NarrativeTrigger>().UniqueTrigger) || !other.GetComponent<NarrativeTrigger>().UniqueTrigger)
                narrhapsody.ActivateNarrativeElement(other.GetComponent<NarrativeTrigger>().ID);
        } else if(other.CompareTag("NarrativeTrigger") && other.GetComponent<NarrativeTrigger>().BOOK){
            if((!narrhapsody.books[other.GetComponent<NarrativeTrigger>().ID].GetComponent<BookControl>().triggered && other.GetComponent<NarrativeTrigger>().UniqueTrigger) || !other.GetComponent<NarrativeTrigger>().UniqueTrigger)
                narrhapsody.ActivateBook(other.GetComponent<NarrativeTrigger>().ID);
        }
    }
}
