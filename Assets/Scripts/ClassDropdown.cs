using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ClassDropdown : MonoBehaviour
{
    [SerializeField] private string characterClass = "Sharpshooter";
    [SerializeField] private int dropdownValue;
    private TMP_Dropdown dropdown;
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
        //SceneManager.activeSceneChanged += OnSceneChange; //add a function that takes in two scenes (current, next)
        //Initialize();
    }

    public void Initialize()
    {
        //get the references
        characterClass = "Sharpshooter";

        dropdown = GameObject.Find("Dropdown").GetComponent<TMP_Dropdown>();

        dropdown.onValueChanged.AddListener(delegate {
            ClassDropdownChoose(dropdown);
        });
    }

/*
    private void OnSceneChange(Scene current, Scene next)
    {
        if (next.name == "Title")
        {
            print("Scene switched to title. ");
            //find the refereneces again
            Initialize();
        }
    }
*/
    
    public void ClassDropdownChoose(TMP_Dropdown dropdown)
    {
        switch (dropdown.value)
        {
            case 0: characterClass = "Sharpshooter"; break;
            case 1: characterClass = "Shredder"; break;
            case 2: characterClass = "Sprayer"; break;
        }
        dropdownValue = dropdown.value;
        Debug.Log("Selected Character Class: " + characterClass);
    }

    public string GetCharacterClass()
    {
        return characterClass;
    }

    public int GetDropdownValue(){
        return dropdownValue;
    }

    void Update()
{
    //Debug.Log("Current Character Class: " + characterClass);
}

}
