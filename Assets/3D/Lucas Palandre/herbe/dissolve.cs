using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class dissolve : MonoBehaviour
{
    Material materialInstance;
    bool transitioning = false;
    public float secToTransition;
    float timer = 0;

    void Start(){
        Init();
    }

    void Update(){
        Transition();
        // if(Input.GetKeyUp(KeyCode.C) && materialInstance.GetFloat("Vector1_FEFF47F1") != 1)
        //     materialInstance.SetFloat("Vector1_FEFF47F1",1);
        // else if(Input.GetKeyUp(KeyCode.C) && materialInstance.GetFloat("Vector1_FEFF47F1") != 0)
        //     materialInstance.SetFloat("Vector1_FEFF47F1",0);
        if(Input.GetKey(KeyCode.C))
            TriggerDissolve();
    }

    public void TriggerDissolve(){
        transitioning = true;
    }

    void Transition(){
        if(transitioning){
            timer += Time.deltaTime;
            float coef = timer/secToTransition;
            materialInstance.SetFloat("Vector1_FEFF47F1",Mathf.Lerp(0,1,coef));
            if(materialInstance.GetFloat("Vector1_FEFF47F1") > 0.5 && GetComponent<BoxCollider>() != null)
                GetComponent<BoxCollider>().isTrigger = true;
            if(materialInstance.GetFloat("Vector1_FEFF47F1") >= 1){
                transitioning = false;
                Destroy(gameObject);
            }
        }
    }

    void Init(){
        if(GetComponent<MeshRenderer>() != null)
            materialInstance = GetComponent<MeshRenderer>().material;
        else
            materialInstance = GetComponent<LineRenderer>().material;
    }
}
