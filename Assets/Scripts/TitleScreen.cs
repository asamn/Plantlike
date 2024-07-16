using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    private GameObject flower;

    public void StartGame()
    {
        print("START");
        SceneManager.LoadScene("PlantGame");

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
