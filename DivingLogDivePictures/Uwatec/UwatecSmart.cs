// UwatecSmart.cs
// René DEVICHI 2011

// some information here:
//  http://www.divesoftware.org/libdc/
//  http://diversity.sourceforge.net/uwatec_smart_format.html
//  http://diversity.sourceforge.net/uwatec_galileo_format.html

using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using DivePictures.Properties;


namespace Uwatec.Smart
{
    /// <summary>
    /// structure to store a dive
    /// </summary>
    public struct dive
    {
        public long DiveNum;
        public DateTime StartTime;
        public int Duration;             // in seconds
        public short SampleInterval;     // in seconds (always 4 for Smart computers)
        public float[] Profile;          // in metres
        public string Site;

#if UWATEC_SMART_VIEWER
        public DiveHeader header;
        public byte[] rawHeader;
        public int model;
        public override string ToString()
        {
            return string.Format("{0} {1}", DiveNum, Site);
            //return base.ToString();
        }
#endif
    }

    /// <summary>
    /// 'Log' column of SmartTRAK database = the dive header
    /// </summary>
    public struct DiveHeader
    {
        public enum LogFormat
        {
            None,
            Format92,
            Smart,
        }

        public LogFormat Format;
        /// <summary>
        /// length in bytes of the header + profile
        /// (bytes 4-7) 
        /// </summary>
        public UInt32 DiveDataLength;
        /// <summary>
        /// half-secondes since 200/01/01 00:00:00 UTC
        /// (bytes 8-11)
        /// </summary>
        public DateTime DiveStartTime;                  
        /// <summary>
        /// decoded values from the header
        /// </summary>
        public Dictionary<string, object> Values;
    }

    enum SampleType
    {
        PRESSURE_DEPTH,
        /// <summary>
        /// This is the number of minutes that the dive computer estimates you have left based on your air consumption. This value requires no conversion.
        /// </summary>
        RBT,
        /// <summary>
        /// To convert to centigrade divide by 2.5
        /// See also the note on <seealso cref="Delta Values"/>.
        /// </summary>
        TEMPERATURE,
        /// <summary>
        /// Absolute Tank pressure, besides indicating what you would expect, also indicates a tank switch for the Smart TEC. For example, if an Absolute Tank 2 Pressure is read, then the diver has switched to Tank 2 and all subsequent Delta Tank Pressures are for Tank 2 - unless of course an Absolute Tank 1 or D Pressure is read.
        /// To convert to bar divide by 4
        /// Note that the current tank pressure can be &lt; 0 which of course is invalid so needs to be checked for.
        /// </summary>
        PRESSURE,
        /// <summary>
        /// The initial absolute depth of a profile is essentially a depth calibration. At this time in the dive the depth is 0M. You need to subtract this value from each subsequent depth to get the correct depth.
        /// When a depth DTI is read, this is also when a profile segment is complete. All current values for temperature, alarms, tank pressure, RBT and, of course, depth are correct for the current profile time. The current time of a profile must be incremented by 4 seconds after the data has been acted upon.
        /// To convert the depth to meters divide by 50.
        /// </summary>
        DEPTH,
        HEARTRATE,
        BEARING,
        ALARMS,
        /// <summary>
        /// This value indicates that the current profile time must be incremented by value * 4 seconds.
        /// </summary>
        TIME,
    }

    struct SampleInfo
    {
        public SampleType type;
        public bool absolute;
        public int index;
        public int ntypebits;       // nombre de bits du type identifier
        public bool ignoretype;     // Ignore any data bits that are stored in the last type byte for certain samples.
        public int extrabytes;      // octets supplémentaires (après ceux qui contiennent le type identifier)

        public SampleInfo(SampleType type, int absolute, int index, int ntypebits, int ignoretype, int extrabytes)
        {
            this.type = type;
            this.absolute = absolute != 0;
            this.index = index;
            this.ntypebits = ntypebits;
            this.ignoretype = ignoretype != 0;
            this.extrabytes = extrabytes;
        }

