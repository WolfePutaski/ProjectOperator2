using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class SC_WeaponUI : MonoBehaviour
{
    [System.Serializable]
    public class WeaponPromptText
    {
        public GameObject Prompt;
        public TextMeshProUGUI TextName;
        public TextMeshProUGUI TextDesc;
        public GameObject TextKeyPrompt;
        public Image timeBar;
    }

    public TextMeshProUGUI TextName;
    public TextMeshProUGUI TextAmmo;
    public Image Heat;
    public Image Condition;
    public Text TextFiremode;
    [SerializeField] public WeaponPromptText weaponPrompt;

    public List<Text> InvSlot;
    [SerializeField] private List<TextMeshProUGUI> ModSlotText;

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
        SetModListUI(weaponItem);
    }

    public void SetInventoryUI(SC_Inventory playerInventory)
    {
        for (int i = 0; i < InvSlot.Count; i++)
        {

            if (playerInventory.currentSlot == i)
                InvSlot[i].color = Color.white;
            else
                InvSlot[i].color = Color.black;

                InvSlot[i].gameObject.SetActive((playerInventory.weaponItemList[i].equippedWeaponStats != null));
        }
    }

    public void SetModListUI(WeaponItem weaponItem)
    {
        for (int i = 0; i < 5; i++)
        {
            var modSlot = ModSlotText[i];

            var modCount = weaponItem.equippedMods.Count;

            if (i >= modCount)
                modSlot.gameObject.SetActive(false);
            else
            {
                var wpnMod = weaponItem.equippedMods[i];

                if (wpnMod != null)
                {
                    modSlot.gameObject.SetActive(true);
                    modSlot.text = wpnMod.modName;
                }
                else
                {
                    modSlot.gameObject.SetActive(false);
                }
            }
        }
    }

    public void SetWeaponPromptText(WeaponItem weaponItem)
    {
        weaponPrompt.TextName.text = weaponItem.equippedWeaponStats.weaponName;
        weaponPrompt.TextDesc.text = weaponItem.equippedWeaponStats.weaponDesc;
    }

    public void SetActiveWeaponPromptText(bool setActive,bool showKeyPrompt = false)
    {
        weaponPrompt.Prompt.SetActive(setActive);
        weaponPrompt.TextKeyPrompt.SetActive(showKeyPrompt);
        weaponPrompt.timeBar.gameObject.SetActive(showKeyPrompt);
    }
}
