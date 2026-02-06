namespace KazymiStateMachine
{
    public class StateMachine : IStateMachine
    {
        public State CurrentState { get; private set; }

        public StateMachine(State state)
        {
            SetState(state);
        }

        public void FixedTick()
        {
            CurrentState.FixedTick();
        }

        public void Tick()
        {
            var newIndex = IsTransitionsCondition();
            if (newIndex != -1)
            {
                SetState(CurrentState.Transitions[newIndex].StateTo);
            }
            else
            {
                CurrentState.Tick();
            }
        }

        private int IsTransitionsCondition()
        {
            var currentTransitions = CurrentState.Transitions;
            for (var i = 0; i != currentTransitions.Count; i++)
            {
                var condition = currentTransitions[i].Condition;
                condition.Tick();
                if (condition.IsConditionSatisfied())
                {
                    return i;
                }
            }

            return -1;
        }

        public void SetState(State state)
        {
            CurrentState?.OnStateExit();
            CurrentState?.DeInitializeTransitions();

            CurrentState = state;
            CurrentState.OnStateEnter();
            CurrentState.InitializeTransitions();
        }
    }
}