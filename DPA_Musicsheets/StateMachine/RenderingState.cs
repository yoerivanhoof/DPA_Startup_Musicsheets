using System.ComponentModel;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;

namespace DPA_Musicsheets.StateMachine
{
    public class RenderingState : State
    {
        public override void HandleClose(CancelEventArgs args)
        {
            MessageBox.Show("Busy rendering.\nCannot close.", "Rendering", MessageBoxButton.OK);
            args.Cancel = true;
        }

        public override string GetDescription()
        {
            return "Rendering...";
        }
    }
}