using DPA_Musicsheets.Visitors;

namespace DPA_Musicsheets.MusicDomain.Symbols
{
    public interface IMusicSymbol
    {
        T Accept<T>(SymbolVisitor<T> visitor);

    }
}
