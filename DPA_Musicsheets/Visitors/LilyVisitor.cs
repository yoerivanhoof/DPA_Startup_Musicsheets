using DPA_Musicsheets.MusicDomain;
using DPA_Musicsheets.MusicDomain.Symbols;

namespace DPA_Musicsheets.Visitors
{
    public class LilyVisitor : SymbolVisitor<string>
    {
        public override string VisitBarlineSymbol(Barline symbol)
        {
            return "|\n";
        }

        public override string VisitNoteSymbol(Note symbol)
        {
            var modifier = "";
            if (symbol.Modifier.Token != ModifierToken.NONE)
            {
                for (int i = 0; i < symbol.Modifier.Count; i++)
                {
                    if (symbol.Modifier.Token == ModifierToken.DOWN)
                    {
                        modifier += ",";
                    }
                    else
                    {
                        modifier += '\'';
                    }
                }
            }

            string extension = symbol.Extended ? "." : "";
            string resonate = symbol.Resonate ? "~" : "";
            return $"{symbol.Pitch}{modifier}{symbol.Duration}{extension}{resonate} ";
        }

        public override string VisitSequenceStartSymbol(SequenceStart symbol)
        {
            return "{\n";
        }

        public override string VisitSequenceEndSymbol(SequenceEnd symbol)
        {
            return "}\n";
        }

        public override string VisitTimeSignatureSymbol(TimeSignature symbol)
        {
            return $"\\time {symbol.Beats}\\{symbol.BeatsPerBar}\n";
        }
    }
}