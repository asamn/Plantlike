using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class TitleScreen : MonoBehaviour
{
    private GameObject flower;
    private int selectedWorldSize = 3;
    [SerializeField] private GameObject startButton, quitButton;
    [SerializeField] private GameObject worldOptions, savedUserOptions;

    [SerializeField] private TMP_Text startButtonText;
    private AudioManager am;

    void Awake()
    {
        am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        savedUserOptions = GameObject.Find("SavedUserOptions");
    }

    public void StartGame()
    {
        //print("START");
        am.StartAmbience();
        startButtonText.text = "Generating...";
        SceneManager.LoadScene("PlantGame");

    }    
    public void ShowWorldOptions()
    {
        startButton.SetActive(false);
        quitButton.SetActive(false);

        worldOptions.SetActive(true);
    }

    public void SetWorldSize(int size)
    {
        if (size <= 1 || size > 50)
        {
            selectedWorldSize = 3;
            print("ERROR! SIZE INVALID! SETTING TO 3. ");
        }
        else
        {
            selectedWorldSize = size;
        }
        if (savedUserOptions == null)
        {
            savedUserOptions = GameObject.Find("SavedUserOptions");
        }
        savedUserOptions.GetComponent<SavedUserOptions>().setWorldSize(selectedWorldSize);
    }

    public void ExitGame()
    {
        print("Quitting");
        Application.Quit();
    }

    public void onHover(GameObject o)
    {
        o.SetActive(true);

    }
    public void onHoverExit(GameObject o)
    {
        o.SetActive(false);
    }
}
