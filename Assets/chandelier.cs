using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chandelier : MonoBehaviour
{
    
    [SerializeField] GameObject lightAssociated;
    [SerializeField] GameObject particlesAssociated;

    public void turnOn(){
        lightAssociated.SetActive(true);
        particlesAssociated.SetActive(true);
        particlesAssociated.GetComponent<ParticleSystem>().Play();
    }

    public void turnOff(){
        lightAssociated.SetActive(false);
        particlesAssociated.GetComponent<ParticleSystem>().Stop();
        particlesAssociated.SetActive(false);
    }
}
