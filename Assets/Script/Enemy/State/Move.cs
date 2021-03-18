using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyState
{
    public class Move : IState
    {
        SC_EnemySystem _enemySystem;
        SC_MoveToObject _moveToObject;

        public Move (SC_EnemySystem enemySystem, SC_MoveToObject moveToSystem, Transform moveTarget)
        {
            _enemySystem = enemySystem;
            _moveToObject = moveToSystem;
            _moveToObject.target = moveTarget;

        }

        public void Tick()
        {
            if (_moveToObject.speed < _enemySystem.DefaultSpeed)
            {
                _moveToObject.speed = Mathf.Min(_moveToObject.speed + (_enemySystem.Acceleration * Time.deltaTime), _enemySystem.DefaultSpeed);
            }

        }

        public void OnEnter()
        {
            _moveToObject.speed = 0 ;
        }

        public void OnExit()
        {
            _moveToObject.speed = 0f;

        }
    }
}

