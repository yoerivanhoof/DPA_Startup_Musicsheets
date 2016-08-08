using Microsoft.Win32;
using PSAMControlLibrary;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MidiPlayerTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MidiPlayer _player;
        public ObservableCollection<TrackLog> TrackLogs { get; } = new ObservableCollection<TrackLog>();

        // De OutputDevice is een midi device of het midikanaal van je PC.
        // Hierop gaan we audio streamen.
        // DeviceID 0 is je audio van je PC zelf.
        private OutputDevice _outputDevice = new OutputDevice(0);

        public MainWindow()
        {
            InitializeComponent();
            DataContext = TrackLogs;
            FillPSAMViewer();
            //notenbalk.LoadFromXmlFile("Resources/example.xml");
        }

        private void FillPSAMViewer()
        {
            notenbalk.ClearMusicalIncipit();

            // Clef = sleutel
            notenbalk.AddMusicalSymbol(new Clef(ClefType.GClef, 2));
            notenbalk.AddMusicalSymbol(new TimeSignature(TimeSignatureType.Numbers, 4, 4));
            /* 
                The first argument of Note constructor is a string representing one of the following names of steps: A, B, C, D, E, F, G. 
                The second argument is number of sharps (positive number) or flats (negative number) where 0 means no alteration. 
                The third argument is the number of an octave. 
                The next arguments are: duration of the note, stem direction and type of tie (NoteTieType.None if the note is not tied). 
                The last argument is a list of beams. If the note doesn't have any beams, it must still have that list with just one 
                    element NoteBeamType.Single (even if duration of the note is greater than eighth). 
                    To make it clear how beamlists work, let's try to add a group of two beamed sixteenths and eighth:
                        Note s1 = new Note("A", 0, 4, MusicalSymbolDuration.Sixteenth, NoteStemDirection.Down, NoteTieType.None, new List<NoteBeamType>() { NoteBeamType.Start, NoteBeamType.Start});
                        Note s2 = new Note("C", 1, 5, MusicalSymbolDuration.Sixteenth, NoteStemDirection.Down, NoteTieType.None, new List<NoteBeamType>() { NoteBeamType.Continue, NoteBeamType.End });
                        Note e = new Note("D", 0, 5, MusicalSymbolDuration.Eighth, NoteStemDirection.Down, NoteTieType.None,new List<NoteBeamType>() { NoteBeamType.End });
                        viewer.AddMusicalSymbol(s1);
                        viewer.AddMusicalSymbol(s2);
                        viewer.AddMusicalSymbol(e); 
            */

            for (int i = 0; i < 100; i++)
            {
                notenbalk.AddMusicalSymbol(new Note("A", 0, 4, MusicalSymbolDuration.Sixteenth, NoteStemDirection.Down, NoteTieType.None, new List<NoteBeamType>() { NoteBeamType.Start, NoteBeamType.Start }));
                notenbalk.AddMusicalSymbol(new Note("C", 1, 5, MusicalSymbolDuration.Sixteenth, NoteStemDirection.Down, NoteTieType.None, new List<NoteBeamType>() { NoteBeamType.Continue, NoteBeamType.End }));
                notenbalk.AddMusicalSymbol(new Note("D", 0, 5, MusicalSymbolDuration.Eighth, NoteStemDirection.Down, NoteTieType.Start, new List<NoteBeamType>() { NoteBeamType.End }));
                notenbalk.AddMusicalSymbol(new Barline());

                notenbalk.AddMusicalSymbol(new Note("D", 0, 5, MusicalSymbolDuration.Whole, NoteStemDirection.Down, NoteTieType.Stop, new List<NoteBeamType>() { NoteBeamType.Single }));
                notenbalk.AddMusicalSymbol(new Note("E", 0, 4, MusicalSymbolDuration.Quarter, NoteStemDirection.Up, NoteTieType.Start, new List<NoteBeamType>() { NoteBeamType.Single }) { NumberOfDots = 1 });
                notenbalk.AddMusicalSymbol(new Barline());

                notenbalk.AddMusicalSymbol(new Note("C", 0, 4, MusicalSymbolDuration.Half, NoteStemDirection.Up, NoteTieType.None, new List<NoteBeamType>() { NoteBeamType.Single }));
                notenbalk.AddMusicalSymbol(
                    new Note("E", 0, 4, MusicalSymbolDuration.Half, NoteStemDirection.Up, NoteTieType.None, new List<NoteBeamType>() { NoteBeamType.Single })
                    { IsChordElement = true });
                notenbalk.AddMusicalSymbol(
                    new Note("G", 0, 4, MusicalSymbolDuration.Half, NoteStemDirection.Up, NoteTieType.None, new List<NoteBeamType>() { NoteBeamType.Single })
                    { IsChordElement = true });
                notenbalk.AddMusicalSymbol(new Barline());
            }
            
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            if(_player != null)
            {
                _player.Dispose();
            }

            _player = new MidiPlayer(_outputDevice);
            _player.Play(txt_MidiFilePath.Text);
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Midi Files(.mid)|*.mid" };
            if (openFileDialog.ShowDialog() == true)
            {
                txt_MidiFilePath.Text = openFileDialog.FileName;
            }
        }
        
        private void btn_Stop_Click(object sender, RoutedEventArgs e)
        {
            if (_player != null)
                _player.Dispose();
        }

        private void btn_ShowContent_Click(object sender, RoutedEventArgs e)
        {
            ShowTrackLogs(MidiReader.ReadMidi(txt_MidiFilePath.Text));

            //tabCtrl_MidiContent.SelectedIndex = 0;
        }

        private void ShowTrackLogs(IEnumerable<TrackLog> tracklogs)
        {
            TrackLogs.Clear();
            foreach (var tracklog in tracklogs)
            {
                TrackLogs.Add(tracklog);
            }

            tabCtrl_MidiContent.SelectedIndex = 0;
        }

        #region Recording

        private bool _isRecording = false;
        private Sequence _recordedSequence = new Sequence();
        private MidiInternalClock _clock;

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (_isRecording)
            {
                int code = 60; // Central C
                switch (e.Key)
                {
                    case System.Windows.Input.Key.A: code += 0; break; // C
                    case System.Windows.Input.Key.W: code += 1; break; // C#
                    case System.Windows.Input.Key.S: code += 2; break; // D
                    case System.Windows.Input.Key.E: code += 3; break; // D#
                    case System.Windows.Input.Key.D: code += 4; break; // E
                    case System.Windows.Input.Key.F: code += 5; break; // F
                    case System.Windows.Input.Key.T: code += 6; break; // F#
                    case System.Windows.Input.Key.G: code += 7; break; // G
                    case System.Windows.Input.Key.Y: code += 8; break; // G#
                    case System.Windows.Input.Key.H: code += 9; break; // A
                    case System.Windows.Input.Key.U: code += 10; break; // A#
                    case System.Windows.Input.Key.J: code += 11; break; // B
                    case System.Windows.Input.Key.K: code += 12; break; // C
                    default: return;
                }

                // Data2: Velocity, hoe hard wordt de noot ingedrukt? 0 is heel zacht (niet), 127 is heel hard
                var message = new ChannelMessage(ChannelCommand.NoteOn, 1, code, 127);
                _recordedSequence[0].Insert(_clock.Ticks, message);

                _outputDevice.Send(message);
            } 
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (_isRecording)
            {
                int code = 60; // Central C
                switch (e.Key)
                {
                    case System.Windows.Input.Key.A: code += 0; break; // C
                    case System.Windows.Input.Key.W: code += 1; break; // C#
                    case System.Windows.Input.Key.S: code += 2; break; // D
                    case System.Windows.Input.Key.E: code += 3; break; // D#
                    case System.Windows.Input.Key.D: code += 4; break; // E
                    case System.Windows.Input.Key.F: code += 5; break; // F
                    case System.Windows.Input.Key.T: code += 6; break; // F#
                    case System.Windows.Input.Key.G: code += 7; break; // G
                    case System.Windows.Input.Key.Y: code += 8; break; // G#
                    case System.Windows.Input.Key.H: code += 9; break; // A
                    case System.Windows.Input.Key.U: code += 10; break; // A#
                    case System.Windows.Input.Key.J: code += 11; break; // B
                    case System.Windows.Input.Key.K: code += 12; break; // C
                    default: return;
                }

                // Data2: Velocity, hoe snel moet de noot losgelaten worden? 0 is langzaam, 127 is meteen
                var message = new ChannelMessage(ChannelCommand.NoteOff, 0, code, 127);
                _recordedSequence[0].Insert(_clock.Ticks, message);

                _outputDevice.Send(message);
            }
        }

        private void btn_Record_Click(object sender, RoutedEventArgs e)
        {
            _isRecording = true;
            _recordedSequence = new Sequence();

            _recordedSequence.Add(new Track());
            _clock = new MidiInternalClock(120 / 4);
            _clock.Start();

            btn_StopRecord.IsEnabled = true;
            btn_Record.IsEnabled = false;
            btn_Replay.IsEnabled = false;
        }

        private void btn_Replay_Click(object sender, RoutedEventArgs e)
        {
            _isRecording = false;
            _player = new MidiPlayer(_outputDevice);
            _player.Play(_recordedSequence);
        }

        private void btn_StopRecord_Click(object sender, RoutedEventArgs e)
        {
            _isRecording = false;
            _clock.Stop();

            btn_StopRecord.IsEnabled = false;
            btn_Record.IsEnabled = true;
            btn_Replay.IsEnabled = true;

            ShowTrackLogs(MidiReader.ReadSequence(_recordedSequence));
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog() { Filter = "Midi Files(.mid)|*.mid" };
            if (sfd.ShowDialog() == true)
            {
                _recordedSequence.SaveCompleted += (saveSender, saveEventArgs) =>
                {
                    if(saveEventArgs.Error != null)
                    {
                        MessageBox.Show(saveEventArgs.Error.Message, "Error");
                    } else
                    {
                        MessageBox.Show("File saved to " + sfd.FileName, "Success");
                    }
                };
                _recordedSequence.SaveAsync(sfd.FileName);
            }
        }

        #endregion Recording

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _outputDevice.Close();
            if (_player != null)
            {
                _player.Dispose();
            }
        }
    }
}
