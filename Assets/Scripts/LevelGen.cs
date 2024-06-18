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

    private Queue<GameObject> markerQueue; 

    // Start is called before the first frame update
    void Start()
    {
        markerQueue = new Queue<GameObject>();
        //the markers of each room should be stored in layer 2 
        GameObject layer2 = startingRoom.transform.Find("Layer2").gameObject; 

        for (int i = 0; i < layer2.transform.childCount; i++)
        {   
            GameObject currentChild = layer2.transform.GetChild(i).gameObject;
            //print(currentTag);
            switch(currentChild.tag) 
            {
            case "North":
                markerQueue.Enqueue(currentChild);
                break;
            case "East":
                markerQueue.Enqueue(currentChild);
                break;
            case "South":
                markerQueue.Enqueue(currentChild);
                break;
            case "West":
                markerQueue.Enqueue(currentChild);
                break;
            default:
                //don't add this to the queue
                break;
            }

        }
        //print(markerQueue);
        while (markerQueue.Count > 0)
        {
            
            GameObject currentMarker = markerQueue.Dequeue();
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


            //todo: make random
            int rngConnector = 0;
            int rngRoom = 0;
            
            Vector3 spawnPosition = currentMarker.transform.position;

            //place connectors, NORTH IS Z POSITIVE, EAST IS X POSITIVE
            if (m.getDir() == Direction.North)
            {
                currentConnector = Instantiate(NSConnectorPool[rngConnector], currentMarker.transform.position, currentMarker.transform.rotation);
                spawnPosition.z += currentMarker.transform.position.z - currentConnector.GetComponent<Room>().GetSouthMarker().transform.position.z - 1; //adjust 
                //make sure the rooms align
                spawnPosition.x += currentMarker.transform.position.x - currentConnector.GetComponent<Room>().GetSouthMarker().transform.position.x;

                connectorMarker = currentConnector.GetComponent<Room>().GetNorthMarker();

            }
            else if (m.getDir() == Direction.East)
            {
                currentConnector = Instantiate(EWConnectorPool[rngConnector], currentMarker.transform.position, currentMarker.transform.rotation);
                spawnPosition.x += currentMarker.transform.position.x - currentConnector.GetComponent<Room>().GetWestMarker().transform.position.x - 1; //adjust 
                //make sure the rooms align
                spawnPosition.z += currentMarker.transform.position.z - currentConnector.GetComponent<Room>().GetWestMarker().transform.position.z;

                connectorMarker = currentConnector.GetComponent<Room>().GetEastMarker();

            }
            else if (m.getDir() == Direction.South)
            {
                currentConnector = Instantiate(NSConnectorPool[rngConnector], currentMarker.transform.position, currentMarker.transform.rotation);
                spawnPosition.z += currentMarker.transform.position.z - currentConnector.GetComponent<Room>().GetNorthMarker().transform.position.z + 1; //adjust 
                //make sure the rooms align
                spawnPosition.x += currentMarker.transform.position.x - currentConnector.GetComponent<Room>().GetNorthMarker().transform.position.x;

                connectorMarker = currentConnector.GetComponent<Room>().GetSouthMarker();

            }
            else if (m.getDir() == Direction.West)
            {
                currentConnector = Instantiate(EWConnectorPool[rngConnector], currentMarker.transform.position, currentMarker.transform.rotation);
                spawnPosition.x += currentMarker.transform.position.x - currentConnector.GetComponent<Room>().GetEastMarker().transform.position.x + 1; //adjust 
                //make sure the rooms align
                spawnPosition.z += currentMarker.transform.position.z - currentConnector.GetComponent<Room>().GetWestMarker().transform.position.z;
                
                connectorMarker = currentConnector.GetComponent<Room>().GetWestMarker();
            }
            currentConnector.name = "Connector: " + markerQueue.Count;

            if(currentConnector != null)
            {
                currentConnector.transform.position = spawnPosition;
            }

            //very important, otherwise physics system may not properly recognize overlaps
            //fix source: https://forum.unity.com/threads/solved-strange-behavior-with-overlapbox.833299/ by Baste
            Physics.SyncTransforms();
            
            //print("OVERLAP?:" + currentConnector.GetComponent<Room>().isOverlapping());

            if (currentConnector != null && currentConnector.GetComponent<Room>().isOverlapping())
            {
                print("OVERLAP!");
                //Destroy(currentConnector);
                continue; //do not run below code,
            }

            //start spawning the room

            spawnPosition = connectorMarker.transform.position;
            currentRoom = Instantiate(roomPool[rngRoom], connectorMarker.transform.position, connectorMarker.transform.rotation);
            
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

            currentRoom.transform.position = spawnPosition;

           //TODO: handle room overlaps, fill walls of remaining unvisited markers
            
            //currentMarker.GetComponent<Marker>().fillWall();
        }


        //print("OVERLAP: " + startingRoom.GetComponent<Room>().isOverlapping());

    }


    // Update is called once per frame
    void Update()
    {
        //print("OVERLAP: " + startingRoom.GetComponent<Room>().isOverlapping());
    }
}
