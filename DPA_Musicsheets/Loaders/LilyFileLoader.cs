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
            str += File.ReadAllText(fileName);
            var musicLoader = new LilyConverter(MusicBuilder);
            var music = musicLoader.ConvertLilyToMusic(str);

#warning test
            musicLoader.ConvertMusicToLily(music);
        }

        public override void Save(string fileName, Music music)
        {
            File.WriteAllText(fileName, new LilyConverter(MusicBuilder).ConvertMusicToLily(music));
        }
    }
}