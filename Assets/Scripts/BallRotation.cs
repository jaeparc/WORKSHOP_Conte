using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class BallRotation : MonoBehaviour
{
    public GameObject Player, Pilier;
    public Vector3 Debut, Fin;
    public float Parcour;
    private float initialAngle = 0f;

    private void Update()
    {
        CheckPlayer();
    }

    public void CheckPlayer()
    {
        Parcour = Player.transform.position.y/30;

        transform.position = new Vector3(transform.position.x, Parcour * 30 + 2, transform.position.z);

        float angle = 360f * -Parcour;
        float angleDifference = angle - initialAngle;
        initialAngle = angle;
        transform.RotateAround(Pilier.transform.position, Vector3.up, angleDifference);
    }
}
