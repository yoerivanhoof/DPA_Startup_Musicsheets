using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiPlayerTest
{
    public class TrackLog
    {
        public string TrackName { get; set; }
        public ObservableCollection<string> Messages { get; } = new ObservableCollection<string>();
    }
}
