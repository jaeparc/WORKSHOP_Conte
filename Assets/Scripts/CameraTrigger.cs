using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    public GameObject cameraToManage; // Ajoutez cette ligne pour assigner une caméra spécifique dans l'inspecteur

    private OtherCameraManager cameraManager;

    private void Start()
    {
        // Assurez-vous que cameraManager est toujours trouvé ou assigné correctement
        cameraManager = FindObjectOfType<OtherCameraManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Transmettez la caméra à gérer lors de l'entrée du collider
            cameraManager.EnterCollider(other, cameraToManage);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Transmettez la caméra à gérer lors de la sortie du collider
            cameraManager.ExitCollider(other, cameraToManage);
        }
    }
}

