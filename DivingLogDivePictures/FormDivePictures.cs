// FormDivePictures.cs
// René DEVICHI 2011

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ZedGraph;


namespace DivePictures
{
    using AndyDragon.DivingLog.ViewModelLibrary;

    public partial class FormDivePictures : Form
    {        
        List<dive> dives = new List<dive>();
        picasaAlbums albums;
        OleDbConnection logbook = null;
        TimeSpan TimeShift = TimeSpan.Zero;

        public FormDivePictures()
        {
            InitializeComponent();            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Properties.Settings.Default.FullVersion = true;
 
            // external tool registration to Diving Log 5.0
            const string divingLogToolName = "Diving Log Dive Pictures";
            if (!DivingLogRegistryHelper.HasExternalTool(divingLogToolName))
            {
                FormLicense dlg = new FormLicense();
                if (dlg.ShowDialog() != DialogResult.Yes)
                {
                    Close();
                    return;
                }

                // Store this as an external tool (and only allow click-once link).
                DivingLogExternalToolHelper.StoreAsExternalTool(divingLogToolName, true);
            }
            else 
            {
                if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
                {
                    if (System.Deployment.Application.ApplicationDeployment.CurrentDeployment.IsFirstRun)
                    {
                        /* */
                    }
                }
            }

            // features not available to all
            foreach (ToolStripItem x in toolStripDropDownButtonOpenLogbook.DropDownItems)
            {
                if (x.Tag != null && (string)x.Tag == "hidden")
                {
                    x.Visible = Properties.Settings.Default.FullVersion;
                }
            }
                   
            buttonAnalyze.Enabled = false;
            buttonAssociate.Enabled = false;
            checkBoxDepthInDescription.Enabled = false;
            buttonBatch.Enabled = false;

            TimeShift = DivePictures.Properties.Settings.Default.TimeShift;
            textBoxTimeShift.Text = TimeShift.ToString();

            // Enable scrollbars if needed
            //zg1.IsShowHScrollBar = true;
            //zg1.IsShowVScrollBar = true;
            //zg1.IsAutoScrollRange = true;
            //zg1.IsScrollY2 = false;

            zg1.GraphPane.XAxis.ScaleFormatEvent += new Axis.ScaleFormatHandler(MyFormatHandler);

            // Fill the axis background with a gradient
            zg1.GraphPane.Chart.Fill = new Fill(Color.White, Color.LightSkyBlue, 45.0f);            

            // OPTIONAL: Show tooltips when the mouse hovers over a point
            zg1.IsShowPointValues = true;
            zg1.PointValueEvent += new ZedGraphControl.PointValueHandler(MyPointValueHandler);

            // OPTIONAL: Add a custom context menu item
            //zg1.ContextMenuBuilder += new ZedGraphControl.ContextMenuBuilderEventHandler(MyContextMenuBuilder);

            // OPTIONAL: Handle the Zoom Event
            //zg1.ZoomEvent += new ZedGraphControl.ZoomEventHandler(MyZoomEvent);

            drawBlob(null);

            listBoxDives.DrawMode = DrawMode.OwnerDrawFixed;
            listBoxDives.ItemHeight = 16;

            comboBoxAlbums.DrawMode = DrawMode.OwnerDrawFixed;
            comboBoxAlbums.ItemHeight = 16;

            searchPicasaAlbums();                        
            comboBox1_TextChanged(null, null);

            openDivingLog50ToolStripMenuItem_Click(null, null);
        }


        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index == -1)
                return;

