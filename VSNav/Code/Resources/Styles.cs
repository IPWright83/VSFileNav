using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VSNav
{
    /// <summary>
    /// Helper class that returns a set of Styles
    /// </summary>
    public static class Styles
    {
        /// <summary>
        /// Grab the style set
        /// </summary>
        public static IStyle GetStyle()
        {
            switch (Version.VSVersion)
            {
                case VS_Version.VS2010: return VS2010;
                case VS_Version.VS2012Dark: return VS2012Dark;
                case VS_Version.VS2012Light: return VS2012Light;
                default:
                    return VS2010;
            }
        }

        /// <summary>
        /// Returns the VS2010 Style
        /// </summary>
        public static IStyle VS2010 
        {
            get { return _vs2010; } 
        }
        private static VS2010Colours _vs2010 = new VS2010Colours();

        /// <summary>
        /// Returns the VS2012 Light Style
        /// </summary>
        public static IStyle VS2012Light 
        {
            get { return _vs2012Light; } 
        }
        private static VS2012LightColours _vs2012Light = new VS2012LightColours();

        /// <summary>
        /// Returns the VS2012 Dark Style
        /// </summary>
        public static IStyle VS2012Dark
        {
            get { return _vs2012Dark; }
        }
        private static VS2012DarkColours _vs2012Dark = new VS2012DarkColours();

        /// <summary>
        /// VS 2010 Colours
        /// </summary>
        private class VS2010Colours : IStyle
        {
            /// <inheritdoc/>
            public Color DialogBackgroundColour
            {
                get { return Color.FromArgb(53, 73, 106); }
            }

            /// <inheritdoc/>
            public Color ContentBackgroundColour
            {
                get { return Color.FromArgb(188, 199, 216); }
            }

            /// <inheritdoc/>
            public Color GridSelectedRowColour 
            {
                get { return Color.FromArgb(255, 232, 166); }
            }

            /// <inheritdoc/>
            public Color FontColour
            {
                get { return Color.FromArgb(0, 0, 0); }
            }

            /// <inheritdoc/>
            public Color GridSelectRowFontColour
            {
                get { return Color.FromArgb(0, 0, 0); }
            }

            /// <inheritdoc/>
            public Color MatchedCharColour
            {
                get { return Color.Red; }
            }

            /// <inheritdoc/>
            public Color SkippedCharColour
            {
                get { return Color.Blue; }
            }

            /// <inheritdoc/>
            public Color GridBorderColour
            {
                get { return Color.FromArgb(230, 233, 237); }
            }

            /// <inheritdoc/>
            public bool CustomizeGridHeader
            {
                get { return false; }
            }

            /// <inheritdoc/>
            public Color GridHeaderBackgroundColour
            {
                get { throw new NotImplementedException(); }
            }

            /// <inheritdoc/>
            public DataGridViewAdvancedCellBorderStyle GridHeaderBorderStyle
            {
                get { throw new NotImplementedException(); }
            }
        }

        /// <summary>
        /// VS 2012 Light Colours
        /// </summary>
        private class VS2012LightColours : IStyle
        {
            /// <inheritdoc/>
            public Color DialogBackgroundColour
            {
                get { return Color.FromArgb(204, 206, 219); }
            }

            /// <inheritdoc/>
            public Color ContentBackgroundColour
            {
                get { return Color.FromArgb(239, 239, 242); }
            }

            /// <inheritdoc/>
            public Color GridSelectedRowColour
            {
                get { return Color.FromArgb(51, 153, 255); }
            }

            /// <inheritdoc/>
            public Color FontColour
            {
                get { return Color.FromArgb(30, 30, 30); }
            }

            /// <inheritdoc/>
            public Color GridSelectRowFontColour
            {
                get { return Color.FromArgb(255, 255, 255); }
            }
            /// <inheritdoc/>
            public Color MatchedCharColour
            {
                get { return Color.Red; }
            }

            /// <inheritdoc/>
            public Color SkippedCharColour
            {
                get { return Color.Blue; }
            }

            /// <inheritdoc/>
            public Color GridBorderColour
            {
                get { return Color.FromArgb(230, 233, 237); }
            }

            /// <inheritdoc/>
            public bool CustomizeGridHeader
            {
                get { return false; }
            }

            /// <inheritdoc/>
            public Color GridHeaderBackgroundColour
            {
                get { throw new NotImplementedException(); }
            }

            /// <inheritdoc/>
            public DataGridViewAdvancedCellBorderStyle GridHeaderBorderStyle
            {
                get { throw new NotImplementedException(); }
            }
        }

        /// <summary>
        /// VS 2012 Dark Colours
        /// </summary>
        public class VS2012DarkColours : IStyle
        {

            /// <inheritdoc/>
            public Color DialogBackgroundColour
            {
                get { return Color.FromArgb(51, 51, 55); }
            }

            /// <inheritdoc/>
            public Color ContentBackgroundColour
            {
                get { return Color.FromArgb(37, 37, 38); }
            }

            /// <inheritdoc/>
            public Color GridSelectedRowColour
            {
                get { return Color.FromArgb(51, 153, 255); }
            }

            /// <inheritdoc/>
            public Color FontColour
            {
                get { return Color.FromArgb(153, 153, 153); }
            }

            /// <inheritdoc/>
            public Color GridSelectRowFontColour
            {
                get { return Color.FromArgb(255, 255, 255); }
            }
            /// <inheritdoc/>
            public Color MatchedCharColour
            {
                get { return Color.Red; }
            }

            /// <inheritdoc/>
            public Color SkippedCharColour
            {
                get { return Color.Blue; }
            }
            /// <inheritdoc/>
            public Color GridBorderColour
            {
                get { return Color.FromArgb(51, 51, 51); }
            }

            /// <inheritdoc/>
            public bool CustomizeGridHeader
            {
                get { return true; }
            }

            /// <inheritdoc/>
            public Color GridHeaderBackgroundColour
            {
                get { return Color.FromArgb(51, 51, 51); }
            }

            /// <inheritdoc/>
            public DataGridViewAdvancedCellBorderStyle GridHeaderBorderStyle
            {
                get { return DataGridViewAdvancedCellBorderStyle.Single; }
            }

        }
    }
}
