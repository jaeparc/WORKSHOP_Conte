using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DallePuzzle : MonoBehaviour
{
    public GameObject door;
    public Dalle poissonDalleScript;
    public Dalle oiseauDalleScript;
    public Dalle araigneeDalleScript;
    List<string> listeDalles = new List<string>();
    List<string> solution = new List<string>{"Poisson", "Poisson", "Oiseau", "Araignee", "Oiseau", "Poisson"};

    void Update()
    {
        // Vous pourriez vouloir déplacer cette logique ailleurs pour ne pas l'exécuter chaque frame
        CheckPressStatus();
    }

    void CheckPressStatus()
    {
        if (poissonDalleScript.dallePoissonPressed)
        {
            listeDalles.Add("Poisson");
            print("Poisson");
            poissonDalleScript.dallePoissonPressed = false; // Réinitialiser pour éviter les ajouts multiples
        }
        if (oiseauDalleScript.dalleOiseauPressed)
        {
            listeDalles.Add("Oiseau");
            print("Oiseau");
            oiseauDalleScript.dalleOiseauPressed = false; // Réinitialiser
        }
        if (araigneeDalleScript.dalleAraigneePressed)
        {
            listeDalles.Add("Araignee");
            print("Araignée");
            araigneeDalleScript.dalleAraigneePressed = false; // Réinitialiser
        }

        if (ContainsSequence(listeDalles, solution))
        {
            // Action si la séquence est correcte, par exemple ouvrir une porte
            door.SetActive(false);
            // Réinitialiser listeDalles si nécessaire pour permettre une nouvelle tentative
            listeDalles.Clear();
        }
    }

    public static bool ContainsSequence<T>(List<T> list, List<T> sequence)
    {
        if (sequence.Count > list.Count) return false;
        int limit = list.Count - sequence.Count + 1;
        for (int i = 0; i < limit; i++)
        {
            bool sequenceFound = true;
            for (int j = 0; j < sequence.Count; j++)
            {
                if (!list[i + j].Equals(sequence[j]))
                {
                    sequenceFound = false;
                    break;
                }
            }
            if (sequenceFound) return true;
        }
        return false;
    }
}
