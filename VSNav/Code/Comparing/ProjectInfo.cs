using EnvDTE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VSNav
{
    /// <summary>
    /// Represents a Project
    /// </summary>
    public class ProjectInfo : IComparable<ProjectInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectInfo" /> class.
        /// </summary>
        /// <param name="project">The project.</param>
        public ProjectInfo(Project project)
        {
            this.Name = project.Name;
            try { this.FullName = project.FullName; } catch (COMException) { }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the unique.
        /// </summary>
        public string FullName { get; set; }

        /// <inheritdoc />
        public int CompareTo(ProjectInfo other)
        {
            if (other == null)
                return -1;

            int retVal = this.Name.CompareTo(other.Name);
            if (retVal != 0)
                return retVal;

            retVal = this.FullName.CompareTo(other.FullName);
            return retVal;
        }
    }
}
