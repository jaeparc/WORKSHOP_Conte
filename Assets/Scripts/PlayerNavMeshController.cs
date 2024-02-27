using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class PlayerNavMeshController : MonoBehaviour
{
    public float speed = 5.0f;
    private Vector3 moveDirection = Vector3.zero;

    
    void Update()
    {
        
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        
        moveDirection = new Vector3(horizontal, 0.0f, vertical).normalized;
        if (moveDirection.sqrMagnitude >= 0.01f) 
        {
            MovePlayer();
        }
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            Debug.Log("Check");
            NavAction();
        }
    }

    private void MovePlayer()
    {
        Vector3 newPosition = transform.position + moveDirection * speed * Time.deltaTime;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(newPosition, out hit, 1.0f, NavMesh.AllAreas))
        {
            newPosition = hit.position;
        }
        else
        {
            return;
        }
        transform.position = newPosition;
    }


    void NavAction()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            switch (hit.collider.gameObject.tag)
            {
                case "Walkable":
                    Debug.Log("You can walk here");
                    break;
                case "Jump":
                    Debug.Log("You can Jump here");
                    break;
                default:
                    Debug.Log("Area not recognized");
                    break;
            }
        }
    }

}




