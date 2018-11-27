using DPA_Musicsheets.Visitors;

namespace DPA_Musicsheets.MusicDomain.Symbols
{
    public class Barline: IMusicSymbol
    {
        public T Accept<T>(SymbolVisitor<T> visitor)
        {
            return visitor.VisitBarlineSymbol(this);
        }
    }
}