        public delegate SampleInfo identify();
    }

    struct Model
    {
        public byte model;
        public string name;
        public SampleInfo[] table;

        public Model(byte model, string name, int headerSize, SampleInfo[] table)
        {
            this.model = model;
            this.name = name;
            this.table = table;
        }
    }


    class parserProfile
    {
        const int NBITS = 8;

        #region définitions des SampleInfo[] par computer

        // Smart Pro
        static SampleInfo[] smart_pro_table = 
        {
            new SampleInfo(SampleType.DEPTH,          0, 0, 1, 0, 0), // 0ddddddd
            new SampleInfo(SampleType.TEMPERATURE,    0, 0, 2, 0, 0), // 10dddddd
            new SampleInfo(SampleType.TIME,           1, 0, 3, 0, 0), // 110ddddd
            new SampleInfo(SampleType.ALARMS,         1, 0, 4, 0, 0), // 1110dddd
            new SampleInfo(SampleType.DEPTH,          0, 0, 5, 0, 1), // 11110ddd dddddddd
            new SampleInfo(SampleType.TEMPERATURE,    0, 0, 6, 0, 1), // 111110dd dddddddd
            new SampleInfo(SampleType.DEPTH,          1, 0, 7, 1, 2), // 1111110d dddddddd dddddddd
            new SampleInfo(SampleType.TEMPERATURE,    1, 0, 8, 0, 2), // 11111110 dddddddd dddddddd
        };

        // Aladin Prime
        // Aladin Tec
        // Aladin Tec 2G
        static SampleInfo[] smart_aladin_table = 
        {
            new SampleInfo(SampleType.DEPTH,          0, 0, 1, 0, 0), // 0ddddddd
            new SampleInfo(SampleType.TEMPERATURE,    0, 0, 2, 0, 0), // 10dddddd
            new SampleInfo(SampleType.TIME,           1, 0, 3, 0, 0), // 110ddddd
            new SampleInfo(SampleType.ALARMS,         1, 0, 4, 0, 0), // 1110dddd
            new SampleInfo(SampleType.DEPTH,          0, 0, 5, 0, 1), // 11110ddd dddddddd
            new SampleInfo(SampleType.TEMPERATURE,    0, 0, 6, 0, 1), // 111110dd dddddddd
            new SampleInfo(SampleType.DEPTH,          1, 0, 7, 1, 2), // 1111110d dddddddd dddddddd
            new SampleInfo(SampleType.TEMPERATURE,    1, 0, 8, 0, 2), // 11111110 dddddddd dddddddd
            new SampleInfo(SampleType.ALARMS,         1, 1, 9, 0, 0), // 11111111 0ddddddd
        };

        // Smart Com
        static SampleInfo[] smart_com_table = 
        {
            new SampleInfo(SampleType.PRESSURE_DEPTH, 0, 0,  1, 0, 1), // 0ddddddd dddddddd
            new SampleInfo(SampleType.RBT,            0, 0,  2, 0, 0), // 10dddddd
            new SampleInfo(SampleType.TEMPERATURE,    0, 0,  3, 0, 0), // 110ddddd
            new SampleInfo(SampleType.PRESSURE,       0, 0,  4, 0, 1), // 1110dddd dddddddd
            new SampleInfo(SampleType.DEPTH,          0, 0,  5, 0, 1), // 11110ddd dddddddd
            new SampleInfo(SampleType.TEMPERATURE,    0, 0,  6, 0, 1), // 111110dd dddddddd
            new SampleInfo(SampleType.ALARMS,         1, 0,  7, 1, 1), // 1111110d dddddddd
            new SampleInfo(SampleType.TIME,           1, 0,  8, 0, 1), // 11111110 dddddddd
            new SampleInfo(SampleType.DEPTH,          1, 0,  9, 1, 2), // 11111111 0ddddddd dddddddd dddddddd
            new SampleInfo(SampleType.PRESSURE,       1, 0, 10, 1, 2), // 11111111 10dddddd dddddddd dddddddd
            new SampleInfo(SampleType.TEMPERATURE,    1, 0, 11, 1, 2), // 11111111 110ddddd dddddddd dddddddd
            new SampleInfo(SampleType.RBT,            1, 0, 12, 1, 1), // 11111111 1110dddd dddddddd
        };

