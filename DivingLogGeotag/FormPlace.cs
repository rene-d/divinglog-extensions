// Geotag dive sites with Google Earth or coordinates embedded into Exif
// René DEVICHI 2011
//
// External components:
// http://www.codeproject.com/KB/recipes/geospatial.aspx (CPOL)
// http://code.google.com/p/exifbitmap/ (ouch ! in Japanese... -- MIT License)
// http://www.codeproject.com/KB/miscctrl/cs_star_rating_control.aspx (no license)
// 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Diagnostics;
using System.IO;
using System.Data.OleDb;
using EARTHLib;
using Microsoft.Win32;
using System.Xml;
using System.IO.Packaging;
using RatingControls;


namespace DL5GeoTag
{
    using AndyDragon.DivingLog.ViewModelLibrary;

    public partial class FormPlace : Form
    {
        static string GEFolderName = "Diving Log Tagging";

        ApplicationGE earth = null;
        bool flag;
        bool UnitFeet = false;

        private StarRatingControl m_starRatingControl = new StarRatingControl();

        public FormPlace()
        {
            InitializeComponent();

            // init the StarRating control
            this.panel2.Controls.Add(m_starRatingControl);
            this.m_starRatingControl.Location = new System.Drawing.Point(173, 114);
            this.m_starRatingControl.StarSpacing = 2;
            this.m_starRatingControl.Width = 96;
            this.m_starRatingControl.DataBindings.Add(new System.Windows.Forms.Binding("SelectedStar", this.placeBindingSource, "Rating", true));
        }


        private void Form3_Load(object sender, EventArgs e)
        {   
            OleDbConnectionStringBuilder dbs = new OleDbConnectionStringBuilder();

            dbs.Provider = "Microsoft.Jet.OLEDB.4.0";
            dbs.DataSource = @"|DataDirectory|\Logbook.mdb";
                       
            RegistryKey hkcu;

            const string divingLogToolName = "Diving Log Geo Tag";
            const string obsoleteDivingLogToolName = "GeoTag";
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

            // Remove the old external tool.
            DivingLogExternalToolHelper.RemoveObsoleteTool(obsoleteDivingLogToolName);

            hkcu = Registry.CurrentUser.OpenSubKey(@"Software\Divinglog\Divinglog50\WindowSizes");
            if (hkcu != null)
            {
                // nota: GetValue throws if key is read-only
                try
                {
                    string[] p = ((string)hkcu.GetValue("frmDL5GeoTag")).Split(' ');

                    if (p.Length == 5)
                    {
                        this.Location = new Point(Convert.ToInt32(p[0]), Convert.ToInt32(p[1]));
                        this.Size = new Size(Convert.ToInt32(p[2]), Convert.ToInt32(p[3]));
                        this.splitContainer1.SplitterDistance = Convert.ToInt32(p[4]);
                    }
                }
                catch { }
            }

            // open the current logbook
            hkcu = Registry.CurrentUser.OpenSubKey(@"Software\Divinglog\Divinglog50\Start");
            if (hkcu != null)
            {
                string value;

                // nota: GetValue throws if key is read-only
                try
                {
                    value = hkcu.GetValue("UnitFeet").ToString();
                    if (value == "1") UnitFeet = true;
                }
                catch { }

                try
                {
                    value = hkcu.GetValue("LogFile").ToString();
                    if (File.Exists(value)) dbs.DataSource = value;
                }
                catch { }
            }

            // the MaxDepth texbox
            if (UnitFeet)
            {
                txtDepth.UnitFeet = true;
                lblMaxDepthUnit.Text = "ft";
            }
            else
            {
                txtDepth.UnitFeet = false;
                lblMaxDepthUnit.Text = "m";
            }

            // 
            lstDiveSites.DrawMode = DrawMode.OwnerDrawFixed;
            lstDiveSites.ItemHeight = 16;

            // open the database
            toolStripStatusLabel1.Text = "no logbook";
            flag = false;
            try
            {
                this.placeTableAdapter.Connection = new OleDbConnection(dbs.ToString());

                // TODO: This line of code loads data into the 'PlaceDataSet.Place' table. You can move, or remove it, as needed.
                this.placeTableAdapter.Fill(this.PlaceDataSet.Place);

                placeBindingSource.MoveLast();

                toolStripStatusLabel1.Text = placeTableAdapter.Connection.DataSource;
            }
            catch { }
            flag = true;

            toolStripButtonSave.Visible = true;
            toolStripButtonOpen.Enabled = (lstDiveSites.Items.Count == 0);
        }


