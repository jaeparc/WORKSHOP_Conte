using System.Collections;
using System.Collections.Generic;
using BookCurlPro;
using UnityEngine;

public class BookControl : MonoBehaviour
{
    [HideInInspector] public bool triggered = false;

    void Update()
    {
        Controls();
    }

    void Controls(){
        int currentPage = GetComponent<BookPro>().currentPaper;
        int maxPage = GetComponent<BookPro>().papers.Length;
        if(Input.GetKeyDown(KeyCode.RightArrow)){
            if(currentPage < maxPage)
                GetComponent<AutoFlip>().FlipRightPage();
            else 
                GetComponent<Animator>().SetTrigger("closeBook");
        } else if(Input.GetKeyDown(KeyCode.LeftArrow)){
            GetComponent<AutoFlip>().FlipLeftPage();
        }
    }

    public void EndAnimationClose(){
        gameObject.SetActive(false);
    }
}
