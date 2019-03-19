using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerStats : MonoBehaviour {
        
    [Range(0,100)]
    public int health = 100;
    [Range(0, 100)]
    public float stamina = 100f;
    public bool IsAlive
    {
        get
        {
            return isAlive;
        }
        set
        {
            isAlive = value;
        }

    }

    public bool IsRunning
    {
        get
        {
            return fpsController.isRun;
        }
    }


    private FirstPersonController fpsController;
    private FirstPersonCamera firstPersonCamera;
    private PlayerUIController playerUIController;
    private MonoBehaviour uiController;
    private bool isAlive;

    private void Start()
    {
        isAlive = true;
        fpsController = GetComponent<FirstPersonController>();
        firstPersonCamera = GetComponent<FirstPersonCamera>();
        playerUIController = GetComponent<PlayerUIController>();
    }

    private void Update () {
        if(health <= 0)
        {
            health = 0;
            isAlive = false;
            firstPersonCamera.CanControl = isAlive;
            fpsController.CanControl = isAlive;
        }
        else
        {
            CheckRunState();
        }

    }

    public void TookDamage(int damage)
    {
        health -= damage;
        StartCoroutine(playerUIController.HitUI());
    }
        

    private void RefillHealth(int amount)
    {
        health += amount;
        if(health > 100)
        {
            health = 100;
        }
    }


    //This is not included with event because it should be run independly.
    private void CheckRunState()
    {
        if (fpsController.isRun)
        {
            stamina -= 30 * Time.deltaTime;
        }
        else
        {
            if(stamina < 100)
                stamina += 10 * Time.deltaTime;
        }

        if(stamina <= 0)
        {
            fpsController.canRun = false;
        }
        else
        {
            fpsController.canRun = true;
        }
    }

}
