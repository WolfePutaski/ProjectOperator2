using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_PlayerBrain : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    SC_PlayerHealth playerHealth;

    void Start()
    {
        TryGetComponent(out playerHealth);
        ForceUnPause();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            TogglePauseMenu();

        }
    }

    void ForceUnPause()
    {
        pauseMenu.SetActive(true);
        TogglePauseMenu();
    }

    void TogglePauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);

        bool isPausedMenuActive = pauseMenu.activeSelf;

        Time.timeScale = isPausedMenuActive ? 0 : 1;


        if (isPausedMenuActive)
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
