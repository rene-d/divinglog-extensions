namespace SyncLogbook
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.buttonSimulate = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.textBoxMaster = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxReplica = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonSynchronize = new System.Windows.Forms.Button();
            this.buttonSetIDs = new System.Windows.Forms.Button();
            this.buttonHelp = new System.Windows.Forms.Button();
            this.buttonOpenReplica = new System.Windows.Forms.Button();
            this.buttonOpenMaster = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonSimulate
            // 
            this.buttonSimulate.Location = new System.Drawing.Point(12, 64);
            this.buttonSimulate.Name = "buttonSimulate";
            this.buttonSimulate.Size = new System.Drawing.Size(79, 25);
            this.buttonSimulate.TabIndex = 3;
            this.buttonSimulate.Text = "Simulate";
            this.buttonSimulate.UseVisualStyleBackColor = true;
            this.buttonSimulate.Click += new System.EventHandler(this.buttonSimulate_Click);
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 95);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(355, 147);
            this.listBox1.TabIndex = 4;
            // 
            // textBoxMaster
            // 
            this.textBoxMaster.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMaster.Location = new System.Drawing.Point(63, 12);
            this.textBoxMaster.Name = "textBoxMaster";
            this.textBoxMaster.Size = new System.Drawing.Size(272, 20);
            this.textBoxMaster.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Master :";
            // 
            // textBoxReplica
            // 
            this.textBoxReplica.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxReplica.Location = new System.Drawing.Point(63, 38);
            this.textBoxReplica.Name = "textBoxReplica";
            this.textBoxReplica.Size = new System.Drawing.Size(272, 20);
            this.textBoxReplica.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Replica :";
            // 
            // buttonSynchronize
            // 
            this.buttonSynchronize.Location = new System.Drawing.Point(97, 64);
            this.buttonSynchronize.Name = "buttonSynchronize";
            this.buttonSynchronize.Size = new System.Drawing.Size(79, 25);
            this.buttonSynchronize.TabIndex = 3;
            this.buttonSynchronize.Text = "Synchronize";
            this.buttonSynchronize.UseVisualStyleBackColor = true;
            this.buttonSynchronize.Click += new System.EventHandler(this.buttonSynchronize_Click);
            // 
            // buttonSetIDs
            // 
            this.buttonSetIDs.Location = new System.Drawing.Point(182, 64);
            this.buttonSetIDs.Name = "buttonSetIDs";
            this.buttonSetIDs.Size = new System.Drawing.Size(79, 25);
            this.buttonSetIDs.TabIndex = 8;
            this.buttonSetIDs.Text = "Set IDs";
            this.buttonSetIDs.UseVisualStyleBackColor = true;
            this.buttonSetIDs.Click += new System.EventHandler(this.buttonSetIDs_Click);
            // 
            // buttonHelp
            // 
            this.buttonHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonHelp.Location = new System.Drawing.Point(320, 64);
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.Size = new System.Drawing.Size(47, 25);
            this.buttonHelp.TabIndex = 9;
            this.buttonHelp.Text = "Help";
            this.buttonHelp.UseVisualStyleBackColor = true;
            this.buttonHelp.Click += new System.EventHandler(this.buttonHelp_Click);
            // 
            // buttonOpenReplica
            // 
            this.buttonOpenReplica.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOpenReplica.Image = global::SyncLogbook.Properties.Resources.Folder;
            this.buttonOpenReplica.Location = new System.Drawing.Point(341, 39);
            this.buttonOpenReplica.Name = "buttonOpenReplica";
            this.buttonOpenReplica.Size = new System.Drawing.Size(26, 19);
            this.buttonOpenReplica.TabIndex = 7;
            this.buttonOpenReplica.UseVisualStyleBackColor = true;
            this.buttonOpenReplica.Click += new System.EventHandler(this.buttonOpenReplica_Click);
            // 
            // buttonOpenMaster
            // 
            this.buttonOpenMaster.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOpenMaster.Image = global::SyncLogbook.Properties.Resources.Folder;
            this.buttonOpenMaster.Location = new System.Drawing.Point(341, 13);
            this.buttonOpenMaster.Name = "buttonOpenMaster";
            this.buttonOpenMaster.Size = new System.Drawing.Size(26, 19);
            this.buttonOpenMaster.TabIndex = 7;
            this.buttonOpenMaster.UseVisualStyleBackColor = true;
            this.buttonOpenMaster.Click += new System.EventHandler(this.buttonOpenMaster_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 250);
            this.Controls.Add(this.buttonHelp);
            this.Controls.Add(this.buttonSetIDs);
            this.Controls.Add(this.buttonOpenReplica);
            this.Controls.Add(this.buttonOpenMaster);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxReplica);
            this.Controls.Add(this.textBoxMaster);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.buttonSynchronize);
            this.Controls.Add(this.buttonSimulate);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Sync Logbook for Diving Log  5.0";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonSimulate;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.TextBox textBoxMaster;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxReplica;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonOpenMaster;
        private System.Windows.Forms.Button buttonOpenReplica;
        private System.Windows.Forms.Button buttonSynchronize;
        private System.Windows.Forms.Button buttonSetIDs;
        private System.Windows.Forms.Button buttonHelp;
    }
}

