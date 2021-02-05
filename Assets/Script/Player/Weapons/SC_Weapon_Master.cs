using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct WeaponFunctionProperties
{
    public int ammoInMag;
    public int currentHeat;
    public int isLoaded;
}

[CreateAssetMenu(fileName = "SCO_Weapon_", menuName = "ScriptableObject_Weapon")]
public class SC_Weapon_Master : ScriptableObject
{
    //Put Default Value Here
    public string weaponName;
    public AudioClip fireSound;
    public enum FireMode {SEMI,AUTO,MANUAL}
    public FireMode fireMode;
    public float rateOfFire;
    public float damage;
    public float velocity;
    public int magCapacity;
    public bool isClosedBolt; //Has extra ammo in chamber.

    //Recoil & Handling
    public float swaySpeed; //How fast gun aim move around
    public float swayRadius; //How far gun aim move around
    public float recoilKick; //Amount of Kick
    public float recoilRecoverySpeed; //How fast to recover from recoil

    //Heat
    public float heatPerShot;
    public float heatCoolRate;
}
