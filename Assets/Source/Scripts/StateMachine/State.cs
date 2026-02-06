using System.Collections.Generic;

namespace KazymiStateMachine
{
    public class State
    {
        public List<IStateTransition> Transitions { get; private set; } = new List<IStateTransition>();
        public virtual void Tick()
        {
        }

        public virtual void FixedTick()
        {
        }
        
        public virtual void OnStateEnter()
        {
           
        }

        public virtual void OnStateExit()
        {
        }
        
        public virtual void InitializeTransitions()
        {
            foreach (var transition in Transitions)
            {
                transition.InitializeCondition();
            }
        }

        public virtual void DeInitializeTransitions()
        {
            foreach (var transition in Transitions)
            {
                transition.DeInitializeCondition();
                if (Transitions.Contains(transition) == false)
                {
                    DeInitializeTransitions();
                    return;
                }
            }
        }

        public void AddTransition(IStateTransition transition)
        {
            Transitions.Add(transition);
        }

        public void RemoveTransition(IStateTransition transition)
        {
            if (Transitions.Contains(transition))
            {
                Transitions.Remove(transition);
            }
        }
        public void RemoveTransitions()
        {
            Transitions = new List<IStateTransition>();
        }
    }
}