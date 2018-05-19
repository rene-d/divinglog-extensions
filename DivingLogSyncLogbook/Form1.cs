using System;
using System.Collections.Generic;
//using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Data.OleDb;
using System.Data.Common;
using Microsoft.Win32;

namespace SyncLogbook
{
    using AndyDragon.DivingLog.ViewModelLibrary;


    public partial class Form1 : Form
    {
        class logbook
        {
            public OleDbConnection db;

            public bool open(string filename)
            {
                try
                {
                    db = null;

                    OleDbConnectionStringBuilder dbs = new OleDbConnectionStringBuilder();

                    dbs.Provider = "Microsoft.Jet.OLEDB.4.0";                    
                    dbs.DataSource = filename;
                    
                    db = new OleDbConnection();
                    db.ConnectionString = dbs.ToString();
                    db.Open();
                }
                catch (OleDbException e)
                {
                    System.Diagnostics.Debug.WriteLine("OleDbException: {0}", e.Message);
                    db = null;
                }

                return db != null;
            }

            public int count(string table)
            {
                if (db == null) return 0;
                OleDbCommand cmd = new OleDbCommand("select count(*) from " + table, db);
                return (int)cmd.ExecuteScalar();
            }

            public int next_id(string table)
            {
                if (db == null) return 0;
                OleDbCommand cmd = new OleDbCommand("select max(ID)+1 from " + table, db);
                object r = cmd.ExecuteScalar();
                if (r == System.DBNull.Value)
                    return 1;
                return (int)r;
            }

            Dictionary<string, OleDbDataAdapter> adapters = new Dictionary<string, OleDbDataAdapter>();


            public DataTable get(string table)
            {
                OleDbDataAdapter adapter = new OleDbDataAdapter("select * from " + table, db);

                // create a command builder
                OleDbCommandBuilder builder = new OleDbCommandBuilder(adapter);
                adapter.InsertCommand = builder.GetInsertCommand(true);
                adapter.UpdateCommand = builder.GetUpdateCommand();

                // problem with some column names
                adapter.RowUpdating += new OleDbRowUpdatingEventHandler(OnRowUpdatingEvent);                

                //create a DataTable to hold the query results                
                DataTable dtable = new DataTable();

                //fill the DataTable
                adapter.Fill(dtable);

                adapters[table] = adapter;

                return dtable;
            }


            public void update(string table, DataTable dtable)
            {
                DataTable changes = dtable.GetChanges();
                if (changes != null)
                {
                    adapters[table].Update(changes);
                }
                dtable.AcceptChanges();
            }


            static void OnRowUpdatingEvent(object sender, OleDbRowUpdatingEventArgs e)
            {
                // damn...
                e.Command.CommandText = e.Command.CommandText.Replace("Currency", "`Currency`");
            }
        }

        class sync
        {
            // the two logbooks
            public logbook master = new logbook();
            public logbook replica = new logbook();

            // ID equivalence tables between master and replica
            Dictionary<string, Dictionary<int, int>> IDequiv = new Dictionary<string, Dictionary<int, int>>();

            //
            bool simulate;
            Action<string> trace;

            public sync(bool simulate, Action<string> trace)       
            {
                this.simulate = simulate;
                this.trace = trace;
            }


            public bool open(string master, string replica)
            {
                if (!this.master.open(master) )
                {
                    trace(string.Format("{0} could not be opened as a logbook.", master));
                    return false;
                }

                if (!this.replica.open(replica))
                {
                    trace(string.Format("{0} could not be opened as a logbook.", replica));
                    return false;
                }

                return true;
            }


