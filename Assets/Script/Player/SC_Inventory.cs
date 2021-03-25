using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponItem
{
    public SCO_Weapon_Master equippedWeaponStats;
    public WeaponFunctionProperties equippedWeaponProperties;
    public List<SCO_WeaponMods> equippedMods;
    
    public WeaponItem(SCO_Weapon_Master weaponClass)
    {
        equippedWeaponStats = Object.Instantiate(weaponClass);
        equippedWeaponProperties = new WeaponFunctionProperties(weaponClass);

    }        

    public WeaponItem(SCO_Weapon_Master weaponClass, List<SCO_WeaponMods> equippedMods)
    {
        equippedWeaponStats = Object.Instantiate(weaponClass);

        if (equippedMods.Count > 0)
        {
            foreach (SCO_WeaponMods _WeaponMods in equippedMods)
            {
                float modByPercent(float a)
                {
                    return 1 + a;
                }

                

                equippedWeaponStats.cycleTime = 60f/( (60f/equippedWeaponStats.cycleTime) * (1+_WeaponMods.ModFirerate));
                equippedWeaponStats.reloadTime *= (1 + _WeaponMods.ModReloadTime);
                equippedWeaponStats.reloadPerfectStart *= (1 + _WeaponMods.ModReloadTime);

                if (_WeaponMods.ModForceOffPerfectLoad)
                    equippedWeaponStats.reloadPerfectWindow = 0;

                equippedWeaponStats.magCapacity += (int)(equippedWeaponStats.magCapacity * _WeaponMods.ModMag);

                equippedWeaponStats.recoilKick *= modByPercent(_WeaponMods.ModKick);
                equippedWeaponStats.swayOffset *= modByPercent(_WeaponMods.ModSway);
            }

            equippedWeaponStats.weaponName += "+";
        }

        equippedWeaponProperties = new WeaponFunctionProperties(equippedWeaponStats);
    }
}

[System.Serializable]
public class WeaponNMod
{
    [SerializeField] public SCO_Weapon_Master weaponClass;
    [SerializeField] public List<SCO_WeaponMods> equippedMods;
}

public class SC_Inventory : MonoBehaviour
{
    public List<WeaponItem> weaponItems;
    public int currentSlot = 0;
    public bool NotReceiveInput;


    [Header("===Debug===")]
    public List<WeaponNMod> startWeaponList;

    void Start()
    {
        for (int i = 0; i < startWeaponList.Count; i++)
        {
            weaponItems[i] = new WeaponItem(startWeaponList[i].weaponClass, startWeaponList[i].equippedMods);
        }
    }

    public void ChangeWeaponTo(int i)
    {
        currentSlot = weaponItems[i].equippedWeaponStats? i : currentSlot ;
    }
}


