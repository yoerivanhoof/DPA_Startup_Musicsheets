using DPA_Musicsheets.ViewModels;
using Microsoft.Practices.ServiceLocation;

namespace DPA_Musicsheets.Shortcuts
{
    public class InsertClefTrebleCommand:Command<MainViewModel>
    {
        public InsertClefTrebleCommand(MainViewModel receiver) : base(receiver)
        {
        }

        public override void Execute()
        {
            ServiceLocator.Current.GetInstance<LilypondViewModel>().InsertClefTreble();
        }
    }
}