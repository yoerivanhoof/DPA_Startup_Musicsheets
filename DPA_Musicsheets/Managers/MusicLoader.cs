using System;
using System.Diagnostics;
using System.IO;
using DPA_Musicsheets.Builders;
using DPA_Musicsheets.Loaders;
using DPA_Musicsheets.MusicDomain;

namespace DPA_Musicsheets.Managers
{
    public class MusicLoader
    {
        public Music Music { get; private set; }

        public event EventHandler<MusicChangedEventArgs> MusicChanged; 

        /// <summary>
        /// Opens a file.
        /// </summary>
        /// <param name="fileName"></param>
        public void OpenFile(string fileName)
        {
            LoaderFactory loaderFactory = new LoaderFactory();
            var loader = loaderFactory.GetLoader(Path.GetExtension(fileName));

            loader.Load(fileName);

            Music = loader.GetMusic();
            MusicChanged?.Invoke(this, new MusicChangedEventArgs(Music));
        }

        public void UpdateMusic(Music music)
        {
            Music = music;
            MusicChanged?.Invoke(this, new MusicChangedEventArgs(Music));
        }

    }
}
