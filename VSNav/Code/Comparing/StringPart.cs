using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSNav
{
    /// <summary>
    /// Describes a part a matched String
    /// </summary>
    public class StringPart
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringPart"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="matchPart">if set to <c>true</c> [match part].</param>
        /// <param name="skippedPart">if set to <c>true</c> [skipped part].</param>
        public StringPart(String text, Boolean matchPart, Boolean skippedPart = false)
        {
            this.Text = text;
            this.MatchPart = matchPart;
            this.SkippedPart = skippedPart;
        }

        /// <summary>
        /// The text for this part
        /// </summary>
        public String Text { get; private set; }

        /// <summary>
        /// Is this part of the match?
        /// </summary>
        public Boolean MatchPart { get; private set; }

        /// <summary>
        /// Is this a skipped part of the Match?
        /// </summary>
        public Boolean SkippedPart { get; private set; }
    }
}
