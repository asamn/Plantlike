using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{

    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float rotationSpeed = 5.0f; // Rotation speed multiplier

    [SerializeField] private GameObject bullet;
    public bool useGamepadControls = true; // Boolean for controlling input method
    


    private Rigidbody rb;
    private Vector3 movement;
    private Camera mainCamera;

    private Quaternion playerRotation;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        playerRotation.x = 0;
        playerRotation.z = 0;
        playerRotation = playerRotation.normalized;
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        movement = new Vector3(horizontal, 0, vertical);
        movement = movement.normalized * playerSpeed * Time.fixedDeltaTime;

        // Aiming input
        Vector3 aimDirection = Vector3.zero;
        if (useGamepadControls)
        {
            float aimHorizontal = Input.GetAxis("Right Stick Horizontal");
            float aimVertical = Input.GetAxis("Right Stick Vertical");
            aimDirection = new Vector3(aimHorizontal, 0.0f, aimVertical).normalized;
        }
        else
        {
            // Use mouse input for aiming
            Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            float rayLength;

            if (groundPlane.Raycast(cameraRay, out rayLength))
            {
                Vector3 pointToLook = cameraRay.GetPoint(rayLength);
                aimDirection = pointToLook - transform.position;
                aimDirection.y = 0; // Keep aim direction parallel to ground
            }
        }

        // Ensure there is a significant input before updating rotation.
        if (aimDirection.sqrMagnitude > 0.01f)  // Using a small threshold instead of dead zone
        {
            // Calculate the target rotation based on the aim direction
            Quaternion targetRotation = Quaternion.LookRotation(aimDirection);
            targetRotation.x = 0;
            targetRotation.z = 0;


            // Smoothly interpolate towards the target rotation
             playerRotation = Quaternion.Lerp(rb.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }


         if (Input.GetMouseButtonDown(0)) //LMB
         {
            fireProjectile();
         }

    }

    //fixed update is better for applying physics calculations
    void FixedUpdate()
    {
        //apply movement
        rb.MovePosition(transform.position + movement);
        rb.MoveRotation(playerRotation);
       
    }

    void fireProjectile()
    {
        Instantiate(bullet, transform.position + transform.TransformDirection(Vector3.forward * 1.5f) , transform.rotation);
    }

}
