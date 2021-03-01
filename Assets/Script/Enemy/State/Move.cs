using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyState
{
    public class Move : IState
    {
        SC_MoveToObject _moveToObject;
        public Move (SC_MoveToObject moveToSystem, Transform moveTarget)
        {
            _moveToObject = moveToSystem;
            _moveToObject.target = moveTarget;

        }

        public void Tick()
        {

        }

        public void OnEnter()
        {
            _moveToObject.speed = 1f;
        }

        public void OnExit()
        {
            _moveToObject.speed = 0f;

        }
    }
}

