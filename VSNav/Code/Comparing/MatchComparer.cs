using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSNav
{
    /// <summary>
    /// Compares two FileInfos by their Name
    /// </summary>
    public class MatchComparer : IComparer<StringMatch>
    {
        public int Compare(StringMatch x, StringMatch y)
        {
            int compare = x.MatchPriority.CompareTo(y.MatchPriority);
            return -compare;
        }
    }
}

