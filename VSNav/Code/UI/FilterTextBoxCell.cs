using System;
using System.Drawing;
using System.Windows.Forms;

namespace VSNav
{
    public class FilterTextBoxCell : DataGridViewTextBoxCell
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterTextBoxCell"/> class.
        /// </summary>
        public FilterTextBoxCell()
            : base()
        {
        }

        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            // Paint the Cell without any content
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, String.Empty, errorText, cellStyle, advancedBorderStyle, paintParts);

            NameInfo info = value as NameInfo;
            if (info != null)
            {
                Point imagePoint = new Point(cellBounds.X + 1, cellBounds.Top + 1);
                Size imageSize = new Size(20, 20);

                if (info.Image != null)
                {
                    imageSize = new Size(info.Image.Width, info.Image.Height);
                    graphics.DrawImage(info.Image, new Rectangle(imagePoint, imageSize));
                }

                PointF fontPoint = new PointF(imagePoint.X + imageSize.Width + 3, cellBounds.Top + 1);
                RenderText(graphics, rowIndex, fontPoint, cellBounds, cellStyle.Font, info);
            }
        }

        private bool IsSelected(int rowIndex)
        {
            DataGridViewRow row = this.OwningRow;
            if (row == null)
                return false;

            DataGridView view = row.DataGridView;
            if (view == null)
                return false;

            return view.Rows[rowIndex].Selected;
        }

        /// <summary>
        /// Renders the text.
        /// </summary>
        public void RenderText(Graphics g, int rowIndex, PointF point, Rectangle cellBounds, Font font, NameInfo info)
        {
            foreach (StringPart part in info.MatchInfo.Parts)
            {
                Brush brush = new SolidBrush(Styles.GetStyle().FontColour);
                if (this.IsSelected(rowIndex)) { brush =  new SolidBrush(Styles.GetStyle().GridSelectRowFontColour); }
                if (part.MatchPart) { brush = new SolidBrush(Styles.GetStyle().MatchedCharColour); }
                else if (part.SkippedPart) { brush = new SolidBrush(Styles.GetStyle().SkippedCharColour); }

                using (brush)
                {
                    g.DrawString(part.Text, font, brush, point);
                    float width = g.MeasureString(part.Text, font, (int)cellBounds.Width, StringFormat.GenericTypographic).Width; // subtract a fudge factor as padding is added
                    point.X += width;
                }
            }
        }
    }
}
