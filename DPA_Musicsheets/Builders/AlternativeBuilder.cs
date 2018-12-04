using DPA_Musicsheets.MusicDomain;
using DPA_Musicsheets.MusicDomain.Symbols;

namespace DPA_Musicsheets.Builders
{
    public class AlternativeBuilder : AbstractBuilder
    {
        private Alternative _alternative;

        public override void Init()
        {
            _alternative = new Alternative();
        }
        
        
        public Alternative GetAlternative()
        {
            return _alternative;
        }
    }
}