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

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
