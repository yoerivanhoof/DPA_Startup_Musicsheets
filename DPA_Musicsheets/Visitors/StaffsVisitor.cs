using System.Collections.Generic;
using DPA_Musicsheets.MusicDomain;
using DPA_Musicsheets.MusicDomain.Symbols;
using PSAMControlLibrary;
using Barline = DPA_Musicsheets.MusicDomain.Symbols.Barline;
using Note = DPA_Musicsheets.MusicDomain.Symbols.Note;
using TimeSignature = DPA_Musicsheets.MusicDomain.Symbols.TimeSignature;

namespace DPA_Musicsheets.Visitors
{
    public class StaffsVisitor : SymbolVisitor<MusicalSymbol>
    {
        public override MusicalSymbol VisitBarlineSymbol(Barline symbol)
        {
            return new PSAMControlLibrary.Barline();
        }

        public override MusicalSymbol VisitNoteSymbol(Note symbol)
        {
            if (symbol.Pitch == Pitch.R)
            {
                return new Rest((MusicalSymbolDuration)symbol.Duration);
            }
            else
            {
                //todo different tie types?
                int alter = 0;
                switch (symbol.Modifier.Token)
                {
                    case ModifierToken.DOWN:
                        alter -= symbol.Modifier.Count;
                        break;
                    case ModifierToken.UP:
                        alter += symbol.Modifier.Count;
                        break;
                }

                return new PSAMControlLibrary.Note(symbol.Pitch.ToString(), alter, symbol.Octave,
                    (MusicalSymbolDuration) symbol.Duration, NoteStemDirection.Up,
                    NoteTieType.None, new List<NoteBeamType> {NoteBeamType.Single});
            }
        }

        public override MusicalSymbol VisitSequenceStartSymbol(SequenceStart symbol)
        {
            return new PSAMControlLibrary.Barline { RepeatSign = RepeatSignType.Backward }; //todo:repeatnumber?
        }

        public override MusicalSymbol VisitSequenceEndSymbol(SequenceEnd symbol)
        {
            return new PSAMControlLibrary.Barline { RepeatSign = RepeatSignType.Backward };//todo:repeatnumber?
        }

        public override MusicalSymbol VisitTimeSignatureSymbol(TimeSignature symbol)
        {
            return new PSAMControlLibrary.TimeSignature(TimeSignatureType.Numbers,(uint) symbol.Beats, (uint) symbol.BeatsPerBar);
        }

        public override MusicalSymbol VisitRepeatSymbol(Repeat symbol)
        {
            return new PSAMControlLibrary.Barline { RepeatSign = RepeatSignType.Forward };
        }

        public override MusicalSymbol VisitAlternativeSymbol(Alternative symbol)
        {
            return new MusicalSymbol();
        }
    }
}