using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour {
    //attributes.
    [Tooltip("maximum 12")]
    public Item[] inventory;
    [Tooltip("maximum 5 , 0 - None ,1 - M416 ,2 - Shotgun ,3 - Handgun, 4 - Magnum ")]
    public GameObject[] weapons;

    private PlayerStats status;
    [SerializeField]
    private int selectedWeapon = 0;

    private void Start()
    {
        status = GetComponent<PlayerStats>();


    }

    private void Update()
    {
        if(InputManager.GetKeyDown(InputNames.wieldM416))
        {
            EquipWeapon(1);
        }
        else if(InputManager.GetKeyDown(InputNames.wieldShotgun))
        {
            EquipWeapon(2);
        }
        else if(InputManager.GetKeyDown(InputNames.wieldHandgun))
        {
            EquipWeapon(3);
        }
        else if(InputManager.GetKeyDown(InputNames.wieldMagnum))
        {
            EquipWeapon(4);
        }

        if(InputManager.GetKeyDown(InputNames.heal))
        {
            int idx = 0;
            GetItem(ItemType.Health, out idx);
            UseItem(idx, 1);
        }
    }

    public bool PickupItem(Transform tr)
    {
        //check item
        Pickable pck = tr.GetComponent<Pickable>();
        switch (pck.itemType)
        {
            case ItemType.Handgun:return PickupOne(pck);
            case ItemType.HandgunAmmo: return PickupAmount(pck);
            case ItemType.M416: return PickupOne(pck);
            case ItemType.M416Ammo: return PickupAmount(pck);
            case ItemType.Magnum: return PickupOne(pck);
            case ItemType.MagnumAmmo: return PickupAmount(pck);
            case ItemType.Shotgun: return PickupOne(pck);
            case ItemType.ShotgunAmmo: return PickupAmount(pck);
            case ItemType.Health: return PickupAmount(pck);
            default: return false; 

        }

    }

    private bool PickupOne(Pickable pck)
    {
        for(int i = 0; i < inventory.Length; i++)
        {
            if(inventory[i].type == ItemType.None)
            {
                AddInventoryItem(pck, i);
                return true;
            }
        }
        return false;
    }

    private bool PickupAmount(Pickable pck)
    {
        int fillable = 0;
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i].type == ItemType.None)
            {
                AddInventoryItem(pck, i);
                return true;
            }
            else
            {
                if(inventory[i].type == pck.itemType)
                {
                    if(inventory[i].amount + pck.amount > inventory[i].maxAmount)
                    {
                        fillable = inventory[i].maxAmount - inventory[i].amount;
                        inventory[i].amount += fillable;
                        pck.amount -= fillable;
                    }
                    else
                    {
                        inventory[i].amount += pck.amount;
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void AddInventoryItem(Pickable pck,int idx)
    {
        switch (pck.itemType)
        {
            case ItemType.Handgun: case ItemType.Shotgun: case ItemType.M416: case ItemType.Magnum: AddWeapon(pck, idx); break;
            case ItemType.HandgunAmmo: case ItemType.ShotgunAmmo: case ItemType.M416Ammo: case ItemType.MagnumAmmo: AddAmmo(pck, idx); break;
            case ItemType.Health: AddHealth(pck, idx); break;
        }
        
    }

    private void AddWeapon(Pickable pck,int idx)
    {
        inventory[idx] = new Weapon();
        inventory[idx].amount = pck.amount;
        inventory[idx].maxAmount = pck.maxAmount;
        inventory[idx].type = pck.itemType;

        switch(pck.itemType)
        {
            case ItemType.M416: weapons[1].GetComponent<WeaponController>().isHaving = true; break;
            case ItemType.Shotgun: weapons[2].GetComponent<WeaponController>().isHaving = true; break;
            case ItemType.Handgun: weapons[3].GetComponent<WeaponController>().isHaving = true; break;
            case ItemType.Magnum: weapons[4].GetComponent<WeaponController>().isHaving = true; break;
            default: break;

        }
    }

    private void AddAmmo(Pickable pck, int idx)
    {
        inventory[idx] = new Ammo();
        inventory[idx].amount = pck.amount;
        inventory[idx].maxAmount = pck.maxAmount;
        inventory[idx].type = pck.itemType;
    }

    private void AddHealth(Pickable pck, int idx)
    {
        inventory[idx] = new Health();
        inventory[idx].amount = pck.amount;
        inventory[idx].maxAmount = pck.maxAmount;
        inventory[idx].type = pck.itemType;
    }

    //Global
    public bool ReloadWeapon(int amount,out int available)
    {
        Weapon weapon = weapons[selectedWeapon].GetComponent<WeaponController>().weaponInfo;
        return ReloadWeaponWithType(weapon.type, amount, out available);
    }

    private bool ReloadWeaponWithType(ItemType itemType, int amount,out int reloadable)
    {
        Item ammo;
        int idx = 0;
        switch(itemType)
        {
            case ItemType.M416: ammo = GetItem(ItemType.M416Ammo, out idx); break;
            case ItemType.Shotgun: ammo = GetItem(ItemType.ShotgunAmmo, out idx); break;
            case ItemType.Handgun: ammo = GetItem(ItemType.HandgunAmmo, out idx); break;
            case ItemType.Magnum: ammo = GetItem(ItemType.MagnumAmmo, out idx); break;
            default: ammo = new Item(); ammo.type = ItemType.None ; break;
        }
        
        //check exist;
        if (ammo.type != ItemType.None)
        {
            //check ammo fulfill reiqure amount
            if (amount > ammo.amount)
            {
                int ammoFill = 0;
                ammoFill += ammo.amount;
                inventory[idx].type = ItemType.None;

                //to fill amount of reqired ammo.
                if(amount > ammoFill)
                {
                    Item search = GetItem(ammo.type, out idx);
                    //keep search until no ammo left in my inventory. 
                    while(search.type != ammo.type)
                    {
                        if (ammoFill >= amount)
                            break;

                        search = GetItem(ammo.type, out idx);
                        int ammoCount = (ammoFill - ammo.amount);
                        ammoFill += ammoCount;
                        inventory[idx].amount -= ammoCount;
                        if (search.amount == 0)
                        {
                            inventory[idx].type = ItemType.None;
                        }
                    }
                    reloadable = ammoFill;
                }
                else
                {
                    inventory[idx].amount -= ammoFill;
                    reloadable = ammoFill;
                }
            }
            else
            {
                inventory[idx].amount -= amount;
                reloadable = amount;
                if (inventory[idx].amount <= 0)
                    inventory[idx].type = ItemType.None;

            }
            return true;
        }
        reloadable = 0;
        return false;
    }

    private Item GetItem(ItemType lookable,out int refer)
    {
        Item returns = new Item();
        refer = 0;
        for (int i = 0; i < inventory.Length; i++)
        {
            //TODO : Check less amount of ammo.
            if(inventory[i].type == lookable)
            {
                if(returns.type == ItemType.None)
                {
                    returns = inventory[i];
                    refer = i;
                }
                else if(inventory[i].amount <= returns.amount)
                {
                    //allocate new
                    returns = inventory[i];
                    refer = i;
                }
            }
        }
        return returns;
    }
        
    private void EquipWeapon(int index)
    {
        int selected = 0;
        WeaponController weaponController = weapons[index].GetComponent<WeaponController>();
        if (GetItem(weaponController.weaponInfo.type, out selected).type != ItemType.None)
        {
            if (weapons[selectedWeapon] != null)
            {
                weapons[selectedWeapon].GetComponent<WeaponController>().UnWieldWeapon();
                
            }
            weaponController.WieldWeapon();
            selectedWeapon = index;
        }
    }

    //Items

    public void DiscardItem(int index)
    {
        inventory[index].type = ItemType.None;
    }
        
    private void UseItem(int index,int amount)
    {
        if(inventory[index].type == ItemType.Health)
        {
            status.health = 100;
            inventory[index].amount -= amount;
            if(inventory[index].amount == 0)
            {
                inventory[index].type = ItemType.None;
            }
        }
    }
}