using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SC_PlayerUI : MonoBehaviour
{
    SC_WeaponUI _weaponUI;
    public SC_WeaponUI weaponUI { get { return _weaponUI; } }
    [SerializeField] private Text killCount;
    [SerializeField] Text healthUI;
    SC_Inventory inventory;
    SC_Health health;
    SC_WeaponPool weaponPool;

    SC_RecordScore recordScore;

    [SerializeField] Image _comboCircle;
    [SerializeField] Text _comboRank;

    void Start()
    {
        TryGetComponent(out inventory);
        TryGetComponent(out health);
        TryGetComponent(out weaponPool);


        _weaponUI = FindObjectOfType<SC_WeaponUI>();
        recordScore = FindObjectOfType<SC_RecordScore>();
    }

    void Update()
    {
       var  currentWeapon = inventory.weaponItemList[inventory.currentSlot];

        _weaponUI.SetInventoryUI(inventory);
        _weaponUI.SetWeaponUIText(currentWeapon);

        killCount.text = FindObjectOfType<SC_RecordScore>().killCount.ToString();
        SetHealthUI();
        SetComboUI();
    }

    void SetHealthUI()
    {
        string healthbar = "";
        for (int i = 0; i < health.HealthCurrent/5;i++)
        {
            healthbar += "I";
        }
        healthUI.text = "HEALTH: " + healthbar;
    }

    void SetComboUI()
    {
        //Set Circle
        float scoreToSubtract = 0;
        if (weaponPool.CurrentComboRank > 0)
            scoreToSubtract = weaponPool.getTotalRankScore(weaponPool.CurrentComboRank - 1);

        _comboCircle.fillAmount = (weaponPool.CurrentComboScore - scoreToSubtract) / weaponPool.RankingList[weaponPool.CurrentComboRank].rankingScoreToReach;

        //Set Text
        List<string> RankName = new List<string>();
        RankName.Add("");
        RankName.Add("D");
        RankName.Add("C");
        RankName.Add("B");
        RankName.Add("A");
        RankName.Add("S");

        _comboRank.text = RankName[weaponPool.CurrentComboRank];
    }


}
