using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Windows.Forms;
using AndyDragon.DivingLog.ViewModelLibrary;

namespace DivePictures
{
    class DivingLog
    {
        static public bool openDL5(out List<dive> dives, out OleDbConnection db, bool alwaysShowDialog)        
        {
            db = new OleDbConnection();
            dives = new List<dive>();
            
            try
            {
                OleDbConnectionStringBuilder dbs = new OleDbConnectionStringBuilder();

                dbs.Provider = "Microsoft.Jet.OLEDB.4.0";
                dbs.DataSource = "logbook.mdb";

                string path = DivingLogRegistryHelper.DefaultLogFile;

                if (path != null && File.Exists(path)) dbs.DataSource = path;                   

                if (alwaysShowDialog || !File.Exists(dbs.DataSource))
                {
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.Filter = "DivingLog5 database (*.mdb)|*.mdb|All Files|*";
                    ofd.FilterIndex = 0;
                    ofd.CheckFileExists = true;
                    ofd.ReadOnlyChecked = false;
                    ofd.Multiselect = false;
                    ofd.RestoreDirectory = true;

                    if (path != null)
                    {
                        path = Path.GetDirectoryName(path);
                        if (Directory.Exists(path)) ofd.InitialDirectory = path;
                    }

                    if (ofd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    {
                        return false;
                    }

                    dbs.DataSource = ofd.FileName;
                }

                db.ConnectionString = dbs.ToString();
                db.Open();

                
                //toolStripStatusLabel1.Text = dbs.DataSource;

                OleDbCommand cmd = new OleDbCommand("SELECT `ID`,`Number`,Profile,ProfileInt,Place,Divedate,Entrytime,Divetime from Logbook order by `Number` DESC", db);
                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {


                        try
                        {
                            dive d = new dive();

                            d.DiveID = (int)reader["ID"];
                            d.DiveNum = (int)reader["Number"];

                            d.StartTime = (DateTime)reader["Divedate"];

                            if (reader["Entrytime"] != System.DBNull.Value)
                            {
                                DateTime t = (DateTime)reader["Entrytime"];         // SECONDS ARE MISSING !!!
                                d.StartTime += t.TimeOfDay;

                                if (reader["Divetime"] != System.DBNull.Value)
                                    d.Duration = (int)((double)reader["Divetime"] * 60);

                                d.Site = reader["Place"] as string;

                                if (reader["ProfileInt"] != System.DBNull.Value)
                                {
                                    d.SampleInterval = (short)(int)reader["ProfileInt"];
                                    if (d.SampleInterval != 0)
                                    {
                                        string ProfileBlob = reader["Profile"] as string;

                                        d.Profile = new float[ProfileBlob.Length / 12];
                                        for (int i = 0; i < ProfileBlob.Length / 12; ++i)
                                        {
                                            d.Profile[i] = float.Parse(ProfileBlob.Substring(i * 12, 5)) / 100;
                                        }
                                    }
                                }
                                /*else
                                {
                                    d.SampleInterval = 0;
                                    d.Profile = null;
                                }
                                */
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
            catch (OleDbException e)
            {
                MessageBox.Show(e.ToString(), "OleDbException");
                return false;
            }

            return true;
        }

    }
}
