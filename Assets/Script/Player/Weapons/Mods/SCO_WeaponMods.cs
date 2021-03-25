using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ModType { Magazine, Action, Muzzle, Stock }

[CreateAssetMenu(fileName = "SCO_WeaponMods_", menuName = "ScriptableObject_WeaponMods")]
public class SCO_WeaponMods : ScriptableObject
{
    public string modName;

    [SerializeField] public float ModMag;
    [SerializeField] public float ModReloadTime;
    [SerializeField] public bool ModForceOffPerfectLoad;

    public float ModFirerate;
    [SerializeField] public float ModCritChance;

    [SerializeField] public float ModKick;
    [SerializeField] public float ModSway;

    [SerializeField] public float ModDurability;
    [SerializeField] public float ModDegradeRate;
    [SerializeField] public float ModHeatRate;
    [SerializeField] public float ModHeatCooldownRate;




}
