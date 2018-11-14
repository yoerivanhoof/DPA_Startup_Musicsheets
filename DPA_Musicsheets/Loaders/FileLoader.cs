using DPA_Musicsheets.MusicDomain;

namespace DPA_Musicsheets.Loaders
{
    public abstract class FileLoader
    {
        public abstract string FileExtension { get; }

        public abstract void Load(string fileName);
        public abstract Music GetMusic();
    }
}