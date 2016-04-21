using System.Windows.Forms;

namespace VSNav
{
    public class FilterTextBoxColumn : DataGridViewTextBoxColumn
    {
        public FilterTextBoxColumn()
        {
            this.CellTemplate = new FilterTextBoxCell();
        }
    }
}
