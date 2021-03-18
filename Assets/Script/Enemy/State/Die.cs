using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyState
{
    public class Die : IState
    {
        private SC_EnemySystem _enemySystem;

        public Die(SC_EnemySystem enemySystem)
        {
            _enemySystem = enemySystem;
        }
        public void Tick()
        {
        }

        public void OnEnter()
        {
            _enemySystem.GetComponent<SC_Health>().Kill();

            var recordScore = Object.FindObjectOfType<SC_RecordScore>();
            var enemySpawner = Object.FindObjectOfType<SC_EnemySpawner>();

            recordScore.addKillCount();
            enemySpawner.AddRankScore(100f);
        }

        public void OnExit()
        {

        }


    }
}
