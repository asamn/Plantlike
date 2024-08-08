using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    East,West,North,South
}
//[DefaultExecutionOrder(1)] //make sure this runs first
public class LevelGen : MonoBehaviour
{
    [SerializeField] private GameObject startingRoomPrefab;

    [SerializeField] private GameObject[] roomPool;
    [SerializeField] private GameObject bossRoom;
    private int roomCount;
    private int currentRoomCount;
    //separate pools for east to west, north to south
    [SerializeField] private GameObject[] NSConnectorPool, EWConnectorPool;

    private List<GameObject> markerList; 

    private bool levelGenerated = false;
    

    void Awake()
    {
        GameObject userOptions = GameObject.Find("SavedUserOptions");

        if (userOptions != null)
        {
            roomCount = userOptions.GetComponent<SavedUserOptions>().getWorldSize();
        }
        else
        {
            roomCount = 3;
        }
        levelGenerated = false;
        GenerateLevel();
    }

    public void GenerateLevel()
    {
        //clear enemies
    
        GameObject[] e = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject g in e)
        {
            Destroy(g);
        }

        GameObject[] p = GameObject.FindGameObjectsWithTag("Pickup");
        foreach(GameObject g in p)
        {
            Destroy(g);
        }
        
        levelGenerated = false;
        currentRoomCount = roomCount;
        markerList = new List<GameObject>();

        //place the starting room
        GameObject startingRoom = Instantiate(startingRoomPrefab, new Vector3(0,0,0), Quaternion.identity);
        
        startingRoom.transform.parent = gameObject.transform;

        //the markers of each room should be stored in layer 2 
        GameObject layer2 = startingRoom.transform.Find("Layer2").gameObject; 

        //add the starting room markers to the starting list
        for (int i = 0; i < layer2.transform.childCount; i++)
        {   
            GameObject currentChild = layer2.transform.GetChild(i).gameObject;
            //print(currentTag);
            switch(currentChild.tag) 
            {
            case "North":
                markerList.Add(currentChild);
                break;
            case "East":
                markerList.Add(currentChild);
                break;
            case "South":
                markerList.Add(currentChild);
                break;
            case "West":
                markerList.Add(currentChild);
                break;
            default:
                //don't add this to the queue
                break;
            }
        }

