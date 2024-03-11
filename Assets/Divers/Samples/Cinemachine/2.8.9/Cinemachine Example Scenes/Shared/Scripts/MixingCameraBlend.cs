using UnityEngine;

namespace Cinemachine.Examples
{
    [AddComponentMenu("")] // Don't display in add component menu
    public class MixingCameraBlend : MonoBehaviour
    {
        public Transform followTarget;
        public float initialBottomWeight = 20f;

        // Ajoutez vos points d'attraction pour chaque caméra
        public Transform attractionPointCamera0;
        public Transform attractionPointCamera1;

        private CinemachineMixingCamera vcam;

        void Start()
        {
            if (followTarget)
            {
                vcam = GetComponent<CinemachineMixingCamera>();
                // Notez que le poids initial pour m_Weight0 est défini ici, mais vous pourriez vouloir ajuster cela
                // basé sur votre logique spécifique ou la position initiale du joueur
                vcam.m_Weight0 = initialBottomWeight;
            }
        }

        void Update()
        {
            if (followTarget)
            {
                // Calculer les distances
                float distanceToCamera0 = Vector3.Distance(followTarget.position, attractionPointCamera0.position);
                float distanceToCamera1 = Vector3.Distance(followTarget.position, attractionPointCamera1.position);

                // Calculer les poids basés sur la distance
                float weight0 = 1 / (distanceToCamera0 + 1); // Ajoute +1 pour éviter la division par zéro
                float weight1 = 1 / (distanceToCamera1 + 1);

                // Normaliser les poids pour qu'ils s'additionnent à 1
                float totalWeight = weight0 + weight1;
                vcam.m_Weight0 = weight0 / totalWeight;
                vcam.m_Weight1 = weight1 / totalWeight;

                // Optionnel : Log pour le debug
                Debug.Log($"Weight0: {vcam.m_Weight0}, Weight1: {vcam.m_Weight1}, Target Position: {followTarget.transform.position}");
            }
        }
    }
}
