using System.ComponentModel;
using DPA_Musicsheets.ViewModels;

namespace DPA_Musicsheets.StateMachine
{
    public class IdleState : State
    {
        public override void HandleClose(CancelEventArgs args)
        {
            ViewModelLocator.Cleanup();
        }

        public override string GetDescription()
        {
            return "Idle";
        }
    }
}