using DPA_Musicsheets.MusicDomain;

namespace DPA_Musicsheets.Loaders
{
    public class LilyFileLoader : FileLoader
    {
        public override string FileExtension => ".ly";
        public override void Load()
        {
            throw new System.NotImplementedException();
        }

        public override Music GetMusic()
        {
            throw new System.NotImplementedException();
        }
    }
}