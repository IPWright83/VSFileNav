using System.Drawing;
namespace VSNav
{
    partial class FileNav
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileNav));
            this.panel1 = new System.Windows.Forms.Panel();
            this.dataGrid = new VSNav.FileDataGridView();
            this.filter = new VSNav.FilterControl();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.dataGrid);
            this.panel1.Location = new System.Drawing.Point(6, 6);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(1);
            this.panel1.Size = new System.Drawing.Size(958, 294);
            this.panel1.TabIndex = 2;
            // 
            // dataGrid
            // 
            this.dataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGrid.Location = new System.Drawing.Point(1, 1);
            this.dataGrid.MultiSelect = false;
            this.dataGrid.Name = "dataGrid";
            this.dataGrid.ReadOnly = true;
            this.dataGrid.Size = new System.Drawing.Size(956, 292);
            this.dataGrid.TabIndex = 1;
            // 
            // filter
            // 
            this.filter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.filter.Location = new System.Drawing.Point(6, 298);
            this.filter.Margin = new System.Windows.Forms.Padding(0);
            this.filter.Name = "filter";
            this.filter.Padding = new System.Windows.Forms.Padding(6);
            this.filter.Size = new System.Drawing.Size(956, 61);
            this.filter.TabIndex = 0;
            // 
            // FileNav
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(968, 365);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.filter);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FileNav";
            this.Padding = new System.Windows.Forms.Padding(6);
            this.Text = "QuickFileNavigation";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private FilterControl filter;
        private FileDataGridView dataGrid;
        private System.Windows.Forms.Panel panel1;
    }
}