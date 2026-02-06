using System;
using UnityEngine;

namespace KazymiStateMachine.Conditions
{
    public class TemporaryAndFuncConditionWithGettingReadyToLeave : StateCondition
    {
        private readonly Func<bool> _func;
        private readonly float _timeToLeave;
        private readonly float _timeToFunc;
        private readonly IPreparedForExit _preparedForExit;


        private float _currentFuncTime;
        private float _currentLeaveTime;

        private bool _isTimeIsUp;
        private bool _isReadyToLeave;

        public TemporaryAndFuncConditionWithGettingReadyToLeave(Func<bool> func, IPreparedForExit preparedForExit,
            float time, float timeToLeave)
        {
            _timeToFunc = time;
            _timeToLeave = timeToLeave;
            _currentLeaveTime = timeToLeave;
            _func = func;
            _preparedForExit = preparedForExit;
            _currentFuncTime = _timeToFunc;
        }

        public override bool IsConditionSatisfied()
        {
            return _isReadyToLeave;
        }

        public override void Tick()
        {
            if (_func.Invoke() && _isTimeIsUp == false)
            {
                _currentFuncTime -= Time.deltaTime;
                if (_currentFuncTime < 0)
                {
                    _preparedForExit.GettingReadyToLeave();
                    _isTimeIsUp = true;
                }
            }
            else
            {
                _currentFuncTime = _timeToFunc;
            }

            if (_isTimeIsUp)
            {
                _currentLeaveTime -= Time.deltaTime;
                if (_currentLeaveTime < 0)
                {
                    _isReadyToLeave = true;
                }
            }
        }

        public override void DeInitializeCondition()
        {
            _currentFuncTime = _timeToFunc;
            _currentLeaveTime = _timeToLeave;
            _isTimeIsUp = false;
            _isReadyToLeave = false;
        }
    }
}