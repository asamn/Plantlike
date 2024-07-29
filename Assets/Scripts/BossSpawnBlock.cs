using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[DefaultExecutionOrder(60)] 
public class BossSpawnBlock : MonoBehaviour
{
    [SerializeField] private GameObject[] bossPool;
    private GameObject level;
    private float timer = 1.5f;
    void Start()
    {
        level = GameObject.Find("GeneratedLevel");
    }
    private int rng = 0;

    //after the first frame
    void LateUpdate()
    {   
        timer -= Time.deltaTime; //wait a few moments after the object is spawned
        if( timer <= 0 ) {
            //do not spawn yet if level is still generating
            if (level != null && level.GetComponent<LevelGen>().isGenerated())
            {
                rng = Random.Range(0,bossPool.Length);
                GameObject boss = Instantiate(bossPool[rng], this.gameObject.transform.position, this.gameObject.transform.rotation);
                boss.name = "BossEnemy";
                Destroy(this.gameObject);
            }
        }
    }
}
