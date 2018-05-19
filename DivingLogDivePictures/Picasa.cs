using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace DivePictures
{
    public class picasaAlbums
    {
        public class album
        {
            string filename;
            public string name { get; private set; }
            picasa2album pal;

            public album(string filename, picasa2album pal)
            {
                this.filename = filename;

                /*
                this.name = (from p in pal.property
                             where p.name == "name"
                             select p.value).First();
               */

                StringDictionary properties = new StringDictionary();

                foreach (var p in pal.property)
                    properties.Add(p.name, p.value);

                this.name = properties["name"];

                double f;
                if (double.TryParse(properties["date"], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out f))
                    this.date = f;

                this.pal = pal;
            }

            override public string ToString()
            {
                return name;
            }

            public string[] files()
            {
                return pal.files;
            }

            public double date { get; private set; }
        };

        List<album> albums = new List<album>();


        public object[] ToArray()
        {
            return albums.ToArray();
        }


        public picasaAlbums()
        {
            string dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Google", "Picasa2Albums");

            if (Directory.Exists(dir))
            {
                foreach (var user in Directory.EnumerateDirectories(dir))
                {
                    if (Path.GetFileName(user) == "backup")
                        continue;

                    foreach (var filename in Directory.EnumerateFiles(user, "*.pal"))
                    {
                        try
                        {
                            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
                            {
                                using (TextReader reader = new StreamReader(fs, Encoding.UTF8))
                                {
                                    XmlSerializer serializer = new XmlSerializer(typeof(picasa2album));

                                    picasa2album pal = serializer.Deserialize(reader) as picasa2album;

                                    //albums.AddRange(from p in pal.property where p.name == "name" select new album(filename, p.value));

                                    albums.Add(new album(filename, pal));
                                }
                            }
                        }
                        catch
                        {
                        }
                    }
                }
            }

            // Nota: the picasa album date does not seems to be very reliable...
            albums.Sort((a, b) => a.date.CompareTo(a.date));
        }

        static public string Expand(string filename)
        {

            if (filename.StartsWith("$My Documents"))
            {
                filename = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + filename.Substring("$My Documents".Length);
            }
            else
            {
                filename = Regex.Replace(filename, @"^\[(\w)\]", @"$1:");
            }

            return filename;
        }
    }

}
