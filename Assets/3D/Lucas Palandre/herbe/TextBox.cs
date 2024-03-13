using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextBox : MonoBehaviour
{
    public TextMeshProUGUI textToDisplay;
    [HideInInspector] public narrativeElement elementAssociated;
    bool started = false;
    float timer;

    // Update is called once per frame
    void Update()
    {
        Anim();
    }

    public void StartAnimation(){
        if(!started){
            textToDisplay.text = elementAssociated.text;
            started = true;
        }
    }

    void Anim(){
        if(started){
            timer += Time.deltaTime;
            if(timer < elementAssociated.dur_app){
                float coefApp = timer/elementAssociated.dur_app;
                float scale = Mathf.Lerp(0f,1f,coefApp);
                GetComponent<RectTransform>().localScale = new Vector3(scale,scale,1);
                textToDisplay.alpha = scale;
            } else if(timer >= elementAssociated.dur_stay+elementAssociated.dur_app){
                float coefApp = (timer-(elementAssociated.dur_app+elementAssociated.dur_stay))/elementAssociated.dur_dis;
                float alpha = Mathf.Lerp(1f,0f,coefApp);
                float scale = Mathf.Lerp(1f,10f,coefApp);
                textToDisplay.alpha = alpha;
                GetComponent<RectTransform>().localScale = new Vector3(scale,scale,1);
            } else if(timer > elementAssociated.dur_stay+elementAssociated.dur_app+elementAssociated.dur_dis)
                Destroy(gameObject);
        }
    }
}
