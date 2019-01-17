namespace DPA_Musicsheets.Memento
{
    public class TextMemento
    {
        private string _text;

        public TextMemento(string text)
        {
            _text = text;
        }

        public string text()
        {
            return _text;
        }

        public static bool operator ==(TextMemento one, TextMemento other)
        {
            return one?._text == other?._text;
        }

        public static bool operator !=(TextMemento one, TextMemento other)
        {
            return !(one == other);
        }
    }
}