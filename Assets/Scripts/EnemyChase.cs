using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    protected GameObject player, exclamation;
    protected PlayerController playerController;
    [SerializeField] protected float speed;
    [SerializeField, Range(0f,1f)] protected float lootChance;
    [SerializeField] protected GameObject[] lootTablePool;
    [SerializeField] protected GameObject deathEffect; //particle system
    protected float distance;
    protected Vector3 attackPoint;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] protected float aggroRange = 20f;
    [SerializeField] protected float attackCooldown = 2.0f;
    protected float attackCooldownTimer = 0.0f;
    [SerializeField] protected int XPReward = 1;
    [SerializeField] protected float attackDamage = 5f;

    protected Animator animator;

    [SerializeField] protected AudioSource hurtSound,attackSound;

    public float HP = 5;
   // private float lastAttackTime = 0.0f;
    public LayerMask playerLayer;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player"); //get the player game object
        playerController = player.GetComponent<PlayerController>(); //get the player's controller script
        animator = GetComponent<Animator>();
        attackCooldownTimer = attackCooldown;
        exclamation = transform.Find("Exclamation").gameObject;
        
        
        //apply dungeon level scaling
        int currentDungeonLvl = playerController.getDungeonLevel();
        speed = speed * (1 + (currentDungeonLvl - 1) * 0.1f);
        attackDamage = attackDamage * (1 + (currentDungeonLvl - 1) * 0.15f);
        HP = HP *  (int) ((float)currentDungeonLvl * 1.025f);
        XPReward = XPReward *  (int) ((float)currentDungeonLvl * 1.025f);


    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (player != null)
        {
            distance = Vector3.Distance(transform.position, player.transform.position);
            Vector3 direction = player.transform.position - transform.position;
            direction.Normalize();
            float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
            //Debug.Log(angle);
            transform.rotation = Quaternion.Euler(Vector3.down * angle);

            //set the attack point to a few inches in front of this enemy
            attackPoint = transform.position + (transform.forward * 0.55f);

            if (distance <= aggroRange) //if can see player
            {
                exclamation.SetActive(true);
                if (attackCooldownTimer > 0.0f)
                {
                    //decrement the timer 
                    attackCooldownTimer -= Time.deltaTime;
                }
                //Debug.Log(this.gameObject.name + " --- Current time: " + attackCooldownTimer);
                
                //if not within range, move towards player until they are
                if(distance > attackRange * .9){
                    transform.position = Vector3.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
                }
                else if(attackCooldownTimer <= 0){ //if ready to attack...  
                    Attack(); 
                    attackCooldownTimer = attackCooldown; //reset the timer
                }
            }
            else
            {
                exclamation.SetActive(false);
            } 
        }
    }

    protected virtual void Attack(){
        attackSound.Play();
        //attack animation
        print("ATTACKED: " + attackPoint);
        animator.SetTrigger("attack");

        //Detect player
        
        Collider[] hitPlayer = Physics.OverlapSphere(attackPoint, attackRange, playerLayer);


        //Attack player
        foreach(Collider playerObject in hitPlayer){
            Debug.Log("hit player");
            playerController.TakeDamage(attackDamage);
            break;
        }   
    }

    public void TakeDamage(float damage)
    {
        hurtSound.Play();
        animator.SetTrigger("hit"); //trigger the hurt anim
        this.HP -= damage;
        if (this.HP <= 0)
        {
            Die();
        }
    }
    protected virtual void Die()
    {
        playerController.GainXP(XPReward);
        Instantiate(deathEffect, (this.gameObject.transform.position + Vector3.up * 0.55f), this.gameObject.transform.rotation);

        float spawnRng = Random.Range(0.0f, 1.0f);
        if (spawnRng <= lootChance)
        {
            int rng = Random.Range(0,lootTablePool.Length);
            Instantiate(lootTablePool[rng], (new Vector3(gameObject.transform.position.x, player.transform.position.y, gameObject.transform.position.z)), Quaternion.identity);

        }
        Destroy(this.gameObject);
    }

    protected void OnDrawGizmosSelected(){
        if(attackPoint == null){
            //return;
        }

        Gizmos.DrawWireSphere(attackPoint, attackRange); //this only draws one sphere
    }


}
