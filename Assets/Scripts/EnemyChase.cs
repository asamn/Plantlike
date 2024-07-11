using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : MonoBehaviour
{

    [SerializeField] GameObject player;
    [SerializeField] float speed;
    private float distance;
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRange = 1f;
    [SerializeField] float attackCooldown = 2.0f;
    [SerializeField] float attackDamage = 5f;
    private float lastAttackTime = 0.0f;
    public LayerMask playerLayer;

    // Start is called before the first frame update
    void Start()
    {
        
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
        

        if(distance > attackRange * .9){
            transform.position = Vector3.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        }

        if(Time.time >= lastAttackTime + attackCooldown){
            //Debug.Log(this.gameObject.name + " --- Current time: " + Time.time + " --- Last attack: " + lastAttackTime + " --- Cooldown: " + attackCooldown);
            Attack();
        }

    }

    void Attack(){
        //attack animation

        //Detect player
        Collider[] hitPlayer = Physics.OverlapSphere(attackPoint.position, attackRange, playerLayer);


        //Attack player
        foreach(Collider playerObject in hitPlayer){
            Debug.Log("hit player");
            playerObject.GetComponent<PlayerController>().TakeDamage(attackDamage);
            lastAttackTime = Time.time;
        }
        
    }

    void OnDrawGizmosSelected(){
        if(attackPoint == null){
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
