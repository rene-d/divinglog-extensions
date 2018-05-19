namespace DivePictures
{
    partial class FormBatchPattern
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBatchPattern));
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.scintilla1 = new ScintillaNet.Scintilla();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonReset = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonUndoChanges = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.depthSiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.depthSiteLogoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.webSiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripDropDownButton();
            this.GPSAltitudeRefToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.websiteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButtonHelp = new System.Windows.Forms.ToolStripButton();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scintilla1)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.scintilla1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(606, 328);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(606, 353);
            this.toolStripContainer1.TabIndex = 0;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // scintilla1
            // 
            this.scintilla1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scintilla1.Location = new System.Drawing.Point(0, 0);
            this.scintilla1.Name = "scintilla1";
            this.scintilla1.Size = new System.Drawing.Size(606, 328);
            this.scintilla1.Styles.BraceBad.FontName = "Verdana";
            this.scintilla1.Styles.BraceLight.FontName = "Verdana";
            this.scintilla1.Styles.ControlChar.FontName = "Verdana";
            this.scintilla1.Styles.Default.FontName = "Verdana";
            this.scintilla1.Styles.IndentGuide.FontName = "Verdana";
            this.scintilla1.Styles.LastPredefined.FontName = "Verdana";
            this.scintilla1.Styles.LineNumber.FontName = "Verdana";
            this.scintilla1.Styles.Max.FontName = "Verdana";
            this.scintilla1.TabIndex = 1;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonReset,
            this.toolStripButtonUndoChanges,
            this.toolStripButton3,
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripButtonHelp});
            this.toolStrip1.Location = new System.Drawing.Point(3, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(365, 25);
            this.toolStrip1.TabIndex = 0;
            // 
            // toolStripButtonReset
            // 
            this.toolStripButtonReset.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonReset.Image = global::DivePictures.Properties.Resources.NewDocument;
            this.toolStripButtonReset.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.toolStripButtonReset.Name = "toolStripButtonReset";
            this.toolStripButtonReset.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonReset.Text = "Reset";
            this.toolStripButtonReset.Click += new System.EventHandler(this.toolStripButtonReset_Click);
            // 
            // toolStripButtonUndoChanges
            // 
            this.toolStripButtonUndoChanges.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonUndoChanges.Image = global::DivePictures.Properties.Resources.RestartHS;
            this.toolStripButtonUndoChanges.ImageTransparentColor = System.Drawing.Color.Black;
            this.toolStripButtonUndoChanges.Name = "toolStripButtonUndoChanges";
            this.toolStripButtonUndoChanges.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonUndoChanges.Text = "Undo changes";
            this.toolStripButtonUndoChanges.Click += new System.EventHandler(this.toolStripButtonUndoChanges_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(70, 22);
            this.toolStripButton3.Text = "Comments";
            this.toolStripButton3.ToolTipText = "Add comments (stand for help)";
            this.toolStripButton3.Click += new System.EventHandler(this.CommentsToolStripButton_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.depthSiteToolStripMenuItem,
            this.depthSiteLogoToolStripMenuItem,
            this.toolStripSeparator1,
            this.webSiteToolStripMenuItem});
            this.toolStripButton1.Image = global::DivePictures.Properties.Resources.ImageMagick;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(108, 22);
            this.toolStripButton1.Text = "ImageMagick";
            // 
            // depthSiteToolStripMenuItem
            // 
            this.depthSiteToolStripMenuItem.Name = "depthSiteToolStripMenuItem";
            this.depthSiteToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.depthSiteToolStripMenuItem.Text = "Depth, Site";
            this.depthSiteToolStripMenuItem.Click += new System.EventHandler(this.depthSiteToolStripMenuItem_Click);
            // 
            // depthSiteLogoToolStripMenuItem
            // 
            this.depthSiteLogoToolStripMenuItem.Name = "depthSiteLogoToolStripMenuItem";
            this.depthSiteLogoToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.depthSiteLogoToolStripMenuItem.Text = "Depth, Site, Logo";
            this.depthSiteLogoToolStripMenuItem.Click += new System.EventHandler(this.depthSiteLogoToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(161, 6);
            // 
            // webSiteToolStripMenuItem
            // 
            this.webSiteToolStripMenuItem.Image = global::DivePictures.Properties.Resources.InsertHyperlink;
            this.webSiteToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.webSiteToolStripMenuItem.Name = "webSiteToolStripMenuItem";
            this.webSiteToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.webSiteToolStripMenuItem.Text = "Website";
            this.webSiteToolStripMenuItem.Click += new System.EventHandler(this.webSiteToolStripMenuItem_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.GPSAltitudeRefToolStripMenuItem,
            this.toolStripSeparator2,
            this.websiteToolStripMenuItem1});
            this.toolStripButton2.Image = global::DivePictures.Properties.Resources.exiftool;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(75, 22);
            this.toolStripButton2.Text = "exiftool";
            // 
            // GPSAltitudeRefToolStripMenuItem
            // 
            this.GPSAltitudeRefToolStripMenuItem.Name = "GPSAltitudeRefToolStripMenuItem";
            this.GPSAltitudeRefToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.GPSAltitudeRefToolStripMenuItem.Text = "GPSAltitudeRef";
            this.GPSAltitudeRefToolStripMenuItem.Click += new System.EventHandler(this.GPSAltitudeRefToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(151, 6);
            // 
            // websiteToolStripMenuItem1
            // 
            this.websiteToolStripMenuItem1.Image = global::DivePictures.Properties.Resources.InsertHyperlink;
            this.websiteToolStripMenuItem1.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.websiteToolStripMenuItem1.Name = "websiteToolStripMenuItem1";
            this.websiteToolStripMenuItem1.Size = new System.Drawing.Size(154, 22);
            this.websiteToolStripMenuItem1.Text = "Website";
            this.websiteToolStripMenuItem1.Click += new System.EventHandler(this.websiteToolStripMenuItem1_Click);
            // 
            // toolStripButtonHelp
            // 
            this.toolStripButtonHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonHelp.Image = global::DivePictures.Properties.Resources.Help;
            this.toolStripButtonHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonHelp.Name = "toolStripButtonHelp";
            this.toolStripButtonHelp.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonHelp.Text = "Help";
            this.toolStripButtonHelp.ToolTipText = "Help";
            this.toolStripButtonHelp.Click += new System.EventHandler(this.toolStripButtonHelp_Click);
            // 
            // FormBatchPattern
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(606, 353);
            this.Controls.Add(this.toolStripContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormBatchPattern";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Batch template editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormBatch_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scintilla1)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private ScintillaNet.Scintilla scintilla1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButtonReset;
        private System.Windows.Forms.ToolStripDropDownButton toolStripButton1;
        private System.Windows.Forms.ToolStripMenuItem depthSiteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem depthSiteLogoToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButtonUndoChanges;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem webSiteToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton toolStripButton2;
        private System.Windows.Forms.ToolStripMenuItem GPSAltitudeRefToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem websiteToolStripMenuItem1;
        private System.Windows.Forms.ToolStripButton toolStripButtonHelp;
    }
}