using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoleManager : MonoBehaviour
{
    public GameObject player;

    public float MaxHeight, MinHeight;

    public bool hasPlayer;

    private void Start()
    {
        hasPlayer = false;
    }
    private void Update()
    {
        if (!hasPlayer)
        {
            transform.position = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
        }

        if (hasPlayer)
        {
            PlayerMove();
        }
    }


    void PlayerMove()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = (transform.up * moveZ * Time.deltaTime);

        transform.position += move;
    }

}
