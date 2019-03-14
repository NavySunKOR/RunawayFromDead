using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PlayerUIController : MonoBehaviour {

    public Text healthText;
    public GameObject inventoryPanel;
    public GameObject inventoryInnerPanel;
    public GameObject popupMenu;
    public GameObject gameOverPanel;
    public GameObject gameClearPanel;
    public GameObject hitPanel;
    public GameObject[] buttons;
    public Text frameText;

    private GameManager gameManager;
    private PlayerStats status;
    private PlayerInventory playerInventory;

    private void Start()
    {
        status = GetComponent<PlayerStats>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        playerInventory = GetComponent<PlayerInventory>();
        OnOFFCrossHair();
        Time.timeScale = 1;
    }

    private void Update()
    {
        frameText.text = (Mathf.Floor(Time.frameCount / Time.time)).ToString();
        if (status.IsAlive && !gameManager.IsGameCleared)
        {
            UpdatingHealthBar();
            if (InputManager.GetKeyDown(InputNames.openInventory))
            {
                OpenCloseInventory();
            }

            if (InputManager.GetKeyDown(InputNames.pause))
            {
                OpenClosePauseMenu();
            }
        }
        else if(!status.IsAlive)
        {
            OpenGameOverPanel();
        }
        else
        {
            OpenGameClearPanel();
        }
    }

    private void UpdatingHealthBar()
    {
        healthText.text = status.health.ToString();
        if (status.health >= 70)
        {
            healthText.color = Color.green;
        }
        else if(status.health >= 40)
        {
            healthText.color = Color.yellow;
        }
        else
        {
            healthText.color = Color.red;
        }
    }

    private void UpdateInventory()
    {
        Transform[] transforms = inventoryInnerPanel.GetComponentsInChildren<Transform>();
        foreach(Transform tr in transforms)
        {
            if(tr != inventoryInnerPanel.transform)
                Destroy(tr.gameObject);
        }

        for (int i = 0; i < playerInventory.inventory.Length; i++)
        {
            switch (playerInventory.inventory[i].type)
            {
                case ItemType.Handgun:
                    GameObject handgunObj = Instantiate(buttons[0], inventoryInnerPanel.transform);
                    handgunObj.GetComponent<ButtonEvent>().index = i;
                    break;
                case ItemType.HandgunAmmo:
                    GameObject handgunAmmoObj = Instantiate(buttons[1], inventoryInnerPanel.transform);
                    handgunAmmoObj.transform.Find("AmountText").GetComponent<Text>().text = playerInventory.inventory[i].amount.ToString();
                    handgunAmmoObj.GetComponent<ButtonEvent>().index = i;
                    break;
                case ItemType.M416:
                    GameObject m416Obj = Instantiate(buttons[2], inventoryInnerPanel.transform);
                    m416Obj.GetComponent<ButtonEvent>().index = i;
                    break;
                case ItemType.M416Ammo:
                    GameObject m416AmmoObj = Instantiate(buttons[3], inventoryInnerPanel.transform);
                    m416AmmoObj.transform.Find("AmountText").GetComponent<Text>().text = playerInventory.inventory[i].amount.ToString();
                    m416AmmoObj.GetComponent<ButtonEvent>().index = i;
                    break;
                case ItemType.Shotgun:
                    GameObject shotgunObj = Instantiate(buttons[4], inventoryInnerPanel.transform);
                    shotgunObj.GetComponent<ButtonEvent>().index = i;
                    break;
                case ItemType.ShotgunAmmo:
                    GameObject shotgunAmmoObj = Instantiate(buttons[5], inventoryInnerPanel.transform);
                    shotgunAmmoObj.transform.Find("AmountText").GetComponent<Text>().text = playerInventory.inventory[i].amount.ToString();
                    shotgunAmmoObj.GetComponent<ButtonEvent>().index = i;
                    break;
                case ItemType.Magnum:
                    GameObject magnumObj = Instantiate(buttons[6], inventoryInnerPanel.transform);
                    magnumObj.GetComponent<ButtonEvent>().index = i;
                    break;
                case ItemType.MagnumAmmo:
                    GameObject magnumAmmoObj = Instantiate(buttons[7], inventoryInnerPanel.transform);
                    magnumAmmoObj.transform.Find("AmountText").GetComponent<Text>().text = playerInventory.inventory[i].amount.ToString();
                    magnumAmmoObj.GetComponent<ButtonEvent>().index = i;
                    break;
                case ItemType.Health:
                    GameObject healthObj = Instantiate(buttons[8], inventoryInnerPanel.transform);
                    healthObj.transform.Find("AmountText").GetComponent<Text>().text = playerInventory.inventory[i].amount.ToString();
                    healthObj.GetComponent<ButtonEvent>().index = i;
                    break;
                default: break;
            }

        }
    }

    private void OpenCloseInventory()
    {
        if (!inventoryPanel.activeSelf)
            UpdateInventory();
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        OnOFFCrossHair();
    }

    private void OpenGameOverPanel()
    {
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);
        if(!popupMenu.activeSelf)
            OpenClosePauseMenu();
    }

    public void OpenGameClearPanel()
    {
        Time.timeScale = 0;
        gameClearPanel.SetActive(true);
        if (!popupMenu.activeSelf)
            OpenClosePauseMenu();
    }

    public void OpenClosePauseMenu()
    {
        popupMenu.SetActive(!popupMenu.activeSelf);
        OnOFFCrossHair();
    }

    public void OnOFFCrossHair()
    {
        Cursor.lockState = (Cursor.lockState == CursorLockMode.Locked) ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = (Cursor.visible) ? false : true;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public IEnumerator HitUI()
    {
        hitPanel.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        hitPanel.SetActive(false);
    }
        
}