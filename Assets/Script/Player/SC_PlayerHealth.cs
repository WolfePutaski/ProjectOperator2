using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_PlayerHealth : SC_Health
{
    public bool isDead => HealthCurrent <= 0;
    [SerializeField] private GameObject GameOverScreen;

    public void Update()
    {    
        if (HealthCurrent <= 0)
        {
            Kill();
        }
    }
    public new void Kill()
    {
        gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.gray;

        GameOverScreen.SetActive(true);

        disablePlayerInput();
        SetHighScore();

        Debug.Log("PlayerDied");
    }

    public void disablePlayerInput()
    {
        gameObject.GetComponentInChildren<SC_WeaponFunction>().enabled = false;
        gameObject.GetComponentInChildren<SC_LookDir>().enabled = false;
        gameObject.GetComponentInChildren<SC_Inventory>().NotReceiveInput = true;
    }

    public void enablePlayerInput()
    {
        gameObject.GetComponentInChildren<SC_WeaponFunction>().enabled = true;
        gameObject.GetComponentInChildren<SC_LookDir>().enabled = true;
        gameObject.GetComponentInChildren<SC_Inventory>().NotReceiveInput = false;
    }

    public void SetHighScore()
    {
        var currentHighScore = PlayerPrefs.GetInt("HighScore", 0);
        var newScore = SC_RecordScore.SharedInstance.killCount;
        if (newScore > currentHighScore)
            PlayerPrefs.SetInt("HighScore", newScore);
    }
}
