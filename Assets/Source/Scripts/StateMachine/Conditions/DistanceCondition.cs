using UnityEngine;

namespace KazymiStateMachine.Conditions
{
    public class DistanceCondition : StateCondition
    {
        private readonly Transform _firstTarget;
        private readonly Transform _secondTarget;
        private readonly float _maxDistance;
        public DistanceCondition(Transform firstTarget, Transform secondTarget,float maxDistance)
        {
            _firstTarget = firstTarget;
            _secondTarget = secondTarget;
            _maxDistance = maxDistance;
        }
        public override bool IsConditionSatisfied()
        {
            return Vector3.Distance(_firstTarget.position, _secondTarget.position) < _maxDistance;
        }
    }
}