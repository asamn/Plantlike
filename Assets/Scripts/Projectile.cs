using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float projectileSpeed = 5.5f; //this will be set by the player.cs 
    [SerializeField] protected float timeout = 4.0f;

    protected float damage = 1; //this will be set by the player.cs
    protected bool hitObject = false;

    private GameObject enemy;


    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);

        timeout -= Time.deltaTime;
        if (timeout <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player" && collision.gameObject.tag != "Bullet") //don't destroy if collided with player or itself
        {
            if (collision.gameObject.tag == "Enemy" && !hitObject) //maybe make tags for ranged and melee enemies
            {
                enemy = collision.gameObject;
                enemy.GetComponent<EnemyChase>().TakeDamage(damage);
                hitObject = true;
            }
            Destroy(this.gameObject);
        }        
    }
    public void setDamage (float dmg)
    {
        damage = dmg;
    }
    public void setSpeed (float speed)
    {
        projectileSpeed = speed;
    }
}
