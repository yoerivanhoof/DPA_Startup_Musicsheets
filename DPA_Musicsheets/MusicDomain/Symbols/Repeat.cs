using DPA_Musicsheets.Visitors;

namespace DPA_Musicsheets.MusicDomain.Symbols
{
    public class Repeat : IMusicSymbol
    {
        public int count { get; set; }
        public T Accept<T>(SymbolVisitor<T> visitor)
        {
            return visitor.VisitRepeatSymbol(this);
        }
    }
}