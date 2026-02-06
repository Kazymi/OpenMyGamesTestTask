using System;
using UnityEngine;

namespace KazymiStateMachine.Conditions
{
    public class TemporaryAndFuncCondition : StateCondition
    {
        private readonly Func<bool> _func;
        private readonly float _time;

        private float _currentTime;

        private bool _isTimeIsUp;

        public TemporaryAndFuncCondition(Func<bool> func,float time)
        {
            _time = time;
            _func = func;
            _currentTime = time;
        }
        
        public override bool IsConditionSatisfied()
        {
            return _isTimeIsUp;
        }

        public override void Tick()
        {
            if (_func.Invoke())
            {
                _currentTime -= Time.deltaTime;
                if (_currentTime < 0) _isTimeIsUp = true;
            }
            else
            {
                _currentTime = _time;
            }
        }

        public override void DeInitializeCondition()
        {
            _currentTime = _time;
            _isTimeIsUp = false;
        }
    }
}