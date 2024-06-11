using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // Assign the player's Transform in the Inspector
    public Vector3 offset = new Vector3(0, 10, -10); // Adjust this to get the desired camera angle
    public float smoothSpeed = 5.0f;

    void LateUpdate()
    {
        // Calculate the desired position for the camera
        Vector3 desiredPosition = player.position + offset;

        // Smoothly interpolate the camera's position towards the desired position - kinda jank, unsure how this makes me feel.
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        
        // Update the camera's position
        transform.position = smoothedPosition;

        // Make the camera always look at the player
        transform.LookAt(player);
    }
}