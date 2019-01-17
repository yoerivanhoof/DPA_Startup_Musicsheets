using System;
using DPA_Musicsheets.MusicDomain;

namespace DPA_Musicsheets.Managers
{
    public class MusicChangedEventArgs : EventArgs
    {
        public Music Music { get; }
        public bool Update { get; }
        public MusicChangedEventArgs(Music music, bool update)
        {
            Music = music;
            Update = update;
        }
    }
}