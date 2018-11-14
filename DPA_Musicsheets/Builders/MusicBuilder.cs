using System.Collections.Generic;
using DPA_Musicsheets.MusicDomain;
using DPA_Musicsheets.MusicDomain.Symbols;

namespace DPA_Musicsheets.Builders
{
    public class MusicBuilder : AbstractBuilder
    {
        private Music _music;

        public override void Init()
        {
            _music = new Music();
        }

        public void SetPitch(Pitch key)
        {
            _music.Key = key;
        }

        public void SetTempo(int tempo)
        {
            _music.Tempo = tempo;
        }

        public void SetClef(Clef clef)
        {
            _music.Clef = clef;
        }

        public void SetSymbols(List<IMusicSymbol> symbols)
        {
            _music.Symbols = symbols;
        }

        public Music GetMusic()
        {
            return _music;
        }
    }
}