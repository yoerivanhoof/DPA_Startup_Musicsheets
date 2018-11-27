using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DPA_Musicsheets.Builders;
using DPA_Musicsheets.MusicDomain;
using DPA_Musicsheets.MusicDomain.Symbols;

namespace DPA_Musicsheets.Loaders
{
    public class LilyConverter
    {
        private readonly MusicBuilder _musicBuilder;

        public LilyConverter(MusicBuilder musicBuilder)
        {
            _musicBuilder = musicBuilder;
        }

        public Music ConvertLilyToMusic(string str)
        {
            var lily = str.Replace('\r', ' ').Replace('\n', ' ').Split(' ').Where(s => !string.IsNullOrWhiteSpace(s)).ToList(); ;
            MusicBuilder MusicBuilder = new MusicBuilder();
            MusicBuilder.Init(); ;

            for (var i = 0; i < lily.Count; i++)
                if (lily[i].Contains("relative"))
                {
                    var builder = new NoteBuilder();
                    builder.Init();

                    builder.SetPitch((Pitch)Enum.Parse(typeof(Pitch), lily[i + 1][0].ToString().ToUpper()));
                    if (lily[i + 1].Length > 1)
                    {
                        switch (lily[i + 1][1])

                        {
                            case '\'':
                                builder.SetModifier(ModifierToken.UP, 1);
#warning count
                                break;
                            case ',':
                                builder.SetModifier(ModifierToken.DOWN, 1);
                                break;
                        }
                    }

                    MusicBuilder.SetPitch(builder.GetNote());
                    i++;
                }
                else if (lily[i].Contains("clef"))
                {
                    MusicBuilder.SetClef((Clef)Enum.Parse(typeof(Clef), lily[i + 1].ToUpper()));
                    i++;
                }
                else if (lily[i].Contains("time"))
                {
                    var builder = new TimeSignatureBuilder();
                    builder.Init();
                    builder.SetBeats(int.Parse(lily[i + 1].Split('/')[0]));
                    builder.SetBeatsPerBar(int.Parse(lily[i + 1].Split('/')[1]));
                    MusicBuilder.AddSymbol(builder.GetTimeSignature());
                    i++;
                }
                else if (lily[i].Contains("tempo"))
                {
                    MusicBuilder.SetTempo(int.Parse(lily[i + 1].Split('=')[1]));
                    i++;
                }
                else if (lily[i].Contains("{"))
                {
                    var builder = new SequenceStartBuilder();
                    builder.Init();
                    MusicBuilder.AddSymbol(builder.GetSequenceStart());
                }
                else if (lily[i].Contains("}"))
                {
                    var builder = new SequenceEndBuilder();
                    builder.Init();
                    MusicBuilder.AddSymbol(builder.GetSequenceEnd());
                }
                else if (lily[i].Contains("|"))
                {
                    var builder = new BarlineBuilder();
                    builder.Init();
                    MusicBuilder.AddSymbol(builder.GetBarline());
                }
                else if (lily[i].Contains("repeat"))
                {
                    //todo
                    i += 2;
                }
                else if (!lily[i].Contains("\\"))
                {
                    var note = Regex.Match(lily[i],
                        "(?'note'[a-gris]{1,3})(?'modifierType'[,']{0,2})(?'duration'[0-9]{1,2})(?'extended'[.]{0,1})(?'resonate'[~]{0,1})");
                    if (note.Success)
                    {
                        var builder = new NoteBuilder();
                        builder.Init();
                        builder.SetPitch((Pitch)Enum.Parse(typeof(Pitch), note.Groups["note"].Value.ToUpper()));
                        builder.SetDuration(int.Parse(note.Groups["duration"].Value));

                        if (note.Groups["extended"].Length > 0)
                        {
                            builder.SetExtended(true);
                        }

                        if (note.Groups["modifierType"].Length > 0)
                        {
                            if (note.Groups["modifierType"].Value.Contains("'"))
                            {
                                builder.SetModifier(ModifierToken.UP, note.Groups["modifierType"].Length);
                            }
                            else if (note.Groups["modifierType"].Value.Contains(","))
                            {
                                builder.SetModifier(ModifierToken.DOWN, note.Groups["modifierType"].Length);
                            }
                        }

                        if (note.Groups["resonate"].Length > 0)
                        {
                            builder.SetResonate(true);
                        }

                        MusicBuilder.AddSymbol(builder.GetNote());
                    }
                }

            return MusicBuilder.GetMusic();
        }

        public string ConvertMusicToLily(Music music)
        {
            string returnstring = "";
#warning comment
            //string modifier = "";

            //if (music.Key.Modifier.type != ModifierType.None)
            //{
            //    if (music.Key.Modifier.type == ModifierType.Down)
            //    {
            //        modifier += ",";
            //    }
            //    else
            //    {
            //        modifier += '\'';
            //    }
            //}
            //returnstring += "\\relative " + music.Key.Pitch.ToString() + modifier + "\n";

            //foreach (IMusicSymbol musicSymbol in music.Symbols)
            //{

            //}

            return returnstring;
        }
    }
}