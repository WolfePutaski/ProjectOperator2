using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SCO_Enemy_", menuName = "ScriptableObject_Enemy")]
public class SCO_Enemy : ScriptableObject
{
    public Sprite sprite;

    [Header("===Speed===")]
    public float DefaultSpeed;
    public float minSpeed;
    public float Acceleration;

    [Header("===Attack===")]
    public float timeToAttack;
}