        private void FormPlace_FormClosing(object sender, FormClosingEventArgs e)
        {
            RegistryKey hkcu;

            hkcu = Registry.CurrentUser.OpenSubKey(@"Software\Divinglog\Divinglog50\WindowSizes", true);
            if (hkcu != null)
            {
                hkcu.SetValue("frmDL5GeoTag", string.Format("{0} {1} {2} {3} {4}",
                    this.Location.X, this.Location.Y, this.Size.Width, this.Size.Height,
                    splitContainer1.SplitterDistance), RegistryValueKind.String);
            }
        }


        // returns the full path of image files
        private string GetResPath(string filename)
        {
            return Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), filename);
        }

        private bool geotag(ref Geospatial.Location coord)
        {
            bool ret = false;

            // reset the COM object if connection has been broken since the first call
            // (i.e. GE has been closed)
            if (earth != null)
            {
                try
                {
                    earth.IsInitialized();
                }
                catch 
                {
                    earth = null;
                }
            }

            if (earth == null)
            {
                // establish the COM connection with Google Earth
                try
                {
                    earth = new EARTHLib.ApplicationGE();
                }
                catch 
                {
                    return false;
                }
            }

            try
            {
                int n = 0;
                while (earth.IsInitialized() == 0)
                {
                    System.Threading.Thread.Sleep(100);

                    // wait 10s
                    if (++n >= 100)
                    {
                        throw new Exception("Google Earth is not available");
                    }
                }

                //trace("IsInitialized={0} VersionAppType={1} IsOnline={2}", earth.IsInitialized(), earth.VersionAppType, earth.IsOnline());
            }
            catch
            {
                earth = null;
                return false;
            }


            // move the camera to the current location, if defined
            if (coord != null)
            {
                earth.SetCameraParams(coord.Latitude.TotalDegrees, coord.Longitude.TotalDegrees, 300, AltitudeModeGE.RelativeToGroundAltitudeGE, 1000, 0, 0, 1);
            }         


            // create a KML to display the crosshairs at the center of GE screen
            string s = string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
<kml xmlns=""http://earth.google.com/kml/2.0"">
  <Folder>
    <name>{0}</name>
    <ScreenOverlay>
      <name>Target</name>
      <Icon>
        <href>{1}</href>
      </Icon>
      <overlayXY x=""0.500000"" y=""0.500000"" xunits=""fraction"" yunits=""fraction"" />
      <screenXY x=""0.500000"" y=""0.500000"" xunits=""fraction"" yunits=""fraction"" />
      <size x=""0"" y=""0"" xunits=""pixels"" yunits=""pixels"" />
    </ScreenOverlay>    
  </Folder>
</kml>
", GEFolderName, GetResPath("xhairs.png"));

            string kml = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()) + ".kml";

            //trace("kml {0}", kml);

            // Creates a file for writing UTF-8 encoded text
            using (StreamWriter sw = File.CreateText(kml))
            {
                sw.Write(s);
            }

            earth.OpenKmlFile(kml, 0);

            // picasa does it, I'm not sure it's necessary
            try
            {
                foreach (FeatureGE f in earth.GetTemporaryPlaces().GetChildren())
                {
                    if (f.Name == GEFolderName)
                    {
                        f.Highlight();
                        break;
                    }
                }
            }
            catch
            {
            }

            //
            try
            {
                FormGeoTag geotag = new FormGeoTag(earth.GetMainHwnd(), txtName.Text);

                DialogResult result = geotag.ShowDialog(this);

                if (result == DialogResult.OK)
                {
                    // retrieve the position

                    //CameraInfoGE ci = earth.GetCamera(1);
                    //trace("CameraInfo {0:.######} {1:.######} {2} {3}", ci.FocusPointLatitude, ci.FocusPointLongitude, ci.FocusPointAltitude, ci.FocusPointAltitudeMode);

                    // better: GetCamera doesn't return the Altitude
                    PointOnTerrainGE pot = earth.GetPointOnTerrainFromScreenCoords(0, 0);
                    //trace("PointOnTerrain {0:.######} {1:.######} {2:.}", pot.Latitude, pot.Longitude, pot.Altitude);

                    coord = new Geospatial.Location(
                        new Geospatial.Latitude(new Geospatial.Angle(pot.Latitude * Math.PI / 180)),
                        new Geospatial.Longitude(new Geospatial.Angle(pot.Longitude * Math.PI / 180)),
                        pot.Altitude);

                    ret = true;
                }
                else
                {
                    //trace("cancelled: {0}", result);
                }
            }
            catch
            {
            }

            // clear the temporary place 'GEFolderName'
            try
            {
                using (StreamWriter sw = File.CreateText(kml))
                {
                    sw.Write(@"<kml/>");
                }
                earth.OpenKmlFile(kml, 1);
                File.Delete(kml);
            }
            catch
            {
            }

            // bring us to the front
            this.Activate();

            return ret;

        }

        private void buttonGeotagFromGE_Click(object sender, EventArgs e)
        {
            Geospatial.Location coord = null;

            Geospatial.Location.TryParse(txtLat.Text + " " + txtLon.Text, CultureInfo.InvariantCulture, out coord);

            if (!geotag(ref coord))
                return;

            setcoord(coord);
        }

        private void setcoord(Geospatial.Location coord)
        {

            if (txtLat2.Visible == false)
            {
                txtLat2.Visible = true; txtLat2.Text = txtLat.Text;
                txtLon2.Visible = true; txtLon2.Text = txtLon.Text;
                txtAltitude2.Visible = true; txtAltitude2.Text = txtAltitude.Text;
            }

            PlaceDataSet.PlaceRow row = (PlaceDataSet.PlaceRow)(((System.Data.DataRowView)placeBindingSource.Current).Row);

            row.BeginEdit();

            row.Updated = DateTime.Now;

            row.Lat = string.Format(CultureInfo.InvariantCulture, "{0:DIVINGLOG}", coord.Latitude).Replace(" ", "");
            row.Lon = string.Format(CultureInfo.InvariantCulture, "{0:DIVINGLOG}", coord.Longitude).Replace(" ", "");

            txtLat.ForeColor = Color.Red;
            txtLon.ForeColor = Color.Red;
            txtAltitude.ForeColor = Color.Red;

            if (coord.Altitude == null)
            {
                //row.Altitude = "";
            }
            else
            {
                if (coord.Altitude < 0)
                {
                    row.Altitude = "0";

                    // do not override MaxDepth field
                    if (row.IsMaxDepthNull())
                    {
                        row.MaxDepth = -(double)coord.Altitude;
                    }
                }
                else
                {
                    if (UnitFeet)
                    {
                        row.Altitude = string.Format(CultureInfo.InvariantCulture, "{0:F0} ft", coord.Altitude / 0.3048);
                    }
                    else
                    {
                        row.Altitude = string.Format(CultureInfo.InvariantCulture, "{0:F0} m", coord.Altitude);
                    }
                }

            }

            row.EndEdit();
        }


        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.Validate();
                this.placeBindingSource.EndEdit();
                this.placeTableAdapter.Update(this.PlaceDataSet.Place);

                toolStripButtonSave.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Update failed: " + ex.Message);
            }
        }

        
        private void placeBindingSource_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (flag)
            {
                toolStripButtonSave.Enabled = true;
            }
        }


        private void toolStripButtonUndo_Click(object sender, EventArgs e)
        {
            placeBindingSource.ResetCurrentItem();

            if (txtLat2.Visible == true)
            {
                txtLat2.Visible = false; txtLat.Text = txtLat2.Text;
                txtLon2.Visible = false; txtLon.Text = txtLon2.Text;
                txtAltitude2.Visible = false; txtAltitude.Text = txtAltitude2.Text;

                txtLat.ForeColor = Color.FromKnownColor(KnownColor.WindowText);
                txtLon.ForeColor = Color.FromKnownColor(KnownColor.WindowText);
                txtAltitude.ForeColor = Color.FromKnownColor(KnownColor.WindowText);
            }

        }


        private void placeBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            if (flag)
            {
                txtLat2.Visible = false;
                txtLon2.Visible = false;
                txtAltitude2.Visible = false;

                txtLat.ForeColor = Color.FromKnownColor(KnownColor.WindowText);
                txtLon.ForeColor = Color.FromKnownColor(KnownColor.WindowText);
                txtAltitude.ForeColor = Color.FromKnownColor(KnownColor.WindowText);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PlaceDataSet.PlaceRow row = (PlaceDataSet.PlaceRow)(((System.Data.DataRowView)placeBindingSource.Current).Row);

            row.Updated = DateTime.Now;
        }




        void findImageGps(string filename)
        {
            try
            {
                using (Bitmap bmp = new Bitmap(filename))
                {
                    Exif.Exif_Base Exif_info = new Exif.Exif_Base(bmp);

                    string lat = Exif_info.緯度;
                    string lon = Exif_info.経度;

                    if (lat != "-1" && lon != "-1")
                    {
                        double latD, lonD;
                        if (double.TryParse(lat, out latD) && double.TryParse(lon, out lonD))
                        {                            
                            if (Exif_info.東経西経 == "W") lonD = -lonD;
                            if (Exif_info.北緯南緯 == "S") latD = -latD;

                            setcoord(CreateLocation(latD, lonD, null));
                        }
                    }
                }
            }
            catch
            {
            }
        }


        Geospatial.Location CreateLocation(double Latitude, double Longitude, double? Altitude)
        {
            if (Altitude == null)
                return new Geospatial.Location(
                        new Geospatial.Latitude(new Geospatial.Angle(Latitude * Math.PI / 180)),
                        new Geospatial.Longitude(new Geospatial.Angle(Longitude * Math.PI / 180)));
            else
                return new Geospatial.Location(
                        new Geospatial.Latitude(new Geospatial.Angle(Latitude * Math.PI / 180)),
                        new Geospatial.Longitude(new Geospatial.Angle(Longitude * Math.PI / 180)),
                        (double)Altitude);
        }

        
        // create a KML with all dive site locations
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();

            dlg.Filter = "KMZ File (*.kmz)|*.kmz|KML File (*.kml)|*.kml";
            dlg.FilterIndex = 1;
            dlg.DefaultExt = "kmz";
            dlg.OverwritePrompt = true;
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() != DialogResult.OK)                
                return;

            Package zip = null;
            Stream kml;
            PackagePart PackagePart = null;

            if (string.Compare(Path.GetExtension(dlg.FileName), ".kmz", true) == 0)
            {
                zip = ZipPackage.Open(dlg.FileName, FileMode.Create);
                
                Uri UriPath = PackUriHelper.CreatePartUri(new Uri("doc.kml", UriKind.Relative));
                PackagePart = zip.CreatePart(UriPath, "application/vnd.google-earth.kml+xml", CompressionOption.Maximum);

                kml = PackagePart.GetStream();
            }
            else 
            {
                kml = new FileStream(dlg.FileName, FileMode.Create);
            }


            using (XmlTextWriter w = new XmlTextWriter(kml, Encoding.UTF8))
            {
                w.Indentation = 2;
                w.IndentChar = ' ';
                w.Formatting = Formatting.Indented;

                w.WriteStartDocument();

                w.WriteStartElement("kml");

                w.WriteAttributeString("xmlns", "http://www.opengis.net/kml/2.2");
                w.WriteAttributeString("xmlns:gx", "http://www.google.com/kml/ext/2.2");
                w.WriteAttributeString("xmlns:kml", "http://www.opengis.net/kml/2.2");
                w.WriteAttributeString("xmlns:atom", "http://www.w3.org/2005/Atom");

                w.WriteStartElement("Document");
                w.WriteElementString("name", "Diving Log");
                w.WriteElementString("open", "1");
                w.WriteElementString("description", "www.divinglog.de");

                AddKMLStyle(w, "default", null, null);

                AddKMLStyle(w, "sn_wreck_iconN",GetResPath( "icon_wreck.png"), null);
                AddKMLStyle(w, "sn_wreck_iconH", GetResPath("icon_wreck.png"), null);
                AddKMLStyleMap(w, "msn_wreck_icon", "sn_wreck_iconN", "sn_wreck_iconH");

                // currently unused
                AddKMLStyle(w, "sn_flag_diver_iconN", GetResPath("icon_flag_diver.gif"), 0.5);
                AddKMLStyle(w, "sn_flag_diver_iconH", GetResPath("icon_flag_diver.gif"), 0.5);
                AddKMLStyleMap(w, "msn_flag_diver_icon", "sn_flag_diver_iconN", "sn_flag_diver_iconH");

                w.WriteStartElement("Style");
                w.WriteAttributeString("id", "default");
                w.WriteEndElement();

                w.WriteStartElement("Folder");
                w.WriteElementString("name", "Dive Sites");
                w.WriteElementString("open", "1");

                for (int i = 0; i < placeBindingSource.Count; ++i)
                {
                    try
                    {
                        var row = (PlaceDataSet.PlaceRow)((System.Data.DataRowView)placeBindingSource[i]).Row;

                        // skip empty coords
                        if (row.IsLatNull() || row.IsLonNull())
                            continue;

                        if (row.Lat == null || row.Lon == null)
                            continue;

                        Geospatial.Location coord;

                        Geospatial.Location.TryParse(row.Lat + " " + row.Lon, CultureInfo.InvariantCulture, out coord);

                        if (coord != null)
                        {

                            w.WriteStartElement("Placemark");

                            if (row.IsPlaceNull())
                                w.WriteElementString("name", "no name");
                            else
                                w.WriteElementString("name", row.Place);


                            if (row.IsCommentsNull() == false)
                            {
                                if (row.Comments != "")
                                {
                                    /*
                                    int k = 0;
                                    for (int j = 0; j < 4; ++j)
                                    {
                                        k = row.Comments.IndexOf('\n', k) + 1;
                                    }
                                    if (k == -1)
                                    {
                                        w.WriteElementString("description", row.Comments);
                                    }
                                    else
                                    {
                                        w.WriteElementString("description", row.Comments.Substring(0, k));
                                    }
                                    */

                                    w.WriteElementString("description", row.Comments);
                                }
                            }

                            w.WriteStartElement("LookAt");
                            w.WriteElementString("longitude", coord.Longitude.TotalDegrees.ToString(CultureInfo.InvariantCulture));
                            w.WriteElementString("latitude", coord.Latitude.TotalDegrees.ToString(CultureInfo.InvariantCulture));
                            w.WriteElementString("altitude", "0");
                            w.WriteElementString("heading", "0");
                            w.WriteElementString("tilt", "0");
                            w.WriteElementString("range", "1000");
                            w.WriteEndElement(); //LookAt                        
                            w.WriteElementString("styleUrl", "#msn_flag_diver_icon"); // "#default"
                            w.WriteStartElement("Point");
                            w.WriteElementString("coordinates", string.Format(CultureInfo.InvariantCulture, "{0},{1},0", coord.Longitude.TotalDegrees, coord.Latitude.TotalDegrees));
                            w.WriteEndElement(); //Point

                            w.WriteEndElement(); //Placemark
                        }
                    }
                    catch (Exception ex) 
                    {
                        string s = ex.ToString();

                        s += "\r\n\r\nPlease report this exception to the author.";

                        if (MessageBox.Show(s, "Exception", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) == DialogResult.Cancel)
                        {
                            break;
                        }
                    }
                }

                w.WriteFullEndElement();
                w.WriteEndDocument();
            }

            kml.Close();

            if (zip != null)
            {
                zip.Close();
            }

            // optionally, we could open the generated file in Google Earth
        }


        private void AddKMLStyle(XmlTextWriter w, string name, string href, double? scale)
        {   
            w.WriteStartElement("Style");
            w.WriteAttributeString("id", name);

            if (href!=null)
            {
                w.WriteStartElement("IconStyle");

                if (scale != null)
                {
                    string.Format(CultureInfo.InvariantCulture, "{0:.##}", scale);
                    w.WriteElementString("scale", scale.ToString());                    
                }
                w.WriteStartElement("Icon");
                w.WriteElementString("href", href.ToString());
                w.WriteEndElement(); //Icon

                w.WriteEndElement(); //IconStyle
            }

                w.WriteEndElement();
        }

        private void AddKMLStyleMap(XmlTextWriter w, string name, string normal, string highlight)        
        {
            w.WriteStartElement("StyleMap");
            w.WriteAttributeString("id", name);

            w.WriteStartElement("Pair");
            w.WriteElementString("key", "normal");
            w.WriteElementString("styleUrl", "#" + normal);
            w.WriteEndElement(); //Pair

            w.WriteStartElement("Pair");
            w.WriteElementString("key", "highlight");
            w.WriteElementString("styleUrl", "#" + highlight);
            w.WriteEndElement(); //Pair

            w.WriteEndElement(); //StyleMap
        }
        

        private void buttonGeotagFromImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.Filter = "JPEG Images (*.jpg;*.jpeg)|*.jpg;*.jpeg";
            dlg.ShowReadOnly = false;
            dlg.Multiselect = false;
            dlg.ReadOnlyChecked = false;
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            findImageGps(dlg.FileName);
        }


        private void toolStripButtonRefresh_Click(object sender, EventArgs e)
        {
            int pos = placeBindingSource.Position;
            //placeBindingSource.ResetBindings(false);
            this.placeTableAdapter.Fill(this.PlaceDataSet.Place);
            placeBindingSource.Position = pos;           
        }
        

        private void lstDiveSites_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index == -1)
                return;

            try
            {
                Brush myBrush = new SolidBrush(e.ForeColor);
                PlaceDataSet.PlaceRow row = (PlaceDataSet.PlaceRow)(((System.Data.DataRowView)lstDiveSites.Items[e.Index]).Row);

                e.DrawBackground();

                if (row.IsLatNull() || row.IsLonNull() || row.Lat == "" || row.Lon == "")
                {
                    // nothing
                }
                else
                {
                    e.Graphics.DrawImage(global::DL5GeoTag.Properties.Resources.FlagPurple,
                        new Rectangle(e.Bounds.X + 1, e.Bounds.Y + 1, 16 - 2, 16 - 2));
                }

                e.Graphics.DrawString(row.Place, e.Font, myBrush,
                    new RectangleF(e.Bounds.X + 16, e.Bounds.Y + 2, e.Bounds.Width, e.Bounds.Height));

                e.DrawFocusRectangle();
            }
            catch{}
        }


        private void toolStripButtonOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.Filter = "Diving Log (*.mdb)|*.mdb";
            dlg.ShowReadOnly = false;
            dlg.Multiselect = false;
            dlg.ReadOnlyChecked = false;
            dlg.CheckFileExists = true;
            if (dlg.ShowDialog() != DialogResult.OK)
                return;                      

            OleDbConnectionStringBuilder dbs = new OleDbConnectionStringBuilder();

            dbs.Provider = "Microsoft.Jet.OLEDB.4.0";
            dbs.DataSource = dlg.FileName;

            flag = false;

            toolStripStatusLabel1.Text = "no logbook";
            try
            {

                this.placeTableAdapter.Connection = new OleDbConnection(dbs.ToString());
                this.placeTableAdapter.Fill(this.PlaceDataSet.Place);
                toolStripStatusLabel1.Text = placeTableAdapter.Connection.DataSource;

                placeBindingSource.MoveLast();
            }
            catch { }
            flag = true;
        }


        private void toolStripButtonHelp_Click(object sender, EventArgs e)
        {
            new FormHelp().ShowDialog(this);
        }


        private void panel2_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Link;
            //string[] f = e.Data.GetFormats();

            /*
            var oo = e.Data.GetData("FileNameW");
            string filename = ((string[])oo)[0];                        
             */        
        }


        private void panel2_DragDrop(object sender, DragEventArgs e)
        {
            object oo = null;

            oo = e.Data.GetData("FileDrop");
            if (oo == null)
                oo = e.Data.GetData("FileNameW");
            if (oo == null)
                return;

            try
            {
                string filename = ((string[])oo)[0];

                findImageGps(filename);
            }
            catch
            {
            }
        }        
    }
}
