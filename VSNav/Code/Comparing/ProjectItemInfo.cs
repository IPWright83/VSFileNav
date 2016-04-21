using EnvDTE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSNav
{
    /// <summary>
    /// Represents a ProjectItem
    /// </summary>
    public class ProjectItemInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectItemInfo" /> class.
        /// </summary>
        /// <param name="item">The item.</param>
        public ProjectItemInfo(ProjectItem item, short index)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            // Only add items with a proper name
            this.FilePath = item.get_FileNames(index);
            this.Name = System.IO.Path.GetFileName(this.FilePath);
            this.Extension = System.IO.Path.GetExtension(this.FilePath);
            this.FileType = this.Extension.ToFileType();
        }

        /// <summary>
        /// The Path to the File
        /// </summary>
        public String FilePath { get; internal set; }

        /// <summary>
        /// The Name of the File
        /// </summary>
        public String Name { get; internal set; }

        /// <summary>
        /// The Extension of the file
        /// </summary>
        public String Extension { get; internal set; }

        /// <summary>
        /// The Type of the File
        /// </summary>
        public FileType FileType { get; internal set; }
    }
}
