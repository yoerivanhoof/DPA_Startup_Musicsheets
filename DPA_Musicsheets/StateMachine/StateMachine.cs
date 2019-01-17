using System;

namespace DPA_Musicsheets.StateMachine
{
    public class StateMachine
    {
        private State _state;
        public State State => _state;

        public event EventHandler<StateChangedEventArgs> StateChanged; 

        public StateMachine()
        {
            _state = new IdleState();
        }

        public void ChangeState(State state)
        {
            _state = state;
            StateChanged?.Invoke(this, new StateChangedEventArgs(_state));
        }

    }
}