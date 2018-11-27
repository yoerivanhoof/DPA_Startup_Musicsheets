using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    }
}
