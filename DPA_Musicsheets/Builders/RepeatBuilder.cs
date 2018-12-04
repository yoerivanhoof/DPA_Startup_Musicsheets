using DPA_Musicsheets.MusicDomain;
using DPA_Musicsheets.MusicDomain.Symbols;

namespace DPA_Musicsheets.Builders
{
    public class RepeatBuilder : AbstractBuilder
    {
        private Repeat _repeat;

        public override void Init()
        {
            _repeat = new Repeat();
        }
        

        public void SetCount(int count)
        {
            _repeat.count = count;
        }
        public Repeat GetRepeat()
        {
            return _repeat;
        }
    }
}