using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavedUserOptions : MonoBehaviour
{
    [Range(2,50)]
    [SerializeField] private int worldSize;
    public void setWorldSize(int value)
    {
        worldSize = value;
    }

    public int getWorldSize()
    {
        return worldSize;
    }

    //singleton
    public static SavedUserOptions instance;

    void Awake()
    {
        //singleton to prevent duplicate managers
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
