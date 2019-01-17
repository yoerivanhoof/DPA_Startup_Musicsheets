using System.ComponentModel;
using System.Windows;
using DPA_Musicsheets.ViewModels;

namespace DPA_Musicsheets.StateMachine
{
    public class UnsavedChangesState : State
    {
        public override void HandleClose(CancelEventArgs args)
        {
            var result = MessageBox.Show("You have unsaved changes.\nDo you want to quit?", "Are you sure?",
                MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No)
            {
                args.Cancel = true;
            }
            else
            {
                ViewModelLocator.Cleanup();
            }
        }

        public override string GetDescription()
        {
            return "Text changed";
        }
    }
}