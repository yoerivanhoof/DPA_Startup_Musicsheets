using System.ComponentModel;

namespace DPA_Musicsheets.StateMachine
{
    public abstract class State
    {
        public abstract void HandleClose(CancelEventArgs args);

        public abstract string GetDescription();
    }
}