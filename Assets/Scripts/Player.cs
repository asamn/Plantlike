using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float playerSpeed = 2f;
    private float speedCap = 15f;
    [SerializeField] private float rotationSpeed = 5.0f;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private bool useGamepad = false; // Control gamepad vs keyboard/mouse
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private GameObject gm, playerModel, corpse, bulletPrefab;
    private AudioManager am;
    private Animator animator;
    [SerializeField] private string playerClass;


    private int level = 1;
    //public float iFrameCooldown = 1f; //how many invincibility frames?
    //private float iFrameCooldownTimer;
    public float maxHealth = 100f;
    [SerializeField]private float currentHealth;
    public HealthBar healthBar;

    public float damage = 1f;
    private int dungeonLvl = 1;
    public float bulletSpeed = 5.0f;
    
    public float maxXP = 100f;
    private float currentXP;
    public XPBar xpBar;
    private Rigidbody rb;
    private Vector3 movement, aimDirection;
    private bool isDead;
    [SerializeField] private ClassDropdown classDropdown;
    private float nextFireTime = 0f;
    private float fireRate = 1f;
    private int projectileCount;
    private float spreadAngle = 0f;

    void Awake()
    {
        am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        useGamepad = false;
        StartCoroutine(CheckGamepad());
    }
    void Start()
    {
        isDead = false;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Prevent rotation affecting movement
        rb.interpolation = RigidbodyInterpolation.Interpolate; // Smoothing the movement
        
        animator = playerModel.GetComponent<Animator>();

        animator.SetBool("isMoving",false);

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        currentXP = 0f;
        xpBar.SetMaxXP(maxXP);
        dungeonLvl = 1;
        classDropdown = FindObjectOfType<ClassDropdown>();
        playerClass = (classDropdown == null) ? "Sharpshooter" : classDropdown.GetCharacterClass();
        Debug.Log("Player Class: " + playerClass);
        levelText.text = (playerClass + " LVL: " + level);
        SetClassVariables();
    }

    //co-routine function, check for gamepads in the background
    IEnumerator CheckGamepad() {
        while (true) {
            var controllers = Input.GetJoystickNames();

            if (!useGamepad && controllers.Length > 0) {
                useGamepad = true;
            
            } else if (useGamepad && controllers.Length == 0) {         
                useGamepad = false;
            }
            yield return new WaitForSeconds(1.5f);
        }
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

    private void FireProjectile(int numberOfProjectiles){
        if (bulletPrefab && bulletSpawnPoint && Time.time >= nextFireTime)
        {
            for (int i = 0; i < numberOfProjectiles; i++){
            // Generate a random spread direction
            float spreadX = Random.Range(-spreadAngle, spreadAngle);
            float spreadY = Random.Range(-spreadAngle, spreadAngle);
            float spreadZ = Random.Range(-spreadAngle, spreadAngle);

            // Create a new rotation with added spread
            Quaternion spreadRotation = Quaternion.Euler(
                bulletSpawnPoint.rotation.eulerAngles.x,
                bulletSpawnPoint.rotation.eulerAngles.y + spreadY,
                bulletSpawnPoint.rotation.eulerAngles.z + spreadZ
            );

            // Instantiate the bullet with the spread rotation
            Projectile bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, spreadRotation).GetComponent<Projectile>();
            bullet.setDamage(damage);
            bullet.setSpeed(bulletSpeed); 
            }
            nextFireTime = Time.time + fireRate;
        }
    }

    public void GainXP(float xpGained){
        currentXP += xpGained;

        if(currentXP >= maxXP){
            currentXP = 0;
            level++;
            Debug.Log("Level Up!");
            healHealth(level * 10);
            am.PlayLevelUp();
            levelText.text = (playerClass + " LVL: " + level);
        }

        xpBar.SetXP(currentXP);
        Debug.Log("Gained " + xpGained + " XP");
    }

    public void TakeDamage(float amount){
        Debug.Log(currentHealth + " " + amount);

        //am.PlayHurt();

        currentHealth -= amount;

        Debug.Log("Damage taken! Current health = " + currentHealth);

        if(currentHealth <= 0){
            currentHealth = 0;
            if (!isDead)
            {
                Die();
            }
            
        }

        healthBar.SetHealth(currentHealth);
        am.PlayHurt();
    }
    void Die(){
        Debug.Log("Player died! ");
        isDead = true;
        gm.GetComponent<GameManager>().ShowDeathScreen();
        am.StopMusic();
        am.StopAmbience();

        Instantiate(corpse, transform.position + (Vector3.up * 0.05f), transform.rotation);

        Destroy(this.gameObject);
    }

    public void increaseSpeed(float amount)
    {
        if (playerSpeed > speedCap) //prevent the player from obtaining a speeding ticket, heal instead
        {
            healHealth(10 * level);
        }
        else
        {
            playerSpeed += amount;
        }
        
    }
    public void increaseBulletSpeed (float amount)
    {
        bulletSpeed += amount;
    }
    public void increaseMaxHealth(float amount)
    {
        maxHealth += amount;
        healthBar.SetMaxHealth(maxHealth);
        healHealth(amount * 0.55f);

    }
    public void healHealth (float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) //overheal
        {
            currentHealth = maxHealth;
        }
        healthBar.SetHealth(currentHealth);
    }
    public void increaseDamage(int amount)
    {
        damage += amount;
    }
    public void increaseFireRate(float amount)
    {
        fireRate -= amount;
        if (fireRate < 0.01f)
        {
            fireRate = 0.01f; //cap
            healHealth(10 * level); //heal instead

        }
    }
    public void increaseDungeonLevel()
    {
        dungeonLvl++;
    }
    public int getDungeonLevel()
    {
        return dungeonLvl;  
    }
    public void resetPosition()
    {
        transform.position = new Vector3(-1f, 0.75f,-6f);
    }

     private void SetClassVariables(){
        if(playerClass == "Sharpshooter"){
            fireRate = .9f;
            damage = 2.4f;
            bulletSpeed = 10f;
            projectileCount = 1;
        }
        else if(playerClass == "Shredder"){
            fireRate = 1.25f;
            damage = .8f;
            bulletSpeed = 2f;
            spreadAngle = 40f;
            projectileCount = 8;

        }
        else if(playerClass == "Sprayer"){
            fireRate = .5f;
            damage = 1.2f;
            bulletSpeed = 3f;
            projectileCount = 1;
        }
    }
    //======================INPUT HANDLING====================================
    //========================================================================
    private void HandleInput()
    {
        if (useGamepad)
        {
            HandleGamepadInput();
            if (Input.GetButton("FireGamepad")) // Use right bumper for gamepad fire button
            {
                // Debug.Log("Right Bumper Pressed"); // Debug log
                FireProjectile(projectileCount);
            }
        }
        else
        {
            HandleKeyboardMouseInput();
            if (Input.GetMouseButton(0)) // Left mouse button for firing
            {
                FireProjectile(projectileCount);
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


        if (horizontal != 0 || vertical != 0)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }


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

        if (horizontal != 0 || vertical != 0)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

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

    private void ApplyMovement()
    {    
        rb.velocity = movement;
    }

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

   

}


