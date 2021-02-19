using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SCO_Weapon_Shotgun", menuName = "ScriptableObject_Weapon_Shotgun")]
public class SCO_Weapon_Class_Shotgun : SCO_Weapon_Master
{
    [Header("=== Shotgun Property ===")]
    public int PelletCount;
    public float Spread;
}
