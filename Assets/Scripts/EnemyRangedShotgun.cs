using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedShotgun : EnemyRanged
{
    [SerializeField] private float angle = 15f;
    private Quaternion adjustedRotation;

    protected override void Attack()
    {
        attackSound.Play();
        //attack animation
        //print("ATTACKED: " + attackPoint);
        animator.SetTrigger("attack");

        adjustedRotation = transform.rotation;
        EnemyProjectile spawnedBullet1 = Instantiate(bullet, attackPoint, adjustedRotation).GetComponent<EnemyProjectile>();
        adjustedRotation = transform.rotation * Quaternion.Euler(0, angle ,0); //shift by angle
        EnemyProjectile spawnedBullet2 = Instantiate(bullet, attackPoint, adjustedRotation).GetComponent<EnemyProjectile>();
        adjustedRotation = transform.rotation * Quaternion.Euler(0, (360f-angle) ,0); //shift by angle
        EnemyProjectile spawnedBullet3 = Instantiate(bullet, attackPoint, adjustedRotation).GetComponent<EnemyProjectile>();
        
        spawnedBullet1.setDamage((int) attackDamage);
        spawnedBullet1.setSpeed(bulletSpeed);
        spawnedBullet2.setDamage((int) attackDamage);
        spawnedBullet2.setSpeed(bulletSpeed);
        spawnedBullet3.setDamage((int) attackDamage);
        spawnedBullet3.setSpeed(bulletSpeed);
    }
}
