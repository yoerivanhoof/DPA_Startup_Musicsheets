using DPA_Musicsheets.ViewModels;

namespace DPA_Musicsheets.Shortcuts
{
    public class SaveMidiCommand:Command<MainViewModel>
    {
        public SaveMidiCommand(MainViewModel receiver) : base(receiver)
        {
        }

        public override void Execute()
        {
            _receiver.SaveMidi();
        }
    }
}