            public void process(string table, string infoColumn)
            {
                int id = replica.next_id(table);

                using (DataTable cmd1 = master.get(table))
                {
                    using (DataTable cmd2 = replica.get(table))
                    {
                        foreach (DataRow src in cmd1.Rows)
                        {
                            var uuid = src["UUID"];

                            bool updated = false;
                            bool found = false;
                            foreach (DataRow dst in cmd2.Rows)
                            {
                                if ((System.Guid)dst["UUID"] == (System.Guid)uuid)
                                {
                                    found = true;

                                    var updated1 = (DateTime)src["Updated"];
                                    var updated2 = (DateTime)dst["Updated"];

                                    if (updated1 > updated2)
                                    {
                                        dst.BeginEdit();
                                        var temp = dst["ID"];
                                        dst.ItemArray = src.ItemArray;
                                        dst["ID"] = temp;
                                        dst.EndEdit();
                                        updated = true;
                                    }

                                    break;
                                }
                            }

                            if (!found && !simulate)
                            {
                                DataRow copy = cmd2.NewRow();
                                copy.ItemArray = src.ItemArray;
                                copy["ID"] = id++;

                                if (copy.Table.Columns.Contains("CountryID"))
                                    doEquiv(copy, src, "Country", "CountryID");

                                if (copy.Table.Columns.Contains("PlaceID"))
                                    doEquiv(copy, src, "Place", "PlaceID");

                                if (copy.Table.Columns.Contains("CityID"))
                                    doEquiv(copy, src, "City", "CityID");

                                if (copy.Table.Columns.Contains("ShopID"))
                                    doEquiv(copy, src, "Shop", "ShopID");

                                cmd2.Rows.Add(copy);
                            }


                            string msg = null;
                            if (found)
                            {
                                if (updated)
                                {
                                    msg = "updated";
                                }
                                else
                                {
                                    //msg = "nothing to do";
                                }
                            }
                            else
                            {
                                msg = "added";
                            }

                            if (msg != null && trace != null)
                            {
                                trace(string.Format("analyze {2} / {0}: {1}", src[infoColumn], msg, table));
                            }
                        }


                        if (!simulate)
                        {

                            replica.update(table, cmd2);

                            // calculate the table ID equivalence between master and replica
                            Dictionary<int, System.Guid> direct = new Dictionary<int, Guid>();
                            Dictionary<System.Guid, int> reverse = new Dictionary<Guid, int>();
                            Dictionary<int, int> ids = new Dictionary<int, int>();

                            foreach (DataRow r in cmd1.Rows)
                            {
                                direct[(int)r["ID"]] = (System.Guid)r["UUID"];
                            }
                            foreach (DataRow r in cmd2.Rows)
                            {
                                reverse[(System.Guid)r["UUID"]] = (int)r["ID"];
                            }

                            foreach (int i in direct.Keys)
                            {
                                ids[i] = reverse[direct[i]];
                            }

                            ids[0] = 0;
                            IDequiv[table] = ids;
                        }
                    }

                }
            }


            private void doEquiv(DataRow copy, DataRow src, string table, string column)
            {
                var a = src[column];
                if (a == System.DBNull.Value)
                    copy[column] = System.DBNull.Value;
                else
                    copy[column] = IDequiv[table][(int)a];
            }           
        }

        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            buttonSetIDs.Hide();     // advanced queries, not for all users

            RegistryKey hkcu;
            
#if true
            const string divingLogToolName = "Diving Log Sync Logbook";
            const string obsoleteDivingLogToolName = "SyncLogbook";
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
            
#else

            hkcu = Registry.CurrentUser.OpenSubKey(@"Software\Divinglog\Divinglog50\Extern", true);
            if (hkcu != null)
            {
                // if the extern tool has not been configured yet,
                //  1. display the software disclosure condition
                //  2. configure it
                //  nota: GetValue doesn't throw if key is writable
                string exe = (string)hkcu.GetValue("SyncLogbook");
                string me = Application.ExecutablePath;
                if (exe == null || exe != me)
                {
                    FormLicense dlg = new FormLicense();
                    if (dlg.ShowDialog() != DialogResult.Yes)
                    {
                        Close();
                        return;
                    }
                    hkcu.SetValue("SyncLogbook", me);
                }
            }
#endif

            hkcu = Registry.CurrentUser.OpenSubKey(@"Software\Divinglog\Divinglog50\WindowSizes");
            if (hkcu != null)
            {
                // nota: GetValue throws if key is read-only
                try
                {
                    string[] p = ((string)hkcu.GetValue("frmSyncLogbook")).Split(' ');

                    if (p.Length == 4)
                    {
                        this.Location = new Point(Convert.ToInt32(p[0]), Convert.ToInt32(p[1]));
                        this.Size = new Size(Convert.ToInt32(p[2]), Convert.ToInt32(p[3]));
                    }
                }
                catch { }
            }


