using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelGen : MonoBehaviour
{
    [SerializeField] private Tilemap tiles;
    public GameObject terrainBlock;
    // Start is called before the first frame update
    void Start()
    {
        //generate a simple 50x50 square
        for (int x = 0; x < 50; x++)
        {
            for (int y=0 ; y < 50; y++)
            {
                //var newGameObject = new GameObject(); // Replace this with your actual GameObject instance
                //newGameObject = terrainBlock;
                ///newGameObject.transform.position = tiles.CellToWorld(gridCellInt);
            }
            

        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
