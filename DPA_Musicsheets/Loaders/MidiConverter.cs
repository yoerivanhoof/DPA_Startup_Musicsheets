using System;
using DPA_Musicsheets.Builders;
using DPA_Musicsheets.MusicDomain;
using DPA_Musicsheets.MusicDomain.Symbols;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.Loaders
{
    public class MidiConverter
    {
        private readonly MusicBuilder _musicBuilder;

        public MidiConverter(MusicBuilder musicBuilder)
        {
            _musicBuilder = musicBuilder;
        }

        public void ConvertMidiToMusic(Sequence sequence)
        {
            _musicBuilder.Init();
            var pitchNoteBuilder = new NoteBuilder();
            pitchNoteBuilder.Init();
            pitchNoteBuilder.MakePreparedKey();
            _musicBuilder.SetPitch(pitchNoteBuilder.GetNote());
            _musicBuilder.SetClef(Clef.TREBLE);


            int division = sequence.Division;
            int previousMidiKey = 60; // Central C;
            int previousNoteAbsoluteTicks = 0;
            double percentageOfBarReached = 0;
            bool startedNoteIsClosed = true;
            int beatNote = 4;
            int beatsPerBar = 0;

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
                                    var timeSignatureBuilder = new Builders.TimeSignatureBuilder();
                                    timeSignatureBuilder.Init();
                                    timeSignatureBuilder.SetBeatsPerBar(beatsPerBar);
                                    timeSignatureBuilder.SetBeats(beatNote);
                                    _musicBuilder.AddSymbol(timeSignatureBuilder.GetTimeSignature());
                                    break;
                                case MetaType.Tempo:
                                    byte[] tempoBytes = metaMessage.GetBytes();
                                    int tempo = (tempoBytes[0] & 0xff) << 16 | (tempoBytes[1] & 0xff) << 8 | (tempoBytes[2] & 0xff);
                                    var bpm = 60000000 / tempo;
                                    _musicBuilder.SetTempo(bpm);
                                    break;
                                case MetaType.EndOfTrack:
                                    if (previousNoteAbsoluteTicks > 0)
                                    {
                                        // Finish the last notelength.
                                        double percentageOfBar;
                                        var lengthInfo = GetNoteLength(previousNoteAbsoluteTicks, midiEvent.AbsoluteTicks,
                                            division, beatNote, beatsPerBar, out percentageOfBar);
                                        noteBuilder.SetDuration(lengthInfo.Duration);
                                        noteBuilder.SetExtended(lengthInfo.Extended);
                                        _musicBuilder.AddSymbol(noteBuilder.GetNote());
                                        percentageOfBarReached += percentageOfBar;
                                        if (percentageOfBarReached >= 1)
                                        {
                                            var barlineBuilder = new BarlineBuilder();
                                            barlineBuilder.Init();
                                            _musicBuilder.AddSymbol(barlineBuilder.GetBarline());
                                            percentageOfBar = percentageOfBar - 1;
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
                                    noteBuilder.SetPitch((Pitch)(channelMessage.Data1 % 12));
                                    int distance = channelMessage.Data1 - previousMidiKey;
                                    while (distance < -6)
                                    {
                                        noteBuilder.SetModifier(ModifierToken.DOWN);
                                        distance += 8;
                                    }

                                    while (distance > 6)
                                    {
                                        noteBuilder.SetModifier(ModifierToken.UP);
                                        distance -= 8;
                                    }

                                    previousMidiKey = channelMessage.Data1;
                                    startedNoteIsClosed = false;
                                }
                                else if (!startedNoteIsClosed)
                                {
                                    // Finish the previous note with the length.
                                    double percentageOfBar;
                                    var lengthInfo = GetNoteLength(previousNoteAbsoluteTicks, midiEvent.AbsoluteTicks,
                                        division, beatNote, beatsPerBar, out percentageOfBar);
                                    noteBuilder.SetDuration(lengthInfo.Duration);
                                    noteBuilder.SetExtended(lengthInfo.Extended);
                                    _musicBuilder.AddSymbol(noteBuilder.GetNote());

                                    previousNoteAbsoluteTicks = midiEvent.AbsoluteTicks;

                                    percentageOfBarReached += percentageOfBar;
                                    if (percentageOfBarReached >= 1)
                                    {
                                        var barlineBuilder = new BarlineBuilder();
                                        barlineBuilder.Init();
                                        _musicBuilder.AddSymbol(barlineBuilder.GetBarline());
                                        percentageOfBarReached -= 1;
                                    }
                                    startedNoteIsClosed = true;
                                }
                                else
                                {
                                    noteBuilder.Init();
                                    noteBuilder.SetPitch(Pitch.R);
                                }
                            }
                            break;
                    }
                }
            }
        }

        public Sequence ConvertMusicToMidi(Music music)
        {
            return new Sequence();
        }

        private NoteLengthInfo GetNoteLength(int absoluteTicks, int nextNoteAbsoluteTicks, int division, int beatNote, int beatsPerBar, out double percentageOfBar)
        {
            var noteLengthInfo = new NoteLengthInfo();
            int dots = 0;
            double deltaTicks = nextNoteAbsoluteTicks - absoluteTicks;

            if (deltaTicks <= 0)
            {
                percentageOfBar = 0;
                return noteLengthInfo;
            }

            double percentageOfBeatNote = deltaTicks / division;
            percentageOfBar = (1.0 / beatsPerBar) * percentageOfBeatNote;

            for (int noteLength = 32; noteLength >= 1; noteLength -= 1)
            {
                double absoluteNoteLength = (1.0 / noteLength);

                if (percentageOfBar <= absoluteNoteLength)
                {
                    if (noteLength < 2)
                        noteLength = 2;

                    int subtractDuration;

                    if (noteLength == 32)
                        subtractDuration = 32;
                    else if (noteLength >= 16)
                        subtractDuration = 16;
                    else if (noteLength >= 8)
                        subtractDuration = 8;
                    else if (noteLength >= 4)
                        subtractDuration = 4;
                    else
                        subtractDuration = 2;

                    if (noteLength >= 17)
                        noteLengthInfo.Duration = 32;
                    else if (noteLength >= 9)
                        noteLengthInfo.Duration = 16;
                    else if (noteLength >= 5)
                        noteLengthInfo.Duration = 8;
                    else if (noteLength >= 3)
                        noteLengthInfo.Duration = 4;
                    else
                        noteLengthInfo.Duration = 2;

                    double currentTime = 0;

                    while (currentTime < (noteLength - subtractDuration))
                    {
                        var addtime = 1 / ((subtractDuration / beatNote) * Math.Pow(2, dots));
                        if (addtime <= 0) break;
                        currentTime += addtime;
                        if (currentTime <= (noteLength - subtractDuration))
                        {
                            dots++;
                            noteLengthInfo.Extended = true;
                        }
                        if (dots >= 4) break;
                    }

                    break;
                }
            }

            return noteLengthInfo;
        }

    }
    class NoteLengthInfo
    {
        public int Duration { get; set; }
        public bool Extended { get; set; }
    }
}