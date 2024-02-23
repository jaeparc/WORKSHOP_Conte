using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherCameraManager : MonoBehaviour
{
    // Dictionnaire pour suivre les colliders par caméra
    private Dictionary<GameObject, HashSet<Collider>> cameraColliders = new Dictionary<GameObject, HashSet<Collider>>();
    // Dictionnaire pour suivre les coroutines de désactivation des caméras
    private Dictionary<GameObject, Coroutine> cameraDisableCoroutines = new Dictionary<GameObject, Coroutine>();

    public void EnterCollider(Collider collider, GameObject cameraToManage)
    {
        if (!cameraColliders.ContainsKey(cameraToManage))
        {
            cameraColliders[cameraToManage] = new HashSet<Collider>();
        }

        cameraColliders[cameraToManage].Add(collider);
        cameraToManage.SetActive(true);
        
        // Si une coroutine de désactivation pour cette caméra est en cours, l'arrêter
        if (cameraDisableCoroutines.TryGetValue(cameraToManage, out Coroutine coroutine))
        {
            StopCoroutine(coroutine);
            cameraDisableCoroutines.Remove(cameraToManage);
        }
    }

    public void ExitCollider(Collider collider, GameObject cameraToManage)
    {
        if (cameraColliders.ContainsKey(cameraToManage) && cameraColliders[cameraToManage].Remove(collider))
        {
            // Vérifier après la sortie du collider si il n'y a plus de colliders pour cette caméra
            if (cameraColliders[cameraToManage].Count == 0)
            {
                // Commencer une coroutine qui attendra avant de désactiver la caméra spécifique
                Coroutine disableCoroutine = StartCoroutine(DisableCameraWithDelay(0.5f, cameraToManage));
                cameraDisableCoroutines[cameraToManage] = disableCoroutine;
            }
        }
    }

    private IEnumerator DisableCameraWithDelay(float delay, GameObject cameraToManage)
    {
        yield return new WaitForSeconds(delay);
        // Vérifier à nouveau si il n'y a toujours pas de colliders pour cette caméra après le délai
        if (cameraColliders.ContainsKey(cameraToManage) && cameraColliders[cameraToManage].Count == 0)
        {
            cameraToManage.SetActive(false);
            cameraDisableCoroutines.Remove(cameraToManage);
        }
    }
}
