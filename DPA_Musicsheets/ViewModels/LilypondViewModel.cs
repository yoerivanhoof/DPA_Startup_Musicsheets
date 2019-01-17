using DPA_Musicsheets.Managers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DPA_Musicsheets.Memento;
using DPA_Musicsheets.Builders;
using DPA_Musicsheets.Loaders;
using DPA_Musicsheets.Shortcuts;
using DPA_Musicsheets.StateMachine;

namespace DPA_Musicsheets.ViewModels
{
    public class LilypondViewModel : ViewModelBase
    {
        private MusicLoader _musicLoader;

        private string _text;
        private string _previousText;
        private string _nextText;


        public TextMemento CreateMemento()
        {
            return (new TextMemento(_text));
        }


        public void test()
        {
            Console.WriteLine("HAAAAAI");
        }

        public void SetMemento(TextMemento memento)
        {
            Console.WriteLine("Restoring state...");
            _text = memento.text();
        }

        public void InsertClefTreble()
        {

        }

        public void InsertTempo()
        {

        }

        public void InsertTime(int first, int second)
        {

        }

        /// <summary>
        /// This text will be in the textbox.
        /// It can be filled either by typing or loading a file so we only want to set previoustext when it's caused by typing.
        /// </summary>
        public string LilypondText
        {
            get
            {
                return _text;
            }
            set
            {
                if (!_waitingForRender && !_textChangedByLoad)
                {
                    _previousText = _text;
                    _textChanged = true;
                }
                _text = value;
                RaisePropertyChanged(() => LilypondText);
            }
        }

        private bool _textChangedByLoad = false;
        private bool _textChanged = false;
        private DateTime _lastChange;
        private static int MILLISECONDS_BEFORE_CHANGE_HANDLED = 1500;
        private bool _waitingForRender = false;
        private ShortcutHandler<LilypondViewModel> _shortcuts;

 
        private StateMachine.StateMachine _stateMachine;
        public LilypondViewModel(MusicLoader musicLoader, StateMachine.StateMachine stateMachine)
        {
            _musicLoader = musicLoader;
            _musicLoader.MusicChanged += (sender, args) =>
            {
                _text = new LilyConverter(new MusicBuilder()).ConvertMusicToLily(args.Music);
                LilypondTextLoaded(_text);
            };
            _stateMachine = stateMachine;
            _text = "Your lilypond text will appear here.";
            



        }

        

        public void LilypondTextLoaded(string text)
        {
            _textChangedByLoad = true;
            LilypondText = _previousText = text;
            _textChangedByLoad = false;
        }

        /// <summary>
        /// This occurs when the text in the textbox has changed. This can either be by loading or typing.
        /// </summary>
        public ICommand TextChangedCommand => new RelayCommand<TextChangedEventArgs>((args) =>
        {
            // If we were typing, we need to do things.
            if (!_textChangedByLoad)
            {
                _waitingForRender = true;
                _lastChange = DateTime.Now;
                
                _stateMachine.ChangeState(new RenderingState());

                Task.Delay(MILLISECONDS_BEFORE_CHANGE_HANDLED).ContinueWith((task) =>
                {
                    if ((DateTime.Now - _lastChange).TotalMilliseconds >= MILLISECONDS_BEFORE_CHANGE_HANDLED)
                    {
                        _waitingForRender = false;
                        UndoCommand.RaiseCanExecuteChanged();

                        _musicLoader.UpdateMusic(new LilyConverter(new MusicBuilder()).ConvertLilyToMusic(LilypondText));

                        if (_textChanged)
                        {
                            _stateMachine.ChangeState(new UnsavedChangesState());
                        }
                        else
                        {
                            _stateMachine.ChangeState(new IdleState());
                        }
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext()); // Request from main thread.
            }
        });

        public ICommand SelectionChangedCommand => new RelayCommand<RoutedEventArgs>((args) =>
        {
            if (!_textChangedByLoad)
            {
                var index = ((TextBox) args.Source).CaretIndex; //index for shortcuts
            }
        });

        public static RoutedCommand MyCommand = new RoutedCommand();

        #region Commands for buttons like Undo, Redo and SaveAs
        public RelayCommand UndoCommand => new RelayCommand(() =>
        {
            _nextText = LilypondText;
            LilypondText = _previousText;
            _previousText = null;
        }, () => _previousText != null && _previousText != LilypondText);

        public RelayCommand RedoCommand => new RelayCommand(() =>
        {
            _previousText = LilypondText;
            LilypondText = _nextText;
            _nextText = null;
            RedoCommand.RaiseCanExecuteChanged();
        }, () => _nextText != null && _nextText != LilypondText);

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

                _textChanged = false;
                _stateMachine.ChangeState(new IdleState());
            }
        });
        #endregion Commands for buttons like Undo, Redo and SaveAs
    }
}
