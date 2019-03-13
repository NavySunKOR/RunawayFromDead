using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour {
    
    public float interactRange;
    private Transform firePos;
    private PlayerInventory playerInventory;
    private GameManager gameManager;

	// Use this for initialization
	void Start () {
        firePos = Camera.main.transform;
        interactRange = 10f;
        playerInventory = GetComponent<PlayerInventory>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

    }
	
	// Update is called once per frame
	void Update () {
		if(InputManager.GetKeyDown(InputNames.use))
        {
            Ray ray = new Ray(firePos.position, firePos.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray,out hit, interactRange, 1<< LayerMask.NameToLayer("Pickable")))
            {
                
                if(hit.transform.CompareTag("Door"))
                {
                    DoorOpen doorOpen = hit.transform.GetComponent<DoorOpen>();
                    if (doorOpen != null)
                    {
                        doorOpen.TryOpenDoor();
                    }
                }
                else if(!hit.transform.CompareTag("Key"))
                {
                    if (playerInventory.PickupItem(hit.transform))
                    {
                        Destroy(hit.transform.gameObject);
                    }
                }
                else
                {
                    gameManager.AddKey(hit.transform.GetComponent<KeyPickable>().keyName);
                    Destroy(hit.transform.gameObject);
                }
            }
        }
	}
}
