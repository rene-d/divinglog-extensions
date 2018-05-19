// UwatecAladin.cs
// René DEVICHI 2011

// http://www.muenster.de/~matthias/aladin/datatrak_format_1_5.pdf 
// http://uwatec-uddf.svn.sourceforge.net

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace Uwatec.Aladin
{

    #region Uwatec constants
    enum Altitudes
    {
        None,                   // 0
        _0_900_m,               // 1
        _900_1750_m,            // 2
        _1750_2700_m,           // 3
        _2700_4000_m,           // 4
    }

    enum Suits
    {
        None,                   // 0
        NoSuit,                 // 1
        Shorty,
        OnePieceWetsuit,
        TwoPiecesWetsuit,
        SemiDrysuit,
        Drysuit,
        Unknown,                // 7
    }

    enum Weathers
    {
        None,                   // 0
        Clear,                  // 1
        Misty,
        Fog,
        Rain,
        Storm,
        Snow,
        Unknown,                // 7
    }

    enum DiveTypes
    {
        // first byte
        NoStop = 0x4,
        Decompression = 0x8,
        SingleAscent = 0x10,
        MultipleAscent = 0x20,
        Freshwater = 0x40,
        Seawater = 0x80,
        // second byte
        Nitrox = 0x100,
        Rebreather = 0x200,
    }


    enum Activities
    {
        // first byte
        Sightseeing = 0x1,
        ClubDive = 0x2,
        Education = 0x4,
        Instruction = 0x8,
        Night = 0x10,
        Cave = 0x20,
        Ice = 0x40,
        Search = 0x80,
        // second byte
        Wreck = 0x0100,             // Exploration ?
        River = 0x0200,
        Drift = 0x0400,
        Photo = 0x0800,
        Other = 0x1000,
    }

    enum Alarms
    {
        // first byte
        Ascent = 0x1,
        RepetitiveDive = 0x2,
        Alarm04 = 0x4,
        Decompression = 0x8,
        Work = 0x10,
        SOS = 0x20,
        Altitude2 = 0x40,            //  900 - 1750 m
        Altitude3 = 0x80,            // 1750 - 2700 m
        Altitude4 = 0xC0,            // 2700 - 4000 m
        // second byte
        Cold = 0x4000,
        Microbubble = 0x8000,
    }

    enum Errors
    {
        deco = 0x1,                         // bit 0
        rbt = 0x2,                          // bit 1: RBT Warnung
        ascent = 0x4,                       // bit 2: ↑ Warnung
        error = 0x8,                        // bit 3: ↓ Warnung
        breath = 0x10,                      // bit 4: ∩ Warnung
        AirTransmissionError = 0x20,        // bit 5: Physiologie Gefäß ??
    }
    #endregion


    internal class DiveData
    {
        internal struct ProfileData                  // a profile each 20 seconds
        {
            public float Depth;
            public Errors Errors;
        }

        internal struct DecompressionInfo            // each minute
        {
            private byte _Physiology;
            public byte Physiology { set { _Physiology = value; } }

            public byte O2Byte;

            // bits 6-4
            public byte PhysicalEffort
            {
                get
                {
                    return (byte)((_Physiology >> 5) & 7);
                }
            }

            // bit 7 or 3
            public string ColdLevel
            {
                get
                {
                    if ((_Physiology & 0x80) == 0x80)
                        return "Cold Level Decrement";
                    else if ((_Physiology & 0x8) == 0x8)
                        return "Cold Level Increment";
                    return string.Empty;
                }
            }

            // bits 2-0
            public byte MicrobubbleDanger
            {
                get
                {
                    return (byte)(_Physiology & 0x7);
                }
            }
        }

        public float AmbientTemp;           // byte 0                       
        public UInt16[] TissueSat;          // bytes 1 - 16
        private byte[] Microbubble;         // bytes 16 - 21

        public byte CNS;                    // if diveComputerType == 0xF? or 0xA?
        private byte ppO2_fO2;              // if diveComputerType == 0xF?

        public ProfileData[] Profile;
        public DecompressionInfo[] DecoInfo;

        public string ArterialMicrobubble
        {
            get
            {
                int micro_bubble = (Microbubble[1] & 0xf0) * 16 + Microbubble[0];

                if (micro_bubble <= 0x010) return "Level 0";
                if (micro_bubble <= 0x080) return "Level 1";
                if (micro_bubble <= 0x100) return "Level 2";
                if (micro_bubble <= 0x180) return "Level 3";
                if (micro_bubble <= 0x480) return "Level 4";
                if (micro_bubble <= 0x700) return "Level 5";
                if (micro_bubble <= 0xa00) return "Level 6";
                if (micro_bubble <= 0xfff) return "Level 7";
                return string.Empty;
            }
        }

        public int IntrapulmonaryRightLeftShunt
        {
            get
            {
                return (Microbubble[1] & 0x0f) * 256 + Microbubble[2];
            }
        }

        public string SkinCool
        {
            get
            {
                float skin_cool = ((Microbubble[4] & 0xf0) * 16 + Microbubble[3]) / 64;

                if (skin_cool >= 30.7) return "Level 0";
                if (skin_cool >= 28.0) return "Level 1";
                if (skin_cool >= 26.0) return "Level 2";
                if (skin_cool >= 24.0) return "Level 3";
                if (skin_cool >= 23.0) return "Level 4";
                if (skin_cool >= 22.0) return "Level 5";
                if (skin_cool >= 21.0) return "Level 6";
                return "Level 7";
            }
        }

        public float ppO2
        { get { return (float)(1.20 + ((ppO2_fO2 & 0xf0) / 16) * 0.05F); } }

        public int fO2
        { get { return Math.Max(21, 22 + ((ppO2_fO2 & 0x0f) - 1) * 2); } }

        private float getDepth(UInt16 data)
        {
            return ((data & 0xffc0) >> 6) * 10 / 64.0F;
        }

        private Errors getFlag(UInt16 data)
        {
            return (Errors)(data & 0x003f);
        }

        public void ReadProfile(byte[] data, byte ComputerType)
        {
            if (data.Length == 0)
                return;

            AmbientTemp = (float)(data[0] / 4.0);

            TissueSat = new UInt16[8];
            Microbubble = new byte[5];

            for (int i = 0; i < 8; ++i) TissueSat[i] = BitConverter.ToUInt16(data, 1 + i * 2);
            for (int i = 0; i < 5; ++i) Microbubble[i] = data[1 + 16 + i];

            int dataIndex = 1 + 16 + 5;

            if (((ComputerType & 0xf0) == 0xf0) || ((ComputerType & 0xf0) == 0xa0))
            {
                CNS = data[dataIndex++];

                if ((ComputerType & 0xf0) == 0xf0)
                {
                    ppO2_fO2 = data[dataIndex++];
                }
            }
            

            int samples;        // nombre de minutes (3 ProfileData + DecompressionInfo)
            int samples2;       // nombre de ProfileData supplémentaires (0 à 2)

            if ((ComputerType & 0xf0) == 0xa0)
            {
                samples = (data.Length - dataIndex) / (6 + 1 + 1);
                samples2 = (data.Length - dataIndex) % (6 + 1 + 1);
            }
            else
            {
                samples = (data.Length - dataIndex) / (6 + 1);
                samples2 = (data.Length - dataIndex) % (6 + 1);
            }

            Profile = new ProfileData[samples * 3 + samples2 / 2];
            DecoInfo = new DecompressionInfo[samples + 1];

            int k = 0;

            while (dataIndex < data.Length)
            {

                for (int i = 0; i < 3; ++i)
                {
                    if (dataIndex + 2 > data.Length)
                        break;

                    UInt16 u = (UInt16)(data[dataIndex] * 256 + data[dataIndex + 1]);

                    ProfileData p = new ProfileData();
                    p.Depth = getDepth(u);
                    p.Errors = getFlag(u);

                    Profile[k * 3 + i] = p;

                    dataIndex += 2;
                }

                if (dataIndex + 1 > data.Length)
                    break;

                DecompressionInfo di = new DecompressionInfo();
                di.Physiology = data[dataIndex++];

                if ((ComputerType & 0xf0) == 0xa0)
                {
                    if (dataIndex + 1 > data.Length)
                        break;

                    di.O2Byte = data[dataIndex++];
                }

                DecoInfo[k] = di;

                ++k;
            }
        }

    }



    class dive
    {
        public DateTime Date;
        public string Location;
        public string Site;
        public Altitudes Altitude;
        public UInt16 SurfaceInterval;          // in minutes
        public Weathers Weather;

        public UInt16 AirTemp;                  // in hundredth of °C
        public Suits Suit;
        public UInt16 TankSize;                 // in hundredth of litres, 32767 <=> unknown
        public UInt16 MaxDepth;                 // in hundredth of metres
        public UInt16 DiveTime;                 // in minutes
        public UInt16 MinWaterTemp;             // in hundredth of °C
        public UInt16 GasUsed;                  // in hundredth of bars, 32767 <=> unknown

        public DiveTypes DiveType;
        public Activities Activity;

        public string OtherActivities;
        public string Buddies;
        public string Remarks;

        public Alarms Alarms;

        public UInt16 Number;
        public UInt32 ComputerTimestamp;
        //public DateTime ComputerDate;

        public byte ComputerType;
        public byte AirUsageEntry;

        public DiveData profile;

        public void ReadDive(UwatecBinaryReader r)
        {
            UInt16 diveHeader = r.ReadUInt16();

            if (diveHeader != 0x00A0)
                throw new Exception("Unknown dive header");

            UInt32 dive_days = r.ReadUInt32();      // since 1600/01/01
            UInt16 dive_min = r.ReadUInt16();       // since 00:00

            Date = new DateTime(1600, 1, 1) + TimeSpan.FromDays(dive_days);
            if (dive_min != 32767) Date += TimeSpan.FromMinutes(dive_min);

            Location = r.ReadString();
            Site = r.ReadString();

            Altitude = (Altitudes)r.ReadByte();

            SurfaceInterval = r.ReadUInt16();
            Weather = (Weathers)r.ReadByte();

            AirTemp = r.ReadUInt16();
            Suit = (Suits)r.ReadByte();

            TankSize = r.ReadUInt16();
            MaxDepth = r.ReadUInt16();
            DiveTime = r.ReadUInt16();
            MinWaterTemp = r.ReadUInt16();
            GasUsed = r.ReadUInt16();

            DiveType = (DiveTypes)r.ReadUInt16_BigEndian();
            Activity = (Activities)r.ReadUInt16_BigEndian();

            OtherActivities = r.ReadString();
            Buddies = r.ReadString();
            Remarks = r.ReadString();

            Alarms = (Alarms)r.ReadUInt16_BigEndian();

            Number = r.ReadUInt16();

            ComputerTimestamp = r.ReadUInt32();
            //ComputerDate = new DateTime(1970, 1, 1) + TimeSpan.FromSeconds(ComputerTimestamp);

            ComputerType = r.ReadByte();
            AirUsageEntry = r.ReadByte();

            // SaturationTime
            r.ReadBytes(6);

            UInt16 ProfileLength = r.ReadUInt16();

            byte[] data = r.ReadBytes(ProfileLength);

            if (ProfileLength > 0)
            {
                profile = new DiveData();
                profile.ReadProfile(data, ComputerType);
            }
        }

        public string ComputerName
        {
            get
            {
                if (UwatecComputers.ContainsKey(ComputerType))
                {
                    return UwatecComputers[ComputerType];
                }
                else
                {
                    return string.Format("unknown type {0}", ComputerType);
                }
            }
        }

        static Dictionary<byte, string> UwatecComputers = new Dictionary<byte, string>()
            {
                { 0x00, "No Computer"},
                { 0x40, "Mares Genius"},
                { 0x34, "Aladin Air Z/X"},
                { 0x44, "Aladin Air Z/X"},
                { 0xa4, "Aladin Air Z/X O2"},
                { 0xf4, "Aladin Air Z/X Nitrox"},
                { 0x48, "Spiro Monitor 3 Air"},
                { 0x1c, "Aladin Air (Twin)"},
                { 0x1d, "Spiro Monitor 2 Plus"},
                { 0x3d, "Spiro Monitor 2 Plus"},
                { 0x1e, "Aladin Sport (Plus)"},
                { 0x3e, "Aladin Sport (Plus)"},
                { 0x1f, "Aladin Pro"},
                { 0x3f, "Aladin Pro"},
                { 0xff, "Aladin Pro Ultra/Nitrox"},
                { 0x1b, "AIRE Aladin Pro"},

                // computer id also present in Data Trak example logbook: 0x73 0x41 0xbc 0x14 0x24                
            };
    }


    class UwatecBinaryReader : BinaryReader
    {

        public UwatecBinaryReader(Stream stream)
            : base(stream, Encoding.GetEncoding(1252))
        {
        }

        public override string ReadString()
        {
            byte len = base.ReadByte();
            return new string(base.ReadChars(len));
        }

        public UInt16 ReadUInt16_BigEndian()
        {
            byte b0 = base.ReadByte();
            byte b1 = base.ReadByte();

            return (ushort)(b0 | (b1 << 8));
        }
    }


    class logbook
    {
        public UInt16 DiveCount { get; private set; }
        public List<dive> Dives { get; private set; }

        public void Open(string Filename)
        {
            Dives = null;            

            using (FileStream f = File.OpenRead(Filename))
            {
                using (UwatecBinaryReader r = new UwatecBinaryReader(f))
                {
                    UInt16 logHeader = r.ReadUInt16();
                    
                    if (logHeader != 0x00A1)
                        throw new Exception("Unknown file header");

                    UInt16 logSerial1 = r.ReadUInt16();
                    UInt16 logSerial2 = r.ReadUInt16();

                    DiveCount = r.ReadUInt16();
                    
                    r.ReadUInt32();       // unknown

                    Dives = new List<dive>(DiveCount);

                    for (int i = 0; i < DiveCount; ++i)
                    {
                        dive d = new dive();

                        d.ReadDive(r);
                        Dives.Add(d);
                    }
                }
            }
        }
    }

}
