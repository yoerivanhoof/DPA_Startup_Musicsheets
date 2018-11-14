﻿using DPA_Musicsheets.MusicDomain;
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

        public void SetModifier(Modifier modifier)
        {
            _note.Modifier = modifier;
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