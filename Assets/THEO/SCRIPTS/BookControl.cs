using System.Collections;
using System.Collections.Generic;
using BookCurlPro;
using UnityEngine;

public class BookControl : MonoBehaviour
{
    [HideInInspector] public bool triggered = false;
    bool turningPage;

    void Update()
    {
        Controls();
    }

    void Controls(){
        int currentPage = GetComponent<BookPro>().currentPaper;
        int maxPage = GetComponent<BookPro>().papers.Length;
        if(Input.GetAxis("Horizontal") > 0 && !turningPage){
            if(currentPage < maxPage)
                GetComponent<AutoFlip>().FlipRightPage();
            else 
                GetComponent<Animator>().SetTrigger("closeBook");
            turningPage = true;
        } else if(Input.GetAxis("Horizontal") < 0 && !turningPage){
            GetComponent<AutoFlip>().FlipLeftPage();
            turningPage = true;
        } else if(Input.GetAxis("Horizontal") == 0){
            turningPage = false;
        }
    }

    public void EndAnimationClose(){
        GameObject.FindWithTag("Player").GetComponent<PlayerController>().enabled = true;
        gameObject.SetActive(false);
    }
}
