using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_PlayerBrain : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] bool enableDebug;
    SC_PlayerHealth _Health;
    SC_Inventory _Inventory;
    SC_WeaponFunction _WeaponFunction;
    SC_WeaponPool _WeaponPool;
    Animator _animator;
    SC_PlayerUI _PlayerUI;
    private SC_LookWithMouse _lookWithMouse;
    private SC_CameraFunctions _cameraFunctions;
    public SC_CameraFunctions cameraFunctions { get { return _cameraFunctions; } }

    private bool NotRecieveInput;

    void Start()
    {
        TryGetComponent(out _Health);
        TryGetComponent(out _Inventory);
        TryGetComponent(out _lookWithMouse);
        TryGetComponent(out _WeaponFunction);
        TryGetComponent(out _animator);
        TryGetComponent(out _WeaponPool);
        TryGetComponent(out _PlayerUI);
        _cameraFunctions = FindObjectOfType<SC_CameraFunctions>();
        ForceUnPause();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            TogglePauseMenu();
        }

        if (!NotRecieveInput)
        {
            InputChangeWeapon();
            InputWeapon();
            _lookWithMouse.enabled = true;
            _WeaponFunction.enabled = true;

            if(enableDebug)
            {
                DebugInputSpawnWeapon();
            }

            //Accept & Decline when Prompt = True
        }

        else
        {
            _lookWithMouse.enabled = false;
            _WeaponFunction.enabled = false;
        }
        
    }

    //void UpdateAnimation()
    //{
    //    _WeaponFunction.currentWeapon;
    //}

    void DebugInputSpawnWeapon()
    {
        if(Input.GetKeyDown(KeyCode.Y))
        {
            Debug.Log("SpawnWeapon");
            var newWeapon = _WeaponPool.generateWeapon();
            if (newWeapon != null)
            {
                _Inventory.AskToSpawnWeapon(newWeapon);
            }
        }
    }

    void InputChangeWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            _Inventory.ChangeWeaponTo(0);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            _Inventory.ChangeWeaponTo(1);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            _Inventory.ChangeWeaponTo(2);
    }
    void InputWeapon()
    {
        if (_Inventory.weaponItemList[_Inventory.currentSlot] != null)
        {
            _WeaponFunction.Input_Fire();
            _WeaponFunction.Input_ChangeFireMode();
            _WeaponFunction.Input_Reload();
            _WeaponFunction.Input_RackSlide();
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
            _Health.disablePlayerInput();
        }
        else
        {
            if (!_Health.isDead)
                _Health.enablePlayerInput();
        }
    }
    public void SetDisableInput(bool boolean)
    {
        NotRecieveInput = boolean;
    }

}
