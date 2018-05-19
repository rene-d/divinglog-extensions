// tools.cs
// René DEVICHI 2011

// http://blogs.msdn.com/b/ansonh/archive/2006/09/11/750056.aspx

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Windows.Media.Imaging;
using LevDan.Exif;      // http://www.codeproject.com/KB/graphics/exiftagcol.aspx


namespace DivePictures
{
    public class picture
    {
        public bool flag = false;          // required in associate function (true => should add picture)

        public string filename { get; private set; }
        public DateTime dateTaken { get; private set; }

        public double depth { get; private set; }       // in metres

        public string title
        {
            get
            {
                if (_title == null) _title = tools.getImageTitle(filename);
                return _title;
            }
        }

        string _title;

        public picture(string filename)
        {
            this.filename = filename;
            this.dateTaken = tools.getImageDateTime(filename);
            this._title = null;
        }

        public override string ToString()
        {
            return Path.GetFileName(filename) + " " + title;
        }


        public void calcDepth(dive d, TimeSpan TimeShift)
        {
            if (d.Profile == null || d.SampleInterval == 0)
                return;

            TimeSpan offset = TimeSpan.Zero;

            double seconds = (dateTaken - d.StartTime - TimeShift).TotalSeconds;

            if (seconds >= 0 && seconds < d.Profile.Length * d.SampleInterval)
            {
                int i1 = (int)Math.Floor(seconds / d.SampleInterval);
                double y1 = d.Profile[i1];

                if (i1 + 1 < d.Profile.Length)
                {
                    double y2 = d.Profile[i1 + 1];

                    depth = y1 + (y2 - y1) * (seconds - i1 * d.SampleInterval) / d.SampleInterval;
                }
                else
                {
                    depth = y1;
                }

            }
        }
    }
    

    public class dive
    {
        public long DiveID;
        public DateTime StartTime;
        public int Duration;                    // in seconds
        public short SampleInterval = 0;        // in seconds
        public float[] Profile = null;          // in metres
        public string Site;
        public long DiveNum;

        public List<picture> Pictures = new List<picture>();

        public override string ToString()
        {
            return string.Format("{0:000} {2} {1}", DiveNum, Site, StartTime);
        }
    }


    public class imgdepth
    {
        public string filename { get; private set; }
        public TimeSpan offset { get; private set; }
        public dive diveOfImage { get; private set; }
        public double depth { get; private set; }                    // in metres
        public DateTime timeTaken { get; private set; }

        override public string ToString()
        {
            return string.Format("{0}  #{2}  {1:f1}m  {3}", Path.GetFileName(filename), depth, diveOfImage.DiveID, offset.ToString());
        }

        static public imgdepth calc(List<dive> dives, TimeSpan TimeShift, string filename)
        {
            double depth = 0;
            DateTime d = tools.getImageDateTime(filename);

            dive diveOfImage = null;
            TimeSpan offset = TimeSpan.Zero;

            foreach (dive di in dives)
            {
                offset = d - di.StartTime - TimeShift;

                if (offset.TotalSeconds >= 0 && offset.TotalSeconds < di.Profile.Length * di.SampleInterval)
                {
                    int i1 = (int)Math.Floor(offset.TotalSeconds / di.SampleInterval);
                    double y1 = di.Profile[i1];

                    if (i1 + 1 < di.Profile.Length * di.SampleInterval)
                    {
                        double y2 = di.Profile[i1 + 1];

                        depth = y1 + (y2 - y1) * (offset.TotalSeconds - i1 * di.SampleInterval) / di.SampleInterval;
                    }
                    else
                    {
                        depth = y1;
                    }

                    diveOfImage = di;

                    break;
                }
            }

            if (diveOfImage == null) return null;

            imgdepth id = new imgdepth();

            id.offset = offset;
            id.diveOfImage = diveOfImage;
            id.filename = filename;
            id.depth = depth;
            id.timeTaken = d;

            return id;
        }

    }



    static class tools
    {
        static public bool IsJpeg(string FileName)
        {
            string ext = Path.GetExtension(FileName).ToLower();

            return ext.CompareTo(".jpg") == 0
                || ext.CompareTo(".jpeg") == 0
                || ext.CompareTo(".jpe") == 0;
        }

        static public DateTime getImageDateTime(Image image)
        {
            foreach (PropertyItem propItem in image.PropertyItems)
            {
                if (propItem.Id == (int)PropertyIDTags.PropertyTagExifDTOrig)
                {
                    //listBox1.Items.Add(string.Format("PropertyTagExifDTOrig  {0}  {1} bytes", propItem.Type, propItem.Len));

                    if (propItem.Type == 2)
                    {
                        System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                        string s = encoding.GetString(propItem.Value);

                        DateTime d = DateTime.ParseExact(s, "yyyy:MM:dd H:mm:ss\0", CultureInfo.InvariantCulture);

                        //textBox1.Text = string.Format("{0} => {1}", s.Replace("\0", "\\0"), d.ToString());

                        return d;
                    }
                }
            }

            return DateTime.MinValue;
        }

