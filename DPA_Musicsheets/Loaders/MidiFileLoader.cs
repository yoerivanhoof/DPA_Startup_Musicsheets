using System;
using System.IO;
using DPA_Musicsheets.MusicDomain;
using Microsoft.Win32;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.Loaders
{
    public class MidiFileLoader : FileLoader
    {
        public override string FileExtension => ".mid";

        public override void Load(string fileName)
        {
            var sequence = new Sequence();
            sequence.Load(fileName);
            var musicLoader = new MidiConverter(MusicBuilder);
            musicLoader.ConvertMidiToMusic(sequence);
        }

        public override void Save(string fileName, Music music)
        {
            
                new MidiConverter(MusicBuilder).ConvertMusicToMidi(music).Save(fileName);
            
        }
    }
}