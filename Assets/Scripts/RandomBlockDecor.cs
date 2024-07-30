using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBlockDecor : MonoBehaviour
{
    public float spawnChance = 0.7f;
    private int rng = 0;
    private float spawnRng = 0;

    [SerializeField] private GameObject[] decorations;
    // Start is called before the first frame update
    void Start()
    {
        spawnRng = Random.Range(0.0f, 1.0f);

        if (spawnRng < spawnChance)
        {
            rng = Random.Range(0,decorations.Length);
            decorations[rng].SetActive(true);
        }


    }

}
