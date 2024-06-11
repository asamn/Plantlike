using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float projectileSpeed = 5.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag != "Player" && collision.gameObject.tag != "Bullet") //don't destroy if collided with player or itself
        {
            if (collision.gameObject.tag == "Enemy")
            {
                Destroy(collision.gameObject);
            }

            Destroy(this.gameObject);

        }

        
    }
}
