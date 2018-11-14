using DPA_Musicsheets.MusicDomain.Symbols;

namespace DPA_Musicsheets.Builders
{
    public class TimeSignatureBuilder : AbstractBuilder
    {
        private TimeSignature _timesignature;

        public override void Init()
        {
            _timesignature = new TimeSignature();
        }

        public TimeSignature GetTimeSignature()
        {
            return _timesignature;
        }
    }
}