            hkcu = Registry.CurrentUser.OpenSubKey(@"Software\Divinglog\Divinglog50\Sync\Logbooks");
            if (hkcu != null)
            {
                try { textBoxMaster.Text = hkcu.GetValue("Master").ToString(); }
                catch { }

                try { textBoxReplica.Text = hkcu.GetValue("Replica").ToString(); }
                catch { }

                try {
                    bool b;
                    if (bool.TryParse(hkcu.GetValue("AdvancedQueries").ToString(), out b))
                        buttonSetIDs.Visible = b;
                }
                catch { }
            }

        }


        private void chooseLogbook(TextBox textBox, string valueName)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Diving Log 5.0 (*.mdb)|*.mdb|All Files|*";
            ofd.FilterIndex = 0;
            ofd.CheckFileExists = true;
            ofd.ReadOnlyChecked = false;
            ofd.Multiselect = false;

            string defaultLogBook = DivingLogRegistryHelper.DefaultLogFile;
            if (defaultLogBook != null)
            {
                defaultLogBook = Path.GetDirectoryName(defaultLogBook);
                if (Directory.Exists(defaultLogBook))
                {
                    ofd.InitialDirectory = defaultLogBook;
                }
            }

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBox.Text = ofd.FileName;

                try
                {
                    RegistryKey hkcu = Registry.CurrentUser.CreateSubKey(@"Software\Divinglog\Divinglog50\Sync\Logbooks");
                    if (hkcu != null)
                    {
                        hkcu.SetValue(valueName, ofd.FileName);
                    }
                }
                catch
                {
                    // ignore all registry errors
                }
            }
        }

        private void addTrace(string s)
        {
            int i = listBox1.Items.Add(s);
            listBox1.TopIndex = i;
        }


        private void countDetails(string filename, string role)
        {
            logbook db = new logbook();

            if (! db.open(filename))
                return;

            addTrace(
                string.Format("{0} ({1}): Country:{2} City:{3} Place:{4} Shop:{5} Trip:{6}",
                    Path.GetFileNameWithoutExtension(filename), role,
                    db.count("Country"),
                    db.count("City"),
                    db.count("Place"),
                    db.count("Shop"),
                    db.count("Trip")));
        }


        private void buttonSimulate_Click(object sender, EventArgs e)
        {
            countDetails(textBoxMaster.Text, "master");
            countDetails(textBoxReplica.Text, "replica");

            sync x = new sync(true, addTrace);

            if (!x.open(textBoxMaster.Text, textBoxReplica.Text))
                return;

            x.process("Country", "Country");
            x.process("City", "City");
            x.process("Place", "Place");
            x.process("Shop", "ShopName");
            x.process("Trip", "TripName");

            addTrace("Simulation done.");
        }


        private void buttonSynchronize_Click(object sender, EventArgs e)
        {
            sync x = new sync(false, addTrace);

            if (!x.open(textBoxMaster.Text, textBoxReplica.Text))
                return;

            // remove bad outer keys (where the primary keys no long exist)
            string[] sql = {
"UPDATE Trip SET ShopID=NULL WHERE ShopID NOT IN (SELECT ID FROM Shop)",
"UPDATE Trip SET CountryID=NULL WHERE CountryID NOT IN (SELECT ID FROM Country)",
"UPDATE Trip SET CityID=NULL WHERE CityID not in (select ID from City)",
"UPDATE Place SET CountryID=NULL WHERE CountryID NOT IN (SELECT ID FROM Country)",
"UPDATE City SET CountryID=NULL WHERE CountryID NOT IN (SELECT ID FROM Country)",
               };

            Array.ForEach(sql,
                query =>
                {
                    using (OleDbCommand cmd = new OleDbCommand(query, x.master.db))
                    {
                        int nb = cmd.ExecuteNonQuery();
                        if (nb != 0)
                            addTrace(string.Format("updated: {0} row(s)", nb));
                    }
                });

            x.process("Country", "Country");
            x.process("City", "City");
            x.process("Place", "Place");
            x.process("Shop", "ShopName");
            x.process("Trip", "TripName");

            // update the text fields
            string[] sql2 = {
"UPDATE Logbook AS l INNER JOIN Country AS t ON t.ID=l.CountryID SET l.Country=t.Country WHERE l.CountryID IS NOT NULL AND l.Country<>t.Country",
"UPDATE Logbook AS l INNER JOIN City AS t ON t.ID=l.CityID SET l.City=t.City WHERE l.CityID IS NOT NULL AND l.City<>t.City",
"UPDATE Logbook AS l INNER JOIN Place AS t ON t.ID=l.PlaceID SET l.Place=t.Place WHERE l.PlaceID IS NOT NULL AND l.Place<>t.Place",
            };

            Array.ForEach(sql2,
                query=> {
                    using (OleDbCommand cmd = new OleDbCommand(query, x.replica.db))
                    {
                        int nb = cmd.ExecuteNonQuery();
                        if (nb != 0)
                            addTrace(string.Format("updated: {0} row(s)", nb));
                    }
                });

            addTrace("Sync done.");
        }


        private void buttonOpenMaster_Click(object sender, EventArgs e)
        {
            chooseLogbook(textBoxMaster, "Master");
        }


        private void buttonOpenReplica_Click(object sender, EventArgs e)
        {
            chooseLogbook(textBoxReplica, "Replica");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            RegistryKey hkcu;

            hkcu = Registry.CurrentUser.OpenSubKey(@"Software\Divinglog\Divinglog50\WindowSizes", true);
            if (hkcu != null)
            {
                hkcu.SetValue("frmSyncLogbook", string.Format("{0} {1} {2} {3}",
                    this.Location.X, this.Location.Y, this.Size.Width, this.Size.Height), RegistryValueKind.String);
            }
        }


        private void buttonSetIDs_Click(object sender, EventArgs e)
        {
            string[] sql = {

// supprime les mauvaises clés étrangères (où il n'y a plus de clé primaire équivalente)
"UPDATE Trip SET ShopID=NULL WHERE ShopID NOT IN (SELECT ID FROM Shop)",
"UPDATE Trip SET CountryID=NULL WHERE CountryID NOT IN (SELECT ID FROM Country)",
"UPDATE Trip SET CityID=NULL WHERE CityID not in (select ID from City)",
"UPDATE Place SET CountryID=NULL WHERE CountryID NOT IN (SELECT ID FROM Country)",
"UPDATE City SET CountryID=NULL WHERE CountryID NOT IN (SELECT ID FROM Country)",

"UPDATE Logbook INNER JOIN Country ON Country.Country=Logbook.Country SET Logbook.CountryId = Country.ID WHERE Logbook.CountryId IS NULL",
"UPDATE Logbook INNER JOIN City ON City.City=Logbook.City SET Logbook.CityId = City.ID WHERE Logbook.CityId IS NULL",
"UPDATE Logbook INNER JOIN Place ON Place.Place=Logbook.Place SET Logbook.PlaceId = Place.ID WHERE Logbook.PlaceId IS NULL",
"UPDATE Place SET MaxDepth=NULL WHERE MaxDepth=0",
            };


            Action<string> norm = new Action<string>(
                Filename =>
                {

                    logbook db = new logbook();
                    db.open(Filename);

                    Array.ForEach(sql,
                        query =>
                        {
                            using (OleDbCommand cmd = new OleDbCommand(query, db.db))
                            {
                                int nb = cmd.ExecuteNonQuery();
                                if (nb != 0)
                                    addTrace(string.Format("{1} normalized: {0} row(s)", nb, Path.GetFileNameWithoutExtension(Filename)));
                            }
                        });                    
                });

            norm(textBoxMaster.Text);
            norm(textBoxReplica.Text);

            addTrace(string.Format("Normalization done."));
        }


        private void buttonHelp_Click(object sender, EventArgs e)
        {
            FormHelp help = new FormHelp();

            help.advancedQueries = buttonSetIDs.Visible;

            if (help.ShowDialog(this) == DialogResult.OK)
            {
                buttonSetIDs.Visible = help.advancedQueries;

                try
                {
                    RegistryKey hkcu = Registry.CurrentUser.CreateSubKey(@"Software\Divinglog\Divinglog50\Sync\Logbooks");
                    if (hkcu != null)
                    {
                        hkcu.SetValue("AdvancedQueries", buttonSetIDs.Visible);
                    }
                }
                catch
                {
                    // ignore all registry errors
                }
            }
        }
    }
}
