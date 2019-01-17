using System.Windows.Input;

namespace DPA_Musicsheets.ChainOfResponsibility
{
    public class ShortcutHandler <T>
    {
        private ShortcutHandler<T> _nextHandler;
        private KeyGesture _keyGesture;
        private Command<T> _command;
        public ShortcutHandler(KeyGesture keyGesture,Command<T> command)
        {
            _keyGesture = keyGesture;
            _command = command;

        }

        public void SetNextHandler(ShortcutHandler<T> nextHandler)
        {
            if (_nextHandler == null)
            {
                _nextHandler = nextHandler;
            }
            else
            {
                _nextHandler.SetNextHandler(nextHandler);
            }
        }

        public void Handle(KeyGesture gesture)
        {
            if (gesture == _keyGesture)
            {
                _command.Execute();
            }
            else
            {
                _nextHandler?.Handle(gesture);
            }
        }
    }
}