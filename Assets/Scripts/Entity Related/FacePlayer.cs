using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    public Transform player; // Assign the player GameObject in the Inspector
    public float rotationSpeed = 15f; // Speed of rotation

    void Update()
    {
        if (player != null)
        {
            // Calculate the direction to the player
            Vector3 direction = player.position - transform.position;
            direction.y = 0; // Ignore the y-axis to prevent tilting up/down

            // Check if the direction is not zero to avoid errors
            if (direction.magnitude > 0)
            {
                // Calculate the rotation needed to face the player
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                // Smoothly rotate towards the player
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}
