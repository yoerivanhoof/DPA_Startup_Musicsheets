using DPA_Musicsheets.ViewModels;

namespace DPA_Musicsheets.Shortcuts
{
    public class SaveLilypondCommand : Command<MainViewModel>
    {
        public SaveLilypondCommand(MainViewModel receiver) : base(receiver)
        {
        }

        public override void Execute()
        {
            _receiver.SaveLilypond();
        }
    }
}