using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace VSNav
{
    public partial class FilterControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterControl" /> class.
        /// </summary>
        public FilterControl()
        {
            InitializeComponent();

            IStyle style = Styles.GetStyle();

            this.btnCancel.BackColor = style.ContentBackgroundColour;
            this.btnExplore.BackColor = style.ContentBackgroundColour;
            this.btnOk.BackColor = style.ContentBackgroundColour;
            this.btnCancel.ForeColor = style.FontColour;
            this.btnExplore.ForeColor = style.FontColour;
            this.btnOk.ForeColor = style.FontColour;
        }
    }
}