        // Smart Tec
        // Smart Z
        static SampleInfo[] smart_tec_table = 
        {
            new SampleInfo(SampleType.PRESSURE_DEPTH, 0, 0,  1, 0, 1), // 0ddddddd dddddddd
            new SampleInfo(SampleType.RBT,            0, 0,  2, 0, 0), // 10dddddd
            new SampleInfo(SampleType.TEMPERATURE,    0, 0,  3, 0, 0), // 110ddddd
            new SampleInfo(SampleType.PRESSURE,       0, 0,  4, 0, 1), // 1110dddd dddddddd
            new SampleInfo(SampleType.DEPTH,          0, 0,  5, 0, 1), // 11110ddd dddddddd
            new SampleInfo(SampleType.TEMPERATURE,    0, 0,  6, 0, 1), // 111110dd dddddddd
            new SampleInfo(SampleType.ALARMS,         1, 0,  7, 1, 1), // 1111110d dddddddd
            new SampleInfo(SampleType.TIME,           1, 0,  8, 0, 1), // 11111110 dddddddd
            new SampleInfo(SampleType.DEPTH,          1, 0,  9, 1, 2), // 11111111 0ddddddd dddddddd dddddddd
            new SampleInfo(SampleType.TEMPERATURE,    1, 0, 10, 1, 2), // 11111111 10dddddd dddddddd dddddddd
            new SampleInfo(SampleType.PRESSURE,       1, 0, 11, 1, 2), // 11111111 110ddddd dddddddd dddddddd
            new SampleInfo(SampleType.PRESSURE,       1, 1, 12, 1, 2), // 11111111 1110dddd dddddddd dddddddd
            new SampleInfo(SampleType.PRESSURE,       1, 2, 13, 1, 2), // 11111111 11110ddd dddddddd dddddddd
            new SampleInfo(SampleType.RBT,            1, 0, 14, 1, 1), // 11111111 111110dd dddddddd
        };

        // Galileo Sol
        static SampleInfo[] galileo_sol_table = 
        {
            new SampleInfo(SampleType.DEPTH,          0, 0, 1, 0, 0), // 0ddd dddd
            new SampleInfo(SampleType.RBT,            0, 0, 3, 0, 0), // 100d dddd
            new SampleInfo(SampleType.PRESSURE,       0, 0, 4, 0, 0), // 1010 dddd
            new SampleInfo(SampleType.TEMPERATURE,    0, 0, 4, 0, 0), // 1011 dddd
            new SampleInfo(SampleType.TIME,           1, 0, 4, 0, 0), // 1100 dddd
            new SampleInfo(SampleType.HEARTRATE,      0, 0, 4, 0, 0), // 1101 dddd
            new SampleInfo(SampleType.ALARMS,         1, 0, 4, 0, 0), // 1110 dddd
            new SampleInfo(SampleType.ALARMS,         1, 1, 8, 0, 1), // 1111 0000 dddddddd
            new SampleInfo(SampleType.DEPTH,          1, 0, 8, 0, 2), // 1111 0001 dddddddd dddddddd
            new SampleInfo(SampleType.RBT,            1, 0, 8, 0, 1), // 1111 0010 dddddddd
            new SampleInfo(SampleType.TEMPERATURE,    1, 0, 8, 0, 2), // 1111 0011 dddddddd dddddddd
            new SampleInfo(SampleType.PRESSURE,       1, 0, 8, 0, 2), // 1111 0100 dddddddd dddddddd
            new SampleInfo(SampleType.PRESSURE,       1, 1, 8, 0, 2), // 1111 0101 dddddddd dddddddd
            new SampleInfo(SampleType.PRESSURE,       1, 2, 8, 0, 2), // 1111 0110 dddddddd dddddddd
            new SampleInfo(SampleType.HEARTRATE,      1, 0, 8, 0, 1), // 1111 0111 dddddddd
            new SampleInfo(SampleType.BEARING,        1, 0, 8, 0, 2), // 1111 1000 dddddddd dddddddd
            new SampleInfo(SampleType.ALARMS,         1, 2, 8, 0, 1), // 1111 1001 dddddddd
        };
        #endregion

