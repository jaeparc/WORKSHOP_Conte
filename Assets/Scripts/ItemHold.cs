using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHold : MonoBehaviour
{
    public GameObject Torch;
    public bool objectHere,IsActivated;
    
    

    private void Start()
    {
        
    }

    private void Update()
    {
        if (objectHere && !IsActivated)
        {
            Torch.SetActive(true);
            IsActivated= true;
        }
        else if (!objectHere && IsActivated)
        {
            Torch.SetActive(false);
            IsActivated = false;
        }
    }

}
