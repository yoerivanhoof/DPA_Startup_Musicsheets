using DPA_Musicsheets.MusicDomain;
using DPA_Musicsheets.MusicDomain.Symbols;

namespace DPA_Musicsheets.Builders
{
    public class NoteBuilder : AbstractBuilder
    {
        private Note _note;

        public override void Init()
        {
            _note = new Note();
        }

        public void MakePreparedKey()
        {
            _note.Pitch = Pitch.C;
            _note.Modifier.Token = ModifierToken.UP;
        }

        public void SetPitch(Pitch pitch)
        {
            _note.Pitch = pitch;
        }

        public void SetDuration(int duration)
        {
            _note.Duration = duration;
        }

        public void SetResonate(bool resonate)
        {
            _note.Resonate = resonate;
        }

        public void SetModifier(ModifierToken modifierToken)
        {
            _note.Modifier.Token = modifierToken;
        }

        public void SetExtended(bool extended)
        {
            _note.Extended = extended;
        }

        public Note GetNote()
        {
            return _note;
        }
    }
}