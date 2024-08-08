using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitialToggleSelector : MonoBehaviour
{

     [SerializeField] private Toggle smallToggle, mediumToggle, largeToggle;
     [SerializeField] private GameObject savedUserOptions;

    // Start is called before the first frame update
    void Start()
    {
        if(savedUserOptions == null){
            savedUserOptions = GameObject.Find("SavedUserOptions");
        }

        int worldSize;
        worldSize = savedUserOptions.GetComponent<SavedUserOptions>().getWorldSize();

        if(worldSize == 6){
            mediumToggle.isOn = true;
        }
        else if(worldSize == 9){
            largeToggle.isOn = true;
        }
        else{
            smallToggle.isOn = true;
        }
    }

        /**
        Debug.Log("savedUserOptions: " + savedUserOptions);
        if (savedUserOptions != null)
        {
            SavedUserOptions savedOptions = savedUserOptions.GetComponent<SavedUserOptions>();
            Debug.Log("SavedUserOptions component: " + savedOptions);
            if (savedOptions != null)
            {
                int worldSize = savedOptions.getWorldSize();
                Debug.Log("World Size: " + worldSize);
                // Further logic here
            }
            else
            {
                Debug.LogWarning("SavedUserOptions component not found.");
            }
        }
        else
        {
            Debug.LogWarning("savedUserOptions GameObject is not assigned or has been destroyed.");
        }
        **/
}
