using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SC_WeaponUI : MonoBehaviour
{
    public TextMeshProUGUI TextName;
    public TextMeshProUGUI TextAmmo;
    public Image Heat;
    public Image Condition;

    public void SetWeaponUIText(string wpnName, int Ammo, int MaxAmmo )
    {
        TextName.text = wpnName;
        TextAmmo.text = Ammo.ToString() + "/" + MaxAmmo.ToString();
        
    }

    public void SetWeaponUIText(WeaponItem weaponItem)
    {
        TextName.text = weaponItem.equippedWeaponStats.weaponName;

        string ammoToShow = weaponItem.equippedWeaponProperties.isLoaded ? 
            (weaponItem.equippedWeaponProperties.ammoInMag + 1).ToString() : weaponItem.equippedWeaponProperties.ammoInMag.ToString();



        TextAmmo.text = ammoToShow + "/" + weaponItem.equippedWeaponStats.magCapacity.ToString();
    }
}