        while (markerList.Count > 0)
        {
            //process a random marker, upper bound of random.range is exclusive for ints
            int rng = Random.Range(0,markerList.Count);
            GameObject currentMarker = markerList[rng];
            markerList.RemoveAt(rng);

            Marker m = currentMarker.GetComponent<Marker>();
            

            if (currentRoomCount <= 0)
            {
                if (!m.isVisited())
                {
                    //do nothing, just fill the marker's wall
                    print("Max room count reached ");
                    m.fillWall();
                    
                }
                continue;
                
            }
            m.setVisited();

            //connectors do not count towards roomCount
            currentRoomCount--;

            GameObject currentConnector = null;
            GameObject currentRoom = null;
            GameObject connectorMarker = null;


            //random connector, make sure there is an equal amount of NS and EW connectors
            rng = Random.Range(0,NSConnectorPool.Length);
            
            Vector3 spawnPosition = currentMarker.transform.position;

            //place connectors, NORTH IS Z POSITIVE, EAST IS X POSITIVE
            if (m.getDir() == Direction.North)
            {
                currentConnector = Instantiate(NSConnectorPool[rng], currentMarker.transform.position, currentMarker.transform.rotation);
                spawnPosition.z += currentMarker.transform.position.z - currentConnector.GetComponent<Room>().GetSouthMarker().transform.position.z - 1; //adjust 
                //make sure the rooms align
                spawnPosition.x += currentMarker.transform.position.x - currentConnector.GetComponent<Room>().GetSouthMarker().transform.position.x;

                connectorMarker = currentConnector.GetComponent<Room>().GetNorthMarker();

            }
            else if (m.getDir() == Direction.East)
            {
                currentConnector = Instantiate(EWConnectorPool[rng], currentMarker.transform.position, currentMarker.transform.rotation);
                spawnPosition.x += currentMarker.transform.position.x - currentConnector.GetComponent<Room>().GetWestMarker().transform.position.x - 1; //adjust 
                //make sure the rooms align
                spawnPosition.z += currentMarker.transform.position.z - currentConnector.GetComponent<Room>().GetWestMarker().transform.position.z;

                connectorMarker = currentConnector.GetComponent<Room>().GetEastMarker();

            }
            else if (m.getDir() == Direction.South)
            {
                currentConnector = Instantiate(NSConnectorPool[rng], currentMarker.transform.position, currentMarker.transform.rotation);
                spawnPosition.z += currentMarker.transform.position.z - currentConnector.GetComponent<Room>().GetNorthMarker().transform.position.z + 1; //adjust 
                //make sure the rooms align
                spawnPosition.x += currentMarker.transform.position.x - currentConnector.GetComponent<Room>().GetNorthMarker().transform.position.x;

                connectorMarker = currentConnector.GetComponent<Room>().GetSouthMarker();

            }
            else if (m.getDir() == Direction.West)
            {
                currentConnector = Instantiate(EWConnectorPool[rng], currentMarker.transform.position, currentMarker.transform.rotation);
                spawnPosition.x += currentMarker.transform.position.x - currentConnector.GetComponent<Room>().GetEastMarker().transform.position.x + 1; //adjust 
                //make sure the rooms align
                spawnPosition.z += currentMarker.transform.position.z - currentConnector.GetComponent<Room>().GetWestMarker().transform.position.z;
                
                connectorMarker = currentConnector.GetComponent<Room>().GetWestMarker();
            }
            else
            {
                print("ERROR! DIRECTION NOT FOUND!");
                break;
            }
            
            //currentConnector.name = "Connector: " + markerList.Count;

            if(currentConnector != null)
            {
                currentConnector.transform.position = spawnPosition;
                currentConnector.transform.parent = gameObject.transform;
            }
            else
            {
                print("ERROR! CURRENT CONNECTOR IS NULL!");
                break;
            }

            //very important, otherwise physics system may not properly recognize overlaps
            //fix source: https://forum.unity.com/threads/solved-strange-behavior-with-overlapbox.833299/ by Baste
            Physics.SyncTransforms();
            
            //Check overlap of the spawned connector
            if (currentConnector.GetComponent<Room>().isOverlapping())
            {
                //print("OVERLAP! CONNECTOR");
                currentRoomCount++;
                DestroyPiece(currentConnector, m);
                continue; //do not run below code, 
            }

            //start spawning the room
            if (currentRoomCount == 0) //spawn the boss room last
            {
                //print("Spawning boss room... ");
                currentRoom = Instantiate(bossRoom, connectorMarker.transform.position, connectorMarker.transform.rotation);
                currentRoom.name = "BossRoom";
            }
            else
            {
                rng = Random.Range(0, roomPool.Length);
                currentRoom = Instantiate(roomPool[rng], connectorMarker.transform.position, connectorMarker.transform.rotation);
            }
            
            spawnPosition = connectorMarker.transform.position;
            
            switch(connectorMarker.tag) 
            {
            case "North":
                if (currentRoom.GetComponent<Room>().GetSouthMarker() == null) //if the room trying to spawn doesn't have a south connection point, don't bother spawning it, it will count as an overlap
                {
                    currentRoomCount++;
                    DestroyPiece(currentRoom, m);
                    DestroyPiece(currentConnector, m);
                    continue; //dont run below codes
                }
                spawnPosition.z += connectorMarker.transform.position.z - currentRoom.GetComponent<Room>().GetSouthMarker().transform.position.z - 1; //adjust 
                //make sure the rooms align
                spawnPosition.x += connectorMarker.transform.position.x - currentRoom.GetComponent<Room>().GetSouthMarker().transform.position.x;
                break;
            case "East":
                if (currentRoom.GetComponent<Room>().GetWestMarker() == null)
                {
                    currentRoomCount++;
                    DestroyPiece(currentRoom, m);
                    DestroyPiece(currentConnector, m);
                    continue;
                }
                spawnPosition.x += connectorMarker.transform.position.x - currentRoom.GetComponent<Room>().GetWestMarker().transform.position.x - 1; //adjust 
                //make sure the rooms align
                spawnPosition.z += connectorMarker.transform.position.z - currentRoom.GetComponent<Room>().GetWestMarker().transform.position.z;
                break;
            case "South":
                if (currentRoom.GetComponent<Room>().GetNorthMarker() == null)
                {
                    currentRoomCount++;
                    DestroyPiece(currentRoom, m);
                    DestroyPiece(currentConnector, m);
                    continue;
                }
                spawnPosition.z += connectorMarker.transform.position.z - currentRoom.GetComponent<Room>().GetNorthMarker().transform.position.z + 1; //adjust 
                //make sure the rooms align
                spawnPosition.x += connectorMarker.transform.position.x - currentRoom.GetComponent<Room>().GetNorthMarker().transform.position.x;
                break;
            case "West":
                if (currentRoom.GetComponent<Room>().GetEastMarker() == null)
                {
                    currentRoomCount++;
                    DestroyPiece(currentRoom, m);
                    DestroyPiece(currentConnector, m);
                    continue;
                }
                spawnPosition.x += connectorMarker.transform.position.x - currentRoom.GetComponent<Room>().GetEastMarker().transform.position.x + 1; //adjust 
                //make sure the rooms align
                spawnPosition.z += connectorMarker.transform.position.z - currentRoom.GetComponent<Room>().GetEastMarker().transform.position.z;
                break;
            default:
                //don't add this to the queue
                break;
            }

            if (currentRoom != null)
            {
                currentRoom.transform.position = spawnPosition;
                currentRoom.transform.parent = gameObject.transform;
            }
            else
            {
                print("ERROR! CURRENT ROOM IS NULL!");
                break;
            }

            Physics.SyncTransforms(); //important

            //Check overlap of the spawned room, also destroy the connector 
            if (currentRoom.GetComponent<Room>().isOverlapping())
            {
                currentRoomCount++;
                //print("OVERLAP!");
                DestroyPiece(currentRoom, m);
                DestroyPiece(currentConnector, m);
                continue; //do not run below code, 
            }

            //on success, add the room's markers to the process list
            //don't add the room's already proccessed side 
            if(connectorMarker.tag != "South" && currentRoom.GetComponent<Room>().GetNorthMarker() != null)
            {
                markerList.Add(currentRoom.GetComponent<Room>().GetNorthMarker());   
            }
            if(connectorMarker.tag != "North" && currentRoom.GetComponent<Room>().GetSouthMarker() != null)
            {
                markerList.Add(currentRoom.GetComponent<Room>().GetSouthMarker());
            }
            if(connectorMarker.tag != "West" && currentRoom.GetComponent<Room>().GetEastMarker() != null)
            {
                markerList.Add(currentRoom.GetComponent<Room>().GetEastMarker());
            }
            if(connectorMarker.tag != "East" && currentRoom.GetComponent<Room>().GetWestMarker() != null)
            {
                markerList.Add(currentRoom.GetComponent<Room>().GetWestMarker());   
            }
        }

        if (GameObject.Find("BossRoom") == null) 
        {
            levelGenerated = false;
            //retry generating the level
            print("LEVEL GEN FAILED");
            clearLevel();
            GenerateLevel();
        }
        else
        {
            levelGenerated = true;
            print("Generated level of size: " + roomCount);
        }
        
        
        
    }

    //o is the room piece, m is the current marker it is trying to spawn at
    void DestroyPiece(GameObject o, Marker m)
    {
        m.fillWall();
        Destroy(o);
    }

    public bool isGenerated()
    {
        return levelGenerated;
    }

    public void clearLevel()
    {
        GameObject[] roomsToClear = new GameObject[transform.childCount]; //save the starting room

        int i = 0;        
        foreach (Transform c in transform)
        {
            roomsToClear[i] = c.gameObject;
            i++;
        }

        foreach (GameObject o in roomsToClear)
        {

            Destroy(o.gameObject);
    
        }
        currentRoomCount = roomCount;
        markerList.Clear();
    }
}
