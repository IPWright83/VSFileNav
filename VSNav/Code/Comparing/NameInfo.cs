using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSNav
{
    /// <summary>
    /// Represents an Image and a Name
    /// </summary>
    public class NameInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NameInfo"/> class.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="name">The name.</param>
        public NameInfo(Image image, String name)
        {
            this.Image = image;
            this.Name = name;
        }

        /// <summary>
        /// Gets the image.
        /// </summary>
        /// <value>The image.</value>
        public Image Image { get; private set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public String Name { get; private set; }

        /// <summary>
        /// Gets or sets the match info. Done here for binding. Yuck
        /// </summary>
        internal StringMatch MatchInfo { get; set; }
    }
}
