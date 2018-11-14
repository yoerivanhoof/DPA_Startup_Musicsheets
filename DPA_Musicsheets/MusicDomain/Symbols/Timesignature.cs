namespace DPA_Musicsheets.MusicDomain.Symbols
{
    public class TimeSignature : IMusicSymbol
    {
        public int BeatsPerBar { get; set; }

        public int Beats { get; set; }
    }
}