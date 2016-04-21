using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace VSNav
{
    /// <summary>
    /// Adjust the Drawing behaviour of a DataGrid
    /// </summary>
    public partial class FileDataGridView : DataGridView
    {
        private IStyle style;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileDataGridView"/> class.
        /// </summary>
        public FileDataGridView()
            : base()
        {
            this.DoubleBuffered = true;
            this.ReadOnly = true;
            this.style = Styles.GetStyle();
        }

        /// <summary>
        /// Initializes the style.
        /// </summary>
        private void InitializeStyle()
        {
            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;
            this.AllowUserToResizeRows = false;
            this.RowHeadersVisible = false;
            this.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.ColumnHeadersHeight = 26;
            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.ShowCellErrors = false;
            this.ShowCellToolTips = false;
            this.ShowEditingIcon = false;
            this.ShowRowErrors = false;
            this.AutoGenerateColumns = false;
            this.GridColor = style.GridBorderColour;
            
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World, ((byte)(0)));
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            if (style.CustomizeGridHeader)
            {
                this.EnableHeadersVisualStyles = false;
                dataGridViewCellStyle1.BackColor = style.GridHeaderBackgroundColour;
                dataGridViewCellStyle1.ForeColor = this.style.FontColour;
                this.AdvancedColumnHeadersBorderStyle.All = style.GridHeaderBorderStyle;
            }
            this.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World, ((byte)(0)));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            dataGridViewCellStyle2.SelectionBackColor = this.style.GridSelectedRowColour;
            dataGridViewCellStyle2.SelectionForeColor = this.style.GridSelectRowFontColour;
            dataGridViewCellStyle2.ForeColor = this.style.FontColour;
            dataGridViewCellStyle2.BackColor = this.style.ContentBackgroundColour;
            this.DefaultCellStyle = dataGridViewCellStyle2;
        }

        /// <summary>
        /// Setups up some pre-defined columns.
        /// </summary>
        /// <param name="symbolSearch">if set to <c>true</c> then we are searching for symbols.</param>
        public void SetupColumns(Boolean symbolSearch)
        {
            if (!symbolSearch)
            {
                FilterTextBoxColumn fileColumn = new FilterTextBoxColumn()
                {
                    HeaderText = "Name",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                    DataPropertyName = "NameInfo",
                    ReadOnly = true,
                    FillWeight = 25,
                };
                DataGridViewTextBoxColumn projectColumn = new DataGridViewTextBoxColumn()
                {
                    HeaderText = "Project",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                    DataPropertyName = "Project",
                    ReadOnly = true,
                    FillWeight = 20
                };
                DataGridViewTextBoxColumn pathColumn = new DataGridViewTextBoxColumn()
                {
                    HeaderText = "Path",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                    DataPropertyName = "Path",
                    ReadOnly = true,
                    FillWeight = 55
                };
                this.Columns.Clear();
                this.Columns.AddRange(new DataGridViewColumn[] { fileColumn, projectColumn, pathColumn });
            }

            this.InitializeStyle();
        }

        /// <summary>
        /// Refreshes the data for the control
        /// </summary>
        public void RefreshData<T>(List<T> data)
        {
            this.SuspendLayout();
            this.DataSource = new List<T>();
            this.DataSource = data;
            this.ResumeLayout();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Paint"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"/> that contains the event data.</param>
        /// <exception cref="T:System.Exception">Any exceptions that occur during this method are ignored unless they are one of the following:<see cref="T:System.NullReferenceException"/><see cref="T:System.StackOverflowException"/><see cref="T:System.OutOfMemoryException"/><see cref="T:System.Threading.ThreadAbortException"/><see cref="T:System.ExecutionEngineException"/><see cref="T:System.IndexOutOfRangeException"/><see cref="T:System.AccessViolationException"/></exception>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            int rowHeight = this.RowTemplate.Height;

            int height = this.ColumnHeadersHeight + rowHeight * this.RowCount;
            int imgWidth = this.Width - 2;
            Rectangle rFrame = new Rectangle(0, 0, imgWidth, rowHeight);
            Rectangle rFill = new Rectangle(1, 1, imgWidth - 2, rowHeight);

            Pen pen = new Pen(this.GridColor, 1);

            Bitmap rowImg = new Bitmap(imgWidth, rowHeight);
            Graphics g = Graphics.FromImage(rowImg);
            g.DrawRectangle(pen, rFrame);
            g.FillRectangle(new SolidBrush(this.DefaultCellStyle.BackColor), rFill);

            Bitmap rowImgAAlternative = rowImg.Clone() as Bitmap;
            Graphics g2 = Graphics.FromImage(rowImgAAlternative);
            g2.FillRectangle(new SolidBrush(this.DefaultCellStyle.BackColor), rFill);

            int width = 0;
            for (int j = 0; j < this.ColumnCount; j++)
            {
                g.DrawLine(pen, new Point(width, 0), new Point(width, rowHeight));
                g2.DrawLine(pen, new Point(width, 0), new Point(width, rowHeight));
                width += this.Columns[j].Width;
            }

            int loop = (this.Height - height) / rowHeight;
            for (int j = 0; j < loop + 1; j++)
            {
                int index = this.RowCount + j;
                if (index % 2 == 0)
                {
                    e.Graphics.DrawImage(rowImg, 1, height + j * rowHeight);
                }
                else
                {
                    e.Graphics.DrawImage(rowImgAAlternative, 1, height + j * rowHeight);
                }
            }
        }

        /// <summary>
        /// Method to move the selected item in the Grid
        /// </summary>
        public virtual void Scroll(int rows)
        {
            // Can't scroll if there are no rows
            if(this.Rows.Count == 0 || this.SelectedRows.Count == 0)
                return;

            // The currently selected Row
            DataGridViewRow selectedRow = this.SelectedRows[0];
            int selectedRowIndex = selectedRow.Index;
            int newSelectedRowIndex = selectedRowIndex;

            // Scrolling Down
            if (rows > 0)
            {
                // Determine the new row Index
                newSelectedRowIndex = Math.Min(selectedRowIndex + rows, this.Rows.Count - 1);
            }
            else
            {
                // Determine the new row Index
                newSelectedRowIndex = Math.Max(selectedRowIndex + rows, 0);
            }

            // Select the new Row
            DataGridViewRow newSelectedRow = this.Rows[newSelectedRowIndex];
            selectedRow.Selected = false;
            newSelectedRow.Selected = true;

            if (rows > 0)
            {
                if (!this.IsRowVisible(newSelectedRowIndex))
                {
                    int rowCount = this.Rows.GetRowCount(DataGridViewElementStates.Displayed);
                    this.FirstDisplayedScrollingRowIndex = (newSelectedRowIndex - rowCount) + 1;
                }
            }
            else
            {
                // If the row is not visible scroll the grid
                if (!this.IsRowVisible(newSelectedRowIndex))
                {
                    this.FirstDisplayedScrollingRowIndex = newSelectedRowIndex;
                }
            }
        }

        /// <summary>
        /// Determine if a row is visible or not
        /// </summary>
        private bool IsRowVisible(int index)
        {
            return this.Rows.GetRowState(index).HasFlag(DataGridViewElementStates.Displayed);
        }

        /// <summary>
        /// Handle the KeyDown event manually
        /// </summary>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up) { e.Handled = true; Scroll(-1); }
            if (e.KeyCode == Keys.Down) { e.Handled = true; Scroll(1); }
            if (e.KeyCode == Keys.PageUp) { e.Handled = true; Scroll(-this.Rows.GetRowCount(DataGridViewElementStates.Displayed)); }
            if (e.KeyCode == Keys.PageDown) { e.Handled = true; Scroll(this.Rows.GetRowCount(DataGridViewElementStates.Displayed)); }

            base.OnKeyDown(e);
        }

        /// <summary>
        /// Initializes the component.
        /// </summary>
        private void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // GridLineDataGridView
            // 
            this.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ShowCellErrors = false;
            this.ShowCellToolTips = false;
            this.ShowEditingIcon = false;
            this.ShowRowErrors = false;
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
