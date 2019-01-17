using DPA_Musicsheets.ViewModels;

namespace DPA_Musicsheets.Shortcuts
{
    public class OpenCommand :Command<MainViewModel>
    {
        public OpenCommand(MainViewModel receiver) : base(receiver)
        {
        }

        public override void Execute()
        {
            _receiver.Open();
        }
    }
}