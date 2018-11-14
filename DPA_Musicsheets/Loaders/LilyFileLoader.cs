using System;
using System.IO;
using System.Linq;
using System.Text;
using DPA_Musicsheets.Builders;
using DPA_Musicsheets.MusicDomain;
using DPA_Musicsheets.MusicDomain.Symbols;

namespace DPA_Musicsheets.Loaders
{
    public class LilyFileLoader : FileLoader
    {
        public override string FileExtension => ".ly";
        public override void Load(string fileName)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var line in File.ReadAllLines(fileName))
            {
                sb.AppendLine(line);
            }

            sb.Replace('\r', ' ');
            sb.Replace('\n', ' ');

            var lily = sb.ToString().Split(' ').Where(s=>!string.IsNullOrWhiteSpace(s)).ToList();
            MusicBuilder.Init();;
            for (int i = 0; i < lily.Count(); i++)
            {
                if (lily[i].Contains("relative"))
                {
                    NoteBuilder builder = new NoteBuilder();
                    builder.Init();

                    builder.SetPitch((Pitch) Enum.Parse(typeof(Pitch), lily[i + 1][0].ToString().ToUpper()));
                    if (lily[i + 1].Length > 1)
                    {
                        switch (lily[i + 1][1])
                        {
                            case '\'':
                                builder.SetModifier(Modifier.Up);
                                break;
                            case ',':
                                builder.SetModifier(Modifier.Down);
                                break;
                        }
                    }
                    MusicBuilder.SetPitch(builder.GetNote());
                    i++;
                }else if (lily[i].Contains("clef"))
                {
                    MusicBuilder.SetClef((Clef) Enum.Parse(typeof(Clef), lily[i+1].ToUpper()));
                    i++;
                }
                else if (lily[i].Contains("time"))
                {

                }
                else if (lily[i].Contains("tempo"))
                {

                }
                else if(lily.Contains("{"))
                {
                   
                }
            }

            var result = MusicBuilder.GetMusic();
        }

    }
}