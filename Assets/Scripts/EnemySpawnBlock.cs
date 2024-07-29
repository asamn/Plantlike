using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[DefaultExecutionOrder(50)] 
public class EnemySpawnBlock : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPool;

    public float spawnChance = 0.7f;
    private float spawnRng = 0;
    private float timer = 0.25f;

    private int rng = 0;

    private GameObject level;
    void Start()
    {
        level = GameObject.Find("GeneratedLevel");
    }

    //after the first frame
    void LateUpdate()
    {
        timer -= Time.deltaTime; //wait a few moments after the object is spawned
        if( timer <= 0 ) {
            //do not spawn yet if level is still generating
            if (level != null && level.GetComponent<LevelGen>().isGenerated())
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
    }
}
