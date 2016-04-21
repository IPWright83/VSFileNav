using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSNav
{
    public enum FileType
    {
        ASCX,
        ASPX,
        EDMX,
        FSX,
        FSI,
        Manifest,
        CSharp,
        CPlusPlus,
        CPlusPlusHeader,
        ScriptFile,
        XML,
        XAML,
        Config,
        Icon,
        Image,
        HTML,
        XSD,
        Resource,
        CSS,
        XSLT,
        Text,
        ASP,
        ClassDiagram,
        FSharp,
        Settings,
        IDL,
        Template,
        VisualBasic,
        Master,
        Ruleset,
        Sequence,
        Sitemap,
        Skin,
        Unknown
    }

    /// <summary>
    /// Describes a File
    /// </summary>
    public class FileInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileInfo"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="itemPath">The item path.</param>
        /// <param name="projectName">Name of the project.</param>
        public FileInfo(ProjectItemInfo itemInfo, ProjectInfo projectInfo)
        {
            this.ProjectInfo = projectInfo;
            this.ItemInfo = itemInfo;
            
             Image image = Images.GetImages().GetIcon(this.FileType);
             this.NameInfo = new NameInfo(image, this.Name);
        }

        #region Properties

        /// <summary>
        /// Gets or sets the Image and Name Pair.
        /// </summary>
        public NameInfo NameInfo { get; private set; }

        /// <summary>
        /// Name of the File, including the extension
        /// </summary>
        [Browsable(false)]
        public String Name { get { return this.ItemInfo.Name; } }

        /// <summary>
        /// Gets the project info.
        /// </summary>
        [Browsable(false)]
        public ProjectInfo ProjectInfo { get; internal set; }

        /// <summary>
        /// Gets the Project Item Info
        /// </summary>
        [Browsable(false)]
        public ProjectItemInfo ItemInfo { get; private set; }

        /// <summary>
        /// Project the file is within
        /// </summary>
        public String Project { get { return this.ProjectInfo.Name;} }

        /// <summary>
        /// Full path to the File
        /// </summary>
        public String Path { get { return this.ItemInfo.FilePath; } }

        /// <summary>
        /// The Type of the file.
        /// </summary>
        [Browsable(false)]
        public FileType FileType { get { return this.ItemInfo.FileType; } }

        #endregion
    }
}
