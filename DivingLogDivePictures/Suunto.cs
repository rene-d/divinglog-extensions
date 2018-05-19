// Suunto.cs
//  read the Suunto Dive Manager 4 database
//  http://www.movescount.com/applications/dm4
// René DEVICHI 2011

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;


namespace DivePictures
{
    class Suunto
    {
        static public bool openDM4(out List<dive> dives, out string database, bool alwaysShowDialog)
        {
            dives = new List<dive>();
            database = null;

            try
            {
                SQLiteConnectionStringBuilder csb = new SQLiteConnectionStringBuilder();

                // Suunto DM4 1.1.7.2946
                csb.DataSource = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        @"Apps\2.0\Data\J9CA6TY6.2ZT\PQRV4HLT.BRN\suun..tion_6a846f4ede1b171c_0001.0001_b707352d50d79da0\Data\DM4.db");

                if (!File.Exists(csb.DataSource))
                {
                    // Suunto DM4 with Movescount 1.1.3.1533
                    csb.DataSource = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        @"Apps\2.0\Data\J9CA6TY6.2ZT\PQRV4HLT.BRN\suun..tion_4ab3b6265acd3e3f_0001.0001_3a21971bfe56d99c\Data\DM4.db");
                }


                if (alwaysShowDialog || !File.Exists(csb.DataSource))
                {
                    OpenFileDialog ofd = new OpenFileDialog();

                    ofd.Filter = "DM4 database (DM4.db)|*.db|All Files|*";
                    ofd.FilterIndex = 0;
                    ofd.CheckFileExists = true;
                    ofd.ReadOnlyChecked = false;
                    ofd.Multiselect = false;
                    ofd.RestoreDirectory = true;
                    ofd.Title = "Open the Suunto Dive Manager 4 database";

                    if (File.Exists(csb.DataSource))
                        ofd.InitialDirectory = Path.GetDirectoryName(csb.DataSource);

                    if (ofd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    {
                        return false;
                    }

                    csb.DataSource = ofd.FileName;
                }


                SQLiteConnection cnn = new SQLiteConnection(csb.ConnectionString);

                cnn.Open();

                database = csb.DataSource;

                using (SQLiteCommand cmd = cnn.CreateCommand())
                {
                    cmd.CommandText = "SELECT DiveID,StartTime+0 As StartTime,Duration,SampleInterval,ProfileBlob,Note FROM Dive order by DiveID DESC";
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                dive d = new dive();

                                d.DiveID = (long)reader["DiveID"];
                                d.StartTime = new DateTime((long)reader["StartTime"]);
                                d.Duration = (int)reader["Duration"];
                                d.SampleInterval = (short)reader["SampleInterval"];
                                d.Site = reader["Note"] as string;

                                d.DiveNum = d.DiveID;

                                byte[] ProfileBlob = reader["ProfileBlob"] as byte[];

                                d.Profile = new float[ProfileBlob.Length / 4];
                                for (int i = 0; i < ProfileBlob.Length / 4; ++i)
                                {
                                    d.Profile[i] = System.BitConverter.ToSingle(ProfileBlob, i * 4);
                                }

                                dives.Add(d);
                            }
                            catch (InvalidCastException ex)
                            {
                                System.Diagnostics.Debug.Write(ex);
                            }
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.ToString(), "SQLiteException");
                return false;
            }

            return true;
        }


        static public bool openDM3(out List<dive> dives, out string database)
        {
            dives = new List<dive>();
            database = @"C:\ProgramData\Suunto\D9.mdb";

            try
            {
                OleDbConnection db = new OleDbConnection();

                OleDbConnectionStringBuilder dbs = new OleDbConnectionStringBuilder();

                dbs.Provider = "Microsoft.Jet.OLEDB.4.0";
                dbs.DataSource = database;
                dbs.Add("Jet OLEDB:Database Password", "koira");

                db.ConnectionString = dbs.ToString();
                db.Open();

                OleDbCommand cmd = new OleDbCommand(
                    "select ItemID, Name, StartTime, EndTime, Distance, i_custom4, t_custom4 "
                    + "from Items where Type=33 order by UserID asc, ItemID desc", db);

                OleDbCommand profileCmd = new OleDbCommand(
                    "select Altitude from TrackPoints where LogID=@ItemID order by SampleTime asc", db);

                OleDbParameter ItemId = profileCmd.Parameters.Add("@ItemID", OleDbType.Integer);

                OleDbDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    dive d = new dive();

                    d.DiveID = (int)dr["ItemID"];
                    d.DiveNum = d.DiveID;
                    d.Duration = (int)(double)dr["Distance"];
                    d.SampleInterval = (short)(int)dr["i_custom4"];
                    d.Site = (string)dr["Name"];        // or (string)dr["t_custom4"]
                    d.StartTime = (DateTime)dr["StartTime"];

                    List<float> depths = new List<float>();

                    ItemId.Value = d.DiveID;
                    using (OleDbDataReader p = profileCmd.ExecuteReader())
                    {
                        depths.Add(0);
                        while (p.Read())
                        {
                            depths.Add((float)(double)p[0]);
                        }
                        depths.Add(0);
                    }

                    d.Profile = depths.ToArray();

                    dives.Add(d);
                }
            }
            catch
            {
                dives = null;
                database = null;
                return false;
            }

            return true;
        }
    }
}
