using UnityEngine;
using UnityEngine.AI;

public class PlayerNavMeshController : MonoBehaviour
{
    public float speed = 5.0f;

    private Vector3 moveDirection = Vector3.zero;

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

    private void NavAction()
    {
        NavMeshHit hit;
    }
}



