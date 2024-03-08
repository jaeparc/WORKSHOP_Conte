using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class NarrhapsodyScript : MonoBehaviour
{
    public string csvFilePath;
    public List<narrativeElement> narrativeElements = new List<narrativeElement>();
    public GameObject textBoxPrefab;

    // Start is called before the first frame update
    void Start()
    {
        ReadCSVfile();
    }

    void updateAnimation(){

    }

    void ReadCSVfile(){
        // Vérifie que le chemin du fichier CSV est valide
        if (!File.Exists(csvFilePath))
        {
            Debug.LogError("Le fichier CSV n'existe pas : " + csvFilePath);
            return;
        }

        int lineIndex = 0;
        // Lecture du fichier CSV
        using (StreamReader sr = new StreamReader(csvFilePath))
        {
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                if(lineIndex != 0){
                    narrativeElement elementToAdd = new(line.Split(";"));
                    narrativeElements.Add(elementToAdd);
                }
                lineIndex++;
            }
        }
    }

    public void ActivateNarrativeElement(int index){
        index -= 2;
        if(!narrativeElements[index].triggered){
            //Instanciation du prefab de textbox
            GameObject instanceTextBox;
            instanceTextBox = Instantiate(textBoxPrefab,transform);
            RectTransform transformInstanceTextBox = instanceTextBox.GetComponent<RectTransform>();
            transformInstanceTextBox.localScale = new Vector3(0.01f,0.01f,0.01f);

            //Détermination de la position sur le canvas
            float xPos = 0;
            float yPos = 0;
            switch(narrativeElements[index].position){
            //     case "TL":
            //         xPos = Random.Range(-Screen.width/2,(-Screen.width/2)+(Screen.width/3));
            //         yPos = Random.Range((Screen.height/2)-(Screen.height/3),Screen.height/2);
            //         break;
            //     case "TC":
            //         xPos = Random.Range((-Screen.width/2)+(Screen.width/3),(-Screen.width/2)+(2*(Screen.width/3)));
            //         yPos = Random.Range((Screen.height/2)-(Screen.height/3),Screen.height/2);
            //         break;
            //     case "TR":
            //         xPos = Random.Range((-Screen.width/2)+(2*(Screen.width/3)),Screen.width/2);
            //         yPos = Random.Range((Screen.height/2)-(Screen.height/3),Screen.height/2);
            //         break;
            //     case "ML":
            //         xPos = Random.Range(-Screen.width/2,(-Screen.width/2)+(Screen.width/3));
            //         yPos = Random.Range((Screen.height/2)-(2*(Screen.height/3)),(Screen.height/2)-(Screen.height/3));
            //         break;
            //     case "MC":
            //         xPos = Random.Range((-Screen.width/2)+(Screen.width/3),(-Screen.width/2)+(2*(Screen.width/3)));
            //         yPos = Random.Range((Screen.height/2)-(2*(Screen.height/3)),(Screen.height/2)-(Screen.height/3));
            //         break;
            //     case "MR":
            //         xPos = Random.Range((-Screen.width/2)+(2*(Screen.width/3)),Screen.width/2);
            //         yPos = Random.Range((Screen.height/2)-(2*(Screen.height/3)),(Screen.height/2)-(Screen.height/3));
            //         break;
            //     case "BL":
            //         xPos = Random.Range(-Screen.width/2,(-Screen.width/2)+(Screen.width/3));
            //         yPos = Random.Range(-Screen.height/2,(Screen.height/2)-(2*(Screen.height/3)));
            //         break;
            //     case "BC":
            //         xPos = Random.Range((-Screen.width/2)+(Screen.width/3),(-Screen.width/2)+(2*(Screen.width/3)));
            //         yPos = Random.Range(-Screen.height/2,(Screen.height/2)-(2*(Screen.height/3)));
            //         break;
            //     case "BR":
            //         xPos = Random.Range((-Screen.width/2)+(2*(Screen.width/3)),Screen.width/2);
            //         yPos = Random.Range(-Screen.height/2,(Screen.height/2)-(2*(Screen.height/3)));
            //         break;
            
                case "TL":
                    xPos = -Screen.width/4;
                    yPos = Screen.height/4;
                    break;
                case "TC":
                    xPos = 0;
                    yPos = Screen.height/4;
                    break;
                case "TR":
                    xPos = Screen.width/4;
                    yPos = Screen.height/4;
                    break;
                case "ML":
                    xPos = -Screen.width/4;
                    yPos = 0;
                    break;
                case "MC":
                    xPos = 0;
                    yPos = 0;
                    break;
                case "MR":
                    xPos = Screen.width/4;
                    yPos = 0;
                    break;
                case "BL":
                    xPos = -Screen.width/4;
                    yPos = -Screen.height/4;
                    break;
                case "BC":
                    xPos = 0;
                    yPos = -Screen.height/4;
                    break;
                case "BR":
                    xPos = Screen.width/4;
                    yPos = -Screen.height/4;
                    break;
            }
            transformInstanceTextBox.anchoredPosition = new Vector3(xPos,yPos,0f);

            //Assignation du texte
            instanceTextBox.GetComponent<TextBox>().elementAssociated = narrativeElements[index];

            //Démarrage animation
            instanceTextBox.GetComponent<TextBox>().StartAnimation();
            narrativeElements[index].triggered = true;
        }
    }
}
