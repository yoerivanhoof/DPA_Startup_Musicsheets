using DPA_Musicsheets.MusicDomain.Symbols;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.Visitors
{
    public class MidiVisitor: SymbolVisitor<Sequence>
    {
        public override Sequence VisitBarlineSymbol(Barline symbol)
        {
            throw new System.NotImplementedException();
        }

        public override Sequence VisitNoteSymbol(Note symbol)
        {
            throw new System.NotImplementedException();
        }

        public override Sequence VisitSequenceStartSymbol(SequenceStart symbol)
        {
            throw new System.NotImplementedException();
        }

        public override Sequence VisitSequenceEndSymbol(SequenceEnd symbol)
        {
            throw new System.NotImplementedException();
        }

        public override Sequence VisitTimeSignatureSymbol(TimeSignature symbol)
        {
            throw new System.NotImplementedException();
        }

        public override Sequence VisitRepeatSymbol(Repeat symbol)
        {
            throw new System.NotImplementedException();
        }

        public override Sequence VisitAlternativeSymbol(Alternative symbol)
        {
            throw new System.NotImplementedException();
        }
    }
}