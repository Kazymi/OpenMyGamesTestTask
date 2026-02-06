using UnityEngine;

namespace KazymiStateMachine.Conditions
{
    public class TemporaryCondition : StateCondition
    {
        private float _currentTime;
        private readonly float _time;
        
        public TemporaryCondition(float time)
        {
            _time = time;
            _currentTime = time;
        }

        public override void Tick()
        {
            _currentTime -= Time.deltaTime;
        }

        public override bool IsConditionSatisfied()
        {
            return _currentTime <= 0;
        }

        public override void DeInitializeCondition()
        {
            _currentTime = _time;
        }
    }
}