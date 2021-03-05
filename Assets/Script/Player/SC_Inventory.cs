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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            ChangeWeaponTo(0);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            ChangeWeaponTo(1);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            ChangeWeaponTo(2);

        //if (Input.GetKeyDown(KeyCode.P))
        //    weaponItems[currentSlot].equippedWeaponStats.magCapacity += 10;
        // For Debugging. Weapon Mod system will be implemented later;
    }

    void ChangeWeaponTo(int i)
    {
        if (!NotReceiveInput)
        currentSlot = weaponItems[i].equippedWeaponStats? i : currentSlot ;
    }
}


