using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_PlayerBrain : MonoBehaviour
{
    GameObject pauseMenu;
    SC_PlayerHealth playerHealth;

    void Start()
    {
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        TryGetComponent(out playerHealth);
        ForceOffPause();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            TogglePauseMenu();

        }
    }

    void ForceOffPause()
    {
        pauseMenu.SetActive(true);
        TogglePauseMenu();
    }

    void TogglePauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);

        bool isPaused = pauseMenu.activeSelf;

        Time.timeScale = isPaused ? 0 : 1;


        if (isPaused)
        {
            playerHealth.disablePlayerInput();
        }
        else
        {
            if (!playerHealth.isDead)
                playerHealth.enablePlayerInput();
        }
    }
}
