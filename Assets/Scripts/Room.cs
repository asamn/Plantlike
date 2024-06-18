using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{

    [SerializeField] int size = 1;

    private float leeway = 2.5f; //provides some leeway when detecting overlapping rooms, higher means smaller bounding box, default is 2.5f



    bool m_Started;
    public LayerMask m_LayerMask;
    private Bounds debugBounds;

    private GameObject layer2;


    void Awake()
    {
        layer2 = this.gameObject.transform.Find("Layer2").gameObject;
        
        //for creating the overlapbox
    }

    // Start is called before the first frame update
    void Start()
    {
        m_Started = true;

    }

    public bool isOverlapping()
    {
        debugBounds = GetBounds();
        Collider[] hitColliders = Physics.OverlapBox(debugBounds.center, debugBounds.size / leeway, Quaternion.identity, m_LayerMask);
        foreach (Collider c in hitColliders)
        {
            if (!c.gameObject.transform.IsChildOf(this.transform)) //ignore block terrains of this room
            {

                return true;
                //break;
            }
        }
        return false;
    }

    //Draw the Box Overlap as a gizmo to show where it currently is testing. Click the Gizmos button to see this
    void OnDrawGizmos()
    {
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        if (m_Started)
        {
            /*
            // bottom
            var p1 = new Vector3(debugBounds.min.x, debugBounds.min.y, debugBounds.min.z);
            var p2 = new Vector3(debugBounds.max.x, debugBounds.min.y, debugBounds.min.z);
            var p3 = new Vector3(debugBounds.max.x, debugBounds.min.y, debugBounds.max.z);
            var p4 = new Vector3(debugBounds.min.x, debugBounds.min.y, debugBounds.max.z);

            Gizmos.color = Color.red;

            Gizmos.DrawLine(p1, p2);
            Gizmos.DrawLine(p2, p3);
            Gizmos.DrawLine(p3, p4);
            Gizmos.DrawLine(p4, p1);

            // top
            var p5 = new Vector3(debugBounds.min.x, debugBounds.max.y, debugBounds.min.z);
            var p6 = new Vector3(debugBounds.max.x, debugBounds.max.y, debugBounds.min.z);
            var p7 = new Vector3(debugBounds.max.x, debugBounds.max.y, debugBounds.max.z);
            var p8 = new Vector3(debugBounds.min.x, debugBounds.max.y, debugBounds.max.z);

            Gizmos.DrawLine(p5, p6);
            Gizmos.DrawLine(p6, p7);
            Gizmos.DrawLine(p7, p8);
            Gizmos.DrawLine(p8, p5);

            // sides
            Gizmos.DrawLine(p1, p5);
            Gizmos.DrawLine(p2, p6);
            Gizmos.DrawLine(p3, p7);
            Gizmos.DrawLine(p4, p8);
            */



            Gizmos.DrawWireCube(debugBounds.center, debugBounds.size / (leeway / 2));
        }
    }


    //for calculating the overlapbox
    private Bounds GetBounds() 
    {
        Bounds b = new Bounds(this.gameObject.transform.position, Vector3.zero);
        foreach (Renderer r in this.gameObject.GetComponentsInChildren<Renderer>()) 
        {
            //grow the bounds 
            b.Encapsulate(r.bounds);
        }
        return b;
    }


    

    public GameObject GetNorthMarker()
    {
        for (int i = 0; i < layer2.transform.childCount; i++)
        {
            GameObject currentChild = layer2.transform.GetChild(i).gameObject;
            if (currentChild.tag == "North")
            {
                return currentChild;
            }
        }
        //ERROR
        return null;
    }
    public GameObject GetEastMarker()
    {
        for (int i = 0; i < layer2.transform.childCount; i++)
        {
            GameObject currentChild = layer2.transform.GetChild(i).gameObject;
            if (currentChild.tag == "East")
            {
                return currentChild;
            }
        }
        //ERROR
        return null;
    }
    public GameObject GetSouthMarker()
    {
        for (int i = 0; i < layer2.transform.childCount; i++)
        {
            GameObject currentChild = layer2.transform.GetChild(i).gameObject;
            if (currentChild.tag == "South")
            {
                return currentChild;
            }
        }
        //ERROR
        return null;
    }
    public GameObject GetWestMarker()
    {
        for (int i = 0; i < layer2.transform.childCount; i++)
        {
            GameObject currentChild = layer2.transform.GetChild(i).gameObject;
            if (currentChild.tag == "West")
            {
                return currentChild;
            }
        }
        //ERROR
        return null;
    }


}
