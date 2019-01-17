using DPA_Musicsheets.ViewModels;

namespace DPA_Musicsheets.Shortcuts
{
    public class SavePDFCommand:Command<MainViewModel>
    {
        public SavePDFCommand(MainViewModel receiver) : base(receiver)
        {
        }

        public override void Execute()
        {
            _receiver.SavePDF();
        }
    }
}