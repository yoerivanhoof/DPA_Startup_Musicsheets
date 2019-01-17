using System.Collections.Generic;

namespace DPA_Musicsheets.Memento
{
    public class TextMementoCaretaker
    {
        private List<TextMemento> _mementos = new List<TextMemento>();
        private int _currentIndex;

        public TextMemento Memento => _mementos[_currentIndex];

        public TextMementoCaretaker(TextMemento initial)
        {
            _mementos.Add(initial);
        }

        public void AddMemento(TextMemento memento)
        {
            if (memento == Memento)
                return;

            int nextIndex = _currentIndex + 1;

            if (_currentIndex != _mementos.Count - 1) //last item
            {
                _mementos.RemoveRange(nextIndex, _mementos.Count-nextIndex);
            }
            _mementos.Add(memento);
            _currentIndex = nextIndex;
        }

        public bool CanUndo()
        {
            var previousIndex = _currentIndex - 1;
            return previousIndex > 0;
        }

        public TextMemento Undo()
        {
            var previousIndex = _currentIndex - 1;
            if (previousIndex > 0)
                _currentIndex = previousIndex;
            return _mementos[_currentIndex];
        }

        public bool CanRedo()
        {
            var nextIndex = _currentIndex + 1;
            return nextIndex < _mementos.Count;
        }

        public TextMemento Redo() {
            var nextIndex = _currentIndex + 1;
            if (nextIndex < _mementos.Count)
                _currentIndex = nextIndex;
            return _mementos[_currentIndex];
        }
    }
}