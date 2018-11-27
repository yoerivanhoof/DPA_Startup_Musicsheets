using System;
using System.IO;
using DPA_Musicsheets.MusicDomain;

namespace DPA_Musicsheets.Loaders
{
    public class LilyFileLoader : FileLoader
    {
        public override string FileExtension => ".ly";
        public override void Load(string fileName)
        {
            string str = "";
            foreach (var line in File.ReadAllLines(fileName))
            {
                str += line;
            }
            var musicLoader = new LilyConverter(MusicBuilder);
            var music = musicLoader.ConvertLilyToMusic(str);

#warning test
            musicLoader.ConvertMusicToLily(music);
        }

        public override void Save(string fileName, Music music)
        {
            throw new NotImplementedException();
        }
    }
}