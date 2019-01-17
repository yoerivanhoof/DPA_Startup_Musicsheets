using DPA_Musicsheets.Managers;
using GalaSoft.MvvmLight;
using PSAMControlLibrary;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DPA_Musicsheets.Loaders;

namespace DPA_Musicsheets.ViewModels
{
    public class StaffsViewModel : ViewModelBase
    {
        // These staffs will be bound to.
        public ObservableCollection<MusicalSymbol> Staffs { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="musicLoader">We need the musicloader so it can set our staffs.</param>
        public StaffsViewModel(MusicLoader musicLoader)
        {
            musicLoader.MusicChanged += (sender, args) =>
            {
                SetStaffs(new StaffsConverter().ConvertMusicToSymbols(args.Music));
            };
            Staffs = new ObservableCollection<MusicalSymbol>();
        }

        /// <summary>
        /// SetStaffs fills the observablecollection with new symbols. 
        /// We don't want to reset the collection because we don't want other classes to create an observable collection.
        /// </summary>
        /// <param name="symbols">The new symbols to show.</param>
        public void SetStaffs(IList<MusicalSymbol> symbols)
        {
            Staffs.Clear();
            foreach (var symbol in symbols)
            {
                Staffs.Add(symbol);
            }
        }
    }
}
