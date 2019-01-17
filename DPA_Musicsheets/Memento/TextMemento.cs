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
    }
}