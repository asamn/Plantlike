using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // Assign the player's Transform in the Inspector
    public Vector3 offset = new Vector3(0, 50, -10); // Adjust this to get the desired camera angle
    public float smoothTime = 20f; // Lower value for quicker response, higher for smoother follow
    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        // Calculate the desired position for the camera
        Vector3 desiredPosition = player.position + offset;

        // Smoothly interpolate the camera's position towards the desired position
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);

        // Update the camera's position
        transform.position = smoothedPosition;

        // Make the camera always look at the player
        transform.LookAt(player);
    }
}