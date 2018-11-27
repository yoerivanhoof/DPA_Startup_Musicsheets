namespace DPA_Musicsheets.MusicDomain
{
    public class Modifier
    {
        private ModifierToken token { get; set; } = ModifierToken.NONE;
        public int count { get; set; } = 0;
    }
}