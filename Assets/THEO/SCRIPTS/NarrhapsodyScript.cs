using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class NarrhapsodyScript : MonoBehaviour
{
    public string csvFilePath;
    public List<narrativeElement> narrativeElements = new List<narrativeElement>();
    public GameObject textBoxPrefab;
    public List<GameObject> books = new List<GameObject>();
    public GameObject[] positions = new GameObject[9];

    // Start is called before the first frame update
    void Start()
    {
        ReadCSVfile();
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
                    narrativeElement elementToAdd = new narrativeElement(line.Split(";"));
                    narrativeElements.Add(elementToAdd);
                }
                lineIndex++;
            }
        }
    }

    public void ActivateBook(int index){
        if(index > books.Count-1){
            Debug.LogError("Book index out of range");
        } else {
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().enabled = false;
            books[index].SetActive(true);
            books[index].GetComponent<Animator>().SetTrigger("openBook");
            books[index].GetComponent<BookControl>().animInProgress = true;
            books[index].GetComponent<BookControl>().triggered = true;
        }
    }

    public void ActivateNarrativeElement(int index){
        index -= 2;
        if(index > narrativeElements.Count-1){
            Debug.LogError("NarrativeElements index out of range");
        }else{
            //Instanciation du prefab de textbox
            GameObject instanceTextBox;
            instanceTextBox = Instantiate(textBoxPrefab,transform);
            RectTransform transformInstanceTextBox = instanceTextBox.GetComponent<RectTransform>();
            transformInstanceTextBox.localScale = new Vector3(0.01f,0.01f,0.01f);

            //Détermination de la position sur le canvas
            Vector2 anchPos;
            switch(narrativeElements[index].position){
                case "TL":
                    anchPos = positions[0].GetComponent<RectTransform>().anchoredPosition;
                    break;
                case "TC":
                    anchPos = positions[1].GetComponent<RectTransform>().anchoredPosition;
                    break;
                case "TR":
                    anchPos = positions[2].GetComponent<RectTransform>().anchoredPosition;
                    break;
                case "ML":
                    anchPos = positions[3].GetComponent<RectTransform>().anchoredPosition;
                    break;
                case "MC":
                    anchPos = positions[4].GetComponent<RectTransform>().anchoredPosition;
                    break;
                case "MR":
                    anchPos = positions[5].GetComponent<RectTransform>().anchoredPosition;
                    break;
                case "BL":
                    anchPos = positions[6].GetComponent<RectTransform>().anchoredPosition;
                    break;
                case "BC":
                    anchPos = positions[7].GetComponent<RectTransform>().anchoredPosition;
                    break;
                case "BR":
                    anchPos = positions[8].GetComponent<RectTransform>().anchoredPosition;
                    break;
                default:
                    anchPos = positions[4].GetComponent<RectTransform>().anchoredPosition;
                    break;
            }
            transformInstanceTextBox.anchoredPosition = anchPos;

            //Assignation du texte
            instanceTextBox.GetComponent<TextBox>().elementAssociated = narrativeElements[index];

            //Démarrage animation
            instanceTextBox.GetComponent<TextBox>().StartAnimation();
            narrativeElements[index].triggered = true;
        }
    }
}
