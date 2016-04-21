using System.Drawing;
using System.Windows.Forms;

namespace VSNav
{
    /// <summary>
    /// Represents a set of Styles
    /// </summary>
    public interface IStyle
    {
        /// <summary>The Colour of a grid row when it is selected</summary>
        Color GridSelectedRowColour { get; }

        /// <summary>The Colour of the grid font</summary>
        Color FontColour { get; }

        /// <summary>The Colour of the Selected Row Font</summary>
        Color GridSelectRowFontColour { get; }

        /// <summary>The Colour of a cell in the grid</summary>
        Color ContentBackgroundColour { get; }

        /// <summary>Background Colour of the Dialog</summary>
        Color DialogBackgroundColour { get; }

        /// <summary>Colour for a character match</summary>
        Color MatchedCharColour { get; }

        /// <summary>Colour for a skipped character in the matching</summary>
        Color SkippedCharColour { get; }

        /// <summary>The Colour to use for the Grid borders</summary>
        Color GridBorderColour { get; }

        /// <summary>Should the Grid headers be customized?</summary>
        bool CustomizeGridHeader { get; }

        /// <summary>The Customized Grid Header background Colour</summary>
        Color GridHeaderBackgroundColour { get; }

        /// <summary>The Border style for a customized header</summary>
        DataGridViewAdvancedCellBorderStyle GridHeaderBorderStyle { get; }
    }
}
