using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DivePictures
{
    public partial class FormBatchPattern : Form
    {
        public FormBatchPattern()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            scintilla1.ConfigurationManager.Language = "batch";

            scintilla1.Margins[0].Width = 20;

            scintilla1.LineWrap.Mode =  ScintillaNet.WrapMode.Word;
            scintilla1.LineWrap.StartIndent = 2;
            scintilla1.LineWrap.VisualFlags = ScintillaNet.WrapVisualFlag.End;

            if (Properties.Settings.Default.BatchPattern == "#empty#")
            {
                toolStripButtonReset_Click(null, null);
            }
            else
            {
                scintilla1.Text = Properties.Settings.Default.BatchPattern;
            }
        }

        private void FormBatch_FormClosing(object sender, FormClosingEventArgs e)
        {
            scintilla1.Text = Properties.Settings.Default.BatchPattern = scintilla1.Text;
        }


        private void toolStripButtonReset_Click(object sender, EventArgs e)
        {
            scintilla1.ResetText();

            scintilla1.InsertText(@"setlocal

rem Add ImageMagick to the PATH
PATH C:\Program Files\ImageMagick-6.7.2-Q16;%PATH%

#BODY#
");

            CommentsToolStripButton_Click(null, null);
            GPSAltitudeRefToolStripMenuItem_Click(null, null);
            depthSiteToolStripMenuItem_Click(null, null);

            scintilla1.InsertText(@"
#ENDBODY#

echo Terminated !");
        }

        private void toolStripButtonUndoChanges_Click(object sender, EventArgs e)
        {
            scintilla1.Text = scintilla1.Text = Properties.Settings.Default.BatchPattern;
        }

        private void CommentsToolStripButton_Click(object sender, EventArgs e)
        {
            scintilla1.InsertText(@"
rem filename  : {0}
rem depth     : {1:f1} m
rem site      : {2}
");
        }

        private void depthSiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            scintilla1.InsertText(
"convert.exe \"{3}\" \"-resize \"x1200>\" -pointsize 20 -fill yellow -style Italic "
             + "-gravity SouthWest -annotate 0x0+8+8 \"Depth: {1:f1} m\\nSite: {2}\" "
             + "\"tagged\\{0}\"\r\n"
             );
        }

        private void depthSiteLogoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            scintilla1.InsertText(
"convert.exe \"{3}\" \"-resize \"x1200>\" -pointsize 20 -fill yellow -style Italic "
             + "-gravity SouthWest -annotate 0x0+8+8 \"Depth: {1:f1} m\\nSite: {2}\" "
             + "-gravity SouthEast -draw \"image over 8,8 0,0 'logo.png'\" "
             + "\"tagged\\{0}\"\r\n"
             );
        }


        private void GPSAltitudeRefToolStripMenuItem_Click(object sender, EventArgs e)
        {
            scintilla1.InsertText("exiftool.exe -GPSAltitude={1:f1} -GPSAltitudeRef=\"Below Sea Level\" \"{3}\"\r\n");
        }

        private void webSiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.imagemagick.org/");
        }

        private void websiteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.sno.phy.queensu.ca/~phil/exiftool/");
        }

        private void toolStripButtonHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show(@"The program will generate a batch with these commands for each image.

  {0} is replaced by the filename (without path)
  {1:f1} is replaced by the depth in meters
  {2} is replaced by the site
  {3} is replaced by the full filename

The two special tags #BODY# and #ENDBODY# are used to separate an optional header and trailer to the image processing commands.

The examples with ImageMagick and exiftool require that you install these tools and add their directories to the PATH. Or adjust the batch.",
                "Batch Patterns Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


    }
}
