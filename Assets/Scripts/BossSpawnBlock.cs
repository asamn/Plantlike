using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(60)] 
public class BossSpawnBlock : MonoBehaviour
{
    [SerializeField] private GameObject[] bossPool;


    private int rng = 0;

    //after the first frame
    void Update()
    {
        rng = Random.Range(0,bossPool.Length);
        GameObject boss = Instantiate(bossPool[rng], this.gameObject.transform.position, this.gameObject.transform.rotation);
        boss.name = "BossEnemy";
        Destroy(this.gameObject);
    }
}
