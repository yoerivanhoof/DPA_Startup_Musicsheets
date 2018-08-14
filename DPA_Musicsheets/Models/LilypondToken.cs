using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models
{
    public class LilypondToken
    {
        public LilypondTokenKind TokenKind { get; set; }
        public string Value { get; set; }

        public LilypondToken NextToken { get; set; }

        public LilypondToken PreviousToken { get; set; }

        /// <summary>
        /// This can be used to print our list and quickly see what it contains.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"Lilypondtoken: {TokenKind} - {Value}";
    }
}
