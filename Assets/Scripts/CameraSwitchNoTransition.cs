using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwitchNoTransition : MonoBehaviour
{
    private CinemachineBrain cinemachineBrain;
    public CinemachineVirtualCamera targetCamera;
    public CinemachineMixingCamera targetMixingCamera;

    // Déclarer originalBlend comme membre de la classe pour qu'elle soit accessible partout dans la classe
    private CinemachineBlendDefinition originalBlend;

    private void Start()
    {
        cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
        // Stocker la configuration originale de blend du CinemachineBrain pour la restaurer plus tard
        originalBlend = cinemachineBrain.m_DefaultBlend;
    }

    public void ActivateCameraWithoutTransition()
    {
        // Désactiver la transition
        cinemachineBrain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, 1);

        // Activer les caméras cibles si elles sont assignées
        if (targetCamera != null)
        {
            targetCamera.Priority = 100;
        }
        if (targetMixingCamera != null)
        {
            targetMixingCamera.Priority = 100;
        }

        // Planifier la restauration du réglage de transition original
        Invoke("ResetBlend", 0.1f);
    }

    public void DeactivateCamera()
    {
        // "Désactiver" les caméras en remettant leur priorité à une valeur basse si elles sont assignées
        if (targetCamera != null)
        {
            targetCamera.Priority = 0;
        }
        if (targetMixingCamera != null)
        {
            targetMixingCamera.Priority = 0;
        }
    }

    public void ResetBlend()
    {
        // Restaurer le réglage de transition original
        //cinemachineBrain.m_DefaultBlend = originalBlend;
        cinemachineBrain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, 2);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Invoke("ActivateCameraWithoutTransition", 0.2f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Invoke("DeactivateCamera", 0.2f);
            Invoke("ResetBlend", 0.3f);
        }
    }
}
