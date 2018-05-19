using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Deployment.Application;

namespace DivePictures
{
    public partial class FormHelp : Form
    {
        public FormHelp()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.AcceptButton = button1;

            textBox3.Text = @"Dive pictures management

Associate underwater pictures to dive and calculate their depth.
";

            textBox4.Text = Application.ProductVersion;            
            textBox2.Text = Application.ExecutablePath;

            if (ApplicationDeployment.IsNetworkDeployed)
            {
                textBox1.Text = ApplicationDeployment.CurrentDeployment.DataDirectory;
                textBox4.Text += " (deploy version: " + ApplicationDeployment.CurrentDeployment.CurrentVersion + ")";
            }
            else
            {
                textBox1.Text = Application.LocalUserAppDataPath;
            }

            linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
        }

        private void linkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            linkLabel1.LinkVisited = true;
            System.Diagnostics.Process.Start("http://www.divinglog.de/phpbb/viewtopic.php?f=10&t=1603");
        }

        private void linkLabel2_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            linkLabel2.LinkVisited = true;
            System.Diagnostics.Process.Start("http://www.divinglog.de/phpbb/ucp.php?i=pm&mode=compose&u=883");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void FormHelp_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.U)
            {
                e.Handled = true;
                Properties.Settings.Default.FullVersion = true;
                Properties.Settings.Default.Save();
            }
        }
    }
}
