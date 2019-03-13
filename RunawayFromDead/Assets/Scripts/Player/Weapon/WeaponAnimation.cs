using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimation : MonoBehaviour {

    public AudioSource audioSource;
    public AudioClip fireSound;
    public AudioClip reloadSound;
    public Animator armAnimator;
    public Animator gunAnimator;

    public delegate void RefilAmmo();
    public delegate void RefilShotgunAmmo();
    public RefilAmmo refilAmmoEvent;
    public RefilShotgunAmmo refilShotgunAmmoEvent;


    public bool reloadingState
    {
        get
        {
            return isReloading;
        }
        set
        {
            isReloading = value;
        }
    }

    private bool isReloading;

    public void FireWeapon()
    {
        audioSource.clip = fireSound;
        audioSource.Play();
        armAnimator.SetTrigger("fireTrigger");
        gunAnimator.SetTrigger("fireTrigger");
    }

    //set animator and status
    public void ReloadStart()
    {
        reloadingState = true;
        if(GetComponent<WeaponController>().weaponInfo.type == ItemType.Shotgun)
            armAnimator.SetBool("IsReloading", reloadingState);
        else
        {
            audioSource.clip = reloadSound;
            audioSource.Play();
        }
        armAnimator.SetTrigger("reloadTrigger");
        gunAnimator.SetTrigger("reloadTrigger");
    }
    
    public void ReloadEnd()
    {
        reloadingState = false;
        if (GetComponent<WeaponController>().weaponInfo.type == ItemType.Shotgun)
        {
            armAnimator.SetBool("IsReloading", reloadingState);
            armAnimator.SetTrigger("reloadEndTrigger");
            gunAnimator.SetTrigger("reloadEndTrigger");
        }
        else
        {
            refilAmmoEvent.Invoke();
        }
    }

    public void ReloadShell()
    {
        audioSource.clip = reloadSound;
        audioSource.Play();
        refilShotgunAmmoEvent.Invoke();
    }

    public void WieldWeapon(ItemType itemType)
    {
        if(itemType == ItemType.M416 || itemType == ItemType.Shotgun)
            armAnimator.SetTrigger("wieldTrigger");
    }

    public void SetRun(bool val)
    {
        armAnimator.SetBool("IsRunning", val);
    }
}
