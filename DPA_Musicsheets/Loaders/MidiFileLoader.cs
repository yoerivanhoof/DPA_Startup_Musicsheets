using DPA_Musicsheets.MusicDomain;

namespace DPA_Musicsheets.Loaders
{
    public class MidiFileLoader : FileLoader
    {
        public override string FileExtension => ".mid";
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