        static public DateTime getImageDateTime(string filename)
        {
            if (!IsJpeg(filename))
                return DateTime.MinValue;

            try
            {
                ExifTagCollection exif = new ExifTagCollection(filename);

                string s = exif[(int)PropertyIDTags.PropertyTagExifDTOrig].Value;

                return DateTime.ParseExact(s, "yyyy:MM:dd H:mm:ss", CultureInfo.InvariantCulture); ;
            }
            catch
            {
                return DateTime.MinValue;
            }

        }

        static public string getImageTitle(string filename)
        {
            // http://stackoverflow.com/questions/5597079/iptc-net-read-write-c-sharp-library

            string title = "";

            try
            {
                var stream = new FileStream(filename, FileMode.Open, FileAccess.Read);
                var decoder = new JpegBitmapDecoder(stream, BitmapCreateOptions.None, BitmapCacheOption.None);
                var metadata = decoder.Frames[0].Metadata as BitmapMetadata;

                if (metadata.Title != null)
                {
                    title = metadata.Title;
                }
                else
                {
                    title = metadata.GetQuery("/xmp/dc:description/x-default") as string;
                }
            }

            catch
            {
            }

            return title;
        }

        /// <summary> 
        /// Open a DataTrak logbook (.LOG) and return the path of the file
        /// and a <see cref="List{dive}"/>
        /// </summary>
        static public bool openDataTrak(out List<dive> dives, out string database)
        {
            dives = null;
            database = null;

            using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
            {
                ofd.Filter = "DataTrak logbook (*.LOG)|*.LOG|All Files|*";
                ofd.FilterIndex = 0;
                ofd.CheckFileExists = true;
                ofd.ReadOnlyChecked = false;
                ofd.Multiselect = false;
                ofd.RestoreDirectory = true;
                ofd.Title = "Open a DataTrak Logbook";

                if (ofd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                {
                    return false;
                }

                database = ofd.FileName;
            }
  
            try
            {
                var log = new Uwatec.Aladin.logbook();

                log.Open(database);

                dives = new List<dive>(log.DiveCount);

                foreach (var u in log.Dives)
                {                   
                    dive d = new dive();

                    d.DiveID = u.Number;
                    d.DiveNum = u.Number;
                    d.Duration = u.DiveTime * 60;
                    d.Site = u.Location + "-" + u.Site;
                    d.StartTime = u.Date;

                    if (u.profile != null && u.profile.Profile != null)
                    {
                        d.SampleInterval = 20;
                        d.Duration = u.profile.Profile.Length * 20;

                        d.Profile = new float[u.profile.Profile.Length];
                        for (int i = 0; i < u.profile.Profile.Length; ++i)
                        {
                            d.Profile[i] = u.profile.Profile[i].Depth;
                        }
                    }

                    dives.Add(d);
                }

                return true;
            }
            catch 
            { 
                return false; 
            }  
        }


        /// <summary> 
        /// Open a SmartTrak logbook (.SLG) and return the path of the file
        /// and a <see cref="List{dive}"/>
        /// </summary>
        static public bool openSmartTRAK(out List<dive> dives, out string database)
        {
            dives = null;
            database = null;

            string LastLogbookPath = null;

            try
            {
                using (Microsoft.Win32.RegistryKey hkcu = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\SmartTRAK\Main"))
                {
                    string v = hkcu.GetValue("FileName_Caption") as string;
                    LastLogbookPath = Path.GetDirectoryName(v);
                }
            }                
            catch
            {
            }

            using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
            {
                ofd.Filter = "SmartTRAK logbook (*.SLG)|*.SLG|All Files|*";
                ofd.FilterIndex = 0;
                ofd.CheckFileExists = true;
                ofd.ReadOnlyChecked = false;
                ofd.Multiselect = false;
                ofd.RestoreDirectory = true;
                ofd.Title = "Open a SmartTRAK Logbook";

                ofd.InitialDirectory = LastLogbookPath;

                if (ofd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                {
                    return false;
                }

                database = ofd.FileName;
            }

            try
            {
                Uwatec.Smart.logbook log = new Uwatec.Smart.logbook();

                log.Open(database);

                dives = new List<dive>(log.Dives.Count);

                foreach (var u in log.Dives)
                {
                    dive d = new dive();

                    d.DiveID = u.DiveNum;
                    d.DiveNum = u.DiveNum;
                    d.Duration = u.Duration;
                    d.Site = u.Site;
                    d.StartTime = u.StartTime;
                    d.SampleInterval = u.SampleInterval;
                    d.Profile = u.Profile;

                    dives.Add(d);
                }
               
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString(), "SmartTRAK import");
                return false;
            }

            return true;
        }
    }   
}