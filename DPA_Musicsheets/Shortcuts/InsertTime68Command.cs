using DPA_Musicsheets.ViewModels;
using Microsoft.Practices.ServiceLocation;

namespace DPA_Musicsheets.Shortcuts
{
    public class InsertTime68Command:Command<MainViewModel>
    {
        public InsertTime68Command(MainViewModel receiver) : base(receiver)
        {
        }

        public override void Execute()
        {
            ServiceLocator.Current.GetInstance<LilypondViewModel>().InsertTime(6,8);
        }
    }
}