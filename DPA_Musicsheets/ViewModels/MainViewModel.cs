using DPA_Musicsheets.Managers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

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
        public MainViewModel(MusicLoader musicLoader, StateMachine.StateMachine stateMachine)
        {
            _musicLoader = musicLoader;
            _stateMachine = stateMachine;
            _stateMachine.StateChanged += (sender, args) => { RaisePropertyChanged(() => CurrentState);};
            FileName = @"Files/Alle-eendjes-zwemmen-in-het-water.mid";
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
                PressedKeys.Add(e.Key);

            Console.WriteLine($"Key down: {e.Key}");
            Console.Write("Pressed keys: ");
            PressedKeys.ForEach(k => Console.Write($"{k} "));
            Console.WriteLine();
        });

        public ICommand OnKeyUpCommand => new RelayCommand<KeyEventArgs>((e) =>
        {
            Console.WriteLine($"Key up: {e.Key}");
            PressedKeys.Remove(e.Key);
            Console.Write("Pressed keys: ");
            PressedKeys.ForEach(k => Console.Write($"{k} "));
            Console.WriteLine();
        });

        public ICommand OnWindowClosingCommand => new RelayCommand<CancelEventArgs>((args) =>
        {
            _stateMachine.State.HandleClose(args);
        });
        #endregion Focus and key commands, these can be used for implementing hotkeys
    }
}
