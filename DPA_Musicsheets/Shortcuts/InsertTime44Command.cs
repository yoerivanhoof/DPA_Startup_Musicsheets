using DPA_Musicsheets.ViewModels;
using Microsoft.Practices.ServiceLocation;

namespace DPA_Musicsheets.Shortcuts
{
    public class InsertTime44Command:Command<MainViewModel>
    {
        public InsertTime44Command(MainViewModel receiver) : base(receiver)
        {
        }

        public override void Execute()
        {
            ServiceLocator.Current.GetInstance<LilypondViewModel>().InsertTime(4,4);
        }
    }
}