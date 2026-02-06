
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace KazymiStateMachine
{
    public class StateTransition : IStateTransition
    {
        public State StateTo { get; private set; }
        public StateCondition Condition { get; private set; }
        public event Action transitionDeInitialized;

        public StateTransition(State state, StateCondition stateConditionCondition)
        {
            StateTo = state;
            Condition = stateConditionCondition;
        }
        
        public void InitializeCondition()
        {
            Condition.InitializeCondition();
        }

        public void DeInitializeCondition()
        {
            Condition.DeInitializeCondition();
            transitionDeInitialized?.Invoke();
        }
    }

    public interface IStateTransition
    {
        public void InitializeCondition();
        public void DeInitializeCondition();
        public State StateTo { get; }
        public StateCondition Condition { get; }
    }
    
    public class RandomStateTransition : IStateTransition
    {
        public State StateTo => states[Random.Range(0, states.Count)];
        public StateCondition Condition { get; private set; }
        public event Action transitionDeInitialized;

        private List<State> states = new List<State>();

        public RandomStateTransition(State firstState, StateCondition stateConditionCondition)
        {
            Condition = stateConditionCondition;
            states.Add(firstState);
        }

        public void AddNewState(State state)
        {
            states.Add(state);
        }

        public void InitializeCondition()
        {
            Condition.InitializeCondition();
        }

        public void DeInitializeCondition()
        {
            Condition.DeInitializeCondition();
            transitionDeInitialized?.Invoke();
        }
    }

}