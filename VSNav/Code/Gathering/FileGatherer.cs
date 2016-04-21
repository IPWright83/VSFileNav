using EnvDTE;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSNav
{
    /// <summary>
    /// Used to Gather files
    /// </summary>
    public class FileGatherer : Gatherer
    {
        private readonly HashSet<String> kindsToIgnore = new HashSet<string>()
        {
            "{66A2671F-8FB5-11D2-AA7E-00C04F688DDE}",
            "{66A26722-8FB5-11D2-AA7E-00C04F688DDE}"
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="FileGatherer"/> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        public FileGatherer(EventManager manager)
            : base(manager)
        { }

        /// <summary>
        /// List of discovered files
        /// </summary>
        private List<FileInfo> files = new List<FileInfo>();

        /// <summary>
        /// List of discovered files
        /// </summary>
        public List<FileInfo> Files
        {
            get
            {
                return files;
            }
        }

        #region Methods

        /// <inheritdoc/>
        public override void Clear()
        {
            this.files = new List<FileInfo>();
        }

        /// <inheritdoc/>
        public override void Process(ProjectItem item, ProjectInfo projectInfo)
        {
            #region Process Files

            try
            {
                if (kindsToIgnore.Contains(item.Kind) == false)
                {
                    // Attempt to get all of the files
                    for (short i = 0; i < item.FileCount; i++)
                    {
                        ProjectItemInfo info = new ProjectItemInfo(item, i);
                        if (!String.IsNullOrEmpty(info.Name) && !String.IsNullOrEmpty(info.Extension))
                        {
                            this.files.Add(new FileInfo(info, projectInfo));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }

            #endregion

            // Process any sub-projects
            if (item.SubProject != null)
            {
                Process(item.SubProject);
            }

            // Process and sub-items
            if (item.ProjectItems != null)
            {
                foreach (ProjectItem subItem in item.ProjectItems)
                {
                    Process(subItem, projectInfo);
                }
            }
        }

        /// <inheritdoc/>
        public override void Remove(Project project)
        {
            base.Remove(project);

            ProjectInfo projectInfo = new ProjectInfo(project);
            this.files.RemoveAll(f => f.ProjectInfo.CompareTo(projectInfo) == 0);
        }

        /// <inheritdoc/>
        public override void Remove(ProjectItem item)
        {
            if (kindsToIgnore.Contains(item.Kind) == false)
            {
                List<String> filePathsToRemove = new List<string>();

                // Attempt to get all of the files
                for (short i = 0; i < item.FileCount; i++)
                {
                    // Only add items with a proper name
                    String filePath = item.get_FileNames(i);
                    filePathsToRemove.Add(filePath);
                }

                this.files.RemoveAll(f => filePathsToRemove.Contains(f.Path));
            }

            base.Remove(item);
        }

        /// <inheritdoc/>
        public override void Rename(Project project, string oldName)
        {
            base.Rename(project, oldName);

            ProjectInfo projectInfo = new ProjectInfo(project);
            this.files.Where(f => f.ProjectInfo.FullName == oldName)
                        .ToList()
                        .ForEach(f => f.ProjectInfo = projectInfo);
        }

        /// <inheritdoc/>
        public override void Rename(ProjectItem item, string oldName)
        {
            base.Rename(item, oldName);
            ProjectInfo projectInfo = new ProjectInfo(item.ContainingProject);
            
            // Remove the old file and re-process the item
            this.files.RemoveAll(f => f.ProjectInfo.CompareTo(projectInfo) == 0 && f.Name == oldName);
            Process(item, projectInfo);
        }

        #endregion
    }
}
