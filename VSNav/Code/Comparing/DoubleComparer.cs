﻿using System.Collections.Generic;

namespace VSNav
{
    /// <summary>
    /// Compares two FileInfos by their Name
    /// </summary>
    public class DoubleComparer : IComparer<FileInfo>
    {
        public int Compare(FileInfo x, FileInfo y)
        {
            int compare = x.NameInfo.MatchInfo.MatchPriority.CompareTo(y.NameInfo.MatchInfo.MatchPriority);
            if (compare == 0)
            {
                return x.Name.CompareTo(y.Name);
            }
            return -compare;
        }
    }
}
