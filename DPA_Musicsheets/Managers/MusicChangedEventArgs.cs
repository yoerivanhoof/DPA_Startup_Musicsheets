using System;
using DPA_Musicsheets.MusicDomain;

namespace DPA_Musicsheets.Managers
{
    public class MusicChangedEventArgs : EventArgs
    {
        public Music Music { get; }
        public MusicChangedEventArgs(Music music)
        {
            Music = music;
        }
    }
}