        #region Uwatec Smart computers
        static Model[] models = 
        {
            new Model(0x10, "Smart Pro", 90, smart_pro_table),
            new Model(0x11, "Galileo Sol", 152, galileo_sol_table),
            new Model(0x12, "Aladin Tec, Prime", 108, smart_aladin_table),
            new Model(0x13, "Aladin Tec 2G", 116, smart_aladin_table),
            new Model(0x14, "Smart Com", 100, smart_com_table),
            new Model(0x18, "Smart Tec", 132, smart_tec_table),
            new Model(0x1C, "Smart Z", 132, smart_tec_table),
        };
        #endregion

        int smart_identify(byte[] data, int offset)
        {
            int count = 0;
            for (int i = offset; i < data.Length; ++i)
            {
                byte value = data[i];
                for (int j = 0; j < NBITS; ++j)
                {
                    int mask = 1 << (NBITS - 1 - j);
                    if ((value & mask) == 0)
                        return count;
                    count++;
                }
            }

            throw new Exception(string.Format("Unknown sample at offset {0}", offset));
        }

        int galileo_identify(byte[] data, int offset)
        {
            byte value = data[offset];

            if ((value & 0x80) == 0)        // Delta Depth
                return 0;

            if ((value & 0xE0) == 0x80)     // Delta RBT
                return 1;

            if ((value & 0xF0) != 0xF0)     // 1010 dddd -> 1110 dddd
                return (value >> 4) - 8;

            if ((value & 0x0F) <= 9)        // 1111 0000 -> 1111 1001
                return (value & 0x0F) + 7;

            throw new Exception(string.Format("Unknown sample at offset {0}", offset));
        }

        int smart_fixsignbit(uint x, int n)
        {

            int signbit = (1 << (n - 1));
            uint mask = (0xFFFFFFFF << n);

            // When turning a two's-complement number with a certain number
            // of bits into one with more bits, the sign bit must be repeated
            // in all the extra bits.
            if ((x & signbit) == signbit)
                return (int)(x | mask);
            else
                return (int)(x & ~mask);
        }

