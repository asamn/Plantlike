using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    private GameObject flower;
    [SerializeField] private GameObject startButton, quitButton;
    [SerializeField] private GameObject worldOptions;
    public void StartGame()
    {
        print("START");
        SceneManager.LoadScene("PlantGame");

    }    
    public void ShowWorldOptions()
    {
        startButton.SetActive(false);
        quitButton.SetActive(false);

        worldOptions.SetActive(true);
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
