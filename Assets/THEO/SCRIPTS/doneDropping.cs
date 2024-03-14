using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doneDropping : MonoBehaviour
{
    void doneAnim(){
        transform.position = transform.GetChild(0).position;
        transform.GetChild(0).localPosition = Vector3.zero;
        transform.GetChild(0).localRotation = Quaternion.Euler(Vector3.zero);
        transform.GetChild(0).GetComponent<movingBox>().moveable = true;
    }
}
