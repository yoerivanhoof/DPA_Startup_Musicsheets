using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DPA_Musicsheets.Builders;
using DPA_Musicsheets.MusicDomain;
using DPA_Musicsheets.MusicDomain.Symbols;
using Sanford.Multimedia.Midi;
using TimeSignatureBuilder = DPA_Musicsheets.Builders.TimeSignatureBuilder;

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
                    TimeSignatureBuilder builder = new TimeSignatureBuilder();
                    builder.Init();
                    builder.SetBeats(int.Parse(lily[i+1].Split('/')[0]));
                    builder.SetBeatsPerBar(int.Parse(lily[i + 1].Split('/')[1]));
                    MusicBuilder.AddSymbol(builder.GetTimeSignature());
                    i++;
                }
                else if (lily[i].Contains("tempo"))
                {
                    MusicBuilder.SetTempo(int.Parse(lily[i+1].Split('=')[1]));
                    i++;
                }
                else if(lily[i].Contains("{"))
                {
                    SequenceStartBuilder builder = new SequenceStartBuilder();
                    builder.Init();
                    MusicBuilder.AddSymbol(builder.GetSequenceStart());
                }else if (lily[i].Contains("}"))
                {
                    SequenceEndBuilder builder = new SequenceEndBuilder();
                    builder.Init();
                    MusicBuilder.AddSymbol(builder.GetSequenceEnd());
                }else if (lily[i].Contains("|"))
                {
                    BarlineBuilder builder = new BarlineBuilder();
                    builder.Init();
                    MusicBuilder.AddSymbol(builder.GetBarline());
                }else if (lily[i].Contains("repeat"))
                {
                    //todo
                    i++;
                    i++;
                }
                else if(!lily[i].Contains("\\"))
                {
                    var note = Regex.Match(lily[i],
                        "(?'note'[a-gris]{1,3})(?'modifier'[,']{0,2})(?'duration'[0-9]{1,2})(?'extended'[.]{0,1})(?'resonate'[~]{0,1})");

                    NoteBuilder builder = new NoteBuilder();
                    builder.Init();
                    builder.SetPitch((Pitch) Enum.Parse(typeof(Pitch), note.Groups["note"].Value.ToUpper()));
                    builder.SetDuration(int.Parse(note.Groups["duration"].Value));

                    if (note.Groups["extended"].Length > 0)
                    {
                        builder.SetExtended(true);
                    }

                    if (note.Groups["modifier"].Length > 0)
                    {
                        if (note.Groups["modifier"].Value.Contains("'"))
                        {
                            builder.SetModifier(Modifier.Up);
                        }else if (note.Groups["modifier"].Value.Contains(","))
                        {
                            builder.SetModifier(Modifier.Down);
                        }
                    }

                    if (note.Groups["resonate"].Length > 0)
                    {
                        builder.SetResonate(true);
                    }
                    MusicBuilder.AddSymbol(builder.GetNote());

                }
            }

            var result = MusicBuilder.GetMusic();
        }

    }
}