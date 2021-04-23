using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemScore<T>
{
    public T itemClass;
    public float Score;
}

[System.Serializable]
class RankClass
{
    [SerializeField] private float _rankingScore;
    [SerializeField] private float _weaponScore;
    public float rankingScoreToReach => _rankingScore;
    public float weaponScore => _weaponScore;

}

public class SC_WeaponPool : MonoBehaviour
{
    [SerializeField] private float currentWeaponScore;
    [SerializeField] private float currentComboScore;
    [SerializeField] private int currentComboRank;
    [SerializeField] List<RankClass> rankingList;
    [SerializeField] float degradeRate = 1f;

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

                if (_newScore + _newMod.Score <= currentWeaponScore)
                {
                    _newEquippedModList.Add(_newMod);
                    _newScore += _newMod.Score;
                }
            }

            bool passScore = _newScore <= currentWeaponScore;
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

    void Update()
    {
        var updatedCurrentComboScore = currentComboScore - (degradeRate * Time.deltaTime);
        currentComboScore = Mathf.Max(0, updatedCurrentComboScore);
        currentWeaponScore = rankingList[currentComboRank].weaponScore;

    }

    public void AddComboScore(float addNum)
    {
        currentComboScore += addNum;
        CheckAddNewWeapon();
    }

    private void CheckAddNewWeapon()
    {
        int rankToCompare = 0;

        for (int i = 0; i < rankingList.Count; i++)
        {
            float scoreToCompare = 0;

            //Get Score To Compare
            for (int j = 0; j < i; j++)
            {
                    scoreToCompare += rankingList[j].rankingScoreToReach;
            }

            if (currentComboScore >= scoreToCompare)
            {
                rankToCompare = i;
            }
        }


        if (currentComboRank < rankToCompare)
        {
            gameObject.GetComponent<SC_Inventory>().AskToSpawnWeapon(generateWeapon());
            currentComboRank = rankToCompare;
            //Debug.Log(getTotalRankScore(rankToCompare));

            //if (currentComboRank >= rankingList.Count)
            //{
            //    currentComboScore -= rankingList[currentComboRank].rankingScoreToReach;
            //    currentComboRank--;
            //}

        }

    }

    private float getTotalRankScore(int rank)
    {
        float scoreToCompare = 0;
        for (int j = 0; j < rank; j++)
            scoreToCompare += rankingList[j].rankingScoreToReach;

        return scoreToCompare;
    }

}

