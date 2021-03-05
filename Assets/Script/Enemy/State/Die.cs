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
            SC_RecordScore.scoreRecorder.addKillCount();
        }

        public void OnExit()
        {

        }


    }
}
