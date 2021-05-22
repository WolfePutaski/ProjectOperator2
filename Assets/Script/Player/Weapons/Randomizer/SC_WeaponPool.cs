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
public class RankClass
{
    [SerializeField] private float _rankingScore;
    [SerializeField] private float _weaponScore;
    public float rankingScoreToReach => _rankingScore;
    public float weaponScore => _weaponScore;

}

public class SC_WeaponPool : MonoBehaviour
{
    private float _currentWeaponScore;
    private float _currentComboScore;
    private int _currentComboRank;
    [SerializeField] public List<RankClass> RankingList;
    [SerializeField] float degradeRate = 1f;

    public SCO_WeaponPoolProfile _weaponPoolProfile;
    private List<ItemScore<SCO_Weapon_Master>> _weaponList => _weaponPoolProfile.weaponList;
    private List<ItemScore<SCO_WeaponMods>> _weaponModList => _weaponPoolProfile.weaponModList;

    public float CurrentComboScore { get => _currentComboScore; }
    public int CurrentComboRank { get => _currentComboRank; }
    public float hitScore;

    public WeaponItem generateWeapon()
    {
        if (_weaponList.Count <= 0 && _currentComboScore <= 0)
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

                if (_newScore + _newMod.Score <= _currentWeaponScore)
                {
                    _newEquippedModList.Add(_newMod);
                    _newScore += _newMod.Score;
                }
            }

            bool passScore = _newScore <= _currentWeaponScore;
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
        var updatedCurrentComboScore = _currentComboScore - (degradeRate * Time.deltaTime);
        _currentComboScore = Mathf.Max(0, updatedCurrentComboScore);

        if (_currentComboRank >= 1 && (_currentComboScore < getTotalRankScore(_currentComboRank - 1)))
        {
            DemoteRank();
        }

    }

    public void AddComboScore(float addNum, bool checkNewWeapon = true)
    {
        _currentComboScore += addNum;
        if (checkNewWeapon)
            CheckAddNewWeapon();
    }

    public void CheckAddNewWeapon()
    {
        if (_currentComboScore >= getTotalRankScore(_currentComboRank))
        {
            if (_currentComboRank >= RankingList.Count - 1)
            {
                _currentComboRank = RankingList.Count - 1;
                _currentComboScore -= RankingList[_currentComboRank].rankingScoreToReach;
            }
            else
                _currentComboRank++;

            _currentWeaponScore = RankingList[_currentComboRank].weaponScore;
            _currentComboScore += degradeRate * 5f;
            gameObject.GetComponent<SC_Inventory>().AskToSpawnWeapon(generateWeapon());
        }
    }

    public void DemoteRank()
    {
        if (_currentComboRank > 0)
        {
            _currentComboRank--;
            _currentComboScore = getTotalRankScore(_currentComboRank) - RankingList[_currentComboRank].rankingScoreToReach * .66f;

        }
    }

    public float getTotalRankScore(int rank)
    {
        float scoreToCompare = 0;
        for (int j = 0; j <= rank; j++)
            scoreToCompare += RankingList[j].rankingScoreToReach;

        return scoreToCompare;
    }

}

