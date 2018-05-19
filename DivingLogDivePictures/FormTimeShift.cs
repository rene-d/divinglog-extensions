using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DivePictures
{
    public partial class FormTimeShift : Form
    {
        DateTime timeCamera;
        DateTime timeComputer;

        public TimeSpan TimeShift = TimeSpan.Zero;
     

        public FormTimeShift()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            this.AcceptButton = buttonAccept;
            this.CancelButton = buttonCancel;

            toolTip1.SetToolTip(textBox2, "The time displayed on the dive computer.");
            toolTip1.SetToolTip(textBox3, "Seconds between the time of the camera and the dive computer.");

            timer1.Interval = 1000;
             
            textBox2.Text = Properties.Settings.Default.SyncTime.ToString();
            textBox4.Text = Properties.Settings.Default.SyncPhoto;

            ShowPhoto();
        }


        private void buttonOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Filter = "JPEG Images (*.jpg;*.jpeg)|*.jpg;*.jpeg|All Files|*";
            ofd.FilterIndex = 0;
            ofd.CheckFileExists = true;
            ofd.ReadOnlyChecked = false;
            ofd.Multiselect = false;
            ofd.RestoreDirectory = true;
            ofd.Title = "Open the picture of the dive computer";

            if (File.Exists(textBox4.Text))
            {
                ofd.InitialDirectory = Path.GetDirectoryName(textBox4.Text);
                ofd.FileName = Path.GetFileName(textBox4.Text);
            }

            if (ofd.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
                return;

            textBox4.Text = ofd.FileName;
            ShowPhoto();
        }


        private void ShowPhoto()
        {
            if (textBox4.Text == "")
            {
                pictureBox1.Image = null;
                timeCamera = DateTime.MinValue;

                textBox1.Clear();
                textBox1.ReadOnly = true;

                textBox2.Clear();
                textBox2.ReadOnly = true;

                return;
            }

            try
            {
                Image image = new Bitmap(textBox4.Text);

                pictureBox1.Image = image;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

                timeCamera = tools.getImageDateTime(image);

                if (timeCamera == DateTime.MinValue)
                {
                    textBox1.Text = "";
                    textBox2.ReadOnly = true;
                }
                else
                {
                    textBox1.Text = timeCamera.ToString();
                    textBox2.ReadOnly = false;
                }
            }
            catch { }

            ComputeTimeShift();
        }


        private void ComputeTimeShift()
        {
            timer1.Stop();

            if (textBox1.Text == "" || textBox2.Text == "")
            {
                if (textBox3.Text == "")
                {
                    TimeShift = TimeSpan.Zero;
                    textBox3.BackColor = Color.LightGreen;
                }
                else if (TimeSpan.TryParse(textBox3.Text, out TimeShift))
                {
                    textBox3.BackColor = Color.LightGreen;
                }
                else
                {
                    textBox3.BackColor = Color.LightCoral;
                }
                return;
            }

            try
            {
                if (!textBox1.ReadOnly)
                {
                    timeCamera = DateTime.Parse(textBox1.Text);
                }
                timeComputer = DateTime.Parse(textBox2.Text);
                TimeShift = timeCamera - timeComputer;

                textBox3.Text = TimeShift.ToString();
                textBox3.BackColor = Color.LightGreen;
            }
            catch
            {
                TimeShift = TimeSpan.Zero;
                textBox3.Clear();
                textBox3.BackColor = Color.LightCoral;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            timer1.Stop();
            timer1.Start();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            timer1.Stop();
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ComputeTimeShift();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void buttonAccept_Click(object sender, EventArgs e)
        {
            ComputeTimeShift();

            DivePictures.Properties.Settings.Default.SyncTime = timeComputer;
            DivePictures.Properties.Settings.Default.SyncPhoto = textBox4.Text;
            //DivePictures.Properties.Settings.Default.Save();

            this.DialogResult = DialogResult.OK;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox4.Clear();
            textBox3.BackColor = SystemColors.Window;
            textBox3.Focus();
         
            ShowPhoto();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            textBox3.BackColor = SystemColors.Window;
            timer1.Stop();
            timer1.Start();
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            ComputeTimeShift();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ComputeTimeShift();
            textBox3.Text = TimeShift.ToString();
        }
         
    }
}
