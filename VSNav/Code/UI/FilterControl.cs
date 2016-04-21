using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace VSNav
{
    public delegate StringMatch ShowStringDelegate(String value);

    public partial class FilterControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterControl" /> class.
        /// </summary>
        public FilterControl()
        {
            InitializeComponent();

            IStyle style = Styles.GetStyle();

            this.btnCancel.BackColor = style.ContentBackgroundColour;
            this.btnExplore.BackColor = style.ContentBackgroundColour;
            this.btnOk.BackColor = style.ContentBackgroundColour;
            this.btnCancel.ForeColor = style.FontColour;
            this.btnExplore.ForeColor = style.FontColour;
            this.btnOk.ForeColor = style.FontColour;
        }

        /// <summary>
        /// Returns a Delegate which determines if a String should be visible or not
        /// </summary>
        /// <returns></returns>
        public ShowStringDelegate GetShowStringDelegate()
        {
            String text = this.txtFilter.Text.Trim();

            #region Empty String

            // We have no filter
            if (String.IsNullOrEmpty(text))
            {
                return (s) =>
                {
                    return new StringMatch(1, s);
                };
            }

            #endregion

            #region Camel Case

            // Is it all uppercase?
            if (text.Length > 1)
            {
                int i = 0;
                foreach (char c in text)
                {
                    if (!Char.IsUpper(c)) { break; }
                    i++;
                }

                //It's all upper case so do camel searching
                if (i == text.Length)
                {
                    return (s) =>
                    {
                        // Grab all the Uppercase characters from the source String
                        List<int> skippedChars = new List<int>();
                        List<int> matchedChars = new List<int>();

                        // Grab the match char
                        int matchIndex = 0;
                        char matchChar = text[matchIndex];

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
                                if (matchIndex < text.Length)
                                {
                                    matchChar = text[matchIndex];
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else { skippedChars.Add(charIndex); } // We skipped a missed out camel
                        }

                        // If we didn't find all the camels then we ignore this result
                        if (matchedChars.Count != text.Length)
                        {
                            return new StringMatch(0, String.Empty);
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
            if (!text.Contains("?") && !text.Contains("*"))
            {
                return (s) =>
                {
                    if (s.ToLower().Contains(text.ToLower()))
                    {
                        return new StringMatch(s, text);
                    }
                    return new StringMatch(0, String.Empty);
                };
            }

            #endregion

            #region Regex

            // Build up a Regex statement
            String regexString = "^";
            foreach (char c in text)
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
                return new StringMatch(0, String.Empty);
            };

            #endregion
        }
    }
}
