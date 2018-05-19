using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace DivePictures
{
    public partial class FormBatch : Form
    {
        public FormBatch()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        static public DialogResult Show(string s)
        {
            FormBatch f = new FormBatch();

            f.textBox1.Text = s;

            return f.ShowDialog();
        }

        private void toolStripButtonCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(textBox1.Text);

            MessageBox.Show(@"Commands have been copied to the Clipboard.
Double check them before running the batch.

Remember to add to the PATH the tools you use (ImageMagick, exiftool, etc.).", "Batch", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.DefaultExt = ".bat";
            sfd.Filter = "Batch files (*.bat;*.cmd)|*.bat;*.cmd|All Files|*";
            sfd.RestoreDirectory = true;            
            sfd.OverwritePrompt = true;

            if (sfd.ShowDialog(this) != DialogResult.OK)
                return;

            using (StreamWriter w = new StreamWriter(sfd.OpenFile(), Encoding.Default))
            {
                w.Write(textBox1.Text);
            }

            MessageBox.Show(string.Format(@"Commands have been saved to {0}.
Double check them before running the batch.

Remember to add to the PATH the tools you use (ImageMagick, exiftool, etc.).", sfd.FileName), "Batch", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
