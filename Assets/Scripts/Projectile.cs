using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float projectileSpeed = 5.5f;

    public int damage = 1;
    private bool hitObject = false;

    private GameObject enemy;


    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);

        //maybe add a timer to de-spawn bullets that somehow clip out the map and travel endlessly  
    }

    void OnCollisionEnter(Collision collision)
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
}
