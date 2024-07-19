using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[DefaultExecutionOrder(50)] 
public class EnemySpawnBlock : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPool;

    public float spawnChance = 0.7f;
    private float spawnRng = 0;

    private int rng = 0;

    //after the first frame
    void Update()
    {
        spawnRng = Random.Range(0.0f, 1.0f);
        //print("RNG: " + spawnRng + " CHANCE: " + spawnChance + "TRUE?: " + (spawnRng < spawnChance));
        
        if (spawnRng < spawnChance)
        {
            //print("SPAWNED ENEMY!");
            rng = Random.Range(0,enemyPool.Length);
            Instantiate(enemyPool[rng], this.gameObject.transform.position, this.gameObject.transform.rotation);
        }
        Destroy(this.gameObject);
    }
}
