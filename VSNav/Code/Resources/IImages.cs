using System.Drawing;

namespace VSNav
{
    /// <summary>
    /// Represents the Images for a particular version of Visual Studio
    /// </summary>
    public interface IImages
    {
        /// <summary>
        /// Retrieve an Icon for the given file type
        /// </summary>
        Image GetIcon(FileType fType);
    }
}
