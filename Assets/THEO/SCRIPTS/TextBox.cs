using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Drawing;

public class TextBox : MonoBehaviour
{
    public GameObject nameBox, textBox;
    public TextMeshProUGUI textToDisplay, nameToDisplay;
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
            if(elementAssociated.nameCharacter != ""){
                nameBox.SetActive(true);
                nameToDisplay.text = elementAssociated.nameCharacter;
            } else {
                nameBox.SetActive(false);
            }
            textToDisplay.text = elementAssociated.text;
            started = true;
        }
    }

    void Anim(){
        if(started){
            timer += Time.deltaTime;
            if(timer < elementAssociated.dur_app){
                float coefApp = timer/elementAssociated.dur_app;
                float scale = Mathf.Lerp(0f,elementAssociated.scale,coefApp);
                GetComponent<RectTransform>().localScale = new Vector3(scale,scale,1);
                textToDisplay.alpha = scale;
            } else if(timer >= elementAssociated.dur_stay+elementAssociated.dur_app){
                float coefApp = (timer-(elementAssociated.dur_app+elementAssociated.dur_stay))/elementAssociated.dur_dis;
                float alpha = Mathf.Lerp(1f,0f,coefApp);
                float scale = Mathf.Lerp(elementAssociated.scale,10f*elementAssociated.scale,coefApp);
                textToDisplay.alpha = alpha;
                Color32 colorTextBox = textBox.GetComponent<UnityEngine.UI.Image>().color;
                textBox.GetComponent<UnityEngine.UI.Image>().color = new Color32(colorTextBox.r,colorTextBox.g,colorTextBox.b,(byte)(255f*alpha));
                if(nameBox.activeSelf){
                    Color32 colorNameBox = nameBox.GetComponent<UnityEngine.UI.Image>().color;
                    nameBox.GetComponent<UnityEngine.UI.Image>().color = new Color32(colorNameBox.r,colorNameBox.g,colorNameBox.b,(byte)(255f*alpha));
                    nameToDisplay.alpha = alpha;
                }
                GetComponent<RectTransform>().localScale = new Vector3(scale,scale,1);
            }
            if(timer > (elementAssociated.dur_stay+elementAssociated.dur_app+elementAssociated.dur_dis)){
                Destroy(gameObject);
            }
            Debug.Log("Timer : "+timer+";Time max : "+(elementAssociated.dur_stay+elementAssociated.dur_app+elementAssociated.dur_dis));
        }
    }
}
