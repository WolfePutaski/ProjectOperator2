using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponItem
{
    public SC_Weapon_Master equippedWeaponStats;
    [SerializeField]
    public WeaponFunctionProperties equippedWeaponProperties;

    
}

public class SC_Inventory : MonoBehaviour
{
    public List<WeaponItem> weaponItems;
    public int currentSlot = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            ChangeWeaponTo(0);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            ChangeWeaponTo(1);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            ChangeWeaponTo(2);
    }

    void ChangeWeaponTo(int i)
    {
        currentSlot = weaponItems[i].equippedWeaponStats? i : currentSlot ;
    }
}


