
namespace KazymiStateMachine
{
    public interface IStateMachine
    {
        State CurrentState { get; }
        void SetState(State state);
        void Tick();
        void FixedTick();
    }
}