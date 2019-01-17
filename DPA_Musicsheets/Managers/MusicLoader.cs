using System;
using Sanford.Multimedia.Midi;
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

        #region Saving to files
        internal void SaveToMidi(string fileName)
        {
            Sequence sequence = new MidiConverter(new MusicBuilder()).ConvertMusicToMidi(Music);

            sequence.Save(fileName);
        }

        internal void SaveToLilypond(string fileName)
        {
            using (StreamWriter outputFile = new StreamWriter(fileName))
            {
                outputFile.Write(new LilyConverter(new MusicBuilder()).ConvertMusicToLily(Music));
                outputFile.Close();
            }
        }

        internal void SaveToPDF(string fileName)
        {
            string tmpFileName = $"{fileName}-tmp.ly";
            SaveToLilypond(tmpFileName);

            string lilypondLocation = @"C:\Program Files (x86)\LilyPond\usr\bin\lilypond.exe";
            string sourceFolder = Path.GetDirectoryName(tmpFileName);
            string sourceFileName = Path.GetFileNameWithoutExtension(tmpFileName);
            string targetFolder = Path.GetDirectoryName(fileName);
            string targetFileName = Path.GetFileNameWithoutExtension(fileName);

            var process = new Process
            {
                StartInfo =
                {
                    WorkingDirectory = sourceFolder,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    Arguments = $"--pdf \"{sourceFolder}\\{sourceFileName}.ly\"",
                    FileName = lilypondLocation
                }
            };

            process.Start();
            while (!process.HasExited) { /* Wait for exit */
                }
                if (sourceFolder != targetFolder || sourceFileName != targetFileName)
            {
                File.Move(sourceFolder + "\\" + sourceFileName + ".pdf", targetFolder + "\\" + targetFileName + ".pdf");
                File.Delete(tmpFileName);
            }
        }
        #endregion Saving to files
    }
}
