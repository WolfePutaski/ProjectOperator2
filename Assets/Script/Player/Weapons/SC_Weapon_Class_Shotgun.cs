using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SCO_Weapon_Shotgun", menuName = "ScriptableObject_Weapon_Shotgun")]
public class SC_Weapon_Class_Shotgun : SC_Weapon_Master
{
    public int PelletCount;
    public int Spread;
}
