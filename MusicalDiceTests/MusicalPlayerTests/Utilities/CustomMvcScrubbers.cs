using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MusicalDiceTests.MusicalPlayerTests.Utilities
{
    public class CustomMvcScrubbers
    {
        public static string CopyrightScrubber(string input)
        {
            string copyrightTag = @"<p>&copy; \d{4}";
            return Regex.Replace(input, copyrightTag, "<p>&copy; 0000");
        }
    }
}
