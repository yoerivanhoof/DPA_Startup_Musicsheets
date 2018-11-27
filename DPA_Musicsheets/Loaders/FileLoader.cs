using DPA_Musicsheets.Builders;
using DPA_Musicsheets.MusicDomain;

namespace DPA_Musicsheets.Loaders
{
    public abstract class FileLoader
    {
        protected MusicBuilder MusicBuilder = new MusicBuilder();

        public abstract string FileExtension { get; }

        public abstract void Load(string fileName);
        public abstract void Save(string fileName, Music music);

        public Music GetMusic()
        {
            return MusicBuilder.GetMusic();
        }
    }
}