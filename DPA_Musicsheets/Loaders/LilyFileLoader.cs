using System.Diagnostics;
using System.IO;
using System.Windows;
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
          
        }

        public override void Save(string fileName, Music music)
        {
            if (music == null)
                return;

            File.WriteAllText(fileName, new LilyConverter(MusicBuilder).ConvertMusicToLily(music));
        }

        public void SavePDF(string filename, Music music)
        {
            if (music == null)
                return;

            string tmpFileName = $"{filename}-tmp.ly";
            Save(tmpFileName, music);
            string lilypondLocation = @"C:\Program Files (x86)\LilyPond\usr\bin\lilypond.exe";
            if (File.Exists(lilypondLocation))
            {
                string sourceFolder = Path.GetDirectoryName(tmpFileName);
                string sourceFileName = Path.GetFileNameWithoutExtension(tmpFileName);
                string targetFolder = Path.GetDirectoryName(filename);
                string targetFileName = Path.GetFileNameWithoutExtension(filename);
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
                try
                {
                    process.Start();
                    while (!process.HasExited)
                    {
                        /* Wait for exit */
                    }
                }
                catch
                {
                    MessageBox.Show(
                        "PDF Generator is broken.\nPlease check if you lilypond is correct.",
                        "PDF Generator error",
                        MessageBoxButton.OK);
                }

                if (sourceFolder != targetFolder || sourceFileName != targetFileName)
                {
                    var sourcePdf = sourceFolder + "\\" + sourceFileName + ".pdf";
                    if (File.Exists(sourcePdf))
                    {
                        File.Move(sourcePdf, targetFolder + "\\" + targetFileName + ".pdf");
                        File.Delete(tmpFileName);
                    }
                    else
                    {
                        MessageBox.Show(
                            "PDF Generator had trouble generating the PDF.\nPlease check if you lilypond is correct.",
                            "PDF Generator error",
                            MessageBoxButton.OK);
                    }
                }
            }
            else
            {
                MessageBox.Show(
                    "Lilypond PDF generator not installed.\nPlease install.",
                    "PDF Generator not found",
                    MessageBoxButton.OK);
            }
        }
    }
}