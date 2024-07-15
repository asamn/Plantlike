using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float playerSpeed = 2f;
    [SerializeField] private float rotationSpeed = 5.0f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private bool useGamepad = true; // Control gamepad vs keyboard/mouse

    //public float iFrameCooldown = 1f; //how many invincibility frames?
    //private float iFrameCooldownTimer;

    public float maxHealth = 100f;
    private float currentHealth;
    public HealthBar healthBar;

    public float maxXP = 100f;
    private float currentXP;
    public XPBar xpBar;

    private Rigidbody rb;
    private Vector3 movement;
    private Vector3 aimDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Prevent rotation affecting movement
        rb.interpolation = RigidbodyInterpolation.Interpolate; // Smoothing the movement
        
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        currentXP = 0f;
        xpBar.SetMaxXP(maxXP);
    }

    void Update()
    {
        HandleInput();
    }

    void FixedUpdate()
    {
        ApplyMovement();
        ApplyRotation();
    }

    private void HandleInput()
    {
        if (useGamepad)
        {
            HandleGamepadInput();
            if (Input.GetButtonDown("FireGamepad")) // Use right bumper for gamepad fire button
            {
                // Debug.Log("Right Bumper Pressed"); // Debug log
                FireProjectile();
            }
        }
        else
        {
            HandleKeyboardMouseInput();
            if (Input.GetMouseButtonDown(0)) // Left mouse button for firing
            {
                FireProjectile();
            }
        }
    }

    private void HandleGamepadInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Implementing a dead zone cuz apparently thats what you do
        if (Mathf.Abs(horizontal) < 0.1f) horizontal = 0;
        if (Mathf.Abs(vertical) < 0.1f) vertical = 0;

        movement = new Vector3(horizontal, 0, vertical);
        if (movement.sqrMagnitude > 1)
        {
            movement.Normalize();
        }
        movement *= playerSpeed;

        float aimHorizontal = Input.GetAxis("Right Stick Horizontal");
        float aimVertical = Input.GetAxis("Right Stick Vertical");

        // Implementing a dead zone for aiming
        if (Mathf.Abs(aimHorizontal) < 0.1f) aimHorizontal = 0;
        if (Mathf.Abs(aimVertical) < 0.1f) aimVertical = 0;

        // Inverting the vertical axis for aiming
        aimDirection = new Vector3(aimHorizontal, 0, -aimVertical);
    }

    private void HandleKeyboardMouseInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        movement = new Vector3(horizontal, 0, vertical);
        if (movement.sqrMagnitude > 1)
        {
            movement.Normalize();
        }
        movement *= playerSpeed;

        // Mouse aiming
        Plane playerPlane = new Plane(Vector3.up, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float hitDist = 0.0f;

        if (playerPlane.Raycast(ray, out hitDist))
        {
            Vector3 targetPoint = ray.GetPoint(hitDist);
            aimDirection = targetPoint - transform.position;
            aimDirection.y = 0; // Keep aiming on the horizontal plane
        }
    }

    private void ApplyMovement(){
        
        rb.velocity = movement;
        //movement = movement.normalized * playerSpeed * Time.fixedDeltaTime;

/*        if(Input.GetKeyDown(KeyCode.Space)){
            GainXP(17);
       } */
    }

    //2 fixed updates after merge?
    //void FixedUpdate()
//{
     //   rb.velocity = movement;
    //}


    private void ApplyRotation()
    {
        if (aimDirection.sqrMagnitude > 0.01f) // There is significant input from the right stick or mouse aiming
        {
            Quaternion targetRotation = Quaternion.LookRotation(aimDirection);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
        else if (movement.sqrMagnitude > 0.01f) // No aiming input, use movement direction
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    private void FireProjectile()
    {
        if (bulletPrefab && bulletSpawnPoint)
        {
            Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        }
        else if (bulletPrefab)
        {
            Instantiate(bulletPrefab, transform.position + transform.forward * 1.5f, transform.rotation);
        }
    }

    public void GainXP(float xpGained){
        currentXP += xpGained;

        if(currentXP >= maxXP){
            currentXP -= maxXP;
            Debug.Log("Level Up!");
        }

        xpBar.SetXP(currentXP);
        Debug.Log("Gained " + xpGained + " XP");
    }

    public void TakeDamage(float amount){
        Debug.Log(currentHealth + " " + amount);
        currentHealth -= amount;

        Debug.Log("Damage taken! Current health = " + currentHealth);

        if(currentHealth <= 0){
            currentHealth = 0;
            Die();
        }

        healthBar.SetHealth(currentHealth);
    }

    void Die(){
        Debug.Log("Player died! ");
        Destroy(this.gameObject);
    }

}

