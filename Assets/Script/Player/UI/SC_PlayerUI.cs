using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_PlayerUI : MonoBehaviour
{
    SC_WeaponUI weaponUI;
    SC_Inventory inventory;
    SC_Health health;

    void Start()
    {
        TryGetComponent(out inventory);

        weaponUI = FindObjectOfType<SC_WeaponUI>();
    }
    void Update()
    {
       var  currentWeapon = inventory.weaponItems[inventory.currentSlot];

        weaponUI.SetInventoryUI(inventory);
        weaponUI.SetWeaponUIText(currentWeapon);

    }
}
