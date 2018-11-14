using DPA_Musicsheets.MusicDomain.Symbols;

namespace DPA_Musicsheets.Builders
{
    public class SequenceEndBuilder : AbstractBuilder
    {
        private SequenceEnd _sequenceEnd;

        public override void Init()
        {
            _sequenceEnd = new SequenceEnd();
        }

        public SequenceEnd GetSequenceEnd()
        {
            return _sequenceEnd;
        }
    }
}