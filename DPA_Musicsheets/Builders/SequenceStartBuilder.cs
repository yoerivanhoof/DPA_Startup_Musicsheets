using DPA_Musicsheets.MusicDomain.Symbols;

namespace DPA_Musicsheets.Builders
{
    public class SequenceStartBuilder : AbstractBuilder
    {
        private SequenceStart _sequenceStart;

        public override void Init()
        {
            _sequenceStart = new SequenceStart();
        }

        public SequenceStart GetSequenceStart()
        {
            return _sequenceStart;
        }
    }
}