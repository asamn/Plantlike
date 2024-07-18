using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
[DefaultExecutionOrder(100)] 
public class GameManager : MonoBehaviour
{
    public Button menuButton, respawnButton;

    private GameObject level; 
    private GameObject player;
    private PlayerController playerController;
    private LevelGen generator;

    private GameObject boss;
    public TMP_Text deathText;
    private bool foundBoss;

    void Start()
    {
        player = GameObject.FindWithTag("Player"); //get the player game object
        level = GameObject.Find("GeneratedLevel");
        playerController = player.GetComponent<PlayerController>(); //get the player's controller script
        menuButton.onClick.AddListener(BackToMenu);
        respawnButton.onClick.AddListener(Respawn);
        generator = level.GetComponent<LevelGen>();
        //boss = GameObject.Find("BossEnemy");
    }

    void LateUpdate()
    {
        if (generator.isGenerated() && !foundBoss)//find boss after level is finished generating
        {
            boss = GameObject.Find("BossEnemy");
            foundBoss = true;
        }
        //
        if (foundBoss) //if found boss object
        {
            if (boss == null) //if boss becomes destroyed
            {
                //print("THE BOSS IS DEAD. LONG LIVE THE BOSS");
                //NextLevel();
            }
            else
            {
                //print("BOSS ALIVE");
            }
        }
    }

    public void ShowDeathScreen()
    {
        menuButton.gameObject.SetActive(true);
        deathText.gameObject.SetActive(true);
        respawnButton.gameObject.SetActive(true);

    }
    
    public void Respawn()
    {
        //just reload the scene
        SceneManager.LoadScene("PlantGame");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Title");
    }

    public void NextLevel()
    {
        foundBoss = false;
        generator.clearLevel();
        playerController.resetPosition();
        playerController.increaseDungeonLevel();

        generator.GenerateLevel();
    }

}
