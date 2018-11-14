using DPA_Musicsheets.MusicDomain;

namespace DPA_Musicsheets.Loaders
{
    public abstract class FileLoader
    {
        public abstract string FileExtension { get; }

        public abstract void Load();
        public abstract Music GetMusic();
    }
}