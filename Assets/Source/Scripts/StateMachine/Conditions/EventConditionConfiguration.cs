using System;

namespace KazymiStateMachine.Conditions
{
    public class EventConditionConfiguration
    {
        public event Action action;

        public void Invoke()
        {
            action?.Invoke();
        }
    }
}