        public float[] parse(byte model, byte[] data)
        {
            List<float> profile = new List<float>();

            uint complete = 0;
            bool calibrated = false;

            uint time = 0;
            int rbt = 99;
            int tank = 0;
            float depth = 0, depth_calibration = 0;
            float temperature = 0;
            float pressure = 0;
            int heartrate = 0;
            uint bearing = 0;
            byte[] alarms = { 0, 0, 0 };

            bool have_depth = false, have_temperature = false, have_pressure = false, have_rbt = false,
                have_heartrate = false, have_alarms = false, have_bearing = false;

            int offset = 0;

            SampleInfo[] table = (from m in models where m.model == model select m.table).First();

            SampleInfo.identify identify = delegate()
            {
                int i;
                if (model == 0x11)
                    i = galileo_identify(data, offset);
                else
                    i = smart_identify(data, offset);
                return table[i];
            };

            while (offset < data.Length)
            {
                int offset0 = offset;

                SampleInfo sample = identify();

                // Skip the processed type bytes.
                offset += sample.ntypebits / NBITS;

                // Process the remaining data bits.
                int nbits = 0;
                uint value = 0;
                int n = sample.ntypebits % NBITS;
                if (n > 0)
                {
                    nbits = NBITS - n;
                    value = (uint)(data[offset] & (0xFF >> n));
                    if (sample.ignoretype)
                    {
                        // Ignore any data bits that are stored in
                        // the last type byte for certain samples.
                        nbits = 0;
                        value = 0;
                    }
                    offset++;
                }

                // Process the extra data bytes.
                for (int i = 0; i < sample.extrabytes; ++i)
                {
                    nbits += NBITS;
                    value <<= NBITS;
                    value += data[offset];
                    offset++;
                }

                // Fix the sign bit.
                int svalue = smart_fixsignbit(value, nbits);


                /*
                Console.WriteLine();
                Console.WriteLine("{0} {1}", sample.type, sample.absolute);

                byte[] buffer = new byte[offset - offset0];
                Array.Copy(data, offset0, buffer, 0, offset - offset0);

                Console.Write("  bin");
                for (int i = 0; i < buffer.Length; ++i)
                {
                    Console.Write(" ");
                    Console.Write(Convert.ToString(buffer[i], 2).PadLeft(8, '0'));
                }
                Console.Write("\n");
                Console.WriteLine("  value {0}", Convert.ToString(value, 2).PadLeft(32, '0'));
                Console.WriteLine("  value {0} {1}", value, svalue);
                */


                // Parse the value.
                switch (sample.type)
                {
                    case SampleType.PRESSURE_DEPTH:
                        pressure += ((sbyte)((svalue >> NBITS) & 0xFF)) / 4.0F;
                        depth += ((sbyte)(svalue & 0xFF)) / 50.0F;
                        complete = 1;
                        break;

                    case SampleType.RBT:
                        if (sample.absolute)
                        {
                            rbt = (int)value;
                            have_rbt = true;
                        }
                        else
                        {
                            rbt += svalue;
                        }
                        break;

                    case SampleType.TEMPERATURE:
                        if (sample.absolute)
                        {
                            temperature = value / 2.5F;
                            have_temperature = true;
                        }
                        else
                        {
                            temperature += svalue / 2.5F;
                        }
                        break;

                    case SampleType.PRESSURE:
                        if (sample.absolute)
                        {
                            tank = sample.index;
                            pressure = value / 4.0F;
                            have_pressure = true;
                        }
                        else
                        {
                            pressure += svalue / 4.0F;
                        }
                        break;

                    case SampleType.DEPTH:
                        if (sample.absolute)
                        {
                            depth = value / 50.0F;
                            if (!calibrated)
                            {
                                calibrated = true;
                                depth_calibration = depth;
                            }
                            have_depth = true;
                        }
                        else
                        {
                            depth += svalue / 50.0F;
                        }
                        complete = 1;
                        break;

                    case SampleType.HEARTRATE:
                        if (sample.absolute)
                        {
                            heartrate = (int)value;
                            have_heartrate = true;
                        }
                        else
                        {
                            heartrate += svalue;
                        }
                        break;

                    case SampleType.BEARING:
                        bearing = value;
                        have_bearing = true;
                        break;

                    case SampleType.ALARMS:
                        alarms[sample.index] = (byte)value;
                        have_alarms = true;
                        break;

                    case SampleType.TIME:
                        complete = value;
                        break;

                    default:
                        throw new Exception("Unknown sample type.");
                }


                while (complete != 0)
                {
                    if (have_temperature)
                    {

                    }

                    if (have_alarms)
                    {
                        Array.Clear(alarms, 0, alarms.Length);
                        have_alarms = false;
                    }

                    if (have_rbt || have_pressure)
                    {
                    }

                    if (have_pressure)
                    {
                    }

                    if (have_heartrate)
                    {
                    }

                    if (have_bearing)
                    {
                        have_bearing = false;
                    }

                    if (have_depth)
                    {
                        profile.Add(depth - depth_calibration);
                    }

                    time += 4;
                    complete--;
                }
            }

            return profile.ToArray();
        }
    }


