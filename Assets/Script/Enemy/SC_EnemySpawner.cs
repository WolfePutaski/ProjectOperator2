using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_EnemySpawner : MonoBehaviour
{
    public float spawnRadius;
    public List<SCO_SquadRank> squadRank;

    [SerializeField] private int _currentRank = 1;
    private float _spawnCooldownCount;
    private float _scoreRank;

    public float scoreRankDegRate;
    public float maxScoreRank;

    bool CoolDownFinished => _spawnCooldownCount <= 0;
    bool EnemyCountIsMax => GameObject.FindGameObjectsWithTag("Enemy").Length >= squadRank[_currentRank].maxEnemyCount;

    // Update is called once per frame
    void Update()
    {
        var currentSquadRank = squadRank[_currentRank];

        if (!CoolDownFinished)
            _spawnCooldownCount -= Time.deltaTime;

        else
        {
            if (!EnemyCountIsMax)
            {
                SpawnSquad(currentSquadRank.squadList[Random.Range(0, currentSquadRank.squadList.Count)]);
                _spawnCooldownCount = currentSquadRank.spawnCooldown;
            }

        }

        UpdateRank();
    }

    void SpawnSquad(SCO_Squad squadToSpawn)
    {
        Vector2 SquadSpawnLocation = Random.insideUnitCircle.normalized * spawnRadius;

        foreach (SCO_Squad.EnemySize nme in squadToSpawn.enemyTypes)
        {
            for (int i = 0; i < nme.count; i++)
            {
                GameObject newNme = Instantiate(nme.enemy);
                newNme.transform.position = SquadSpawnLocation + Random.insideUnitCircle.normalized * squadToSpawn.squadSpread;
            }
        }

    }

    void UpdateRank()
    {
        if (_currentRank == 1)
        {
            _scoreRank = Mathf.Max(0, _scoreRank - Time.deltaTime * scoreRankDegRate);

            if(_scoreRank >= maxScoreRank)
            {
                StartCoroutine(_forceSetRankForTime(2, 20));
            }
        }
    }

    public void forceSetRankForTime(int newRank, float time)
    {
        StopAllCoroutines();
        StartCoroutine(_forceSetRankForTime(newRank, time));
    }

    IEnumerator _forceSetRankForTime(int newRank, float time)
    {
        _scoreRank = 0;
        _currentRank = newRank;

        yield return new WaitForSeconds(time);

        _scoreRank = 0;

        if (_currentRank == newRank)
            _currentRank = 1;
    }

    public void SetRankScore(float newRankScore)
    {

    }    

    public void AddRankScore(float modRankScore)
    {
        _scoreRank += modRankScore;
    }
}
