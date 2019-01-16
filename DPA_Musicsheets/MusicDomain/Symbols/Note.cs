using DPA_Musicsheets.Visitors;

namespace DPA_Musicsheets.MusicDomain.Symbols
{
    public class Note : IMusicSymbol
    {
        public Pitch Pitch { get; set; }
        public int Duration { get; set; }
        public bool Resonate { get; set; }
        public Modifier Modifier { get; set; }
        public bool Extended { get; set; }
        public int Octave { get; set; } = 4;


        public Note()
        {
            Modifier = new Modifier();
        }


        public T Accept<T>(SymbolVisitor<T> visitor)
        {
            return visitor.VisitNoteSymbol(this);
        }
    }
}
