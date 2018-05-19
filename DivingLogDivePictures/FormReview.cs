using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace DivePictures
{
    public partial class FormReview : Form
    {

        DataTable dataTable1 = new DataTable("MyTable");
        DataSet dsForDGV = new DataSet();
        BindingSource bs = new BindingSource();

        public FormReview()
        {
            InitializeComponent();


            dataTable1.Columns.Add(new DataColumn("Num"));
            dataTable1.Columns.Add(new DataColumn("Site"));
            dataTable1.Columns.Add(new DataColumn("Start Time"));
            dataTable1.Columns.Add(new DataColumn("Count"));
            dataTable1.Columns.Add(new DataColumn("Pictures"));
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.AcceptButton = buttonOK;
            this.CancelButton = buttonCancel;

            dsForDGV.Tables.Add(dataTable1);

            bs.DataSource = dsForDGV;
            bs.DataMember = dsForDGV.Tables[0].TableName;
            dataGridView1.DataSource = bs;
            dataGridView1.Refresh();

            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.AllowUserToResizeColumns = true;
            dataGridView1.Columns[0].Width = 48;
            dataGridView1.Columns[1].Width = 144;
            dataGridView1.Columns[3].Width = 40;
            dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dataGridView1.ReadOnly = true;
        }


        internal void fill(List<dive> dives)
        {
            int n1 = 0, n2 = 0;

            foreach (dive d in dives)
            {
                if (d.Pictures.Count() == 0) continue;
                
                ++n1;
                n2 += d.Pictures.Count();

                string s1 = d.StartTime.ToShortDateString() + " " + d.StartTime.ToShortTimeString();
                string s2 = d.Pictures.Aggregate("", (s, p) => s + " " + System.IO.Path.GetFileName(p.filename));
                                        
                dataTable1.Rows.Add(d.DiveNum, d.Site, s1, d.Pictures.Count(), s2);
            }

            label1.Text = string.Format("{0} picture(s) found for {1} dive(s)", n2, n1);
        }


        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
