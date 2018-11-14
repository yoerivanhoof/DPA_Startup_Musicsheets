using System;
using DPA_Musicsheets.Builders;
using DPA_Musicsheets.Managers;
using DPA_Musicsheets.MusicDomain;
using DPA_Musicsheets.MusicDomain.Symbols;
using Sanford.Multimedia.Midi;
using TimeSignatureBuilder = DPA_Musicsheets.Builders.TimeSignatureBuilder;

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
            var pitchNoteBuilder = new NoteBuilder();
            pitchNoteBuilder.Init();
            pitchNoteBuilder.MakePreparedKey();
            MusicBuilder.SetPitch(pitchNoteBuilder.GetNote());
            MusicBuilder.SetClef(Clef.TREBLE);


            int division = sequence.Division;
            int previousMidiKey = 60; // Central C;
            int previousNoteAbsoluteTicks = 0;
            double percentageOfBarReached = 0;
            bool startedNoteIsClosed = true;
            int beatNote = 4;
            int beatsPerBar;

            foreach (var track in sequence)
            {
                var noteBuilder = new NoteBuilder();
                foreach (var midiEvent in track.Iterator())
                {
                    IMidiMessage midiMessage = midiEvent.MidiMessage;
                    switch (midiMessage.MessageType)
                    {
                        case MessageType.Meta:
                            var metaMessage = midiMessage as MetaMessage;
                            switch (metaMessage.MetaType)
                            {
                                case MetaType.TimeSignature:
                                    byte[] timeSignatureBytes = metaMessage.GetBytes();
                                    beatNote = timeSignatureBytes[0];
                                    beatsPerBar = (int)(1 / Math.Pow(timeSignatureBytes[1], -2));
                                    var timeSignatureBuilder = new TimeSignatureBuilder();
                                    timeSignatureBuilder.Init();
                                    timeSignatureBuilder.SetBeatsPerBar(beatsPerBar);
                                    timeSignatureBuilder.SetBeats(beatNote);
                                    MusicBuilder.AddSymbol(timeSignatureBuilder.GetTimeSignature());
                                    break;
                                case MetaType.Tempo:
                                    byte[] tempoBytes = metaMessage.GetBytes();
                                    int tempo = (tempoBytes[0] & 0xff) << 16 | (tempoBytes[1] & 0xff) << 8 | (tempoBytes[2] & 0xff);
                                    var bpm = 60000000 / tempo;
                                    MusicBuilder.SetTempo(bpm);
                                    break;
                                case MetaType.EndOfTrack:
                                    if (previousNoteAbsoluteTicks > 0)
                                    {
                                        // Finish the last notelength.
                                        double percentageOfBar;
                                     //   lilypondContent.Append(MidiToLilyHelper.GetLilypondNoteLength(previousNoteAbsoluteTicks, midiEvent.AbsoluteTicks, division, beatNote, beatsPerBar, out percentageOfBar));
                                        MusicBuilder.AddSymbol(noteBuilder.GetNote());
                                      //  percentageOfBarReached += percentageOfBar;
                                        if (percentageOfBarReached >= 1)
                                        {
                                            var barlineBuilder = new BarlineBuilder();
                                            barlineBuilder.Init();
                                            MusicBuilder.AddSymbol(barlineBuilder.GetBarline());
                                        //    percentageOfBar = percentageOfBar - 1;
                                        }
                                    }
                                    break;
                            }
                            break;
                        case MessageType.Channel:
                            var channelMessage = midiEvent.MidiMessage as ChannelMessage;
                            if (channelMessage.Command == ChannelCommand.NoteOn)
                            {
                                if (channelMessage.Data2 > 0) // Data2 = loudness
                                {
                                    // Append the new note.
                                    noteBuilder.Init();

                               //     lilypondContent.Append(MidiToLilyHelper.GetLilyNoteName(previousMidiKey, channelMessage.Data1));

                                    previousMidiKey = channelMessage.Data1;
                                    startedNoteIsClosed = false;
                                }
                                else if (!startedNoteIsClosed)
                                {
                                    // Finish the previous note with the length.
                                    double percentageOfBar;
                                //    lilypondContent.Append(MidiToLilyHelper.GetLilypondNoteLength(previousNoteAbsoluteTicks, midiEvent.AbsoluteTicks, division, beatNote, beatsPerBar, out percentageOfBar));
                                    previousNoteAbsoluteTicks = midiEvent.AbsoluteTicks;
                                  //  lilypondContent.Append(" ");

                                  //  percentageOfBarReached += percentageOfBar;
                                    if (percentageOfBarReached >= 1)
                                    {
                                 //       lilypondContent.AppendLine("|");
                                        percentageOfBarReached -= 1;
                                    }
                                    startedNoteIsClosed = true;
                                }
                                else
                                {
                             //       lilypondContent.Append("r");
                                }
                            }
                            break;
                    }
                }
            }






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