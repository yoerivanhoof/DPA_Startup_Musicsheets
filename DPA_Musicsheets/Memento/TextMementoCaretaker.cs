namespace DPA_Musicsheets.Memento
{
    public class TextMementoCaretaker
    {
        private TextMemento _textMemento;

        public TextMemento TextMemento
        {
            get { return _textMemento; }
            set { _textMemento = value; }
        }
    }
}