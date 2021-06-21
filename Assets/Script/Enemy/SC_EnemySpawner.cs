using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyLimit
{
    public GameObject enemyToLimit;
    public int limitCount;
    public float overLimitSpawnChance;
}

public class SC_EnemySpawner : MonoBehaviour
{
    public float spawnRadius;
    public List<SCO_SquadRank> squadRank;

    public List<EnemyLimit> enemyLimitList;

    [SerializeField] private int _currentRank = 1;
    private float _spawnCooldownCount;
    private float _scoreRank;

    public float scoreRankDegRate;
    public float maxScoreRank;

    bool CoolDownFinished => _spawnCooldownCount <= 0;
    int enemyCount;
    bool EnemyCountIsMax => enemyCount >= squadRank[_currentRank].maxEnemyCount;

    void OnEnable()
    {
        forceSetRankForTime(0, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsOfType<SC_EnemySystem>().Length;

        var currentSquadRank = squadRank[_currentRank];

        if (!EnemyCountIsMax)
        {
            if (!CoolDownFinished)
                _spawnCooldownCount -= Time.deltaTime;

            else
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
                if (CheckSpawn())
                    spawnEnemy(nme.enemy);
            }

            bool CheckSpawn()
            {
                foreach (EnemyLimit nmeLimit in enemyLimitList)
                {
                    if (nme.enemy == nmeLimit.enemyToLimit)
                    {
                        if (!CheckEnemyTypeCount(nme.enemy, nmeLimit.limitCount))
                        {
                            float randomNum = Random.Range(0f, 1f);
                            if (randomNum > nmeLimit.overLimitSpawnChance)
                            {
                                return false;
                            }
                        }

                    }

                }
                return true;
            }
        }



        void spawnEnemy(GameObject nmeToSpawn)
        {
            GameObject newNme = Instantiate(nmeToSpawn);
            newNme.transform.position = SquadSpawnLocation + Random.insideUnitCircle.normalized * squadToSpawn.squadSpread;
        }

    }

    void UpdateRank()
    {
        if (_currentRank == 1)
        {
            _scoreRank = Mathf.Max(0, _scoreRank - Time.deltaTime * scoreRankDegRate);

            if(_scoreRank >= maxScoreRank && CheckPlayerInventory())
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

        Debug.Log("FINISH FORCE SET RANK");

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

    public bool CheckPlayerInventory()
    {
        SC_Inventory playerInventory;
        playerInventory = FindObjectOfType<SC_Inventory>();
        return playerInventory.weaponItemList.Count >= 3 ? true : false;
    }

    public bool CheckEnemyTypeCount(GameObject nmeToCheck, int maxCount)
    {
        GameObject[] FindGameObjectsWithSameName(string name)
        {
            GameObject[] allObjs = Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
            List<GameObject> likeNames = new List<GameObject>();
            foreach (GameObject obj in allObjs)
            {
                if (obj.name == name)
                {
                    likeNames.Add(obj);
                }
            }
            return likeNames.ToArray();
        }

        return FindGameObjectsWithSameName(nmeToCheck.name + "(Clone)").Length < maxCount;
    }
}
