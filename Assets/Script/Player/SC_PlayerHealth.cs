using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_PlayerHealth : SC_Health
{
    public bool isDead => HealthCurrent <= 0;
    [SerializeField] private GameObject GameOverScreen;
    private SC_PlayerBrain playerBrain;

    new void Start()
    {
        base.Start();
        TryGetComponent(out playerBrain);
    }

    public void Update()
    {    
        if (HealthCurrent <= 0)
        {
            Kill();
        }
    }
    public override void Kill()
    {
        gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.gray;

        GameOverScreen.SetActive(true);

        disablePlayerInput();
        SetHighScore();

        Debug.Log("PlayerDied");
    }

    public override void Damage(float damage)
    {
        base.Damage(damage);
        playerBrain.cameraFunctions.ShakeDamage();

        FindObjectOfType<SC_EnemySpawner>().forceSetRankForTime(0, 8);
    }

    public void disablePlayerInput()
    {
        playerBrain.SetDisableInput(true);
    }

    public void enablePlayerInput()
    {
        playerBrain.SetDisableInput(false);
    }

    public void SetHighScore()
    {
        var currentHighScore = PlayerPrefs.GetInt("HighScore", 0);
        var newScore = FindObjectOfType<SC_RecordScore>().killCount;
        if (newScore > currentHighScore)
            PlayerPrefs.SetInt("HighScore", newScore);
    }
}
