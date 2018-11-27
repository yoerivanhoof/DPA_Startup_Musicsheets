using DPA_Musicsheets.Visitors;

namespace DPA_Musicsheets.MusicDomain.Symbols
{
    public class TimeSignature : IMusicSymbol
    {
        public int BeatsPerBar { get; set; }

        public int Beats { get; set; }

        public T Accept<T>(SymbolVisitor<T> visitor)
        {
            return visitor.VisitTimeSignatureSymbol(this);
        }
    }
}