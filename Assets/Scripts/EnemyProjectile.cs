using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Projectile
{
    private GameObject player;
    
    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Enemy" && collision.gameObject.tag != "Bullet") 
        {
            if (collision.gameObject.tag == "Player" && !hitObject) //maybe make tags for ranged and melee enemies
            {
                player = collision.gameObject;
                player.GetComponent<PlayerController>().TakeDamage((float)damage);
                hitObject = true;
            }
            Destroy(this.gameObject);
        }        
    }
}
