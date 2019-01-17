using DPA_Musicsheets.Managers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using DPA_Musicsheets.Loaders;
using DPA_Musicsheets.Shortcuts;
using DPA_Musicsheets.StateMachine;
using Microsoft.Practices.ServiceLocation;

namespace DPA_Musicsheets.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string _fileName;
        public string FileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                _fileName = value;
                RaisePropertyChanged(() => FileName);
            }
        }

        /// <summary>
        /// The current state can be used to display some text.
        /// "Rendering..." is a text that will be displayed for example.
        /// </summary>

        public string CurrentState => _stateMachine.State.GetDescription();

        private MusicLoader _musicLoader;
        private StateMachine.StateMachine _stateMachine;
        private ShortcutHandler<MainViewModel> shortcuts;
        public MainViewModel(MusicLoader musicLoader, StateMachine.StateMachine stateMachine)
        {
            _musicLoader = musicLoader;
            _stateMachine = stateMachine;
            _stateMachine.StateChanged += (sender, args) => { RaisePropertyChanged(() => CurrentState);};
            FileName = @"Files/Alle-eendjes-zwemmen-in-het-water.mid";

            shortcuts = new ShortcutHandler<MainViewModel>(new List<Key>(){Key.LeftCtrl,Key.S,Key.L},new SaveLilypondCommand(this) );
            shortcuts.SetNextHandler(new ShortcutHandler<MainViewModel>(new List<Key>() {Key.LeftCtrl,Key.S,Key.M},new SaveMidiCommand(this) ));
            shortcuts.SetNextHandler(new ShortcutHandler<MainViewModel>(new List<Key>() { Key.LeftCtrl, Key.S, Key.P }, new SavePDFCommand(this)));
            shortcuts.SetNextHandler(new ShortcutHandler<MainViewModel>(new List<Key>() { Key.LeftCtrl, Key.O }, new OpenCommand(this)));
            shortcuts.SetNextHandler(new ShortcutHandler<MainViewModel>(new List<Key>() { Key.LeftCtrl, Key.I, Key.C }, new InsertClefTrebleCommand(this)));
            shortcuts.SetNextHandler(new ShortcutHandler<MainViewModel>(new List<Key>() { Key.LeftCtrl, Key.I, Key.T }, new InsertTempoCommand(this)));
            shortcuts.SetNextHandler(new ShortcutHandler<MainViewModel>(new List<Key>() { Key.LeftCtrl, Key.I, Key.D3 }, new InsertTime34Command(this)));
            shortcuts.SetNextHandler(new ShortcutHandler<MainViewModel>(new List<Key>() { Key.LeftCtrl, Key.I, Key.D4 }, new InsertTime44Command(this)));
            shortcuts.SetNextHandler(new ShortcutHandler<MainViewModel>(new List<Key>() { Key.LeftCtrl, Key.I, Key.D6 }, new InsertTime68Command(this)));

            //Command<MainViewModel> baa = new Command<MainViewModel>(this);
        }

        public ICommand SaveAsCommand => new GalaSoft.MvvmLight.CommandWpf.RelayCommand(() =>
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = "Midi|*.mid|Lilypond|*.ly|PDF|*.pdf" };
            if (saveFileDialog.ShowDialog() == true)
            {
                string extension = Path.GetExtension(saveFileDialog.FileName);
                if (extension.EndsWith(".mid"))
                {
                    SaveMidi(saveFileDialog.FileName);
                }
                else if (extension.EndsWith(".ly"))
                {
                    SaveLilypond(saveFileDialog.FileName);
                }
                else if (extension.EndsWith(".pdf"))
                {
                    //_musicLoader.SaveToPDF(saveFileDialog.FileName);
                }
                else
                {
                    MessageBox.Show($"Extension {extension} is not supported.");
                }
                _stateMachine.ChangeState(new IdleState());
            }
        });

        public void SaveLilypond(string filename = "")
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = "Lilypond|*.ly" };
            if (filename != "" || saveFileDialog.ShowDialog() == true)
            {
                if (filename == "")
                    filename = saveFileDialog.FileName;
                
                var lily = new LilyFileLoader();
                lily.Save(filename, _musicLoader.Music);
            }
        }

        public void SaveMidi(string filename = "")
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = "Midi|*.mid" };
            if (filename != "" || saveFileDialog.ShowDialog() == true)
            {
                if (filename == "")
                    filename = saveFileDialog.FileName;

                var midi = new MidiFileLoader();
                midi.Save(filename,_musicLoader.Music);
            }
        }

        public void SavePDF()
        {
            Console.WriteLine("Save PDF");

        }
        public void Open()
        {
            OpenFileCommand.Execute(null);
        }
        public ICommand OpenFileCommand => new RelayCommand(() =>
        {
            OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Midi or LilyPond files (*.mid *.ly)|*.mid;*.ly" };
            if (openFileDialog.ShowDialog() == true)
            {
                FileName = openFileDialog.FileName;
            }
        });

        public ICommand LoadCommand => new RelayCommand(() =>
        {
            _musicLoader.OpenFile(FileName);
        });

        #region Focus and key commands, these can be used for implementing hotkeys
        public ICommand OnLostFocusCommand => new RelayCommand(() =>
        {
            Console.WriteLine("Maingrid Lost focus");
            PressedKeys.Clear();
        });

        List<Key> PressedKeys = new List<Key>();

        public ICommand OnKeyDownCommand => new RelayCommand<KeyEventArgs>((e) =>
        {
            if (!PressedKeys.Contains(e.Key))
            {
                PressedKeys.Add(e.Key);
                if (shortcuts.Handle(PressedKeys))
                {
                    PressedKeys.Clear();
                }
            }
            
           
        });

        public ICommand OnKeyUpCommand => new RelayCommand<KeyEventArgs>((e) =>
        {
            PressedKeys.Remove(e.Key);
        });

        public ICommand OnWindowClosingCommand => new RelayCommand<CancelEventArgs>((args) =>
        {
            _stateMachine.State.HandleClose(args);
        });
        #endregion Focus and key commands, these can be used for implementing hotkeys
    }
}
