namespace VitaliyNULL.StateMachine
{
    public interface IStateSwitcher
    {
        void SwitchState<T>() where T : State;
    }
}