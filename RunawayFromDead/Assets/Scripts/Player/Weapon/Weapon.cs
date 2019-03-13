using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weapon : Item {

    //effects 
    public GameObject muzzleFlash;
    public GameObject bulletHole;

    //status
    public int damage;
    public float rpm;
    public float range;
    [Tooltip("maximum capacity of mag and current loaded.")]
    public int maximumMagazine;
    public int currentMagazine;
    public float maxRecoilRadius;
    public float recoilIncreament;


    //for shotgun only
    private float shotgunSpread;
    private int shotgunPalletCount = 8;


    public Weapon() {
        currentMagazine = maximumMagazine;
        shotgunSpread = 1f;
    }


    public float GetShotgunSpread()
    {
        return shotgunSpread;
    }

    public int GetShotgunPalletCount()
    {
        return shotgunPalletCount;
    }

    
}
