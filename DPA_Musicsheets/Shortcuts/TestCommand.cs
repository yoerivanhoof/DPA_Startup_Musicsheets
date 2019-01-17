using DPA_Musicsheets.ViewModels;

namespace DPA_Musicsheets.ChainOfResponsibility
{
    public class TestCommand : Command<LilypondViewModel>
    {
        public TestCommand(LilypondViewModel receiver) : base(receiver)
        {
        }

        public override void Execute()
        {
            _receiver.test();
        }
    }
}