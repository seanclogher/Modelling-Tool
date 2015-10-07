namespace ModellingTool
{
    partial class frmMIDIParent
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
            this.mSPDataLoad = new System.Windows.Forms.MenuStrip();
            this.fileMSDataLoad = new System.Windows.Forms.ToolStripMenuItem();
            this.loadDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.WindowsMSDataLoad = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.mSPDataLoad.SuspendLayout();
            this.SuspendLayout();
            // 
            // mSPDataLoad
            // 
            this.mSPDataLoad.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMSDataLoad,
            this.WindowsMSDataLoad});
            this.mSPDataLoad.Location = new System.Drawing.Point(0, 0);
            this.mSPDataLoad.Name = "mSPDataLoad";
            this.mSPDataLoad.Size = new System.Drawing.Size(990, 24);
            this.mSPDataLoad.TabIndex = 3;
            this.mSPDataLoad.Text = "MSPdataloadoptions";
            // 
            // fileMSDataLoad
            // 
            this.fileMSDataLoad.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadDataToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileMSDataLoad.Name = "fileMSDataLoad";
            this.fileMSDataLoad.Size = new System.Drawing.Size(37, 20);
            this.fileMSDataLoad.Text = "File";
            // 
            // loadDataToolStripMenuItem
            // 
            this.loadDataToolStripMenuItem.Name = "loadDataToolStripMenuItem";
            this.loadDataToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.loadDataToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.loadDataToolStripMenuItem.Text = "Load Data";
            this.loadDataToolStripMenuItem.Click += new System.EventHandler(this.loadData_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(164, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitMDIParent_Click);
            // 
            // WindowsMSDataLoad
            // 
            this.WindowsMSDataLoad.Name = "WindowsMSDataLoad";
            this.WindowsMSDataLoad.Size = new System.Drawing.Size(68, 20);
            this.WindowsMSDataLoad.Text = "Windows";
            // 
            // frmMIDIParent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(990, 546);
            this.Controls.Add(this.mSPDataLoad);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.mSPDataLoad;
            this.Name = "frmMIDIParent";
            this.Text = "Modelling Start Point";
            this.mSPDataLoad.ResumeLayout(false);
            this.mSPDataLoad.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mSPDataLoad;
        private System.Windows.Forms.ToolStripMenuItem fileMSDataLoad;
        private System.Windows.Forms.ToolStripMenuItem loadDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem WindowsMSDataLoad;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;

    }
}

