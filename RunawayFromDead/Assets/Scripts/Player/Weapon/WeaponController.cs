using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {

    public Weapon weaponInfo;
    public WeaponAnimation gunAnimation;
    public bool fireReady;
    public bool isHaving;
    private const float MAX_MUZZLE_INTERVAL = 0.05f;
    private Transform firePos;
    private float currentRecoilRadius;
    private float fireInterval;
    private float fireTime = 0f;
    private PlayerInventory playerInventory;
    private int ammosToReload;
    private FirstPersonController status;
    private PlayerUIController playerUIController;

    // Use this for initialization
    private void Start () {
        fireReady = true;
        fireInterval = 60f / weaponInfo.rpm;
        firePos = GameObject.FindGameObjectWithTag("MainCamera").transform;
        playerInventory = GetComponentInParent<PlayerInventory>();
        gunAnimation = GetComponent<WeaponAnimation>();
        ammosToReload = 0;
        status = GetComponentInParent<FirstPersonController>();
        playerUIController = GetComponentInParent<PlayerUIController>();
    }

    private void OnEnable()
    {
        gunAnimation.refilAmmoEvent += RefilAmmo;
        gunAnimation.refilShotgunAmmoEvent += RefilShell;
    }
    private void OnDisable()
    {
        gunAnimation.refilAmmoEvent -= RefilAmmo;
        gunAnimation.refilShotgunAmmoEvent -= RefilShell;
    }

    // Update is called once per frame
    private void Update () {
        CheckFireReady();
        if (fireReady && !gunAnimation.reloadingState)
        {
            if(InputManager.GetKey(InputNames.fire))
            {
                FireWeapon();

            }
        }

        if(InputManager.GetKeyDown(InputNames.reload) 
            && !gunAnimation.reloadingState)
        {
            ammosToReload = weaponInfo.maximumMagazine - weaponInfo.currentMagazine;
            int available = 0;
            if (playerInventory.ReloadWeapon(ammosToReload,out available))
            {
                //TODO : check this code.
                ammosToReload = available;
                ReloadWeapon();
            }
        }

        if(weaponInfo.type == ItemType.Shotgun && gunAnimation.reloadingState)
        {
            CheckReloadShotgun();
        }

        //set running
        gunAnimation.SetRun(status.isRun);

        //reduce recoil;
        if(currentRecoilRadius > 0)
        {
            currentRecoilRadius -= weaponInfo.recoilIncreament * Time.deltaTime;
        }
    }

    private void CheckFireReady()
    {
        fireReady = (weaponInfo.currentMagazine > 0 && Time.time - fireTime > fireInterval && !status.isRun && !playerUIController.IsPaused) ? true : false;
    }

    private void FireWeapon()
    {
        fireTime = Time.time;

        gunAnimation.FireWeapon();
        // Check WeaponType
        // FIre Different weapontype and play animation;
        switch(weaponInfo.type)
        {
            case ItemType.Handgun: FireSingleShot(); break;
            case ItemType.M416: FireSingleShot(); break;
            case ItemType.Magnum: FireSingleShot(); break;
            case ItemType.Shotgun: FireShotgun(); break;
            default: Debug.LogError("There's no weapon!"); break;
        }
    }

    private void FireSingleShot()
    {
        StartCoroutine(OpenFireEffect());

        Vector3 randomVector = Random.insideUnitCircle * currentRecoilRadius;
        Ray ray = new Ray(firePos.position + firePos.forward * 1f + randomVector, firePos.forward);
        RaycastHit hit;
        if(Physics.Raycast(ray,out  hit, weaponInfo.range))
        {
            AIStatus aiStatus = hit.collider.GetComponentInParent<AIStatus>();
            if(aiStatus != null)
            {
                aiStatus.TookHit(hit.collider.transform, weaponInfo.damage);
                GameObject bloodEffect = Instantiate(aiStatus.bloodEffect, hit.point, Quaternion.identity) as GameObject;
                float playLength = 1f; //TODO: Change this to particle play length;
                Destroy(bloodEffect, playLength);
            }
            else
            {
                GameObject bulletHole = Instantiate(weaponInfo.bulletHole, hit.point + (hit.normal * 0.001f), Quaternion.FromToRotation(Vector3.forward,hit.normal));
                Destroy(bulletHole, 3f);
            }
        }
        
        DecreaseMagazine();
        IncreaseRecoilRadius();
    }

    private void FireShotgun()
    {
        StartCoroutine(OpenFireEffect());

        for (int i = 0; i < weaponInfo.GetShotgunPalletCount(); i++)
        {
            Vector3 randomVector = Random.insideUnitCircle * weaponInfo.GetShotgunSpread();
            Ray ray = new Ray(firePos.position + firePos.forward * 1f + randomVector, firePos.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, weaponInfo.range))
            {
                AIStatus aiStatus = hit.collider.GetComponentInParent<AIStatus>();
                if (aiStatus != null)
                {
                    aiStatus.TookHit(hit.collider.transform, weaponInfo.damage);
                    GameObject bloodEffect = Instantiate(aiStatus.bloodEffect, hit.point, Quaternion.identity) as GameObject;
                    float playLength = 1f; //TODO: Change this to particle play length;
                    Destroy(bloodEffect, playLength);
                }
                else
                {
                    GameObject bulletHole = Instantiate(weaponInfo.bulletHole, hit.point + (hit.normal * 0.001f), Quaternion.FromToRotation(Vector3.forward, hit.normal));
                    Destroy(bulletHole, 3f);
                }
            }
        }

        DecreaseMagazine();
        IncreaseRecoilRadius();
    }

    IEnumerator OpenFireEffect()
    {
        weaponInfo.muzzleFlash.SetActive(true);
        if(!weaponInfo.muzzleFlash.GetComponent<ParticleSystem>().isPlaying)
        weaponInfo.muzzleFlash.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(MAX_MUZZLE_INTERVAL);
        weaponInfo.muzzleFlash.SetActive(false);
    }

    private void DecreaseMagazine()
    {
        weaponInfo.currentMagazine--;
    }

    private void IncreaseRecoilRadius()
    {
        currentRecoilRadius += weaponInfo.recoilIncreament;

        if (currentRecoilRadius > weaponInfo.maxRecoilRadius)
        {
            currentRecoilRadius = weaponInfo.maxRecoilRadius;
        }
    }

    private void DecreaseRecoilRadius()
    {
        currentRecoilRadius -= weaponInfo.recoilIncreament * Time.deltaTime;
        if (currentRecoilRadius < 0f)
        {
            currentRecoilRadius = 0f;
        }
    }

   private void ReloadWeapon()
   {
        switch(weaponInfo.type)
        {
            case ItemType.Handgun: ReloadMagazine(); break;
            case ItemType.M416: ReloadMagazine(); break;
            case ItemType.Magnum: ReloadMagazine(); break;
            case ItemType.Shotgun: ReloadMagazine(); break;
            default: Debug.LogError("There's no weapon!"); break;
        }
   }

   private void ReloadMagazine()
   {
        gunAnimation.ReloadStart();
   }
    
   //Shotgun only
   private void CheckReloadShotgun()
   {
        ammosToReload = weaponInfo.maximumMagazine - weaponInfo.currentMagazine;
        if (ammosToReload <= 0 )
        {
            gunAnimation.ReloadEnd();
        }
   }
   private void RefilAmmo()
   {
        weaponInfo.currentMagazine += ammosToReload;
        ammosToReload = 0;
   }

   private void RefilShell()
   {
        weaponInfo.currentMagazine++;
   }


    public void WieldWeapon()
    {
        gameObject.SetActive(true);
        gunAnimation.WieldWeapon(weaponInfo.type);
    }
    public void UnWieldWeapon()
    {
        gameObject.SetActive(false);
    }
}
