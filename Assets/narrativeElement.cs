using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class narrativeElement
{
    public string text, position;
    public float dur_app, dur_stay, dur_dis;
    public bool triggered = false;

    public narrativeElement(string[] entries){
        text = entries[0].Replace("\"\"","");
        position = entries[1];
        dur_app = float.Parse(entries[2]);
        dur_stay = float.Parse(entries[3]);
        dur_dis = float.Parse(entries[4]);
    }
}
