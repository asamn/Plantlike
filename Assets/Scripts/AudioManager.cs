using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource music,ambience,projectile,hurt,lvlUp,slimeHit;

    //singleton
    public static AudioManager instance;
    // Start is called before the first frame update
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
        
        StartMusic();
    }

    public void StartAmbience()
    {
        ambience.Play();
    }

    public void StartMusic()
    {
        music.Play();
    }

    public void StopAmbience()
    {
        ambience.Stop();
    }

    public void StopMusic()
    {
        music.Stop();
    }


    public void PlayProjectile()
    {
        projectile.Play();
    }

    public void PlayHurt()
    {
        hurt.Play();
    }

    public void PlayLevelUp()
    {
        lvlUp.Play();
    }
    public void PlaySlimeHit()
    {
        slimeHit.Play();
    }


}
