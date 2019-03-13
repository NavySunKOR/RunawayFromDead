using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour {

    public string requiredKey;
    public float openDoorAngle;

    private PlayerUIController playerUIController;
    private GameManager gameManager;
    private bool isDoorOpened;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        playerUIController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerUIController>();
    }


    public void TryOpenDoor()
    {
        if(requiredKey.Equals("") && !isDoorOpened)
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, openDoorAngle, transform.localEulerAngles.z);
            isDoorOpened = true;
        }
        else
        {
            if (gameManager.HasKey(requiredKey) && requiredKey.Equals("MainDoor"))
            {
                gameManager.GameCleared();
            }
            if (gameManager.HasKey(requiredKey) && !isDoorOpened)
            {
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, openDoorAngle, transform.localEulerAngles.z);
                isDoorOpened = true;
            }
            
        }
    }
}
