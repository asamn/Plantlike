using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class GameManager : MonoBehaviour
{
    public Button menuButton, respawnButton;
    public TMP_Text deathText;

    void Start()
    {
        menuButton.onClick.AddListener(BackToMenu);
        respawnButton.onClick.AddListener(Respawn);
    }
    public void ShowDeathScreen()
    {
        menuButton.gameObject.SetActive(true);
        deathText.gameObject.SetActive(true);
        respawnButton.gameObject.SetActive(true);

    }
    
    void Respawn()
    {
        //just reload the scene
        SceneManager.LoadScene("PlantGame");
    }

    void BackToMenu()
    {
        SceneManager.LoadScene("Title");
    }
}
