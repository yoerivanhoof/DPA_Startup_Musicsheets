using DPA_Musicsheets.Visitors;

namespace DPA_Musicsheets.MusicDomain.Symbols
{
    public class SequenceEnd : IMusicSymbol
    {
        public T Accept<T>(SymbolVisitor<T> visitor)
        {
            return visitor.VisitSequenceEndSymbol(this);
        }
    }
}