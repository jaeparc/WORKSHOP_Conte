using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class PlayerNavMeshController : MonoBehaviour
{
    public float speed = 5.0f;
    private Vector3 moveDirection = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        // Use Unity's legacy input system to read horizontal and vertical inputs
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Combine inputs into a movement vector
        moveDirection = new Vector3(horizontal, 0.0f, vertical).normalized;

        // Check if there is significant movement input
        if (moveDirection.sqrMagnitude >= 0.01f) // Use a small threshold to ignore negligible input
        {
            MovePlayer();
        }

        // Check if a specific key is pressed to trigger an action
        if (Input.GetKeyDown(KeyCode.Space)) // You can change KeyCode.Space to any key you'd like to use
        {
            Debug.Log("Check");
            NavAction();
        }
    }

    private void MovePlayer()
    {
        // Calculate the new position based on input direction, speed, and deltaTime
        Vector3 newPosition = transform.position + moveDirection * speed * Time.deltaTime;

        // Optionally, use NavMesh.SamplePosition to ensure the newPosition is on the NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(newPosition, out hit, 1.0f, NavMesh.AllAreas))
        {
            newPosition = hit.position;
        }
        else
        {
            // If there's no valid NavMesh position close to newPosition, don't move
            return;
        }

        // Directly set the player's position to the new position
        transform.position = newPosition;
    }


    void NavAction()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            switch (hit.collider.gameObject.tag)
            {
                case "Walkable": // Make sure the spelling matches the tag exactly.
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




