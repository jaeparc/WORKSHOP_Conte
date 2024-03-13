using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorteAnimation : MonoBehaviour
{
    public Animator animator;

    public GameObject porte;

    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void DeclenchementAnim(){
        animator.SetTrigger("puzzleReussi");
        Invoke("TurnOffCam",4);
    }

    void TurnOffCam(){
        animator.SetTrigger("finAnim");
    }   

}
