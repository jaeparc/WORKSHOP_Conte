using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class narrativeElement
{
    public string text, position, nameCharacter;
    public float dur_app, dur_stay, dur_dis, scale;
    public bool triggered = false;

    public narrativeElement(string[] entries){
        for(int i = 0; i < entries.Length; i++){
            Debug.Log(entries[i]);
        }
        text = entries[0].Replace("\"\"","");
        nameCharacter = entries[1];
        position = entries[2];
        switch(entries[3]){
            case "small":
                scale = 0.5f;
                break;
            case "mid":
                scale = 1f;
                break;
            case "big":
                scale = 2f;
                break;
            default:
                scale = 1f;
                break;
        }
        dur_app = float.Parse(entries[4]);
        dur_stay = float.Parse(entries[5]);
        dur_dis = float.Parse(entries[6]);
    }
}
