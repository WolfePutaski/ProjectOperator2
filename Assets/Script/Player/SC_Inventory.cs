using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponItem
{
    public SCO_Weapon_Master equippedWeaponStats;
    public WeaponFunctionProperties equippedWeaponProperties;

    
    public WeaponItem(SCO_Weapon_Master weaponClass)
    {
        equippedWeaponStats = Object.Instantiate(weaponClass);
        equippedWeaponProperties = new WeaponFunctionProperties(weaponClass);
    }
}

public class SC_Inventory : MonoBehaviour
{
    public List<WeaponItem> weaponItems;
    public int currentSlot = 0;
    public bool NotReceiveInput;


    [Header("===Debug===")]
    public List<SCO_Weapon_Master> startWeaponList;

    void Start()
    {
        for (int i = 0; i < startWeaponList.Count; i++)
        {
            weaponItems[i] = new WeaponItem(startWeaponList[i]);
        }
    }

    public void ChangeWeaponTo(int i)
    {
        currentSlot = weaponItems[i].equippedWeaponStats? i : currentSlot ;
    }
}


