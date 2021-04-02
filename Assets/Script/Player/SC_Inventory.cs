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

    public WeaponItem(SCO_Weapon_Master weaponClass, List<SCO_WeaponMods> newEquippedMods)
    {
        equippedWeaponStats = Object.Instantiate(weaponClass);
        equippedMods = new List<SCO_WeaponMods>();

        if (newEquippedMods.Count > 0)
        {
            foreach (SCO_WeaponMods _WeaponMods in newEquippedMods)
            {
                bool incompatibleMod()
                {
                    foreach (SCO_WeaponMods currentMod in equippedMods)
                    {
                        if (_WeaponMods != null)
                        {
                            bool sameType = _WeaponMods.modType == currentMod.modType;
                            if (equippedMods.Contains(_WeaponMods) || sameType)
                            {
                                return true;
                            }
                        }
                        else return true;

                    }

                    equippedMods.Add(_WeaponMods);
                    return false;
                }

                if (incompatibleMod())
                    break;

                float modByPercent(float a)
                {
                    return 1 + a;
                }

                equippedWeaponStats.cycleTime /= (1 + _WeaponMods.ModFirerate);
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
    public List<WeaponItem> weaponItemList;
    public int currentSlot = 0;
    public bool NotReceiveInput;


    [Header("===Debug===")]
    public List<WeaponNMod> startWeaponList;

    void Start()
    {
        for (int i = 0; i < startWeaponList.Count; i++)
        {
            weaponItemList[i] = new WeaponItem(startWeaponList[i].weaponClass, startWeaponList[i].equippedMods);
        }
    }

    public void ChangeWeaponTo(int i)
    {
        currentSlot = weaponItemList[i].equippedWeaponStats? i : currentSlot ;
    }
    public void ReplaceWeapon(WeaponItem newWeapon)
    {
        weaponItemList[currentSlot] = newWeapon;
    }
}