using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_WeaponPool : MonoBehaviour
{
    public List<SCO_Weapon_Master> weaponList;
    public List<SCO_WeaponMods> weaponModList;

    public WeaponItem createWeapon()
    {
        List<SCO_WeaponMods> equippedMods = new List<SCO_WeaponMods>();

        for (int i = 0; i < 3; i++)
        {
            equippedMods.Add(weaponModList[Random.Range(0, weaponModList.Count)]);
        }

        return new WeaponItem(weaponList[Random.Range(0, weaponList.Count)],equippedMods);
    }
}

