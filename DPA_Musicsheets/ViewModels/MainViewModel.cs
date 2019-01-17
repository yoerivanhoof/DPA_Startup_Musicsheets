using DPA_Musicsheets.Managers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using DPA_Musicsheets.Shortcuts;
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
            shortcuts.SetNextHandler(new ShortcutHandler<MainViewModel>(new List<Key>() { Key.LeftCtrl, Key.T }, new InsertTempoCommand(this)));
            shortcuts.SetNextHandler(new ShortcutHandler<MainViewModel>(new List<Key>() { Key.LeftCtrl, Key.D3 }, new InsertTime34Command(this)));
            shortcuts.SetNextHandler(new ShortcutHandler<MainViewModel>(new List<Key>() { Key.LeftCtrl, Key.D4 }, new InsertTime44Command(this)));
            shortcuts.SetNextHandler(new ShortcutHandler<MainViewModel>(new List<Key>() { Key.LeftCtrl, Key.D6 }, new InsertTime68Command(this)));

            //Command<MainViewModel> baa = new Command<MainViewModel>(this);
        }

        public void SaveLilypond()
        {
            Console.WriteLine("Save lilypond");
        }

        public void SaveMidi()
        {
            Console.WriteLine("Save Midi");
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
