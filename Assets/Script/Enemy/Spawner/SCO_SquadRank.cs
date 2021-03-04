using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SCO_SquadRank_", menuName = "ScriptableObject_SquadRank")]
public class SCO_SquadRank : ScriptableObject
{
    [SerializeField] private List<SCO_Squad> squadList;
    public float spawnCooldown;
    public int maxEnemyCount;

    //public List<SC_Squad.EnemySize> specialEnemy;
}
