using DPA_Musicsheets.MusicDomain.Symbols;

namespace DPA_Musicsheets.Visitors
{
    public abstract class SymbolVisitor<T>
    {
        public abstract T VisitBarlineSymbol(Barline symbol);
        public abstract T VisitNoteSymbol(Note symbol);
        public abstract T VisitSequenceStartSymbol(SequenceStart symbol);
        public abstract T VisitSequenceEndSymbol(SequenceEnd symbol);
        public abstract T VisitTimeSignatureSymbol(TimeSignature symbol);
        public abstract T VisitRepeatSymbol(Repeat symbol);
        public abstract T VisitAlternativeSymbol(Alternative symbol);

    }
}
