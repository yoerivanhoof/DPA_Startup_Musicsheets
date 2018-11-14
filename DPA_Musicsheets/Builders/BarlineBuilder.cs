using DPA_Musicsheets.MusicDomain.Symbols;

namespace DPA_Musicsheets.Builders
{
    public class BarlineBuilder : AbstractBuilder
    {
        private Barline _barline;
        public override void Init()
        {
            _barline = new Barline();
        }

        public Barline GetBarline()
        {
            return _barline;
        }
    }
}