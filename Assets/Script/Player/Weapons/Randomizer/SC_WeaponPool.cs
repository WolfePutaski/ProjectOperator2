using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemScore<T>
{
    public T itemClass;
    public float Score;
}
public class SC_WeaponPool : MonoBehaviour
{
    public float currentScore;

    public SCO_WeaponPoolProfile _weaponPoolProfile;

    private List<ItemScore<SCO_Weapon_Master>> _weaponList => _weaponPoolProfile.weaponList;
    private List<ItemScore<SCO_WeaponMods>> _weaponModList => _weaponPoolProfile.weaponModList;

    public WeaponItem generateWeapon()
    {
        if (_weaponList.Count <= 0)
            return null;

        SCO_Weapon_Master newWeaponClass = null;
        List<SCO_WeaponMods> newEquippedModList = new List<SCO_WeaponMods>();

        while (newWeaponClass == null)
        {
            float _newScore = 0;

            ItemScore<SCO_Weapon_Master> _weaponScore = _weaponList[Random.Range(0, _weaponList.Count)];
            _newScore += _weaponScore.Score;

            List<ItemScore<SCO_WeaponMods>> _newEquippedModList = new List<ItemScore<SCO_WeaponMods>>();
            for (int i = 0; i < 3; i++)
            {
                var _newMod = _weaponModList[Random.Range(0, _weaponModList.Count)];

                if (_newScore + _newMod.Score <= currentScore)
                {
                    _newEquippedModList.Add(_newMod);
                    _newScore += _newMod.Score;
                }
            }

            bool passScore = _newScore <= currentScore;
            if (passScore)
            {
                foreach (ItemScore<SCO_WeaponMods> _mod in _newEquippedModList)
                {
                    newEquippedModList.Add(_mod.itemClass);
                }

                newWeaponClass = _weaponScore.itemClass;
            }

        }

        return new WeaponItem(newWeaponClass,newEquippedModList);
    }
}

