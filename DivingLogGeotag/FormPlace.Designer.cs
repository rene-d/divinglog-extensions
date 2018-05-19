namespace DL5GeoTag
{
    partial class FormPlace
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPlace));
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lstDiveSites = new System.Windows.Forms.ListBox();
            this.placeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.PlaceDataSet = new DL5GeoTag.PlaceDataSet();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblMaxDepthUnit = new System.Windows.Forms.Label();
            this.buttonGeotagFromImage = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtDepth = new DL5GeoTag.TextBoxUnit();
            this.txtAltitude2 = new System.Windows.Forms.TextBox();
            this.txtLon2 = new System.Windows.Forms.TextBox();
            this.txtLat2 = new System.Windows.Forms.TextBox();
            this.buttonGeotagFromGE = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtComments = new System.Windows.Forms.TextBox();
            this.txtAltitude = new System.Windows.Forms.TextBox();
            this.txtLon = new System.Windows.Forms.TextBox();
            this.txtLat = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonOpen = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonUndo = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonHelp = new System.Windows.Forms.ToolStripButton();
            this.placeTableAdapter = new DL5GeoTag.PlaceDataSetTableAdapters.PlaceTableAdapter();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.placeBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PlaceDataSet)).BeginInit();
            this.panel2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.statusStrip1);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainer1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(610, 336);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(610, 383);
            this.toolStripContainer1.TabIndex = 0;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(610, 22);
            this.statusStrip1.TabIndex = 0;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(79, 17);
            this.toolStripStatusLabel1.Text = "logbook.mdb";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lstDiveSites);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel2);
            this.splitContainer1.Size = new System.Drawing.Size(610, 336);
            this.splitContainer1.SplitterDistance = 230;
            this.splitContainer1.TabIndex = 8;
            // 
            // lstDiveSites
            // 
            this.lstDiveSites.DataSource = this.placeBindingSource;
            this.lstDiveSites.DisplayMember = "Place";
            this.lstDiveSites.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstDiveSites.FormattingEnabled = true;
            this.lstDiveSites.Location = new System.Drawing.Point(0, 0);
            this.lstDiveSites.Name = "lstDiveSites";
            this.lstDiveSites.Size = new System.Drawing.Size(230, 336);
            this.lstDiveSites.TabIndex = 8;
            this.lstDiveSites.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lstDiveSites_DrawItem);
            // 
            // placeBindingSource
            // 
            this.placeBindingSource.DataMember = "Place";
            this.placeBindingSource.DataSource = this.PlaceDataSet;
            this.placeBindingSource.CurrentChanged += new System.EventHandler(this.placeBindingSource_CurrentChanged);
            this.placeBindingSource.ListChanged += new System.ComponentModel.ListChangedEventHandler(this.placeBindingSource_ListChanged);
            // 
            // PlaceDataSet
            // 
            this.PlaceDataSet.DataSetName = "PlaceDataSet";
            this.PlaceDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // panel2
            // 
            this.panel2.AllowDrop = true;
            this.panel2.Controls.Add(this.lblMaxDepthUnit);
            this.panel2.Controls.Add(this.buttonGeotagFromImage);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.txtDepth);
            this.panel2.Controls.Add(this.txtAltitude2);
            this.panel2.Controls.Add(this.txtLon2);
            this.panel2.Controls.Add(this.txtLat2);
            this.panel2.Controls.Add(this.buttonGeotagFromGE);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.txtComments);
            this.panel2.Controls.Add(this.txtAltitude);
            this.panel2.Controls.Add(this.txtLon);
            this.panel2.Controls.Add(this.txtLat);
            this.panel2.Controls.Add(this.txtName);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(376, 336);
            this.panel2.TabIndex = 7;
            this.panel2.DragDrop += new System.Windows.Forms.DragEventHandler(this.panel2_DragDrop);
            this.panel2.DragEnter += new System.Windows.Forms.DragEventHandler(this.panel2_DragEnter);
            // 
            // lblMaxDepthUnit
            // 
            this.lblMaxDepthUnit.AutoSize = true;
            this.lblMaxDepthUnit.Location = new System.Drawing.Point(142, 122);
            this.lblMaxDepthUnit.Name = "lblMaxDepthUnit";
            this.lblMaxDepthUnit.Size = new System.Drawing.Size(15, 13);
            this.lblMaxDepthUnit.TabIndex = 14;
            this.lblMaxDepthUnit.Text = "m";
            this.lblMaxDepthUnit.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // buttonGeotagFromImage
            // 
            this.buttonGeotagFromImage.Location = new System.Drawing.Point(284, 100);
            this.buttonGeotagFromImage.Name = "buttonGeotagFromImage";
            this.buttonGeotagFromImage.Size = new System.Drawing.Size(80, 35);
            this.buttonGeotagFromImage.TabIndex = 13;
            this.buttonGeotagFromImage.Text = "Geo-tag from image";
            this.buttonGeotagFromImage.UseVisualStyleBackColor = true;
            this.buttonGeotagFromImage.Click += new System.EventHandler(this.buttonGeotagFromImage_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(0, 122);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Max depth :";
            // 
            // txtDepth
            // 
            this.txtDepth.DataBindings.Add(new System.Windows.Forms.Binding("ValueUnit", this.placeBindingSource, "MaxDepth", true));
            this.txtDepth.Location = new System.Drawing.Point(74, 119);
            this.txtDepth.Name = "txtDepth";
            this.txtDepth.Size = new System.Drawing.Size(62, 20);
            this.txtDepth.TabIndex = 11;
            this.txtDepth.UnitFeet = false;
            this.txtDepth.ValueUnit = 0D;
            // 
            // txtAltitude2
            // 
            this.txtAltitude2.Location = new System.Drawing.Point(181, 93);
            this.txtAltitude2.Name = "txtAltitude2";
            this.txtAltitude2.ReadOnly = true;
            this.txtAltitude2.Size = new System.Drawing.Size(96, 20);
            this.txtAltitude2.TabIndex = 10;
            this.txtAltitude2.Visible = false;
            // 
            // txtLon2
            // 
            this.txtLon2.Location = new System.Drawing.Point(181, 67);
            this.txtLon2.Name = "txtLon2";
            this.txtLon2.ReadOnly = true;
            this.txtLon2.Size = new System.Drawing.Size(96, 20);
            this.txtLon2.TabIndex = 9;
            this.txtLon2.Visible = false;
            // 
            // txtLat2
            // 
            this.txtLat2.Location = new System.Drawing.Point(181, 41);
            this.txtLat2.Name = "txtLat2";
            this.txtLat2.ReadOnly = true;
            this.txtLat2.Size = new System.Drawing.Size(96, 20);
            this.txtLat2.TabIndex = 8;
            this.txtLat2.Visible = false;
            // 
            // buttonGeotagFromGE
            // 
            this.buttonGeotagFromGE.Image = global::DL5GeoTag.Properties.Resources.GoogleEarth32;
            this.buttonGeotagFromGE.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonGeotagFromGE.Location = new System.Drawing.Point(284, 41);
            this.buttonGeotagFromGE.Name = "buttonGeotagFromGE";
            this.buttonGeotagFromGE.Size = new System.Drawing.Size(80, 53);
            this.buttonGeotagFromGE.TabIndex = 7;
            this.buttonGeotagFromGE.Text = "Geo-tag";
            this.buttonGeotagFromGE.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonGeotagFromGE.UseVisualStyleBackColor = true;
            this.buttonGeotagFromGE.Click += new System.EventHandler(this.buttonGeotagFromGE_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(1, 145);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Comments :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(0, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Altitude :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Longitude :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Latitude :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Name :";
            // 
            // txtComments
            // 
            this.txtComments.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtComments.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.placeBindingSource, "Comments", true));
            this.txtComments.Location = new System.Drawing.Point(3, 161);
            this.txtComments.Multiline = true;
            this.txtComments.Name = "txtComments";
            this.txtComments.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtComments.Size = new System.Drawing.Size(361, 172);
            this.txtComments.TabIndex = 4;
            // 
            // txtAltitude
            // 
            this.txtAltitude.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.placeBindingSource, "Altitude", true));
            this.txtAltitude.Location = new System.Drawing.Point(74, 93);
            this.txtAltitude.Name = "txtAltitude";
            this.txtAltitude.Size = new System.Drawing.Size(96, 20);
            this.txtAltitude.TabIndex = 3;
            // 
            // txtLon
            // 
            this.txtLon.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.placeBindingSource, "Lon", true));
            this.txtLon.Location = new System.Drawing.Point(74, 67);
            this.txtLon.Name = "txtLon";
            this.txtLon.Size = new System.Drawing.Size(96, 20);
            this.txtLon.TabIndex = 2;
            // 
            // txtLat
            // 
            this.txtLat.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.placeBindingSource, "Lat", true));
            this.txtLat.Location = new System.Drawing.Point(74, 41);
            this.txtLat.Name = "txtLat";
            this.txtLat.Size = new System.Drawing.Size(96, 20);
            this.txtLat.TabIndex = 1;
            // 
            // txtName
            // 
            this.txtName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.placeBindingSource, "Place", true));
            this.txtName.Location = new System.Drawing.Point(74, 15);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(290, 20);
            this.txtName.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonOpen,
            this.toolStripSeparator1,
            this.toolStripButton2,
            this.toolStripButtonSave,
            this.toolStripButtonUndo,
            this.toolStripSeparator2,
            this.toolStripButton1,
            this.toolStripButtonHelp});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(610, 25);
            this.toolStrip1.Stretch = true;
            this.toolStrip1.TabIndex = 0;
            // 
            // toolStripButtonOpen
            // 
            this.toolStripButtonOpen.Image = global::DL5GeoTag.Properties.Resources.openfolder_24;
            this.toolStripButtonOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonOpen.Name = "toolStripButtonOpen";
            this.toolStripButtonOpen.Size = new System.Drawing.Size(106, 22);
            this.toolStripButtonOpen.Text = "Open Logbook";
            this.toolStripButtonOpen.Click += new System.EventHandler(this.toolStripButtonOpen_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = global::DL5GeoTag.Properties.Resources.Refresh;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "Refresh";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButtonRefresh_Click);
            // 
            // toolStripButtonSave
            // 
            this.toolStripButtonSave.Image = global::DL5GeoTag.Properties.Resources.Save;
            this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSave.Name = "toolStripButtonSave";
            this.toolStripButtonSave.Size = new System.Drawing.Size(51, 22);
            this.toolStripButtonSave.Text = "Save";
            this.toolStripButtonSave.Click += new System.EventHandler(this.toolStripButtonSave_Click);
            // 
            // toolStripButtonUndo
            // 
            this.toolStripButtonUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonUndo.Image = global::DL5GeoTag.Properties.Resources.Edit_Undo;
            this.toolStripButtonUndo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonUndo.Name = "toolStripButtonUndo";
            this.toolStripButtonUndo.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonUndo.Text = "Undo current site";
            this.toolStripButtonUndo.Click += new System.EventHandler(this.toolStripButtonUndo_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(35, 22);
            this.toolStripButton1.Text = "KML";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButtonHelp
            // 
            this.toolStripButtonHelp.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonHelp.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonHelp.Image")));
            this.toolStripButtonHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonHelp.Name = "toolStripButtonHelp";
            this.toolStripButtonHelp.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonHelp.Text = "Help";
            this.toolStripButtonHelp.Click += new System.EventHandler(this.toolStripButtonHelp_Click);
            // 
            // placeTableAdapter
            // 
            this.placeTableAdapter.ClearBeforeFill = true;
            // 
            // FormPlace
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(610, 383);
            this.Controls.Add(this.toolStripContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormPlace";
            this.Text = "Dive site geotagging for Diving Log 5.0";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormPlace_FormClosing);
            this.Load += new System.EventHandler(this.Form3_Load);
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.placeBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PlaceDataSet)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.BindingSource placeBindingSource;
        private PlaceDataSet PlaceDataSet;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonSave;
        private PlaceDataSetTableAdapters.PlaceTableAdapter placeTableAdapter;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonOpen;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripButton toolStripButtonUndo;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox lstDiveSites;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button buttonGeotagFromImage;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtAltitude2;
        private System.Windows.Forms.TextBox txtLon2;
        private System.Windows.Forms.TextBox txtLat2;
        private System.Windows.Forms.Button buttonGeotagFromGE;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtComments;
        private System.Windows.Forms.TextBox txtAltitude;
        private System.Windows.Forms.TextBox txtLon;
        private System.Windows.Forms.TextBox txtLat;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblMaxDepthUnit;
        private DL5GeoTag.TextBoxUnit txtDepth;
        private System.Windows.Forms.ToolStripButton toolStripButtonHelp;


    }
}