using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VSNav
{
    /// <summary>
    /// Details of a StringMatch
    /// </summary>
    public class StringMatch
    {
        public delegate StringMatch GetMatchDelegate(String value);

        private StringMatch()
        {
            Parts = new List<StringPart>();
        }

        /// <summary>
        /// Used when whole string is matched
        /// </summary>
        public StringMatch(String fullString)
            : this()
        {
            MatchCharacters = 0;
            MatchFraction = 1;
            MatchPriority = 1;
            Parts.Add(new StringPart(fullString, false));
        }

        /// <summary>
        /// Used for camel case searches
        /// </summary>
        public StringMatch(String fullString, List<int> matchChars, List<int> skipChars)
        {
            Parts = new List<StringPart>();
            MatchPriority = Math.Max(0.0, (double)(matchChars.Count - skipChars.Count));

            if (matchChars[0] == 0) { MatchPriority++; }

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

            MatchCharacters = matchChars.Count;
            MatchFraction = ((Double)MatchCharacters / (Double)fullString.Length);
            MatchPriority += MatchCharacters + MatchFraction;
        }

        /// <summary>
        /// Used for Regex or String.Contains
        /// </summary>
        public StringMatch(String fullString, String matchString)
            : this()
        {
            int startIndex = fullString.IndexOf(matchString.ToLower(), StringComparison.OrdinalIgnoreCase);
            int length = matchString.Length;

            // Get the 1st part of the String
            if (startIndex > 0)
            {
                Parts.Add(new StringPart(fullString.Substring(0, startIndex), false));
            }

            // Get the next matched part of the String
            Parts.Add(new StringPart(fullString.Substring(startIndex, length), true));

            // Get the last part of the String
            if (startIndex + length < fullString.Length)
            {
                Parts.Add(new StringPart(fullString.Substring(startIndex + length), false));
            }

            MatchCharacters = length;
            MatchFraction = ((Double)MatchCharacters / (Double)fullString.Length);
            MatchPriority = MatchCharacters + MatchFraction;
        }

        /// <summary>
        /// Used to combine multiple matches into the final result.
        /// </summary>
        private StringMatch(String fullString, List<StringMatch> matches)
            : this()
        {
            int matchCount = 0;
            bool isAddingMatches = false;
            String subString = "";

            for (int n = 0; n < fullString.Length; n++ )
            {
                bool matched = false;
                foreach (var match in matches)
                {
                    if (match.IsMatchAt(n))
                    {
                        matched = true;
                        matchCount++;
                        break;
                    }
                }

                if (matched != isAddingMatches)
                {
                    if (subString.Length > 0)
                    {
                        Parts.Add(new StringPart(subString, isAddingMatches));
                    }
                    subString = "";
                    isAddingMatches = !isAddingMatches;
                }

                subString += fullString[n];
            }

            if (subString.Length > 0)
            {
                Parts.Add(new StringPart(subString, isAddingMatches));
            }

            MatchPriority = 0;
            foreach (var match in matches)
            {
                MatchPriority = Math.Max(MatchPriority, match.MatchPriority);
            }

            MatchCharacters = matchCount;
            MatchFraction = ((Double)MatchCharacters / (Double)fullString.Length);
            MatchPriority += MatchCharacters + MatchFraction;
        }

        /// <summary>
        /// Returns a delegate that will find a StringMatch for a given pattern
        /// where that pattern may be composed of multiple strings
        /// </summary>
        public static GetMatchDelegate GetShowStringDelegates(String fullPattern)
        {
            List<GetMatchDelegate> matchFuncs = new List<GetMatchDelegate>();
            String[] patterns = fullPattern.Split(new char[] { ' ' });
            foreach (String pattern in patterns)
            {
                matchFuncs.Add(GetShowStringDelegate(pattern));
            }

            return (s) =>
            {
                List<StringMatch> matches = new List<StringMatch>();
                foreach (GetMatchDelegate func in matchFuncs)
                {
                    StringMatch match = func(s);
                    matches.Add(match);
                }

                if (matches.Count > 1)
                {
                    return new StringMatch(s, matches);
                }
                else
                {
                    return matches.First();
                }
            };
        }

        /// <summary>
        /// Returns a delegate that will find a StringMatch for a given pattern
        /// </summary>
        private static GetMatchDelegate GetShowStringDelegate(String pattern)
        {
            #region Empty String

            // We have no filter
            if (String.IsNullOrEmpty(pattern))
            {
                return (s) =>
                {
                    return new StringMatch(s);
                };
            }

            #endregion

            #region Camel Case

            // Is it all uppercase?
            if (pattern.Length > 1)
            {
                int i = 0;
                foreach (char c in pattern)
                {
                    if (!Char.IsUpper(c)) { break; }
                    i++;
                }

                //It's all upper case so do camel searching
                if (i == pattern.Length)
                {
                    return (s) =>
                    {
                        // Grab all the Uppercase characters from the source String
                        List<int> skippedChars = new List<int>();
                        List<int> matchedChars = new List<int>();

                        // Grab the match char
                        int matchIndex = 0;
                        char matchChar = pattern[matchIndex];

                        // Go through all the characters
                        for (int charIndex = 0; charIndex < s.Length; charIndex++)
                        {
                            // Skip lower case
                            Char c = s[charIndex];
                            if (!Char.IsUpper(c)) { continue; }

                            // We matched
                            if (c == matchChar)
                            {
                                // Add the character
                                matchedChars.Add(charIndex);

                                // Move to the next matching character
                                matchIndex++;
                                if (matchIndex < pattern.Length)
                                {
                                    matchChar = pattern[matchIndex];
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else { skippedChars.Add(charIndex); } // We skipped a missed out camel
                        }

                        // If we didn't find all the camels then we ignore this result
                        if (matchedChars.Count != pattern.Length)
                        {
                            return new StringMatch();
                        }
                        else
                        {
                            return new StringMatch(s, matchedChars, skippedChars);
                        }
                    };
                }
            }

            #endregion

            #region String Contains

            // Just do a normal String.Contains
            if (!pattern.Contains("?") && !pattern.Contains("*"))
            {
                return (s) =>
                {
                    if (s.ToLower().Contains(pattern.ToLower()))
                    {
                        return new StringMatch(s, pattern);
                    }
                    return new StringMatch();
                };
            }

            #endregion

            #region Regex

            // Build up the Regex statement
            String regexString = "^";
            foreach (char c in pattern)
            {
                if (c == '?') { regexString += "."; }
                else if (c == '*') { regexString += ".*"; }
                else { regexString += c; }
            }

            return (s) =>
            {
                Match match = Regex.Match(s, regexString, RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    return new StringMatch(s, match.Value);
                }

                return new StringMatch();
            };

            #endregion
        }

        /// <summary>
        /// Returns whether the character at position n is part of a match.
        /// </summary>
        private bool IsMatchAt(int n)
        {
            var isMatch = false;
            var charInPart = n;

            foreach (var part in Parts)
            {
                if (charInPart >= part.Text.Length)
                {
                    charInPart -= part.Text.Length;
                }
                else
                {
                    isMatch = part.MatchPart;
                    break;
                }
            }

            return isMatch;
        }

        /// <summary>
        /// The number of characters matched.
        /// </summary>
        private int MatchCharacters;

        /// <summary>
        /// The total fraction of the filename that has been matched.
        /// </summary>
        private Double MatchFraction;

        /// <summary>
        /// The final order in the list, including the above character count.
        /// </summary>
        public Double MatchPriority { get; }

        /// <summary>
        /// The parts of the String
        /// </summary>
        public List<StringPart> Parts { get; private set; }
    }
}
