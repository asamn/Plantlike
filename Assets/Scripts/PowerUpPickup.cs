using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Type
{
    Healing,
    MaxHealth,
    Damage,
    Speed,
    BulletSpeed,
    FireRate,
}

public class PowerUpPickup : MonoBehaviour
{
    private GameObject player;
    private PlayerController playerController;

    [SerializeField] private Type pickupType;
    [SerializeField] float strength = 5f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player"); //get the player game object
        playerController = player.GetComponent<PlayerController>(); //get the player's controller script
        
    }

       private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                applyEffect();
                print("PICKUP! ");
                Destroy(this.gameObject);
            }
        }

    public void applyEffect()
    {
        switch (pickupType)
        {
            case Type.Healing:
                playerController.healHealth(strength);
                break;
            case Type.MaxHealth:
                playerController.increaseMaxHealth(strength);
                break;
            case Type.Damage:
                playerController.increaseDamage((int) strength);
                break;
            case Type.Speed:
                playerController.increaseSpeed(strength);
                break;
            case Type.BulletSpeed:
                playerController.increaseBulletSpeed (strength);
                break;
            case Type.FireRate:
                playerController.increaseFireRate (strength);
                break;
            default:
                break;
        }
    }
}
