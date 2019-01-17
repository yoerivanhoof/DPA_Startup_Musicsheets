using System.Collections.Generic;
using System.Linq;
using DPA_Musicsheets.MusicDomain;
using DPA_Musicsheets.Visitors;
using PSAMControlLibrary;

namespace DPA_Musicsheets.Loaders
{
    public class StaffsConverter
    {
        public List<MusicalSymbol> ConvertMusicToSymbols(Music music)
        {
            var symbols = new List<MusicalSymbol>();
            int clefLine = music.Clef == MusicDomain.Symbols.Clef.TREBLE ? 2 : 4;
            symbols.Add(new Clef((ClefType)(int)music.Clef, clefLine));
            var visitor = new StaffsVisitor();
            symbols.AddRange(music.Symbols.Select(s => s.Accept(visitor)));
            return symbols;
        }
    }
}