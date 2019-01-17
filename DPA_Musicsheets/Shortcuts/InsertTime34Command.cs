using DPA_Musicsheets.ViewModels;
using Microsoft.Practices.ServiceLocation;

namespace DPA_Musicsheets.Shortcuts
{
    public class InsertTime34Command:Command<MainViewModel>
    {
        public InsertTime34Command(MainViewModel receiver) : base(receiver)
        {
        }

        public override void Execute()
        {
            ServiceLocator.Current.GetInstance<LilypondViewModel>().InsertTime(3,4);
        }
    }
}