using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="SCO_WeaponPoolProfile_",menuName = "ScriptableObject_WeaponPoolProfile")]
public class SCO_WeaponPoolProfile : ScriptableObject
{
    public List<ItemScore<SCO_Weapon_Master>> weaponList;
    public List<ItemScore<SCO_WeaponMods>> weaponModList;
}
