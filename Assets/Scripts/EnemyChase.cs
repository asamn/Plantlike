using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    private GameObject player;
    private PlayerController playerController;
    [SerializeField] private float speed;
    private float distance;
    private Vector3 attackPoint;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackCooldown = 2.0f;
    private float attackCooldownTimer = 0.0f;
    [SerializeField] private float attackDamage = 5f;

    [SerializeField] private Animator animator;

    public int HP = 5;
   // private float lastAttackTime = 0.0f;
    public LayerMask playerLayer;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player"); //get the player game object
        playerController = player.GetComponent<PlayerController>(); //get the player's controller script
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);
        Vector3 direction = player.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        //Debug.Log(angle);
        transform.rotation = Quaternion.Euler(Vector3.down * angle);

        //set the attack point to a few inches in front of this enemy
        attackPoint = transform.position + (transform.forward * 0.55f);

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

    void Attack(){
        //attack animation
        print("ATTACKED: " + attackPoint);

        
        //Detect player
        
        Collider[] hitPlayer = Physics.OverlapSphere(attackPoint, attackRange, playerLayer);


        //Attack player
        foreach(Collider playerObject in hitPlayer){
            Debug.Log("hit player");
            playerController.TakeDamage(attackDamage);
            break;
        }
        
    }

    public void TakeDamage(int damage)
    {
        animator.SetTrigger("hit"); //trigger the hurt anim
        this.HP -= damage;
        if (this.HP <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        playerController.GainXP(1);
        //death animation?
        Destroy(this.gameObject);
    }

    void OnDrawGizmosSelected(){
        if(attackPoint == null){
            //return;
        }

        Gizmos.DrawWireSphere(attackPoint, attackRange); //this only draws one sphere
    }

}
