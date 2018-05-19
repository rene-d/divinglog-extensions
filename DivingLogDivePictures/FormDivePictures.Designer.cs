namespace DivePictures
{
    partial class FormDivePictures
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDivePictures));
            this.listBoxDives = new System.Windows.Forms.ListBox();
            this.checkBoxRecursive = new System.Windows.Forms.CheckBox();
            this.buttonAnalyze = new System.Windows.Forms.Button();
            this.listBoxPictures = new System.Windows.Forms.ListBox();
            this.comboBoxAlbums = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonAssociate = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButtonOpenLogbook = new System.Windows.Forms.ToolStripDropDownButton();
            this.openDivingLog50ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.openSuuntoDiveManager4ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.suuntoDiveManager3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uwatecDataTrakToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uwatecSmartTrakToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonResetDiveList = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonKeepSelection = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRemoveSelection = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonTimeShift = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonBatch = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonHelp = new System.Windows.Forms.ToolStripButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.zg1 = new ZedGraph.ZedGraphControl();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxTimeShift = new System.Windows.Forms.TextBox();
            this.buttonBatch = new System.Windows.Forms.Button();
            this.checkBoxDepthInDescription = new System.Windows.Forms.CheckBox();
            this.buttonTimeShiftHelp = new System.Windows.Forms.Button();
            this.buttonBrowseAlbum = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label9 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBoxDives
            // 
            this.listBoxDives.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBoxDives.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBoxDives.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxDives.FormattingEnabled = true;
            this.listBoxDives.ItemHeight = 16;
            this.listBoxDives.Location = new System.Drawing.Point(3, 52);
            this.listBoxDives.Name = "listBoxDives";
            this.listBoxDives.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxDives.Size = new System.Drawing.Size(280, 516);
            this.listBoxDives.TabIndex = 0;
            this.listBoxDives.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBox1_DrawItem);
            this.listBoxDives.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // checkBoxRecursive
            // 
            this.checkBoxRecursive.AutoSize = true;
            this.checkBoxRecursive.Location = new System.Drawing.Point(289, 179);
            this.checkBoxRecursive.Name = "checkBoxRecursive";
            this.checkBoxRecursive.Size = new System.Drawing.Size(69, 17);
            this.checkBoxRecursive.TabIndex = 3;
            this.checkBoxRecursive.Text = "recursive";
            this.checkBoxRecursive.UseVisualStyleBackColor = true;
            // 
            // buttonAnalyze
            // 
            this.buttonAnalyze.Location = new System.Drawing.Point(286, 202);
            this.buttonAnalyze.Name = "buttonAnalyze";
            this.buttonAnalyze.Size = new System.Drawing.Size(69, 27);
            this.buttonAnalyze.TabIndex = 4;
            this.buttonAnalyze.Text = "analyze";
            this.buttonAnalyze.UseVisualStyleBackColor = true;
            this.buttonAnalyze.Click += new System.EventHandler(this.button4_Click);
            // 
            // listBoxPictures
            // 
            this.listBoxPictures.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxPictures.FormattingEnabled = true;
            this.listBoxPictures.Location = new System.Drawing.Point(652, 52);
            this.listBoxPictures.Name = "listBoxPictures";
            this.listBoxPictures.Size = new System.Drawing.Size(253, 225);
            this.listBoxPictures.TabIndex = 5;
            this.listBoxPictures.SelectedIndexChanged += new System.EventHandler(this.listBox2_SelectedIndexChanged);
            this.listBoxPictures.SelectedValueChanged += new System.EventHandler(this.listBox2_SelectedValueChanged);
            this.listBoxPictures.DoubleClick += new System.EventHandler(this.listBox2_DoubleClick);
            // 
            // comboBoxAlbums
            // 
            this.comboBoxAlbums.FormattingEnabled = true;
            this.comboBoxAlbums.Location = new System.Drawing.Point(286, 152);
            this.comboBoxAlbums.Name = "comboBoxAlbums";
            this.comboBoxAlbums.Size = new System.Drawing.Size(324, 21);
            this.comboBoxAlbums.TabIndex = 6;
            this.comboBoxAlbums.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboBox1_DrawItem);
            this.comboBoxAlbums.TextChanged += new System.EventHandler(this.comboBox1_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Begin :";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(52, 21);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(115, 20);
            this.textBox2.TabIndex = 10;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(52, 47);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(115, 20);
            this.textBox3.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(286, 137);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(118, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Path or Picasa albums :";
            // 
            // buttonAssociate
            // 
            this.buttonAssociate.Location = new System.Drawing.Point(286, 250);
            this.buttonAssociate.Name = "buttonAssociate";
            this.buttonAssociate.Size = new System.Drawing.Size(69, 27);
            this.buttonAssociate.TabIndex = 4;
            this.buttonAssociate.Text = "associate";
            this.buttonAssociate.UseVisualStyleBackColor = true;
            this.buttonAssociate.Click += new System.EventHandler(this.buttonAssociate_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 578);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(909, 22);
            this.statusStrip1.TabIndex = 13;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButtonOpenLogbook,
            this.toolStripSeparator1,
            this.toolStripButtonResetDiveList,
            this.toolStripButtonKeepSelection,
            this.toolStripButtonRemoveSelection,
            this.toolStripSeparator2,
            this.toolStripButtonTimeShift,
            this.toolStripButtonBatch,
            this.toolStripButtonHelp});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(909, 31);
            this.toolStrip1.TabIndex = 14;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButtonOpenLogbook
            // 
            this.toolStripDropDownButtonOpenLogbook.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openDivingLog50ToolStripMenuItem,
            this.toolStripSeparator3,
            this.openSuuntoDiveManager4ToolStripMenuItem,
            this.suuntoDiveManager3ToolStripMenuItem,
            this.uwatecDataTrakToolStripMenuItem,
            this.uwatecSmartTrakToolStripMenuItem});
            this.toolStripDropDownButtonOpenLogbook.Image = global::DivePictures.Properties.Resources.Book_Green_48x48;
            this.toolStripDropDownButtonOpenLogbook.ImageTransparentColor = System.Drawing.Color.Black;
            this.toolStripDropDownButtonOpenLogbook.Name = "toolStripDropDownButtonOpenLogbook";
            this.toolStripDropDownButtonOpenLogbook.Size = new System.Drawing.Size(120, 28);
            this.toolStripDropDownButtonOpenLogbook.Text = "Open logbook";
            // 
            // openDivingLog50ToolStripMenuItem
            // 
            this.openDivingLog50ToolStripMenuItem.Image = global::DivePictures.Properties.Resources.DivingLog;
            this.openDivingLog50ToolStripMenuItem.Name = "openDivingLog50ToolStripMenuItem";
            this.openDivingLog50ToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.openDivingLog50ToolStripMenuItem.Text = "Diving Log 5.0";
            this.openDivingLog50ToolStripMenuItem.Click += new System.EventHandler(this.openDivingLog50ToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(191, 6);
            this.toolStripSeparator3.Tag = "";
            // 
            // openSuuntoDiveManager4ToolStripMenuItem
            // 
            this.openSuuntoDiveManager4ToolStripMenuItem.Image = global::DivePictures.Properties.Resources.DM4_48x48;
            this.openSuuntoDiveManager4ToolStripMenuItem.Name = "openSuuntoDiveManager4ToolStripMenuItem";
            this.openSuuntoDiveManager4ToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.openSuuntoDiveManager4ToolStripMenuItem.Tag = "";
            this.openSuuntoDiveManager4ToolStripMenuItem.Text = "Suunto DiveManager 4";
            this.openSuuntoDiveManager4ToolStripMenuItem.Click += new System.EventHandler(this.openSuuntoDiveManager4ToolStripMenuItem_Click);
            // 
            // suuntoDiveManager3ToolStripMenuItem
            // 
            this.suuntoDiveManager3ToolStripMenuItem.Image = global::DivePictures.Properties.Resources.DM3;
            this.suuntoDiveManager3ToolStripMenuItem.Name = "suuntoDiveManager3ToolStripMenuItem";
            this.suuntoDiveManager3ToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.suuntoDiveManager3ToolStripMenuItem.Tag = "hidden";
            this.suuntoDiveManager3ToolStripMenuItem.Text = "Suunto DiveManager 3";
            this.suuntoDiveManager3ToolStripMenuItem.Click += new System.EventHandler(this.suuntoDiveManager3ToolStripMenuItem_Click);
            // 
            // uwatecDataTrakToolStripMenuItem
            // 
            this.uwatecDataTrakToolStripMenuItem.Image = global::DivePictures.Properties.Resources.uwatec;
            this.uwatecDataTrakToolStripMenuItem.Name = "uwatecDataTrakToolStripMenuItem";
            this.uwatecDataTrakToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.uwatecDataTrakToolStripMenuItem.Tag = "hidden";
            this.uwatecDataTrakToolStripMenuItem.Text = "Uwatec DataTrak";
            this.uwatecDataTrakToolStripMenuItem.Click += new System.EventHandler(this.dataTrakToolStripMenuItem_Click);
            // 
            // uwatecSmartTrakToolStripMenuItem
            // 
            this.uwatecSmartTrakToolStripMenuItem.Image = global::DivePictures.Properties.Resources.smarttrak;
            this.uwatecSmartTrakToolStripMenuItem.Name = "uwatecSmartTrakToolStripMenuItem";
            this.uwatecSmartTrakToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.uwatecSmartTrakToolStripMenuItem.Tag = "hidden";
            this.uwatecSmartTrakToolStripMenuItem.Text = "Uwatec SmartTRAK";
            this.uwatecSmartTrakToolStripMenuItem.Click += new System.EventHandler(this.uwatecSmartTrakToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // toolStripButtonResetDiveList
            // 
            this.toolStripButtonResetDiveList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonResetDiveList.Image = global::DivePictures.Properties.Resources.RefreshArrow_Green_32x32_72;
            this.toolStripButtonResetDiveList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonResetDiveList.Name = "toolStripButtonResetDiveList";
            this.toolStripButtonResetDiveList.Size = new System.Drawing.Size(28, 28);
            this.toolStripButtonResetDiveList.Text = "Reset dive list";
            this.toolStripButtonResetDiveList.Click += new System.EventHandler(this.buttonResetList_Click);
            // 
            // toolStripButtonKeepSelection
            // 
            this.toolStripButtonKeepSelection.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonKeepSelection.Image = global::DivePictures.Properties.Resources.format_indent_more;
            this.toolStripButtonKeepSelection.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonKeepSelection.Name = "toolStripButtonKeepSelection";
            this.toolStripButtonKeepSelection.Size = new System.Drawing.Size(28, 28);
            this.toolStripButtonKeepSelection.Text = "Keep selection";
            this.toolStripButtonKeepSelection.Click += new System.EventHandler(this.buttonKeepSelection_Click);
            // 
            // toolStripButtonRemoveSelection
            // 
            this.toolStripButtonRemoveSelection.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRemoveSelection.Image = global::DivePictures.Properties.Resources.format_indent_less;
            this.toolStripButtonRemoveSelection.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRemoveSelection.Name = "toolStripButtonRemoveSelection";
            this.toolStripButtonRemoveSelection.Size = new System.Drawing.Size(28, 28);
            this.toolStripButtonRemoveSelection.Text = "Remove selection";
            this.toolStripButtonRemoveSelection.Click += new System.EventHandler(this.buttonDeleteSelection_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 31);
            // 
            // toolStripButtonTimeShift
            // 
            this.toolStripButtonTimeShift.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonTimeShift.Image = global::DivePictures.Properties.Resources.clock_link_32x32;
            this.toolStripButtonTimeShift.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonTimeShift.Name = "toolStripButtonTimeShift";
            this.toolStripButtonTimeShift.Size = new System.Drawing.Size(28, 28);
            this.toolStripButtonTimeShift.Text = "Time shift setting";
            this.toolStripButtonTimeShift.Click += new System.EventHandler(this.toolStripButtonTimeShift_Click);
            // 
            // toolStripButtonBatch
            // 
            this.toolStripButtonBatch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonBatch.Image = global::DivePictures.Properties.Resources.ms_dos_batch_file;
            this.toolStripButtonBatch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonBatch.Name = "toolStripButtonBatch";
            this.toolStripButtonBatch.Size = new System.Drawing.Size(28, 28);
            this.toolStripButtonBatch.Text = "Batch pattern editor";
            this.toolStripButtonBatch.Click += new System.EventHandler(this.toolStripButtonBatch_Click);
            // 
            // toolStripButtonHelp
            // 
            this.toolStripButtonHelp.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonHelp.Image = global::DivePictures.Properties.Resources.Help;
            this.toolStripButtonHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonHelp.Name = "toolStripButtonHelp";
            this.toolStripButtonHelp.Size = new System.Drawing.Size(28, 28);
            this.toolStripButtonHelp.Text = "Help";
            this.toolStripButtonHelp.Click += new System.EventHandler(this.toolStripButtonHelp_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox3);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(286, 42);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(177, 80);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Dive details";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "End :";
            // 
            // zg1
            // 
            this.zg1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.zg1.EditButtons = System.Windows.Forms.MouseButtons.Left;
            this.zg1.Location = new System.Drawing.Point(286, 283);
            this.zg1.Name = "zg1";
            this.zg1.PanModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)));
            this.zg1.ScrollGrace = 0D;
            this.zg1.ScrollMaxX = 0D;
            this.zg1.ScrollMaxY = 0D;
            this.zg1.ScrollMaxY2 = 0D;
            this.zg1.ScrollMinX = 0D;
            this.zg1.ScrollMinY = 0D;
            this.zg1.ScrollMinY2 = 0D;
            this.zg1.Size = new System.Drawing.Size(619, 285);
            this.zg1.TabIndex = 16;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(649, 36);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(122, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Pictures of current dive :";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox4);
            this.groupBox2.Controls.Add(this.textBox1);
            this.groupBox2.Controls.Add(this.textBox5);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Location = new System.Drawing.Point(469, 42);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(177, 104);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Image details";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(52, 73);
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(115, 20);
            this.textBox4.TabIndex = 10;
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(52, 21);
            this.textBox5.Name = "textBox5";
            this.textBox5.ReadOnly = true;
            this.textBox5.Size = new System.Drawing.Size(115, 20);
            this.textBox5.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 76);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(42, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Depth :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 24);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(36, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "Time :";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(0, 36);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(40, 13);
            this.label8.TabIndex = 9;
            this.label8.Text = "Dives :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(372, 208);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Time shift :";
            // 
            // textBoxTimeShift
            // 
            this.textBoxTimeShift.Location = new System.Drawing.Point(438, 205);
            this.textBoxTimeShift.Name = "textBoxTimeShift";
            this.textBoxTimeShift.ReadOnly = true;
            this.textBoxTimeShift.Size = new System.Drawing.Size(97, 20);
            this.textBoxTimeShift.TabIndex = 18;
            // 
            // buttonBatch
            // 
            this.buttonBatch.Location = new System.Drawing.Point(577, 251);
            this.buttonBatch.Name = "buttonBatch";
            this.buttonBatch.Size = new System.Drawing.Size(69, 26);
            this.buttonBatch.TabIndex = 20;
            this.buttonBatch.Text = "batch";
            this.buttonBatch.UseVisualStyleBackColor = true;
            this.buttonBatch.Click += new System.EventHandler(this.buttonBatch_Click);
            // 
            // checkBoxDepthInDescription
            // 
            this.checkBoxDepthInDescription.AutoSize = true;
            this.checkBoxDepthInDescription.Location = new System.Drawing.Point(363, 255);
            this.checkBoxDepthInDescription.Name = "checkBoxDepthInDescription";
            this.checkBoxDepthInDescription.Size = new System.Drawing.Size(149, 17);
            this.checkBoxDepthInDescription.TabIndex = 21;
            this.checkBoxDepthInDescription.Text = "Prefix depth in Description";
            this.checkBoxDepthInDescription.UseVisualStyleBackColor = true;
            // 
            // buttonTimeShiftHelp
            // 
            this.buttonTimeShiftHelp.Image = global::DivePictures.Properties.Resources.Help;
            this.buttonTimeShiftHelp.Location = new System.Drawing.Point(541, 203);
            this.buttonTimeShiftHelp.Name = "buttonTimeShiftHelp";
            this.buttonTimeShiftHelp.Size = new System.Drawing.Size(24, 24);
            this.buttonTimeShiftHelp.TabIndex = 19;
            this.buttonTimeShiftHelp.UseVisualStyleBackColor = true;
            this.buttonTimeShiftHelp.Click += new System.EventHandler(this.buttonHelpTimeShift_Click);
            // 
            // buttonBrowseAlbum
            // 
            this.buttonBrowseAlbum.Image = global::DivePictures.Properties.Resources.Folder;
            this.buttonBrowseAlbum.Location = new System.Drawing.Point(616, 152);
            this.buttonBrowseAlbum.Name = "buttonBrowseAlbum";
            this.buttonBrowseAlbum.Size = new System.Drawing.Size(30, 21);
            this.buttonBrowseAlbum.TabIndex = 7;
            this.buttonBrowseAlbum.UseVisualStyleBackColor = true;
            this.buttonBrowseAlbum.Click += new System.EventHandler(this.button5_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 50);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(46, 13);
            this.label9.TabIndex = 11;
            this.label9.Text = "Shifted :";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(52, 47);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(115, 20);
            this.textBox1.TabIndex = 10;
            // 
            // FormDivePictures
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(909, 600);
            this.Controls.Add(this.checkBoxDepthInDescription);
            this.Controls.Add(this.buttonBatch);
            this.Controls.Add(this.buttonTimeShiftHelp);
            this.Controls.Add(this.textBoxTimeShift);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.zg1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.buttonBrowseAlbum);
            this.Controls.Add(this.comboBoxAlbums);
            this.Controls.Add(this.listBoxPictures);
            this.Controls.Add(this.buttonAssociate);
            this.Controls.Add(this.buttonAnalyze);
            this.Controls.Add(this.checkBoxRecursive);
            this.Controls.Add(this.listBoxDives);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormDivePictures";
            this.Text = "Dive Pictures for Diving Log 5.0";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormDivePictures_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxDives;
        private System.Windows.Forms.CheckBox checkBoxRecursive;
        private System.Windows.Forms.Button buttonAnalyze;
        private System.Windows.Forms.ListBox listBoxPictures;
        private System.Windows.Forms.ComboBox comboBoxAlbums;
        private System.Windows.Forms.Button buttonBrowseAlbum;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonAssociate;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStrip toolStrip1;        
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonOpenLogbook;
        private System.Windows.Forms.ToolStripMenuItem openDivingLog50ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openSuuntoDiveManager4ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonResetDiveList;
        private System.Windows.Forms.ToolStripButton toolStripButtonRemoveSelection;
        private System.Windows.Forms.ToolStripButton toolStripButtonKeepSelection;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private ZedGraph.ZedGraphControl zg1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxTimeShift;
        private System.Windows.Forms.ToolStripButton toolStripButtonTimeShift;
        private System.Windows.Forms.Button buttonTimeShiftHelp;
        private System.Windows.Forms.Button buttonBatch;
        private System.Windows.Forms.ToolStripButton toolStripButtonBatch;
        private System.Windows.Forms.ToolStripButton toolStripButtonHelp;
        private System.Windows.Forms.CheckBox checkBoxDepthInDescription;
        private System.Windows.Forms.ToolStripMenuItem uwatecDataTrakToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uwatecSmartTrakToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem suuntoDiveManager3ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label9;
    }
}

