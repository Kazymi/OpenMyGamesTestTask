using System;
using UnityEngine;

namespace KazymiStateMachine.Conditions
{
    public class FuncCondition : StateCondition
    {
        private readonly Func<bool> _func;

        public FuncCondition(Func<bool> func)
        {
            _func = func;
        }

        public override bool IsConditionSatisfied()
        {
            return _func.Invoke();
        }
    }

    public class AnimationFinishCondition : StateCondition
    {
        private readonly Animator animator;
        private readonly string name;
        private readonly float finishTime;

        public AnimationFinishCondition(Animator animator, string name, float finishTime = 0.8f)
        {
            this.animator = animator;
            this.name = name;
            this.finishTime = finishTime;
        }

        public override bool IsConditionSatisfied()
        {
            return animator.GetCurrentAnimatorStateInfo(0).normalizedTime > finishTime && animator
                .GetCurrentAnimatorStateInfo(0)
                .IsName(name);
        }
    }
}