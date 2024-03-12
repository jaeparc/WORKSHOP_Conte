using Cinemachine;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public CinemachineVirtualCamera targetCamera; // La caméra à activer
    private CinemachineBrain cinemachineBrain;

    void Start()
    {
        // Trouver le CinemachineBrain dans la scène
        cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
    }

    public void ActivateCameraWithoutTransition()
    {
        // Désactiver la transition
        var originalBlend = cinemachineBrain.m_DefaultBlend;
        cinemachineBrain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.Cut, 0);

        // Activer la caméra cible
        targetCamera.Priority = 100; // Assurez-vous que cette priorité est supérieure à celle des autres caméras

        // Restaurer le réglage de transition original si nécessaire
        cinemachineBrain.m_DefaultBlend = originalBlend;
    }

    public void DeactivateCamera()
    {
        // "Désactiver" la caméra en remettant sa priorité à une valeur basse
        // Assurez-vous que cette valeur est inférieure à celle des autres caméras actives
        targetCamera.Priority = 0;
    }
}
