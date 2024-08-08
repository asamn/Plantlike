using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChase : EnemyChase
{
    [SerializeField] private GameObject nextLevelPortal;
    // Inherits the same code as EnemyChase
    protected override void Die()
    {
        isDead = true;
        playerController.GainXP(XPReward);
        Instantiate(deathEffect, (this.gameObject.transform.position + Vector3.up * 0.75f), this.gameObject.transform.rotation);

        float spawnRng = Random.Range(0.0f, 1.0f);
        if (spawnRng <= lootChance)
        {
            int rng = Random.Range(0,lootTablePool.Length);
            Instantiate(lootTablePool[rng], (new Vector3(gameObject.transform.position.x, player.transform.position.y, gameObject.transform.position.z) + transform.forward * 1.50f), Quaternion.identity);
        }


        Instantiate(nextLevelPortal, (new Vector3(gameObject.transform.position.x, 0.55f, gameObject.transform.position.z)), Quaternion.LookRotation(Vector3.down, Vector3.up) );
        Destroy(this.gameObject);
    }
}
