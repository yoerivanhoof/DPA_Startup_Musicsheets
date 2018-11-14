using DPA_Musicsheets.Builders;
using DPA_Musicsheets.MusicDomain;
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
            LoadMusic(sequence);
        }

        private void LoadMusic(Sequence sequence)
        {
            MusicBuilder.Init();

            foreach (var track in sequence)
            {
                foreach (var midiEvent in track.Iterator())
                {
                    var message = midiEvent.MidiMessage;
                    var metaMessage = midiEvent.MidiMessage as MetaMessage;
                    var channelMessage = midiEvent.MidiMessage as ChannelMessage;   

                }
            }
        }

    }
}