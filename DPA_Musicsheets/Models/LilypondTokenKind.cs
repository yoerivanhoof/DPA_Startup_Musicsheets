using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models
{
    /// <summary>
    /// These enums will be needed when loading an Lilypond file.
    /// These are the types we currently support. It is not an exhausted list.
    /// </summary>
    public enum LilypondTokenKind
    {
        Unknown,
        Note,
        Rest,
        Bar,
        Clef,
        Time,
        Tempo,
        Staff,
        Repeat,
        Alternative,
        SectionStart,
        SectionEnd
    }
}
