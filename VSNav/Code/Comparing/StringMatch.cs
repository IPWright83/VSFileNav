using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSNav
{
    /// <summary>
    /// Details of a StringMatch
    /// </summary>
    public class StringMatch
    {
        private StringMatch()
        {
            this.Parts = new List<StringPart>();
        }

        /// <summary>
        /// Used when no match takes place
        /// </summary>
        public StringMatch(Double fraction, String fullString)
            : this()
        {
            this.MatchFraction = fraction;
            this.Parts.Add(new StringPart(fullString, false));
        }

        /// <summary>
        /// Used for camel case searches
        /// </summary>
        public StringMatch(String fullString, List<int> matchChars, List<int> skipChars)
        {
            this.Parts = new List<StringPart>();
            this.MatchFraction = (double)(matchChars.Count - skipChars.Count);

            if (matchChars[0] == 0) { this.MatchFraction++; }

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < fullString.Length; i++)
            {
                if (matchChars.Contains(i))
                {
                    // Add the non matched strings
                    if (sb.Length > 0)
                    {
                        Parts.Add(new StringPart(sb.ToString(), false));
                        sb.Length = 0;
                    }
                    Parts.Add(new StringPart(fullString[i].ToString(), true));
                }
                else if (skipChars.Contains(i))
                {
                    // Add the non matched strings
                    if (sb.Length > 0)
                    {
                        Parts.Add(new StringPart(sb.ToString(), false));
                        sb.Length = 0;
                    }
                    Parts.Add(new StringPart(fullString[i].ToString(), false, true));
                }
                else
                {
                    sb.Append(fullString[i]);
                }
            }

            if (sb.Length > 0)
            {
                Parts.Add(new StringPart(sb.ToString(), false));
            }
        }

        /// <summary>
        /// Used for Regex or String.Contains
        /// </summary>
        public StringMatch(String fullString, String matchString)
            : this()
        {
            this.MatchFraction = ((double)matchString.Length / (double)fullString.Length);

            int startIndex = fullString.IndexOf(matchString.ToLower(), StringComparison.OrdinalIgnoreCase);
            int length = matchString.Length;

            // Get the 1st part of the String
            if (startIndex > 0)
            {
                this.Parts.Add(new StringPart(fullString.Substring(0, startIndex), false));
            }

            // Get the next matched part of the String
            this.Parts.Add(new StringPart(fullString.Substring(startIndex, length), true));

            // Get the last part of the String
            if (startIndex + length < fullString.Length)
            {
                this.Parts.Add(new StringPart(fullString.Substring(startIndex + length), false));
            }
        }

        /// <summary>
        /// The Percentage Match
        /// </summary>
        public Double MatchFraction;

        /// <summary>
        /// The parts of the String
        /// </summary>
        public List<StringPart> Parts { get; private set; }
    }
}
