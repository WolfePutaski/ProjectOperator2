using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FireMode { SEMI, AUTO, BURST, MANUAL }

[System.Serializable]
public class WeaponFunctionProperties
{
    public int ammoInMag;
    public int currentHeat;
    public bool isLoaded = true;
    public FireMode CurrentFiremode;

    public WeaponFunctionProperties(SCO_Weapon_Master weaponClass)
    {
        ammoInMag = weaponClass.magCapacity;
        isLoaded = true;
        CurrentFiremode = weaponClass.fireMode;

       if (!weaponClass.isClosedBolt)
            ammoInMag--;
    }
}

[CreateAssetMenu(fileName = "SCO_Weapon_", menuName = "ScriptableObject_Weapon")]
public class SCO_Weapon_Master : ScriptableObject
{
    //Put Default Value Here
    public string weaponName;
    public AudioClip fireSound;

    [Header("=== Loading Mechanism ===")]
    public FireMode fireMode;
    public bool FiremodeToggle;
    public float cycleTime;
    public float reloadTime;
    public float reloadPerfectStart;
    public float reloadPerfectWindow;

    [Header("=== Ammo ===")]
    public float damage;
    public float critChance;
    public float velocity;
    public int magCapacity;
    public bool isClosedBolt; //Has extra ammo in chamber.
    public float TriggerDelay; //Delay before start firing after pressing trigger

    [Header("=== Recoil & Handling ===")]
    public float swayPeriod; //For every seconds in swayPeriod, the crosshair will move to new position. Guns with higher number will feel more shaky.
    public float swayRadius; //How far crosshair move around
    public float recoilKick; //Amount of Kick
    public float recoilRecoverySpeed; //How fast to recover from recoil

    [Header("=== Heat ===")]
    public float heatPerShot;
    public float heatCoolRate;
}
