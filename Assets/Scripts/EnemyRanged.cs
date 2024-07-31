using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged : EnemyChase
{

    [SerializeField] protected GameObject bullet;
    [SerializeField] protected float bulletSpeed;
    protected override void Update()
    {
        if (player != null)
        {
            distance = Vector3.Distance(transform.position, player.transform.position);
            Vector3 direction = player.transform.position - transform.position;
            direction.Normalize();
            float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg - 90;
            //Debug.Log(angle);
            transform.rotation = Quaternion.Euler(Vector3.down * angle);

            //set the attack point to a few inches in front of this enemy
            attackPoint = transform.position + (transform.forward * 0.95f);

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
                if(attackCooldownTimer <= 0){ //if ready to attack...  
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

    protected override void Attack()
    {
        attackSound.Play();
        //attack animation
        //print("ATTACKED: " + attackPoint);
        animator.SetTrigger("attack");

        EnemyProjectile spawnedBullet = Instantiate(bullet, attackPoint, transform.rotation).GetComponent<EnemyProjectile>();
        spawnedBullet.setDamage((int) attackDamage);
        spawnedBullet.setSpeed(bulletSpeed);
        
    }

}
