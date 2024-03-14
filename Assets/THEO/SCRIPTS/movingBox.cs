using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingBox : MonoBehaviour
{
    Vector3 _destination, _detector;
    /*[HideInInspector]*/ public bool moving, moveable = true; 
    [SerializeField] float speed;
    [SerializeField] char axisDrop;

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public void SetDestination(Transform target, Transform detector){
        _destination = target.position;
        _detector = detector.position;
        moving = true;
    }

    void Move(){
        if(moving){
            RaycastHit hit;
            Vector3 oppositeDetector = _detector-transform.position;
            Ray ray = new Ray(transform.position-oppositeDetector,_destination-transform.position);
            if(
                (Physics.Raycast(ray, out hit) && hit.distance < 0.025f) ||
                (Vector3.Distance(transform.position,_destination) < 0.01f)
            ){
                moving = false;
            } else {
                float realSpeed = speed*Time.deltaTime;
                transform.parent.position = Vector3.MoveTowards(transform.parent.position, _destination, realSpeed);
            }
        }
    }
}
