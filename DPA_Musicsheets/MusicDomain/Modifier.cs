namespace DPA_Musicsheets.MusicDomain
{
    public class Modifier
    {
        public ModifierToken Token { get; set; } = ModifierToken.NONE;
        public int Count { get; set; } = 0;
    }
}