using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassDropdown : MonoBehaviour
{
    [SerializeField] private string characterClass = "Sharpshooter";
    private static ClassDropdown instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("ClassDropdown instance created and will persist across scenes.");
        }
        else
        {
            Debug.LogWarning("Duplicate ClassDropdown instance detected; destroying duplicate.");
            Destroy(gameObject);
        }
    }

    public void ClassDropdownChoose(int index)
    {
        switch (index)
        {
            case 0: characterClass = "Sharpshooter"; break;
            case 1: characterClass = "Shredder"; break;
            case 2: characterClass = "Sprayer"; break;
        }
        Debug.Log("Selected Character Class: " + characterClass);
    }

    public string GetCharacterClass()
    {
        return characterClass;
    }

    void Update()
{
    Debug.Log("Current Character Class: " + characterClass);
}

}
