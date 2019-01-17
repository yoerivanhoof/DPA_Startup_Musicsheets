using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace DPA_Musicsheets.Shortcuts
{
    public class ShortcutHandler <T>
    {
        private ShortcutHandler<T> _nextHandler;
        private List<Key> _keys;
        private Command<T> _command;
        public ShortcutHandler(List<Key> keys,Command<T> command)
        {
            _keys = keys;
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

        public bool Handle(List<Key> gesture)
        {
            var result1 = gesture.Except(_keys).Count();
            var result2 = _keys.Except(gesture).Count();
            if (result1 + result2 == 0)
            {
                _command.Execute();
                return true;
            }
            else
            {
                return _nextHandler != null && _nextHandler.Handle(gesture);
            }
        }
    }
}