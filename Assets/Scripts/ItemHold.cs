using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHold : MonoBehaviour
{
    public GameObject Torch;
    public GameObject ObjectPrefab;
    public bool objectHere,IsActivated;
    
    

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public void MakeObjectAppear()
    {
        Torch.SetActive(false);
    }

    public void MakeObjectDisappear() 
    {
        Torch.SetActive(true);
    }
}
