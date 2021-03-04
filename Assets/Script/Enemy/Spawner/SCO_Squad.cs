using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SCO_Squad_", menuName = "ScriptableObject_Squad")]
public class SCO_Squad : ScriptableObject
{
    [System.Serializable]
    public struct EnemySize
    {
        public GameObject enemy;
        public int count;
    }

    public float squadSpread;
    public List<EnemySize> enemyTypes;
}
