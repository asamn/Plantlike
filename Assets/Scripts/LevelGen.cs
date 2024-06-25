using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    East,West,North,South
}

public class LevelGen : MonoBehaviour
{

    [SerializeField] private GameObject startingRoom;

    [SerializeField] private GameObject[] roomPool;
    [SerializeField] private int roomCount;
    //separate pools for east to west, north to south
    [SerializeField] private GameObject[] NSConnectorPool, EWConnectorPool;

    private List<GameObject> markerList; 

    // Start is called before the first frame update
    void Start()
    {
        markerList = new List<GameObject>();
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
            m.setVisited();

            if (roomCount <= 0)
            {
                //do nothing, just fill the marker's wall
                m.fillWall();
                continue;
            }

            //connectors do not count towards roomCount
            roomCount--;

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
                print("OVERLAP! CONNECTOR");
                m.fillWall();
                roomCount++; //retry the room
                Destroy(currentConnector);
                continue; //do not run below code, 
            }

            //start spawning the room
            rng = Random.Range(0, roomPool.Length);
            spawnPosition = connectorMarker.transform.position;
            currentRoom = Instantiate(roomPool[rng], connectorMarker.transform.position, connectorMarker.transform.rotation);
            
            switch(connectorMarker.tag) 
            {
            case "North":
                spawnPosition.z += connectorMarker.transform.position.z - currentRoom.GetComponent<Room>().GetSouthMarker().transform.position.z - 1; //adjust 
                //make sure the rooms align
                spawnPosition.x += connectorMarker.transform.position.x - currentRoom.GetComponent<Room>().GetSouthMarker().transform.position.x;
                break;
            case "East":
                spawnPosition.x += connectorMarker.transform.position.x - currentRoom.GetComponent<Room>().GetWestMarker().transform.position.x - 1; //adjust 
                //make sure the rooms align
                spawnPosition.z += connectorMarker.transform.position.z - currentRoom.GetComponent<Room>().GetWestMarker().transform.position.z;
                break;
            case "South":
                spawnPosition.z += connectorMarker.transform.position.z - currentRoom.GetComponent<Room>().GetNorthMarker().transform.position.z + 1; //adjust 
                //make sure the rooms align
                spawnPosition.x += connectorMarker.transform.position.x - currentRoom.GetComponent<Room>().GetNorthMarker().transform.position.x;
                break;
            case "West":
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
                print("OVERLAP!");
                m.fillWall();
                roomCount++; //retry the room
                Destroy(currentRoom);
                Destroy(currentConnector); //destroy this as well
                continue; //do not run below code, 
            }

            //on success, add the room's markers to the process list
            if(connectorMarker.tag != "South")
            {
                markerList.Add(currentRoom.GetComponent<Room>().GetNorthMarker());
            }
            if(connectorMarker.tag != "North")
            {
                markerList.Add(currentRoom.GetComponent<Room>().GetSouthMarker());
            }
            if(connectorMarker.tag != "West")
            {
                markerList.Add(currentRoom.GetComponent<Room>().GetEastMarker());
            }
            if(connectorMarker.tag != "East")
            {
                markerList.Add(currentRoom.GetComponent<Room>().GetWestMarker());
            }
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        //print("OVERLAP: " + startingRoom.GetComponent<Room>().isOverlapping());
    }
}
