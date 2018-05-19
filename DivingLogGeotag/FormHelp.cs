using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Deployment.Application;

namespace DL5GeoTag
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

            textBox3.Text = @"Geotag dive sites with Google Earth desktop application or coordinates into EXIF.";

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

            linkLabel1.Text = "Support forum on Diving Log website";
            linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
        }

        private void linkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            // Specify that the link was visited.
            linkLabel1.LinkVisited = true;

            // Navigate to a URL.
            System.Diagnostics.Process.Start("http://www.divinglog.de/phpbb/viewtopic.php?f=10&t=1560");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
