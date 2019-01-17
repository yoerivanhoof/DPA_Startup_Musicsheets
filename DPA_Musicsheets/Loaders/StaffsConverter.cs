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
            var visitor = new StaffsVisitor();
            return music.Symbols.Select(s => s.Accept(visitor)).ToList();
        }
    }
}