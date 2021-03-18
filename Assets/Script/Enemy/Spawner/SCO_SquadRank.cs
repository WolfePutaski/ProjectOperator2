using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SCO_SquadRank_", menuName = "ScriptableObject_SquadRank")]
public class SCO_SquadRank : ScriptableObject
{
    [SerializeField] private List<SCO_Squad> _squadList;
    [SerializeField] private float _spawnCooldown;
    [SerializeField] private int _maxEnemyCount;

    public List<SCO_Squad> squadList { get { return _squadList; } }
    public float spawnCooldown { get { return _spawnCooldown; } }
    public int maxEnemyCount { get { return _maxEnemyCount; } }

    //public List<SC_Squad.EnemySize> specialEnemy;
}