    class parserLog
    {
        private static bool ByteArrayCompare(byte[] a1, int offset1, byte[] a2, int offset2, int length)
        {
            if (a1.Length < offset1 + length || a2.Length < offset2 + length)
                return false;

            for (int i = 0; i < length; ++i)
                if (a1[i + offset1] != a2[i + offset2])
                    return false;

            return true;
        }

        static byte[] Format92 = { 0x46, 0x6F, 0x72, 0x6D, 0x61, 0x74, 0x39, 0x32 };    // Format92    
        static int SmartSignature = 0x5a5aa5a5;

        static Headers headers = null;

        public parserLog()
        {
            if (headers == null)
            {
                using (TextReader reader = new StringReader(Resources.UwatecSmartHeaders))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Smart.Headers));

                    headers = serializer.Deserialize(reader) as Headers;
                }
            }
        }


        public DiveHeader parse(byte model, byte[] data)
        {
            DiveHeader info = new DiveHeader();

            info.Format = DiveHeader.LogFormat.None;
            info.DiveDataLength = 0;
            info.DiveStartTime = DateTime.MinValue;
            info.Values = new Dictionary<string, object>();

            if (data.Length == 10)
            {
                if (ByteArrayCompare(data, 2, Format92, 0, Format92.Length))
                {
                    info.Format = DiveHeader.LogFormat.Format92;
                }
            }
            else
            {
                var lhc = from i in headers.Items where i.model == model select i;
                if (lhc.Count() == 0)
                    return info;

                HeadersComputer hc = lhc.ElementAt(0);

                if (data.Length == hc.headerLength)
                {
                    UInt32 signature = BitConverter.ToUInt32(data, 0);

                    if (signature == SmartSignature)
                    {
                        info.Format = DiveHeader.LogFormat.Smart;

                        info.DiveDataLength = BitConverter.ToUInt32(data, 4);
                        UInt32 halfSeconds = BitConverter.ToUInt32(data, 8);
                        info.DiveStartTime = (new DateTime(2000, 1, 1, 0, 0, 0)).AddSeconds(halfSeconds / 2);

                        foreach (var h in hc.header)
                        {
                            object value = getValue(h, data);

#if UWATEC_SMART_VIEWER
                            /*
                            if (value == null)
                            {
                                value = BitConverter.ToString(data, h.offset, h.size);
                            }
                            else
                            */
                            {
                                string raw = BitConverter.ToString(data, h.offset, h.size);
                                string text;
                                if (value != null)
                                {
                                    text = value.ToString();
                                    if (h.unit != null) text += " " + h.unit;
                                }
                                else
                                {
                                    text = "(null)";
                                }

                                value = new object[] { value, raw, h };
                            }
#endif

                            if (value != null)
                            {

                                if (info.Values.ContainsKey(h.name))
                                    info.Values.Add(h.name + "_" + h.offset.ToString(), value);
                                else
                                    info.Values.Add(h.name, value);
                            }

                        }
                    }
                }
            }

            return info;
        }

        private object getValue(HeadersComputerHeader h, byte[] data)
        {
            // ignore les champs mal définis
            if (h.offset + h.size > data.Length) 
                return null;
             
            if (h.size == 1)
            {
                if (h.type == "sbyte")
                    return (sbyte)data[h.offset];
                else
                    return data[h.offset];
            }
            else if (h.size == 2)
            {
                UInt16 u = BitConverter.ToUInt16(data, h.offset);

                if (h.type == "float")
                {
                    float f = (float)u;

                    if (h.formula != "") 
                    {
                        try
                        {
                            SoftCircuits.Eval e = new SoftCircuits.Eval();

                            e.ProcessSymbol += new SoftCircuits.Eval.ProcessSymbolHandler(
                                (sender, ev) =>
                                {
                                    if (ev.Name == "value")
                                        ev.Result = f;
                                    else
                                        ev.Status = SoftCircuits.SymbolStatus.UndefinedSymbol;
                                });

                            f = (float)e.Execute(h.formula);
                        }
                        catch 
                        {
                        }
                    }

                    return f;
                }
                else if (h.type == "hh:mm")
                {
                    return string.Format("{0,2:00}:{1,2:00}", u / 60, u % 60);
                }
                else if (h.type == "hh:mm:ss")
                {
                    return string.Format("{0:00}:{1:00}:{2:00}", u / 3600, (u / 60) % 60, u % 60);
                }
                else
                {
                    return BitConverter.ToUInt16(data, h.offset);
                }
            }
            else if (h.size == 4)
            {
                return BitConverter.ToUInt32(data, h.offset);;
            }

            return null;
        }
    }


    public class logbook
    {
        public List<dive> Dives { get; private set; }

        private parserProfile profile = new parserProfile();
        private parserLog log = new parserLog();

        public void Open(string Filename)
        {
            OleDbConnection db = new OleDbConnection();

            OleDbConnectionStringBuilder dbs = new OleDbConnectionStringBuilder();

            dbs.Provider = "Microsoft.Jet.OLEDB.4.0";
            dbs.DataSource = Filename;
            dbs.Add("Jet OLEDB:Database Password", "gU6dokZ6sk9R");

            db.ConnectionString = dbs.ToString();
            db.Open();

            OleDbCommand cmd = new OleDbCommand(@"
select Dives.DiveNumber, Dives.Date, Dives.Immersion, Dives.DiveTime, 
       Site.Text as SiteText, 
       Dives.DiveComputerType, Dives.Log, Dives.Profile 
from Dives inner join Site on Site.Idx = Dives.SiteIdx
order by Dives.DiveNumber desc", db);
            OleDbDataReader dr = cmd.ExecuteReader();

            Dives = new List<dive>();

            while (dr.Read())
            {
                try
                {
                    if (dr["Log"] == System.DBNull.Value) continue;
                    if (dr["Profile"] == System.DBNull.Value) continue;

                    int model;
                    byte[] Log, Profile;

                    model = (int)dr["DiveComputerType"];

                    // bugged .slg
                    //if (model == 63) model += 256;      // Aladin Pro
                    //if (model == 0) model = 256;        // Format92 without computer (manually edited profile with WLog)

                    Log = (byte[])dr["Log"];
                    Profile = (byte[])dr["Profile"];

                    DiveHeader header = log.parse((byte)model, Log);

                    if (header.Format == DiveHeader.LogFormat.None)
                        continue;

                    dive d = new dive();

#if UWATEC_SMART_VIEWER
                    d.header = header;
                    d.rawHeader = Log;
                    d.model = model;
#endif

                    // general dive parameters
                    d.Site = dr["SiteText"] as string;
                    d.DiveNum = (int)dr["DiveNumber"];

                    DateTime xd, xt;

                    xt = (DateTime)dr["DiveTime"];
                    d.Duration = (xt.Hour * 60 + xt.Minute) * 60 + xt.Second;

                    xd = (DateTime)dr["Date"];
                    xt = (DateTime)dr["Immersion"];

                    d.StartTime = new DateTime(xd.Year, xd.Month, xd.Day, xt.Hour, xt.Minute, xt.Second);
                    
                    // the profile and the sample interval (always constant for Uwatec computer)
                    if (header.Format == DiveHeader.LogFormat.Smart)
                    {
                        d.Profile = profile.parse((byte)(model & 0xFF), Profile);
                        d.SampleInterval = 4;
                    }
                    else if (header.Format == DiveHeader.LogFormat.Format92)
                    {
                        d.SampleInterval = 20;

                        Aladin.DiveData data = new Aladin.DiveData();
                        data.ReadProfile(Profile, (byte)(model & 0xff));

                        d.Profile = new float[data.Profile.Length];
                        for (int i = 0; i < data.Profile.Length; ++i)
                        {
                            d.Profile[i] = data.Profile[i].Depth;
                        }
                    }

                    Dives.Add(d);
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.ToString());
                }
            }

            dr.Close();
        }         
    }

}
