namespace DPA_Musicsheets.MusicDomain.Symbols
{
    public class Note : IMusicSymbol
    {
        
        public Pitch Pitch { get; set; }
        public int Duration { get; set; }
        public bool Resonate { get; set; }
        public Modifier Modifier { get; set; }
        public bool Extended { get; set; }
       


        public Note()
        {
            Modifier = new Modifier();
        }
    }
}
