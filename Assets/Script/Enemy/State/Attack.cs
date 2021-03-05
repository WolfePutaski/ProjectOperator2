using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyState
{
    public class Attack : IState
    {
        float attackTimeCount;
        float _timeToAttack;
        SC_Health _target;
        SC_EnemySystem _enemySystem;

        public Attack(float timeToAttack, SC_Health target)
        {
            _timeToAttack = timeToAttack;
            _target = target;
        }

        public Attack(SC_EnemySystem enemySystem)
        {
            _enemySystem = enemySystem;
            _timeToAttack = enemySystem.timeToAttack;
            _target = GameObject.FindGameObjectWithTag("Player").GetComponent<SC_Health>();
        }
        public void Tick()
        {
            attackTimeCount += Time.deltaTime;

            if (attackTimeCount > _timeToAttack)
            {
                _target.Damage(10);
                _enemySystem.GetComponent<SC_Health>().Kill();
                SC_CameraFunctions.sharedInstance.ShakeDamage();

            }
        }

        public void OnEnter()
        {
            attackTimeCount = 0;
        }

        public void OnExit()
        {
        }
    }
}
