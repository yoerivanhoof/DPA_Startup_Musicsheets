using System;
using System.Collections.Generic;
using DPA_Musicsheets.MusicDomain;
using DPA_Musicsheets.MusicDomain.Symbols;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.Visitors
{
    public class MidiVisitor: SymbolVisitor<Dictionary<IMidiMessage, double>>
    {
        public override Dictionary<IMidiMessage, double> VisitBarlineSymbol(Barline symbol)
        {
            return new Dictionary<IMidiMessage, double>();
        }

        public override Dictionary<IMidiMessage, double> VisitNoteSymbol(Note symbol)
        {
            var notesList = new Dictionary<IMidiMessage, double>();

            List<string> notesOrderWithCrosses = new List<string> { "c", "cis", "d", "dis", "e", "f", "fis", "g", "gis", "a", "ais", "b" };
            // Calculate duration
            double absoluteLength = 1.0 / symbol.Duration;
            absoluteLength += (absoluteLength / 2.0) * symbol.Modifier.Count;

            // Calculate height
            int noteHeight = notesOrderWithCrosses.IndexOf(symbol.Pitch.ToString().ToLower()) + ((symbol.Octave + 1) * 12);

            switch (symbol.Modifier.Token)
            {
                case ModifierToken.DOWN:
                    noteHeight -= symbol.Modifier.Count;
                    break;
                case ModifierToken.UP:
                    noteHeight += symbol.Modifier.Count;
                    break;
            }

            notesList.Add(new ChannelMessage(ChannelCommand.NoteOn, 1, noteHeight, 90), 0); // Data2 = volume

            notesList.Add(new ChannelMessage(ChannelCommand.NoteOn, 1, noteHeight, 0), absoluteLength); // Data2 = volume


            return notesList;
        }

        public override Dictionary<IMidiMessage, double> VisitSequenceStartSymbol(SequenceStart symbol)
        {
            return new Dictionary<IMidiMessage, double>();
        }

        public override Dictionary<IMidiMessage, double> VisitSequenceEndSymbol(SequenceEnd symbol)
        {
            return new Dictionary<IMidiMessage, double>();
        }

        public override Dictionary<IMidiMessage, double> VisitTimeSignatureSymbol(TimeSignature symbol)
        {
            var timeSignatureList = new Dictionary<IMidiMessage, double>();

            byte[] timeSignature = new byte[4];
            timeSignature[0] = (byte)symbol.BeatsPerBar;
            timeSignature[1] = (byte)(Math.Log(symbol.Beats) / Math.Log(2));
            timeSignatureList.Add(new MetaMessage(MetaType.TimeSignature, timeSignature), 0);

            return timeSignatureList;
        }

        public override Dictionary<IMidiMessage, double> VisitRepeatSymbol(Repeat symbol)
        {
            return new Dictionary<IMidiMessage, double>();
        }

        public override Dictionary<IMidiMessage, double> VisitAlternativeSymbol(Alternative symbol)
        {
            return new Dictionary<IMidiMessage, double>();
        }
    }
}