using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour
{

    [SerializeField] private Direction dir;
    [SerializeField]private bool visited;

    public Direction getDir()
    {
        return this.dir;
    }

    public void setVisited()
    {
        this.visited = true;
    }

    public bool isVisited()
    {
        return this.visited;
    }

    //fill the wall attached to this marker
    public void fillWall()
    {   
        //set the wall active, set the grass inactive, make sure the order is correct in the hierachy 
        this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        this.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        this.visited = true;
    }

    void Awake()
    {
        this.visited = false;
        switch(this.gameObject.tag) 
        {
            case "North":
                this.dir = Direction.North;
                break;
            case "East":
                this.dir = Direction.East;
                break;
            case "South":
                this.dir = Direction.South;
                break;
            case "West":
                this.dir = Direction.West;
                break;
            default:
                break;
        }
    }
}