            try
            {
                dive d = listBoxDives.Items[e.Index] as dive;

                Brush myBrush = new SolidBrush(e.ForeColor);

                e.DrawBackground();

                Bitmap icon;

                if (d.SampleInterval == 0)
                {
                    icon = Properties.Resources.withoutprofile;
                }
                else
                {
                    icon = Properties.Resources.withprofile;
                }

                e.Graphics.DrawImage(icon, new Rectangle(e.Bounds.X + 1, e.Bounds.Y + 1, 16 - 2, 16 - 2));

                e.Graphics.DrawString(
                    string.Format("{0,4}  {1}", d.DiveNum, d.Site),
                    e.Font, myBrush, new RectangleF(e.Bounds.X + 16, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));

                e.DrawFocusRectangle();
            }
            catch { }

        }

        private void comboBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index == -1)
                return;

            try
            {
                picasaAlbums.album d = comboBoxAlbums.Items[e.Index] as picasaAlbums.album;

                Brush myBrush = new SolidBrush(e.ForeColor);
               
                e.DrawBackground();

                Bitmap icon;

                if (d == null)
                {
                    icon = Properties.Resources.picasa_square_16x13;
                }
                else
                {
                    icon = Properties.Resources.picasa_square_16x13;
                }
                
                e.Graphics.DrawImage(icon, new Rectangle(e.Bounds.X + 1, e.Bounds.Y + 1, 16 - 2, 16 - 2));                

                e.Graphics.DrawString(d.name, e.Font, myBrush, new RectangleF(e.Bounds.X + 16, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));

                e.DrawFocusRectangle();
            }
            catch { }
        }


        /// <summary>
        /// Display customized tooltips when the mouse hovers over a point
        /// </summary>
        private string MyPointValueHandler(ZedGraphControl control, GraphPane pane, CurveItem curve, int iPt)
        {
            PointPair pt = curve[iPt];
            TimeSpan ts = TimeSpan.FromSeconds(pt.X);
            return "Depth is " + pt.Y.ToString("f1") + " m at " + ts.ToString();
        }

        private string MyFormatHandler(GraphPane pane, Axis axis, double val, int index)
        {
            TimeSpan x = new TimeSpan((long)(TimeSpan.TicksPerSecond * val));

            if (val < 3600)
                return x.ToString(@"mm\:ss");
            else
                return x.ToString("c");
        }


        private void buttonDeleteSelection_Click(object sender, EventArgs e)
        {
            listBoxDives.BeginUpdate();
            while (listBoxDives.SelectedItems.Count > 0)
            {
                listBoxDives.Items.Remove(listBoxDives.SelectedItem);
            }
            listBoxDives.EndUpdate();

        }

        private void buttonKeepSelection_Click(object sender, EventArgs e)
        {
            listBoxDives.BeginUpdate();
            /*
            for (int i = 0; i < listBox1.Items.Count; ++i)
            {
                listBox1.SetSelected(i, !listBox1.GetSelected(i));
            }
            */
            for (int i = listBoxDives.Items.Count; i > 0; )
            {
                --i;
                if (!listBoxDives.GetSelected(i))
                    listBoxDives.Items.RemoveAt(i);
            }            
            listBoxDives.EndUpdate();
        }

        private void buttonResetList_Click(object sender, EventArgs e)
        {
            listBoxDives.BeginUpdate();
            listBoxDives.Items.Clear();
            listBoxDives.Items.AddRange(dives.ToArray());
            listBoxDives.EndUpdate();
        }


        private void button4_Click(object sender, EventArgs e)
        {
            SortedSet<int> toto = new SortedSet<int>();

            Action<string> test = new Action<string>(
                fileName =>
                {
                    picture pict = new picture(fileName);

                    if (pict.dateTaken != DateTime.MinValue)
                    {
                        //foreach (dive d in listBox1.Items)
                        for (int i = 0; i < listBoxDives.Items.Count; ++i)
                        {
                            var d = listBoxDives.Items[i] as dive;

                            // la honte... quel oubli !
                            var shiftedStartTime = d.StartTime + TimeShift;

                            if (pict.dateTaken.CompareTo(shiftedStartTime.AddMinutes(-15)) >= 0
                                && pict.dateTaken.CompareTo(shiftedStartTime.AddMinutes(d.Duration / 60 + 15)) <= 0)
                            {
                                pict.calcDepth(d, TimeShift);

                                d.Pictures.Add(pict);
                                toto.Add(i);
                                //listBox1.SetSelected(i, true);
                            }
                        }
                    }
                }
            );


            if (comboBoxAlbums.SelectedIndex == -1)
            {
                if (comboBoxAlbums.Text == "") 
                    return;

                if (!Directory.Exists(comboBoxAlbums.Text))
                {
                    MessageBox.Show(string.Format("Path {0} does not exist.", comboBoxAlbums.Text));
                    return;
                }
            }


            Cursor.Current = Cursors.WaitCursor;

            
            listBoxPictures.Items.Clear();
            listBoxDives.BeginUpdate();

            try
            {

                // reset the dive list if none
                if (listBoxDives.Items.Count == 0)
                {
                    listBoxDives.Items.AddRange(dives.ToArray());
                }

                listBoxDives.ClearSelected();

                foreach (dive d in listBoxDives.Items)
                {
                    d.Pictures.Clear();
                }

                if (comboBoxAlbums.SelectedIndex == -1)
                {
                    if (!Directory.Exists(comboBoxAlbums.Text))
                        return;

                    SearchOption so = checkBoxRecursive.Checked ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

                    Ini.IniFile ini = new Ini.IniFile("");

                    foreach (var fileName in Directory.EnumerateFiles(comboBoxAlbums.Text, "*", so))
                    {
                        if (!tools.IsJpeg(fileName)) continue;

                        ini.path = Path.Combine(Path.GetDirectoryName(fileName), ".picasa.ini");

                        if (ini.IniReadValue(Path.GetFileName(fileName), "suppress") == "yes") continue;

                        test(fileName);
                    }
                }
                else
                {
                    picasaAlbums.album a = comboBoxAlbums.SelectedItem as picasaAlbums.album;

                    Array.ForEach(a.files(), f => test(picasaAlbums.Expand(f)));
                }

                for (int i = listBoxDives.Items.Count; i > 0; )
                {
                    --i;
                    //if (!listBox1.GetSelected(i))
                    if (!toto.Contains(i))
                        listBoxDives.Items.RemoveAt(i);
                }                 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "analyze", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            buttonBatch.Enabled = dives.Aggregate(0, (total, d) => total += d.Pictures.Count) != 0;
            buttonAssociate.Enabled = (logbook != null) && buttonBatch.Enabled;
            checkBoxDepthInDescription.Enabled = buttonAssociate.Enabled;
            
            listBoxDives.ClearSelected();
            listBoxDives.EndUpdate();

            Cursor.Current = Cursors.Default;
        }


        private void searchPicasaAlbums()
        {
            try
            {
                albums = new picasaAlbums();

                comboBoxAlbums.Items.AddRange(albums.ToArray());
            }
            catch { }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxDives.SelectedItem == null)
                return;

            dive d = listBoxDives.SelectedItem as dive;

            textBox2.Text = d.StartTime.ToString();
            textBox3.Text = (d.StartTime.AddSeconds(d.Duration)).ToString();

            textBox4.Clear();
            textBox5.Clear();
            listBoxPictures.Items.Clear();
            foreach (var s in d.Pictures)
            {
                listBoxPictures.Items.Add(s);
            }

            drawBlob(d);
        }

        private void listBox2_SelectedValueChanged(object sender, EventArgs e)
        {
           
        }


        private void button5_Click(object sender, EventArgs e)
        {            
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Filter = "JPEG Images (*.jpg;*.jpeg)|*.jpg;*.jpeg|All Files|*";
            ofd.FilterIndex = 0;
            ofd.CheckFileExists = true;
            ofd.ReadOnlyChecked = false;
            ofd.Multiselect = false;
            ofd.RestoreDirectory = true;
            ofd.Title = "Choose the image directory to scan";

            if (ofd.ShowDialog(this) != DialogResult.OK)
                return;

            comboBoxAlbums.SelectedIndex = -1;
            comboBoxAlbums.Text = Path.GetDirectoryName(ofd.FileName);
            comboBox1_TextChanged(null, null);
        }

      
        private void listBox2_DoubleClick(object sender, EventArgs e)
        {
            var i = listBoxPictures.SelectedItem as picture;
            if (i != null)
                System.Diagnostics.Process.Start(i.filename);
        }
         

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            checkBoxRecursive.Enabled = (comboBoxAlbums.SelectedIndex == -1);

            buttonAnalyze.Enabled = (comboBoxAlbums.SelectedIndex != -1) || Directory.Exists(comboBoxAlbums.Text);
        }


        private void buttonAssociate_Click(object sender, EventArgs e)
        {
            if (logbook == null) return;
            if (logbook.State != ConnectionState.Open) return;

            int total = dives.Aggregate(0, (i, d) => i += d.Pictures.Count);

            if (total == 0)
            {
                MessageBox.Show("There is no picture avalaible.", "associate", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            FormReview f = new FormReview();
            f.fill(dives);
            if (f.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }
            
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                OleDbDataAdapter adapter = new OleDbDataAdapter("select * from Pictures", logbook);

                // create a command builder
                OleDbCommandBuilder builder = new OleDbCommandBuilder(adapter);
                adapter.InsertCommand = builder.GetInsertCommand(true);
                adapter.UpdateCommand = builder.GetUpdateCommand();

                // problem with some column names
                //adapter.RowUpdating += new OleDbRowUpdatingEventHandler(OnRowUpdatingEvent);                

                //create a DataTable to hold the query results                
                DataTable dtable = new DataTable();

                //fill the DataTable
                adapter.Fill(dtable);

                int added = 0;
                int presents = 0;
                int id = 0;
                int updated = 0;

                foreach (DataRow row in dtable.Rows)
                {
                    int i = (int)row["ID"];
                    if (i > id) id = i;
                }
                
                Regex myRegex = new Regex(@"^\[\d+[,.]?\d*\s?[ftm]*]");

                foreach (dive d in dives)
                {
                    if (d.Pictures.Count == 0) continue;

                    foreach (DataRow row in dtable.Rows)
                    {
                        if ((int)row["LogID"] == d.DiveID)
                        {
                            foreach (picture p in d.Pictures)
                            {
                                if (p.filename == (string)row["Path"])
                                {
                                    p.flag = true;  // <=> already in Pictures table
                                    ++presents;

                                    if (checkBoxDepthInDescription.Checked)
                                    {
                                        string desc = row["Description"] as string;

                                        if (!myRegex.IsMatch(desc))
                                        {
                                            row.BeginEdit();
                                            row["Description"] = string.Format("[{0:f1} m] {1}", p.depth, desc);
                                            row.EndEdit();

                                            ++updated;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // we have to add only non flagged picture

                    foreach (picture p in d.Pictures)
                    {
                        if (p.flag == true) continue;

                        DataRow row = dtable.NewRow();

                        row["ID"] = ++id;
                        row["LogID"] = d.DiveID;
                        row["Path"] = p.filename;

                        if (checkBoxDepthInDescription.Checked && p.depth != 0)
                        {
                            row["Description"] = string.Format("[{0:f1} m] {1}", p.depth, p.title);
                        }
                        else
                        {
                            row["Description"] = p.title;
                        }

                        row["UUID"] = System.Guid.NewGuid();
                        row["Updated"] = DateTime.Now;
                        dtable.Rows.Add(row);

                        ++added;

                    }
                }

                DataTable changes = dtable.GetChanges();
                if (changes != null)
                {
                    adapter.Update(changes);
                }

                dtable.AcceptChanges();

                MessageBox.Show(
                    string.Format("{0}/{1} picture(s) added to the Picture table, {2} were already present and {3} updated with depth.", added, total, presents, updated),
                    "associate", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "associate", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Cursor.Current = Cursors.Default;
        }

        private void drawBlob(dive d)
        {

            //listBox2.Items.Clear();
            //listBox2.Items.Add(d.StartTime.ToLongDateString() + " - " + d.StartTime.ToLongTimeString());
            //listBox2.Items.Add("Duration=" + d.Duration);
            //listBox2.Items.Add("SampleInterval=" + d.SampleInterval);
            //listBox2.Items.Add("DiveTime=" + reader["DiveTime"].ToString());


            // Get a reference to the GraphPane instance in the ZedGraphControl
            GraphPane myPane = zg1.GraphPane;


            if (d == null || d.Profile == null)
            {     
                myPane.Title.IsVisible = false;   
                //myPane.Legend.IsVisible = false;
                myPane.XAxis.IsVisible = false;
                myPane.YAxis.IsVisible = false;
                //myPane.Y2Axis.IsVisible = false;
                
                myPane.CurveList.Clear();

                zg1.Invalidate();

                return;
            }


            // Set the titles and axis labels
            myPane.Title.Text = string.Format("Profile for dive #{0} of {1} at {2}", d.DiveNum, d.StartTime, d.Site);
            myPane.Title.IsVisible = true;
            myPane.Legend.IsVisible = false;
            myPane.XAxis.IsVisible = true;
            myPane.YAxis.IsVisible = true;
            myPane.XAxis.Title.Text = "Time (hh:mm:ss)";
            myPane.YAxis.Title.Text = "Depth (m)";
            myPane.Y2Axis.IsVisible = false;

            double y_min = 0;

            // Make up some data points based on the Sine function
            PointPairList list = new PointPairList();
            for (int i = 0; i < d.Profile.Length; ++i)
            {
                double x = i * d.SampleInterval;
                double y = -d.Profile[i];
                list.Add(x, y);

                if (y_min > y) y_min = y;
            }

            myPane.CurveList.Clear();

            // Generate a red curve with diamond symbols, and "Alpha" in the legend
            LineItem myCurve = myPane.AddCurve("Profile", list, Color.Red, SymbolType.None);

            // Fill the symbols with white
            myCurve.Symbol.Fill = new Fill(Color.White);

            // Show the x axis grid
            myPane.XAxis.MajorGrid.IsVisible = true;

            // Make the Y axis scale red
            myPane.YAxis.Scale.FontSpec.FontColor = Color.Red;
            myPane.YAxis.Title.FontSpec.FontColor = Color.Red;

            // turn off the opposite tics so the Y tics don't show up on the Y2 axis
            myPane.YAxis.MajorTic.IsOpposite = false;
            myPane.YAxis.MinorTic.IsOpposite = false;

            // Don't display the Y zero line
            myPane.YAxis.MajorGrid.IsZeroLine = false;

            // Show the Y axis grid
            myPane.YAxis.MajorGrid.IsVisible = true;

            // Align the Y axis labels so they are flush to the axis
            myPane.YAxis.Scale.Align = AlignP.Inside;

            // Manually set the axis range
            myPane.YAxis.Scale.Min = y_min - 1;
            myPane.YAxis.Scale.Max = 0;

            myPane.XAxis.Scale.Min = 0;
            myPane.XAxis.Scale.Max = d.SampleInterval * d.Profile.Length;

            // Fill the axis background with a gradient
            //myPane.Chart.Fill = new Fill(Color.White, Color.LightGray, 45.0f);

            // Tell ZedGraph to calculate the axis ranges
            // Note that you MUST call this after enabling IsAutoScrollRange, since AxisChange() sets
            // up the proper scrolling parameters
            zg1.AxisChange();

            // Make sure the Graph gets redrawn
            zg1.Invalidate();
        }


        private void openSuuntoDiveManager4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<dive> dives;
            string database;

            if (Suunto.openDM4(out dives, out database, true))
            {
                listBoxDives.Items.Clear();
                listBoxDives.Items.AddRange(dives.ToArray());

                this.dives = dives;
                this.logbook = null;
                this.toolStripStatusLabel1.Text = database;
            }
        }

        private void openDivingLog50ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<dive> dives;
            OleDbConnection logbook;

            if (DivingLog.openDL5(out dives, out logbook, sender != null))
            {
                listBoxDives.Items.Clear();
                listBoxDives.Items.AddRange(dives.ToArray());

                this.dives = dives;
                this.logbook = logbook;
                this.toolStripStatusLabel1.Text = logbook.DataSource;
            }
        }

       

        private void FormDivePictures_FormClosing(object sender, FormClosingEventArgs e)
        {
            DivePictures.Properties.Settings.Default.Save();
        }

        private void toolStripButtonTimeShift_Click(object sender, EventArgs e)
        {

            FormTimeShift f = new FormTimeShift();

            if (f.ShowDialog(this) != DialogResult.OK)
                return;

            TimeShift = f.TimeShift;
            textBoxTimeShift.Text = TimeShift.ToString("c");

            DivePictures.Properties.Settings.Default.TimeShift = TimeShift;            
        }

        private void buttonHelpTimeShift_Click(object sender, EventArgs e)
        {        
            MessageBox.Show(@"This is the difference between the clock of your dive computer (which is recorded in the profile and dive details) and the clock of your underwater camera (which is used to timestamp the pictures).

The format is [+ / -] ddd.hh:mm:ss 
where ddd=days, hh=hours, mm=minutes, ss=seconds.

A positive value means that the dive computer clock is ahead of the camera clock. A negative value means the inverse.

Beware of jet lag !

Use the button in the toolstrip to set this value.", "Time Shift", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var i = listBoxPictures.SelectedItem as picture;
            if (i != null)
            {
                textBox5.Text = i.dateTaken.ToString();
                textBox4.Text = i.depth.ToString("f1");
                textBox1.Text = (i.dateTaken - TimeShift).ToString();
            }
        }

        private void buttonBatch_Click(object sender, EventArgs e)
        {
            string pattern = Properties.Settings.Default.BatchPattern;

            if (pattern == "" || pattern == "#empty#")
            {
                MessageBox.Show("The batch pattern has not been defined yet.");
                return;
            }

            string header = null, trailer = null;
            
            int p1 = pattern.IndexOf("#BODY#");
            int p2 = pattern.IndexOf("#ENDBODY#");

            if (p1 != -1 || p2 != -1)
            {
                if (p1 != -1 && p2 != -1 && p2 < p1)
                {
                    MessageBox.Show("Tags misordered.");
                    return;
                }

                if (p2 != -1)
                {
                    trailer = pattern.Substring(p2 + "#ENDBODY#".Length);
                    pattern = pattern.Remove(p2);
                }

                if (p1 != -1)
                {
                    header = pattern.Substring(0, p1);
                    pattern = pattern.Remove(0, p1 + "#BODY#".Length);
                }            
            }

            try
            {
                string.Format(CultureInfo.InvariantCulture, pattern, "image.jpg", 1.0, "site", "image.jpg", 1.0);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bad pattern. Please correct it.\r\n" + ex.Message);
                return;
            }

            StringBuilder batch = new StringBuilder();

            if (header != null)
                batch.AppendFormat(header);

            foreach (dive d in dives)
            {
                foreach (picture p in d.Pictures)
                {
                    string site = d.Site.Replace("\"", "");

                    batch.AppendFormat(CultureInfo.InvariantCulture, pattern,
                        Path.GetFileName(p.filename), p.depth, site, p.filename, p.depth / 0.3048);
                }
            }

            if (trailer != null)
            batch.AppendFormat(trailer);

            FormBatch.Show(batch.ToString());
        }

        private void toolStripButtonBatch_Click(object sender, EventArgs e)
        {
            new FormBatchPattern().ShowDialog(this);
        }

        private void toolStripButtonHelp_Click(object sender, EventArgs e)
        {
            new FormHelp().ShowDialog(this);            
        }

        private void dataTrakToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<dive> dives;
            string database;

            if (tools.openDataTrak(out dives, out database) && dives != null && database != null)
            {
                listBoxDives.Items.Clear();
                listBoxDives.Items.AddRange(dives.ToArray());

                this.dives = dives;
                this.logbook = null;
                this.toolStripStatusLabel1.Text = database;
            }
        }

        private void uwatecSmartTrakToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<dive> dives;
            string database;

            if (tools.openSmartTRAK(out dives, out database) && dives != null && database != null)
            {
                listBoxDives.Items.Clear();
                listBoxDives.Items.AddRange(dives.ToArray());

                this.dives = dives;
                this.logbook = null;
                this.toolStripStatusLabel1.Text = database;
            }
        }

        private void suuntoDiveManager3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<dive> dives;
            string database;

            if (Suunto.openDM3(out dives, out database))
            {
                listBoxDives.Items.Clear();
                listBoxDives.Items.AddRange(dives.ToArray());

                this.dives = dives;
                this.logbook = null;
                this.toolStripStatusLabel1.Text = database;
            }
        }
    }
}
