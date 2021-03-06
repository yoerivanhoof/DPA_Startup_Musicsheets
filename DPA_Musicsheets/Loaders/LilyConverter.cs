﻿using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DPA_Musicsheets.Builders;
using DPA_Musicsheets.MusicDomain;
using DPA_Musicsheets.MusicDomain.Symbols;
using DPA_Musicsheets.Visitors;

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

            _musicBuilder.Init(); ;

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

                    _musicBuilder.SetPitch(builder.GetNote());
                    i++;
                }
                else if (lily[i].Contains("clef"))
                {
                    _musicBuilder.SetClef((Clef)Enum.Parse(typeof(Clef), lily[i + 1].ToUpper()));
                    i++;
                }
                else if (lily[i].Contains("time"))
                {
                    var builder = new TimeSignatureBuilder();
                    builder.Init();
                    builder.SetBeats(int.Parse(lily[i + 1].Split('/')[0]));
                    builder.SetBeatsPerBar(int.Parse(lily[i + 1].Split('/')[1]));
                    _musicBuilder.AddSymbol(builder.GetTimeSignature());
                    i++;
                }
                else if (lily[i].Contains("tempo"))
                {
                    _musicBuilder.SetTempo(int.Parse(lily[i + 1].Split('=')[1]));
                    i++;
                }
                else if (lily[i].Contains("{"))
                {
                    var builder = new SequenceStartBuilder();
                    builder.Init();
                    _musicBuilder.AddSymbol(builder.GetSequenceStart());
                }
                else if (lily[i].Contains("}"))
                {
                    var builder = new SequenceEndBuilder();
                    builder.Init();
                    _musicBuilder.AddSymbol(builder.GetSequenceEnd());
                }
                else if (lily[i].Contains("|"))
                {
                    var builder = new BarlineBuilder();
                    builder.Init();
                    _musicBuilder.AddSymbol(builder.GetBarline());
                }
                else if (lily[i].Contains("alternative"))
                {
                    var builder = new AlternativeBuilder();
                    builder.Init();
                    _musicBuilder.AddSymbol(builder.GetAlternative());
                }
                else if (lily[i].Contains("repeat"))
                {
                    var builder = new RepeatBuilder();
                    builder.Init();
                    builder.SetCount(int.Parse(lily[i+2]));
                    _musicBuilder.AddSymbol(builder.GetRepeat());
                  
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
                        Pitch p;
                        Enum.TryParse(note.Groups["note"].Value.ToUpper(), out p);
                        builder.SetPitch(p);
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

                        _musicBuilder.AddSymbol(builder.GetNote());
                    }
                }

            return _musicBuilder.GetMusic();
        }

        public string ConvertMusicToLily(Music music)
        {
            string returnstring = "";
            string modifier = "";

            if (music.Key.Modifier.Token != ModifierToken.NONE)
            {
                if (music.Key.Modifier.Token == ModifierToken.DOWN)
                {
                    modifier += ",";
                }
                else
                {
                    modifier += '\'';
                }
            }

            var symbols = music.Symbols.ToList();
            returnstring += $"\\relative {music.Key.Pitch.ToString().ToLower()}{modifier} {{ \n";
            symbols.Remove(symbols.First());
            returnstring += $"\\clef {music.Clef.ToString().ToLower()} \n";
            returnstring += $"\\tempo 4={music.Tempo.ToString()} \n";
            

            foreach (IMusicSymbol musicSymbol in symbols)
            {
                LilyVisitor visitor = new LilyVisitor();
                returnstring += musicSymbol.Accept(visitor).ToLower();
            }

            return returnstring;
        }
    }
}