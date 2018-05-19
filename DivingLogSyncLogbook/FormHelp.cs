using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SyncLogbook
{
    public partial class FormHelp : Form
    {
        public bool advancedQueries
        {
            set 
            {
                checkBox1.Checked = value;
            }
            get
            {
                return checkBox1.Checked;
            }
        }

        public FormHelp()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            label1.Text = @"This program synchronizes the following tables from a logbook (the « Master ») to another one (the « Replica »):
         Country City Place Shop Trip .

It does not delete rows that are not in the master logbook. It adds only rows that are not in the replica one or updates older rows.

The algorithm is based on the UUID and Updated columns and is fair with table ID's.

The advanced queries are a set of queries that try to set foreign keys in Logbook table with the corresponding text fields (e.g. set Logbook.PlaceID if there is the Logbook.Place is defined in Place table, etc.).

Please backup your logbooks before trying this program.";
            
            linkLabel1.Text = "Support forum on Diving Log website";
            linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
        }

        private void linkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            // Specify that the link was visited.
            this.linkLabel1.LinkVisited = true;

            // Navigate to a URL.
            System.Diagnostics.Process.Start("http://www.divinglog.de/phpbb/viewtopic.php?f=10&t=1583");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
