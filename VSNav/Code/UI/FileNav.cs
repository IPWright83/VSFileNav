using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace VSNav
{
    /// <summary>
    /// Main File Navigation Dialog
    /// </summary>
    public partial class FileNav : Form
    {
#warning IPW : This form doesn't work at 120dpi?

        private List<FileInfo> allFiles;
        private FileGatherer fileGatherer;

        private static Point? lastLocation = null;
        private static Size? lastSize = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuickFileNav"/> class.
        /// </summary>
        /// <param name="gatherer">The gatherer.</param>
        public FileNav(FileGatherer fileGatherer)
        {
            InitializeComponent();

            if (lastSize.HasValue) this.Size = lastSize.Value;
            if (lastLocation.HasValue)
            {
                this.WindowState = FormWindowState.Normal;
                this.StartPosition = FormStartPosition.Manual;
                this.BringToFront();
                this.Location = lastLocation.Value;
            }

            this.fileGatherer = fileGatherer;
            this.allFiles = fileGatherer.Files;
            this.dataGrid.SetupColumns(false);
            SetStyles();

            this.dataGrid.CellDoubleClick += new DataGridViewCellEventHandler(BtnOkClick);
            this.filter.btnOk.Click += new EventHandler(BtnOkClick);
            this.filter.btnCancel.Click += new EventHandler(btnCancel_Click);
            this.filter.btnExplore.Click += new EventHandler(btnExplore_Click);

            // Handle all the KeyDown events. Note that we need to
            // pipe messages from the grid through to the textbox control.
            this.dataGrid.KeyPress += (o, e) =>
            {
                this.filter.txtFilter.Focus();
                SendKeys.Send(e.KeyChar.ToString());
                e.Handled = true;
            };
            this.filter.txtFilter.TextChanged += (o, e) => { UpdateFilter(); };
            this.filter.txtFilter.KeyDown += new KeyEventHandler(OnKeyDown);
            this.dataGrid.KeyDown += new KeyEventHandler(OnKeyDown);            
            this.KeyDown += new KeyEventHandler(OnKeyDown);
            this.filter.txtFilter.MouseWheel += (o, e) => { this.dataGrid.Scroll(-e.Delta / 40); };

            // Set the initial filter and update the dialog
            this.filter.txtFilter.Text = fileGatherer.LastSearchString;
            UpdateFilter();

            // Select the first row
            if (this.dataGrid.Rows.Count > 0)
                this.dataGrid.Rows[0].Selected = true;
        }

        /// <summary>
        /// Update the styles of the dialog
        /// </summary>
        public void SetStyles()
        {
            IStyle style = Styles.GetStyle();

            this.BackColor = style.DialogBackgroundColour;
            this.filter.txtFilter.BackColor = style.ContentBackgroundColour;
            this.filter.txtFilter.ForeColor = style.FontColour;
            this.filter.panel1.BackColor = style.FontColour;
            this.panel1.BackColor = style.FontColour;
        }

        /// <summary>
        /// Cancel Clicked
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Explore clicked.
        /// </summary>
        private void btnExplore_Click(object sender, EventArgs e)
        {
            Explore();
        }

        /// <summary>
        /// Opens the files.
        /// </summary>
        private void Open()
        {
            var dte = this.fileGatherer.DTE;
            if (dte != null)
            {
                foreach (DataGridViewRow row in this.dataGrid.SelectedRows)
                {
                    String filePath = row.Cells[2].Value.ToString();
                    dte.ItemOperations.OpenFile(filePath);
                }
            }
        }

        /// <summary>
        /// Explores to a file.
        /// </summary>
        private void Explore()
        {
            foreach (DataGridViewRow row in this.dataGrid.SelectedRows)
            {
                String filePath = row.Cells[2].Value.ToString();
                if (File.Exists(filePath))
                {
                    string argument = @"/select, " + filePath;
                    System.Diagnostics.Process.Start("explorer.exe", argument);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Unable to Open File", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Updates the filter.
        /// </summary>
        private void UpdateFilter()
        {
            ShowStringDelegate showStringDelegate = this.filter.GetShowStringDelegate();
            IComparer<FileInfo> comparer = new DoubleComparer();
            List<FileInfo> filteredFiles = allFiles.Where((f) =>
            {
                f.NameInfo.MatchInfo = showStringDelegate(f.Name);
                return f.NameInfo.MatchInfo.MatchFraction > 0;
            })
                .ToList();

            filteredFiles.Sort(comparer);
            this.dataGrid.RefreshData(filteredFiles);
            this.Text = "File Navigation [ Showing " + filteredFiles.Count.ToString() + " of " + this.allFiles.Count + " files ]";
        }

        /// <summary>
        /// Handles the ok click.
        /// </summary>
        private void BtnOkClick(object sender, EventArgs e)
        {
            Open();
            this.Close();
        }

        //protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        //{
        //    if (keyData == (Keys.PageUp | Keys.PageDown))
        //    {
        //        //ShowReplaceDialog() or whatever it is you want to do here.
        //        return true; //we handled the key
        //    }

        //    return false; //we didn't handle it
        //}

        /// <summary>
        /// Handles the KeyDown event of the QuickFileNav control.
        /// </summary>
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            // Close the dialog and don't do anything
            if (e.KeyCode == Keys.Escape) 
            {
                this.Close();
            }
            // Open the item
            else if (e.KeyCode == Keys.Enter)
            {
                Open();
                this.Close();
            }
            // Explore to the file
            else if (e.KeyCode == Keys.E && e.Alt)
            {
                Explore();
            }
            // Scroll the data grid up/down
            else if (!(sender is DataGridView))
            {
                if (e.KeyCode == Keys.Up) { e.Handled = true; this.dataGrid.Scroll(-1); }
                if (e.KeyCode == Keys.Down) { e.Handled = true; this.dataGrid.Scroll(1); }
                if (e.KeyCode == Keys.PageUp) { e.Handled = true; this.dataGrid.Scroll(-this.dataGrid.Rows.GetRowCount(DataGridViewElementStates.Displayed)); }
                if (e.KeyCode == Keys.PageDown) { e.Handled = true; this.dataGrid.Scroll(this.dataGrid.Rows.GetRowCount(DataGridViewElementStates.Displayed)); }
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Closing"/> event.
        /// </summary>
        protected override void OnClosing(CancelEventArgs e)
        {
            this.fileGatherer.LastSearchString = this.filter.txtFilter.Text;
            base.OnClosing(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Shown"/> event.
        /// </summary>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.filter.txtFilter.Focus();
            this.filter.txtFilter.SelectAll();
        }

        /// <summary>
        /// Raised when the form has finished moving
        /// </summary>
        private void FileNav_ResizeEnd(object sender, EventArgs e)
        {
            lastSize = this.Size;
            lastLocation = this.Location;
        }
    }
}
