using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SC_PlayerUI : MonoBehaviour
{
    SC_WeaponUI weaponUI;
    [SerializeField] private Text killCount;
    [SerializeField] Text healthUI;
    SC_Inventory inventory;
    SC_Health health;

    SC_RecordScore recordScore;

    void Start()
    {
        TryGetComponent(out inventory);
        TryGetComponent(out health);

        weaponUI = FindObjectOfType<SC_WeaponUI>();
        recordScore = FindObjectOfType<SC_RecordScore>();
    }
    void Update()
    {
       var  currentWeapon = inventory.weaponItems[inventory.currentSlot];

        weaponUI.SetInventoryUI(inventory);
        weaponUI.SetWeaponUIText(currentWeapon);

        killCount.text = FindObjectOfType<SC_RecordScore>().killCount.ToString();
        SetHealthUI();

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
}
