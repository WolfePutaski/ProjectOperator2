using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WeaponItem
{
    public SC_Weapon_Master equippedWeapon;
    [SerializeField]
    public WeaponFunctionProperties equippedWeaponProperty;
}

public class SC_Inventory : MonoBehaviour
{
    //public int EquippedSlot;
    //public List<WeaponItem> weaponItems;

    public SC_Weapon_Master equippedWeapon;
    [SerializeField]
    public WeaponFunctionProperties equippedWeaponProperty;
}
