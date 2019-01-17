using DPA_Musicsheets.ViewModels;
using Microsoft.Practices.ServiceLocation;

namespace DPA_Musicsheets.Shortcuts
{
    public class InsertTempoCommand :Command<MainViewModel>
    {
        public InsertTempoCommand(MainViewModel receiver) : base(receiver)
        {
        }

        public override void Execute()
        {
            ServiceLocator.Current.GetInstance<LilypondViewModel>().InsertTempo();
        }
    }
}