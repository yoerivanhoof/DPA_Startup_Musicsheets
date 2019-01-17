namespace DPA_Musicsheets.StateMachine
{
    public class StateChangedEventArgs
    {
        public State State { get; }
        public StateChangedEventArgs(State state)
        {
            State = state;
        }
    }
}