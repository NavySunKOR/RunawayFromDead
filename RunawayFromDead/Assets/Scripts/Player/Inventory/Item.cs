using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    None,Health,M416Ammo, ShotgunAmmo, HandgunAmmo, MagnumAmmo ,
    Handgun,Shotgun,Magnum,M416
}

[System.Serializable]
public class Item {
    public int amount;
    public int maxAmount;
    public ItemType type;
    
}
