using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonEvent : MonoBehaviour, IPointerClickHandler
{
    public int index;
    private PlayerInventory playerInventory;
    private PlayerUIController playerUIController;
    public void OnPointerClick(PointerEventData eventData)
    {
        playerInventory = GetComponentInParent<PlayerInventory>();
        playerUIController = GetComponentInParent<PlayerUIController>();
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            playerInventory.DiscardItem(index);
            playerUIController.SendMessage("UpdateInventory");
        }
    }
}
