﻿using System.Collections.Generic;
using DPA_Musicsheets.MusicDomain.Symbols;

namespace DPA_Musicsheets.MusicDomain
{
    public class Music
    {
        public Note Key { get; set; }
        public int Tempo { get; set; }
        public Clef Clef { get; set; }

        public List<IMusicSymbol> Symbols;


    }
}