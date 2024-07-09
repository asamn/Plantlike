using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawnBlock : MonoBehaviour
{
    [SerializeField] private GameObject[] bossPool;


    private int rng = 0;

    //after the first frame
    void Update()
    {
        rng = Random.Range(0,bossPool.Length);
        Instantiate(bossPool[rng], this.gameObject.transform.position, this.gameObject.transform.rotation);
        Destroy(this.gameObject);
    }
}
