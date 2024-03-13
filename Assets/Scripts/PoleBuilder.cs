using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoleBuilder : MonoBehaviour
{
    public Pole pole;
    void Start()
    {
        pole.PoleGameObject = gameObject;

        pole.Lenght = pole.PoleGameObject.transform.localScale.y;

        pole.Base = new Vector3(transform.position.x, transform.position.y + pole.Lenght/2, transform.position.z);

        pole.End = new Vector3(transform.position.x, transform.position.y - pole.Lenght / 2, transform.position.z);

        


    }

}
