using DPA_Musicsheets.Managers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DPA_Musicsheets.Memento;
using DPA_Musicsheets.Builders;
using DPA_Musicsheets.Loaders;
using DPA_Musicsheets.StateMachine;

namespace DPA_Musicsheets.ViewModels
{
    public class LilypondViewModel : ViewModelBase
    {
        private MusicLoader _musicLoader;
        private TextMementoCaretaker _mementoCaretaker = new TextMementoCaretaker(new TextMemento("Your lilypond text will appear here."));
        private List<TextMemento> _bookmarks;

        public string LilypondText
        {
            get => _mementoCaretaker.Memento.text();
            set
            {
                _mementoCaretaker.AddMemento(new TextMemento(value));
                
                RaisePropertyChanged(() => LilypondText);
            }
        }

        private bool _textChangedByLoad = false;
        private bool _textChangedByUndoRedo = false;
        private DateTime _lastChange;
        private static int MILLISECONDS_BEFORE_CHANGE_HANDLED = 1500;
        private StateMachine.StateMachine _stateMachine;
        private int _insertionIndex;

        public LilypondViewModel(MusicLoader musicLoader, StateMachine.StateMachine stateMachine)
        {
            _musicLoader = musicLoader;
            _musicLoader.MusicChanged += (sender, args) =>
            {
                if (!_textChangedByUndoRedo)
                {
                    _textChangedByLoad = true;
                    if (!args.Update)
                        _mementoCaretaker.Clear();
                    _mementoCaretaker.AddMemento(
                        new TextMemento(new LilyConverter(new MusicBuilder()).ConvertMusicToLily(args.Music)));
                    if (!args.Update)
                        _bookmarks = new List<TextMemento> { new TextMemento(LilypondText), new TextMemento(LilypondText), new TextMemento(LilypondText) };
                    RaisePropertyChanged(() => LilypondText);
                    _textChangedByLoad = false;
                }
            };
            _stateMachine = stateMachine;
        }

        /// <summary>
        /// This occurs when the text in the textbox has changed. This can either be by loading or typing.
        /// </summary>
        public ICommand TextChangedCommand => new RelayCommand<TextChangedEventArgs>((args) =>
        {
            if (_textChangedByLoad)
                return;

            // If we were typing, we need to do things.
            _lastChange = DateTime.Now;
            
            _stateMachine.ChangeState(new RenderingState());

            Task.Delay(MILLISECONDS_BEFORE_CHANGE_HANDLED).ContinueWith((task) =>
            {
                if ((DateTime.Now - _lastChange).TotalMilliseconds >= MILLISECONDS_BEFORE_CHANGE_HANDLED)
                {
                    UndoCommand.RaiseCanExecuteChanged();

                    _musicLoader.UpdateMusic(new LilyConverter(new MusicBuilder()).ConvertLilyToMusic(LilypondText));

                    _stateMachine.ChangeState(new UnsavedChangesState());

                    _textChangedByUndoRedo = false;
                }
            }, TaskScheduler.FromCurrentSynchronizationContext()); // Request from main thread.
            
        });

        public void InsertClefTreble()
        {
            LilypondText = LilypondText.Insert(_insertionIndex, "\\clef treble");
        }

        public void InsertTempo()
        {
            LilypondText = LilypondText.Insert(_insertionIndex, "\\tempo 4=120");
        }

        public void InsertTime(int first, int second)
        {
            LilypondText = LilypondText.Insert(_insertionIndex, $"\\time {first}/{second}");
        }

        public ICommand SelectionChangedCommand => new RelayCommand<RoutedEventArgs>((args) =>
        {
            _insertionIndex = ((TextBox) args.Source).CaretIndex; //index for shortcuts         
        });


        #region Commands for buttons like Undo, Redo and SaveAs
        public RelayCommand UndoCommand => new RelayCommand(() =>
        {
            _textChangedByUndoRedo = true;
            _mementoCaretaker.Undo();
            UndoCommand.RaiseCanExecuteChanged();
            RaisePropertyChanged(() => LilypondText);
        }, () => _mementoCaretaker.CanUndo());

        public RelayCommand RedoCommand => new RelayCommand(() =>
        {
            _textChangedByUndoRedo = true;
            _mementoCaretaker.Redo();
            RedoCommand.RaiseCanExecuteChanged();
            RaisePropertyChanged(() => LilypondText);
        }, () => _mementoCaretaker.CanRedo());

        public ICommand SaveAsCommand => new RelayCommand(() =>
        {
            // TODO: In the application a lot of classes know which filetypes are supported. Lots and lots of repeated code here...
            // Can this be done better?
            SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = "Midi|*.mid|Lilypond|*.ly|PDF|*.pdf" };
            if (saveFileDialog.ShowDialog() == true)
            {
                string extension = Path.GetExtension(saveFileDialog.FileName);
                if (extension.EndsWith(".mid"))
                {
                    _musicLoader.SaveToMidi(saveFileDialog.FileName);
                }
                else if (extension.EndsWith(".ly"))
                {
                    _musicLoader.SaveToLilypond(saveFileDialog.FileName);
                }
                else if (extension.EndsWith(".pdf"))
                {
                    _musicLoader.SaveToPDF(saveFileDialog.FileName);
                }
                else
                {
                    MessageBox.Show($"Extension {extension} is not supported.");
                }

                _stateMachine.ChangeState(new IdleState());
            }
        });

        public ICommand SaveBookmarkCommand => new RelayCommand<int>((arg) =>
            {
                _bookmarks[arg] = _mementoCaretaker.Memento;
            });
        public ICommand LoadBookmarkCommand => new RelayCommand<int>((arg) =>
        {
            _textChangedByUndoRedo = true;
            _mementoCaretaker.AddMemento(_bookmarks[arg]);
            RaisePropertyChanged(() => LilypondText);
        });

        #endregion Commands for buttons like Undo, Redo and SaveAs
    }
}
