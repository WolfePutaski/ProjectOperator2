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
    public Text TextFiremode;

    public List<Text> InvSlot;

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

        string _textFiremode()
        {
            switch (weaponItem.equippedWeaponProperties.CurrentFiremode)
            {
                case FireMode.SEMI:
                    return "SEMI-AUTO";
                case FireMode.AUTO:
                    return "FULL-AUTO";
                case FireMode.BURST:
                    return "BURST";
                case FireMode.MANUAL:
                    return "Manual";
                default:
                    return "???";
            }
        }
        

        TextFiremode.text = _textFiremode();
    }

    public void SetInventoryUI(SC_Inventory playerInventory)
    {
        for (int i = 0; i < InvSlot.Count; i++)
        {

            if (playerInventory.currentSlot == i)
                InvSlot[i].color = Color.white;
            else
                InvSlot[i].color = Color.black;
        }
    }
}
