using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Squad
{
    [System.Serializable]
    public struct EnemySize
    {
        public GameObject enemy;
        public int count;
    }

    public float squadSpread;
    public List<EnemySize> enemies;
    
}

public class SC_EnemySpawner : MonoBehaviour
{
    public float spawnRadius;
    public float spawnCooldown;
    float spawnCooldownCount;

    public int maxEnemyCount;

    public List<Squad> squads;

    public int SquadRank;

    public bool CoolDownFinished => spawnCooldownCount <= 0;
    public bool EnemyCountIsMax => GameObject.FindGameObjectsWithTag("Enemy").Length >= maxEnemyCount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!CoolDownFinished)
            spawnCooldownCount -= Time.deltaTime;

        else
        {
            if (!EnemyCountIsMax)
            {
                SpawnSquad(squads[Random.Range(0,squads.Count)]);
                spawnCooldownCount = spawnCooldown;
            }

        }    
    }

    void SpawnSquad(Squad squadToSpawn)
    {
        Vector2 SquadSpawnLocation = Random.insideUnitCircle.normalized * spawnRadius;

        foreach (Squad.EnemySize nme in squadToSpawn.enemies)
        {
            for (int i = 0; i < nme.count; i++)
            {
                GameObject newNme = Instantiate(nme.enemy);
                newNme.transform.position = SquadSpawnLocation + Random.insideUnitCircle.normalized * squadToSpawn.squadSpread;
            }
        }

    }
}
