using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;
using System.Drawing.Imaging;

namespace Exif
{
    public class Exif_Base
    {

        Bitmap bitmap2;

        //MimeTypeで指定されたImageCodecInfoを探して返す
        private static System.Drawing.Imaging.ImageCodecInfo GetEncoderInfo(string mineType)
        {
            //GDI+ に組み込まれたイメージ エンコーダに関する情報をすべて取得
            System.Drawing.Imaging.ImageCodecInfo[] encs = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
            //指定されたMimeTypeを探して見つかれば返す
            foreach (System.Drawing.Imaging.ImageCodecInfo enc in encs)
                if (enc.MimeType == mineType)
                    return enc;
            return null;
        }

        // GPS タグのバージョン 0x00
        private string GPSVersionID
        {
            /*            
            Tag 0x0000 
            Type PropertyTagTypeByte :1
            Count 4 
            */
            get
            {
                int id = 0x0000;
                int[] pils = bitmap2.PropertyIdList;

                //Exif情報取得
                int index = Array.IndexOf(pils, id);
                if (index == -1)
                    return "-1";

                PropertyItem pi = bitmap2.PropertyItems[index];

                //TYPE1
                pi = bitmap2.PropertyItems[index];

                string Ver = "";
                for (int i = 0; i < pi.Len; i++)
                {
                    Ver = Ver + pi.Value[i].ToString() + ".";
                }

                return Ver;
            }
        }


        /// <summary>
        /// 北緯(N) or 南緯(S)を返す 0x01
        /// エラー:-1
        /// </summary>
        /// <returns></returns>
        private string GPSLatitudeRef()
        {
            /* 北緯・南緯取得
            Tag 0x0001 
            Type PropertyTagTypeASCII   type2
            Count 2 (one character plus the NULL terminator) 
            */

            //返値　"N" "S" "-1"
            string ret_decode_exifid;
            if ((ret_decode_exifid = decode_exifid(bitmap2, 0x01)) != "-1")
                return ret_decode_exifid;

            return "-1";
        }

        private string LatitudeRef = "";
        /// <summary>
        /// 北緯:N 南緯:S
        /// </summary>
        public string 北緯南緯
        {
            set { this.LatitudeRef = value; }
            get { return this.GPSLatitudeRef(); }
        }


        /// <summary>
        /// 緯度(度)取得  0x02
        /// エラー:-1
        /// </summary>
        /// <returns></returns>
        private string GPSLatitude()
        {
            /* 緯度取得
            Tag 0x0002 
            Type PropertyTagTypeRational type5
            Count 3   len=24  緯度（度、分、秒）
             ExifにGPSデータを記録する時は、度数分（dms）形式にする
            */
            string latitude = "";

            int index;
            PropertyItem pi;
            int[] pils = bitmap2.PropertyIdList;

            index = Array.IndexOf(pils, 0x02);
            if (index == -1)
                return "-1";

            pi = bitmap2.PropertyItems[index];

            // MessageBox.Show(pi.Id.ToString(),"pi.id"); //"pi.id"=Tag

            //   MessageBox.Show("" + BitConverter.ToString(pi.Value), "Lati位置");

            double deg = BitConverter.ToUInt32(pi.Value, 0);
            uint deg_div = BitConverter.ToUInt32(pi.Value, 4);

            double min = BitConverter.ToUInt32(pi.Value, 8);
            uint min_div = BitConverter.ToUInt32(pi.Value, 12);

            double mmm = BitConverter.ToUInt32(pi.Value, 16);
            uint mmm_div = BitConverter.ToUInt32(pi.Value, 20);

            double m = 0;
            if (deg_div != 0 || deg != 0)    //度分秒で入っているので、度に統一する
            {
                m = (deg / deg_div);
            }

            if (min_div != 0 || min != 0)
            {
                m = m + (min / min_div) / 60;
            }

            if (mmm_div != 0 || mmm != 0)
            {
                m = m + (mmm / mmm_div / 3600);
            }
            latitude = m.ToString();

            //   MessageBox.Show(deg_div.ToString(), "度分数");

            return latitude;
        }

        private string lat = "";
        /// <summary>
        /// 緯度を10進表記 0..90
        /// </summary>
        public string 緯度
        {
            set { this.lat = value; }
            get { return this.GPSLatitude(); }
        }



        /// <summary>
        /// 東経(E) or 西経(W)を返す 0x03
        /// エラー:-1
        /// </summary>
        /// <returns></returns>
        private string GPSLongitudeRef()
        {
            //返値　"E" "W" "-1"
            string ret_decode_exifid;
            if ((ret_decode_exifid = decode_exifid(bitmap2, 0x03)) != "-1")
                return ret_decode_exifid;

            return "-1";
        }

        private string LongitudeRef = "";
        /// <summary>
        /// 東経:W 西経:E
        /// </summary>
        public string 東経西経
        {
            set { this.LongitudeRef = value; }
            get { return this.GPSLongitudeRef(); }
        }


        /// <summary>
        /// 経度 (数値) 0x04
        /// Error:-1
        /// </summary>
        /// <returns></returns>
        private string GPSLongitude()
        {
            /* 経度取得
         Tag 0x0004
         Type PropertyTagTypeRational type5 (pi.Type == 5)
         Count 3   len=24 経度（度、分、秒）
         */

            int index;
            PropertyItem pi;
            int[] pils = bitmap2.PropertyIdList;
            string Longitude = "";

            // pils : PropertyIdList に格納されているリスト一覧　のTag:0x04番を indexに取得という意味
            index = Array.IndexOf(pils, 0x04);
            if (index == -1)
                return "-1";    //値が無い場合

            pi = bitmap2.PropertyItems[index];

            // MessageBox.Show(pi.Id.ToString(),"pi.id");


            //で pi.Type:PropertyTagTypeRational形式で値が格納される
            //  MessageBox.Show("pi.Type:"+pi.Type.ToString(),"pi type:5(PropertyTagTypeRational)");

            //     pi.Valueに値がそれぞれの形式で格納される
            //      MessageBox.Show("pi.Value:" + pi.Value.ToString(), "pi.Value");


            //   MessageBox.Show("" + BitConverter.ToString(pi.Value), "Lati位置");

            //unsigned long integers

            double deg = BitConverter.ToUInt32(pi.Value, 0);
            uint deg_div = BitConverter.ToUInt32(pi.Value, 4);

            double min = BitConverter.ToUInt32(pi.Value, 8);
            uint min_div = BitConverter.ToUInt32(pi.Value, 12);

            double mmm = BitConverter.ToUInt32(pi.Value, 16);
            uint mmm_div = BitConverter.ToUInt32(pi.Value, 20);

            double m = 0;
            if (deg_div != 0 || deg != 0)    //36.26.0140
            {
                m = (deg / deg_div);
            }

            if (min_div != 0 || min != 0)
            {
                m = m + (min / min_div) / 60;
            }

            if (mmm_div != 0 || mmm != 0)    //36.26.0140
            {
                m = m + (mmm / mmm_div / 3600);
            }

            Longitude = m.ToString();

            return Longitude;
        }

        private string lon = "";
        /// <summary>
        /// 経度 0..180
        /// </summary>
        public string 経度
        {
            set { this.lon = value; }
            get { return this.GPSLongitude(); }
        }


        /// <summary>
        /// 高度取得 (m) 0x06
        /// Error:-1
        /// </summary>
        /// <returns></returns>
        private string GPSAltitude()
        {
            /* 高度取得
         Tag 0x0006
         Type PropertyTagTypeRational type5
         Count 1  
             */

            int index;
            PropertyItem pi;
            int[] pils = bitmap2.PropertyIdList;

            index = Array.IndexOf(pils, 0x0006);
            if (index == -1)
                return "-1";

            pi = bitmap2.PropertyItems[index];

            double deg = BitConverter.ToUInt32(pi.Value, 0);
            uint deg_div = BitConverter.ToUInt32(pi.Value, 4);

            double m = 0;
            if (deg_div != 0 || deg != 0)
            {
                m = (deg / deg_div);
            }

            return m.ToString();
        }

        private string alt = "";
        /// <summary>
        /// 高度 単位は 0.00(M)
        /// </summary>
        public string 高度
        {
            set { this.alt = value; }
            get { return this.GPSAltitude(); }
        }


        /// <summary>
        /// GPS 時間 (原子時計の時間) 0x07
        /// </summary>
        /// <returns></returns>
        private string GPSTimeStamp()
        {
            /* 経度取得        
                PropertyTagGpsGpsTime
                Time as coordinated universal time (UTC). The value is expressed as three rational numbers that give the hour, minute, and second.
                Tag 0x0007 
                Type PropertyTagTypeRational 
                Count 3 
             */

            int index;
            PropertyItem pi;
            int[] pils = bitmap2.PropertyIdList;
            string TimeStamp = "";

            index = Array.IndexOf(pils, 0x07);

            if (index == -1)    //データが無い場合
                return "-1";

            pi = bitmap2.PropertyItems[index];

            for (int i = 0; i < pi.Len - 1; i++)
            {
                TimeStamp = TimeStamp + pi.Value[i].ToString();
            }

            return TimeStamp;
        }

        /// <summary>
        /// 測地系を文字列で返す 0x18
        /// </summary>
        /// <returns></returns>
        private string GpsMapDatum()
        {
            try
            {
                //TYPE2
                int index;
                PropertyItem pi;
                int[] pils = bitmap2.PropertyIdList;

                index = Array.IndexOf(pils, 0x12);
                pi = bitmap2.PropertyItems[index];

                return Encoding.ASCII.GetString(pi.Value, 0, pi.Len - 1);
            }
            catch
            {
                return "-1";
            }
        }

        private string MapDatum = "";

        /// <summary>
        /// 測地系を返す ex."TOKYO"
        /// </summary>
        public string 測地系
        {
            set { this.MapDatum = value; }
            get { return this.GpsMapDatum(); }
        }

        //---設定
        private void Exif_SetStrings(int tag, string NewDescription)
        {

            NewDescription = NewDescription + "\0";

            byte[] bDescription = Encoding.ASCII.GetBytes(NewDescription);

            PropertyItem[] PropertyItems;
            PropertyItems = bitmap2.PropertyItems;
            PropertyItems[0].Id = tag; // 0x010e as specified in EXIF standard
            PropertyItems[0].Type = 2;
            PropertyItems[0].Len = NewDescription.Length;
            PropertyItems[0].Value = bDescription;
            bitmap2.SetPropertyItem(PropertyItems[0]);
        }

        //漢字を使う場合はこちらにしている(暫定でutf-8(unicode)保存)
        private void Exif_SetStringsSjis(int tag, string NewDescription)
        {
            /*
             * string str;
                //Shift JISとして文字列に変換
                str = System.Text.Encoding.GetEncoding(932).GetString(bytesData);

                //JISとして変換
                str = System.Text.Encoding.GetEncoding(50220).GetString(bytesData);

                //EUCとして変換
                str = System.Text.Encoding.GetEncoding(51932).GetString(bytesData);

                //UTF-8として変換
                str = System.Text.Encoding.UTF8.GetString(bytesData);        
            */

            /* Flickr NG カシミール3D(sjis/utf-16) OK 
            Encoding sjis = Encoding.GetEncoding("shift-jis");
            byte[] bstr_str = sjis.GetBytes(NewDescription);
            byte[] bstr = { 0x4A, 0x49, 0x53, 0x00, 0x00, 0x00, 0x00, 0x00 };
            */
            //  byte[] bstr = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };// undefined

            //Flickr は UTF-8 Unicodeしか受け付けないutf-16はNG
            //但し先頭の8文字は別にUnidode指定でなくてもデフォルトでUnicodeとしているふしがあるので特に入れなくてもよさそう

            Encoding unicode = Encoding.GetEncoding("utf-8");
            NewDescription = NewDescription + "\0";
            byte[] bstr_str = unicode.GetBytes(NewDescription);

            NewDescription = "456";
            byte[] bstr = { 0x55, 0x4e, 0x49, 0x43, 0x4f, 0x44, 0x45, 0x00 }; //Unicode

            // bstrにbstr_strを接続する
            int len = bstr.Length;
            Array.Resize(ref bstr, len + bstr_str.Length);
            Array.Copy(bstr_str, 0, bstr, len, bstr_str.Length);

            PropertyItem[] PropertyItems;
            PropertyItems = bitmap2.PropertyItems;
            PropertyItems[0].Id = tag; // 0x010e as specified in EXIF standard
            PropertyItems[0].Type = 2;
            PropertyItems[0].Len = bstr.Length;
            PropertyItems[0].Value = bstr;
            bitmap2.SetPropertyItem(PropertyItems[0]);
        }

        // 0x10E 画像タイトル 2Byte文字列は使わない(規格上)
        private void SetImageDescription(string str)
        {
            //932    shift_jis                 
            //50220  iso-2022-jp    JIS            

            //    Exif_SetStrings(0x10E, str);    //規格どおり　ASCIIオンリーの場合はこちら
            //    Exif_SetStringsSjis(0x10E, str); //CommentのようにUnicode+Strの場合はこちら

            //規格に違反して２バイト文字をUnicodeで埋め込むの場合はこちら
            //Flickr は UTF-8 Unicodeしか受け付けないutf-16はNG
            //       Encoding unicode = Encoding.GetEncoding("utf-8");
            Encoding unicode = Encoding.GetEncoding("utf-8");

            byte[] bstr = unicode.GetBytes(str);

            PropertyItem[] PropertyItems;
            PropertyItems = bitmap2.PropertyItems;
            PropertyItems[0].Id = 0x10e; // 0x010e as specified in EXIF standard
            PropertyItems[0].Type = 2;
            PropertyItems[0].Len = bstr.Length;
            PropertyItems[0].Value = bstr;
            bitmap2.SetPropertyItem(PropertyItems[0]);

        }

        //0x9286 コメント
        private void SetUserComment(string str)
        {
            Exif_SetStringsSjis(0x9286, str);
        }

        //0x9000 Exif バージョン
        private void SetExifVersion(string Exifバージョン)
        {
            //   string Exifバージョン = "0210";

            PropertyItem[] PropertyItems;
            PropertyItems = bitmap2.PropertyItems;

            byte[] ExifバージョンDescription = Encoding.ASCII.GetBytes(Exifバージョン);

            PropertyItems[0].Id = 0x9000; // WGSとか
            PropertyItems[0].Type = 7;
            PropertyItems[0].Len = Exifバージョン.Length;
            PropertyItems[0].Value = ExifバージョンDescription;
            bitmap2.SetPropertyItem(PropertyItems[0]);
        }



        //画像保存 品質を100に固定している
        private bool Save(string filename)
        {
            try
            {
                //JPGの時は圧縮せずにExif追加
                if (filename.ToLower().EndsWith(".jpg"))
                {
                    //Exifのみ追加
                    bitmap2.Save(filename);
                }
                else
                {
                    //JPG以外の時はクオリティ100%で圧縮してExif追加 JPG化
                    int QV = 100;  //品質を指定
                    EncoderParameters enp = new EncoderParameters(1);
                    System.Drawing.Imaging.ImageCodecInfo ici;
                    ici = GetEncoderInfo("image/jpeg");

                    enp.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, QV);
                    bitmap2.Save(filename, ici, enp);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        //画像保存
        private bool Save(string filename, int QV)
        {
            if (QV < 0 || QV > 100)
            {
                return false;
            }

            try
            {
                if (QV == 100 || QV == 0)
                {
                    //Exifのみ追加
                    bitmap2.Save(filename);
                }
                else
                {
                    EncoderParameters enp = new EncoderParameters(1);
                    System.Drawing.Imaging.ImageCodecInfo ici;
                    ici = GetEncoderInfo("image/jpeg");

                    enp.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, QV);
                    bitmap2.Save(filename, ici, enp);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        //度から度.分.秒.秒秒へ変換
        private string deg2dms(string deg2)
        {
            double deg = double.Parse(deg2);
            double sf = Math.Round(deg * 360000);       //秒秒
            double s = Math.Floor(sf / 100) % 60;       //秒
            double m = Math.Floor(sf / 6000) % 60;      //分
            double d = Math.Floor(sf / 360000);          //度

            //       MessageBox.Show(sf.ToString(),"sf");

            sf %= 100;
            string mm = m.ToString();
            string ss = s.ToString();
            string ssf = sf.ToString();

            //      MessageBox.Show(ssf,"ssf");

            if (m < 10) { mm = "0" + m.ToString(); }
            if (s < 10) { ss = "0" + s.ToString(); }
            if (sf < 10) { ssf = "0" + sf.ToString(); }
            string dms = "" + d.ToString() + "." + mm + "." + ss + "." + ssf;
            return dms;
        }


        private void SetGPSInfo()
        {
            ulong b = 1234;
            PropertyItem[] PropertyItems;
            PropertyItems = bitmap2.PropertyItems;

            PropertyItems[0].Id = 0x8825; // 0x0110 as ModelName
            PropertyItems[0].Type = 4; //4;
            PropertyItems[0].Len = 2; // 4;
            PropertyItems[0].Value = BitConverter.GetBytes(b);   //intをバイト列へ
            bitmap2.SetPropertyItem(PropertyItems[0]);

        }

        //測地形の設定
        private void SetGPSMapDatum(string NewDescription)
        {
            //測地系
            /*
            if (!NewDescription.Equals("TOKYO"))
            {
                NewDescription = "WGS84";
            }
            */
            NewDescription = NewDescription + "\0";

            byte[] bDescription2 = Encoding.ASCII.GetBytes(NewDescription);
            PropertyItem[] PropertyItems;
            PropertyItems = bitmap2.PropertyItems;

            PropertyItems[0].Id = 0x12; // WGSとか
            PropertyItems[0].Type = 2;
            PropertyItems[0].Len = NewDescription.Length;
            PropertyItems[0].Value = bDescription2;
            bitmap2.SetPropertyItem(PropertyItems[0]);

        }

        //北緯・南緯 0x01
        private void SetGPSLatitudeRef(string NS)
        {
            if (!NS.Equals("S"))
            {
                NS = "N";
            }

            NS = NS + "\0\0\0";//本来ならNULL１つで良いが、後に得体の知れん文字列が並ぶようなので埋め

            byte[] bDescription2 = Encoding.ASCII.GetBytes(NS);

            PropertyItem[] PropertyItems;
            PropertyItems = bitmap2.PropertyItems;
            PropertyItems[0].Id = 0x01; // 
            PropertyItems[0].Type = 2;
            //  PropertyItems[0].Len = NS.Length;
            //  PropertyItems[0].Len = bDescription2.Length;
            PropertyItems[0].Len = 4;
            PropertyItems[0].Value = bDescription2;
            bitmap2.SetPropertyItem(PropertyItems[0]);


        }

        //緯度 0x02
        private void SetGpsLatitude(string 度のみ)
        {
            度のみ = 度のみ.Replace("-", "");
            string a = deg2dms(度のみ);
            string[] words;
            words = a.Split('.');

            words[2] = words[2] + words[3];
            //     MessageBox.Show(words[2], "words[2]");
            UInt32 度 = UInt32.Parse(words[0]);
            byte[] 度度 = BitConverter.GetBytes(度);
            UInt32 度分数 = 1;
            byte[] 度度分数 = BitConverter.GetBytes(度分数);

            UInt32 分 = UInt32.Parse(words[1]);
            byte[] 分分 = BitConverter.GetBytes(分);
            UInt32 分分数 = 1;
            byte[] 分分分数 = BitConverter.GetBytes(分分数);

            UInt32 秒 = UInt32.Parse(words[2]);
            byte[] 秒秒 = BitConverter.GetBytes(秒);
            UInt32 秒秒数 = 100;
            byte[] 秒秒分数 = BitConverter.GetBytes(秒秒数);



            byte[] まとめ = { 度度[0], 度度[1], 度度[2], 度度[3], 度度分数[0], 度度分数[1], 度度分数[2], 度度分数[3], 分分[0], 分分[1], 分分[2], 分分[3], 分分分数[0], 分分分数[1], 分分分数[2], 分分分数[3], 秒秒[0], 秒秒[1], 秒秒[2], 秒秒[3], 秒秒分数[0], 秒秒分数[1], 秒秒分数[2], 秒秒分数[3] };

            PropertyItem[] PropertyItems;
            PropertyItems = bitmap2.PropertyItems;

            PropertyItems[0].Id = 0x02;
            PropertyItems[0].Type = 5;
            PropertyItems[0].Len = 24;
            PropertyItems[0].Value = まとめ;   //バイト列で放り込む必要あり
            bitmap2.SetPropertyItem(PropertyItems[0]);

        }

        //東経・西経 0x03
        private void SetGPSLongitudeRef(string EW)
        {
            if (!EW.Equals("W"))
            {
                EW = "E";
            }

            //EW = EW + "\0";
            EW = EW + "\0\0\0"; //本来ならNULL１つで良いが、後に得体の知れん文字列が並ぶようなので埋め

            byte[] bDescription2 = Encoding.ASCII.GetBytes(EW);

            PropertyItem[] PropertyItems;
            PropertyItems = bitmap2.PropertyItems;

            PropertyItems[0].Id = 0x03; // 東経/西経別
            PropertyItems[0].Type = 2;
            //    PropertyItems[0].Len = EW.Length;
            PropertyItems[0].Len = 4;
            PropertyItems[0].Value = bDescription2;
            bitmap2.SetPropertyItem(PropertyItems[0]);
        }

        //経度 0x04
        private void SetGpsLongitude(string 度のみ)
        {
            度のみ = 度のみ.Replace("-", "");
            string a = deg2dms(度のみ);
            string[] words;
            words = a.Split('.');

            words[2] = words[2] + words[3];

            UInt32 度 = UInt32.Parse(words[0]);
            byte[] 度度 = BitConverter.GetBytes(度);
            UInt32 度分数 = 1;
            byte[] 度度分数 = BitConverter.GetBytes(度分数);

            UInt32 分 = UInt32.Parse(words[1]);
            byte[] 分分 = BitConverter.GetBytes(分);
            UInt32 分分数 = 1;
            byte[] 分分分数 = BitConverter.GetBytes(分分数);

            UInt32 秒 = UInt32.Parse(words[2]);
            byte[] 秒秒 = BitConverter.GetBytes(秒);
            UInt32 秒秒数 = 100;
            byte[] 秒秒分数 = BitConverter.GetBytes(秒秒数);

            byte[] まとめ2 = { 度度[0], 度度[1], 度度[2], 度度[3], 度度分数[0], 度度分数[1], 度度分数[2], 度度分数[3], 分分[0], 分分[1], 分分[2], 分分[3], 分分分数[0], 分分分数[1], 分分分数[2], 分分分数[3], 秒秒[0], 秒秒[1], 秒秒[2], 秒秒[3], 秒秒分数[0], 秒秒分数[1], 秒秒分数[2], 秒秒分数[3] };

            PropertyItem[] PropertyItems;
            PropertyItems = bitmap2.PropertyItems;

            PropertyItems[0].Id = 0x04;
            PropertyItems[0].Type = 5;
            PropertyItems[0].Len = 24;
            PropertyItems[0].Value = まとめ2;   //バイト列で放り込む必要あり
            bitmap2.SetPropertyItem(PropertyItems[0]);

        }

        //高度の基準 0x05 値は0の海抜高度しかない Type=1
        private void SetGpsAltitudeRef()
        {
            byte[] bDescription2 ={ 0 };

            PropertyItem[] PropertyItems;
            PropertyItems = bitmap2.PropertyItems;

            PropertyItems[0].Id = 0x05; // GPSタグのバージョン
            PropertyItems[0].Type = 1;
            PropertyItems[0].Len = bDescription2.Length;
            PropertyItems[0].Value = bDescription2;
            bitmap2.SetPropertyItem(PropertyItems[0]);

        }

        //高度 0x06; //小数点第一位で10cm
        private void SetGPSAltitude(string 高度文字列)
        {
            string[] words;
            UInt32 高度分子 = 1;

            if (高度文字列.Contains("."))
            {
                words = 高度文字列.Split('.');
                int size = words[1].Length;

                if (size == 1)  //小数点第一 最小10cm
                {
                    高度分子 = 10;
                }

                if (size == 2)  //小数点第一 最小1cm
                {
                    //本来なら　100で1cm単位だが他のソフトが反応しないので10cmに丸めている
                    高度分子 = 100;
                }

                if (double.Parse(高度文字列) < 1.0 && size == 1)
                {
                    高度分子 = 10;
                }

                if (double.Parse(高度文字列) < 1.0 && size == 2)
                {
                    高度分子 = 100;
                }

                //高度文字列切り捨て処理

                /*
                int index = 高度文字列.IndexOf(".");
                高度文字列 = 高度文字列.Substring(0, index + 1);
                */

                //   MessageBox.Show(高度分子.ToString(), "高度分子");
                高度文字列 = 高度文字列.Replace(".", "");
            }

            if (高度文字列.Length > 7)
            {
                高度文字列 = "0";
            }

            UInt32 高度 = UInt32.Parse(高度文字列);

            byte[] 高度度 = BitConverter.GetBytes(高度);

            //小数点以下を扱う場合は　exp. 205.5mの場合 入力で2055 分子を10に指定する
            byte[] 高度度分子 = BitConverter.GetBytes(高度分子);

            byte[] 高度まとめ = { 高度度[0], 高度度[1], 高度度[2], 高度度[3], 高度度分子[0], 高度度分子[1], 高度度分子[2], 高度度分子[3] };

            PropertyItem[] PropertyItems;
            PropertyItems = bitmap2.PropertyItems;

            PropertyItems[0].Id = 0x06;
            PropertyItems[0].Type = 5;
            PropertyItems[0].Len = 8;
            PropertyItems[0].Value = 高度まとめ;   //バイト列で放り込む必要あり
            bitmap2.SetPropertyItem(PropertyItems[0]);

        }

        //撮影した画像の方向単位 0x10
        private void SetGPSImgDirectionRef(string Ref)
        {
            if (!Ref.Equals("T"))
            {
                Ref = "M";
            }

            Ref = Ref + "\0";

            byte[] bDescription2 = Encoding.ASCII.GetBytes(Ref);

            PropertyItem[] PropertyItems;
            PropertyItems = bitmap2.PropertyItems;

            PropertyItems[0].Id = 0x10; // 真方位:T/M:磁方位
            PropertyItems[0].Type = 2;
            PropertyItems[0].Len = Ref.Length;
            PropertyItems[0].Value = bDescription2;
            bitmap2.SetPropertyItem(PropertyItems[0]);
        }

        //撮影した画像の方向（0.00～359.99） 0x11
        private void SetGPSImgDirection(string D)
        {
            /*
                撮影した画像の方向（数値） G P S I m g D i r e c t i o n
                記録した画像の撮影方向を示す。値は0.00～359.99 までの範囲をとる。
                Tag = 17 （11.H）
                Type = RATIONAL
                Count = 1
                Default = なし
             */
            string[] words;
            UInt32 分子 = 1;

            if (D.Contains("."))
            {
                words = D.Split('.');
                int size = words[1].Length;

                if (size == 1)  //小数点第一 最小10cm
                {
                    分子 = 10;
                }

                if (size == 2)  //小数点第一 最小1cm
                {
                    分子 = 100;
                }

                //    MessageBox.Show(分子.ToString(), "高度分子");
                D = D.Replace(".", "");
            }


            byte[] 方向 = BitConverter.GetBytes(UInt32.Parse(D));
            byte[] 方向分子 = BitConverter.GetBytes(分子);

            byte[] 方向まとめ = { 方向[0], 方向[1], 方向[2], 方向[3], 方向分子[0], 方向分子[1], 方向分子[2], 方向分子[3] };


            PropertyItem[] PropertyItems;
            PropertyItems = bitmap2.PropertyItems;

            PropertyItems[0].Id = 0x11;
            PropertyItems[0].Type = 5;
            PropertyItems[0].Len = 8;
            PropertyItems[0].Value = 方向まとめ;   //バイト列で放り込む必要あり
            bitmap2.SetPropertyItem(PropertyItems[0]);

        }

        //GPS タグのバージョン そのうち変えるようにする
        private void SetGPSVersionID()
        {
            /*
            Encoding sjisEnc = Encoding.GetEncoding("Shift_JIS");
            byte[] bDescription2 = sjisEnc.GetBytes(Ver);
            */

            byte[] bDescription2 ={ 0x02, 0x00, 0x00, 0x00 };

            PropertyItem[] PropertyItems;
            PropertyItems = bitmap2.PropertyItems;

            PropertyItems[0].Id = 0x0000; // GPSタグのバージョン
            PropertyItems[0].Type = 1;
            PropertyItems[0].Len = bDescription2.Length;
            PropertyItems[0].Value = bDescription2;
            bitmap2.SetPropertyItem(PropertyItems[0]);
        }


        /// <summary>
        /// Exifが存在していれば True
        /// </summary>
        public bool Exif存在
        {
            get
            {
                try
                {
                    int[] pils = bitmap2.PropertyIdList;

                    if (pils.Length < 3)    //2以下Exifが無いとする簡易判断
                    {
                        return false;
                    }
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        private void GPSデータ初期化()
        {
            タイトル = "";
            コメント = "";

            緯度 = "";
            経度 = "";
            高度 = "";
            測地系 = "WGS84";
            撮影方位基準 = "";
            撮影方向 = "";
            北緯南緯 = "N";
            東経西経 = "E";
            SaveQuality = 100;

            /*
            メーカー = "";
            カメラ = "";
            撮影著作者 = "";
            編集著作者 = "";
            */
        }


        /// <summary>
        /// 必要な内容はプロパティで設定する
        /// </summary>
        /// <param name="ファイル名"></param>
        /// <returns></returns>
        public bool Exif保存(string ファイル名)
        {
            if (ファイル名.Equals(""))
            {
                return false;
            }

            //撮影方位単位が無ければ方向は入れない
            if (ImgDirectionRef.Equals("M") || ImgDirectionRef.Equals("T"))
            {
                SetGPSImgDirectionRef(ImgDirectionRef);

                if (!ImgDirection.Equals(""))
                {
                    SetGPSImgDirection(ImgDirection);
                }
            }


            if (!コメント.Equals(""))
            {
                SetUserComment(コメント);
            }

            if (!タイトル.Equals(""))
            {
                SetImageDescription(タイトル);
            }


            SetExifVersion("0210");             //JPG/PNGはOKだか、その他(BMP/GIF)が通らん

            //     SetGPSInfo();
            SetGPSVersionID();  //GPS タグのバージョン

            SetGPSMapDatum(MapDatum);  //測地系 デフォルトで WGS84

            if (this.lat.Contains("-"))
            {
                LatitudeRef = "S";
                this.lat = this.lat.Replace("-", "");
            }

            if (this.lon.Contains("-"))
            {
                LongitudeRef = "W";
                this.lon = this.lon.Replace("-", "");
            }

            if (!this.lat.Equals(""))
            {
                SetGpsLatitude(this.lat);
            }

            if (!this.lon.Equals(""))
            {
                SetGpsLongitude(this.lon);
            }


            SetGPSLatitudeRef(LatitudeRef);    //北緯・南緯 "N or S"
            SetGPSLongitudeRef(LongitudeRef);   //東経・西経 E or W
            SetGpsAltitudeRef();            //海抜高度  海抜高度以外の設定は無い

            if (!alt.Equals(""))
            {
                SetGPSAltitude(alt);
            }


            //DateTime変更 変更日時
            if (!Datetime.ToString().Equals("0001/01/01 0:00:00"))
            {
                SetDateTime(Datetime);
            }

            //DateTimeOriginal 撮影日時
            if (!datetimeoriginal.ToString().Equals("0001/01/01 0:00:00"))
            {
                SetDateTimeOriginal(datetimeoriginal);
            }

            //DateTimeDigitized ディジタルデータ作成日時
            if (!datetimedigitized.ToString().Equals("0001/01/01 0:00:00"))
            {
                SetDateTimeDigitized(datetimedigitized);
            }

            if (SaveQuality.Equals(""))
            {
                Save(ファイル名);
            }
            else
            {
                Save(ファイル名, SaveQuality);
            }


            GPSデータ初期化();
            return true;

        }

        //Exif情報を解釈する
        private string decode_exifid(Bitmap bitmap, int id)
        {
            try
            {
                int[] pils = bitmap.PropertyIdList;
                int index = Array.IndexOf(pils, id);

                if (index == -1)
                    return "-1";

                //Exif情報取得
                PropertyItem pi = bitmap.PropertyItems[index];


                /* typeの対応表
            
                    $BYTE      =  1;
                    $ASCII     =  2;
                    $SHORT     =  3;
                    $LONG      =  4;
                    $RATIONAL  =  5;
                    $SBYTE     =  6;
                    $UNDEFINED =  7;
                    $SSHORT    =  8;
                    $SLONG     =  9;
                    $SRATIONAL = 10;
                    $FLOAT     = 11;
                    $DOUBLE    = 12;
             タイプ 名前  	サイズ(byte)  	内容
                1 	BYTE 	    1 	        8ビット符合なし整数
                2 	ASCII 	    ANY	        ASCII文字の集合'0.H'で終端
                3 	SHORT 	    2 	        16ビット符合なし整数
                4 	LONG 	    4 	        32ビット符合なし整数
                5 	RATIONAL 	8 	        LONG２つ、分子／分母 有理数を表す 2 つの Byte オブジェクトの配列

                7 	UNDEFINED 	ANY	        未定義のバイト列
                9 	SLONG 	    4 	        32ビット符合あり整数
                10 	SRATIONAL 	8 	        SLONG２つ、分子／分母
            
                */

                if (pi.Type == 1)
                {
                    int iso = BitConverter.ToChar(pi.Value, 0);
                    return iso.ToString();
                }

                if (pi.Type == 2)
                    return Encoding.ASCII.GetString(pi.Value, 0, pi.Len - 1);


                if (pi.Type == 3)
                {
                    int iso = BitConverter.ToUInt16(pi.Value, 0);
                    return iso.ToString();
                }

                if (pi.Type == 4)
                {
                    uint iso = BitConverter.ToUInt32(pi.Value, 0);
                    return iso.ToString();
                }

                if (pi.Type == 7) //実際は最後の改行は無い
                {
                    return Encoding.ASCII.GetString(pi.Value, 0, pi.Len - 1);
                }




                return "-1";
            }
            catch
            {
                return "-1";
            }
        }

        public Exif_Base(Bitmap bitmap)
        {
            bitmap2 = bitmap;
            GPSデータ初期化();
        }


        //原画像データの生成日時 10進表記のタグ:36867 16進表記のタグ:0x9003
        //実際に直接9003というバイナリがファイル中に書き込まれている
        //出力は"2000/01/01 00:00:00" 
        // DateTimeOriginal()と同じ内容で返り値の書式が違う
        private DateTime DateTimeFormated()
        {
            DateTime dt = DateTime.Parse("2000/01/01 00:00:00"); //DateTimeで使えるように整形
            try
            {
                /*
                撮影日時
                Tag 0x9003 
                Type PropertyTagTypeASCII 2
                 * ASCII 形式でエンコードされた Byte オブジェクトの配列
                Count 20 
                */
                /*
                int[] pils = bitmap2.PropertyIdList;

                //Exif情報取得
                string ret_decode_exifid;

                if ((ret_decode_exifid = decode_exifid(bitmap2, 0x9003)) == "-1")
                {
                    return "-1";    //取得失敗
                }

                string[] datetime = ret_decode_exifid.Split(' ');
                string date = datetime[0];
                string time = datetime[1];
                date = date.Replace(':', '/');

                //Exif時間を　dt に投入してフォーマットをそろえる
                DateTime dt = DateTime.Parse(date + " " + time); //DateTimeで使えるように整形
                date = dt.Year.ToString() + "/" + dt.Month.ToString("00") + "/" + dt.Day.ToString("00");
                time = dt.Hour.ToString("00") + ":" + dt.Minute.ToString("00") + ":" + dt.Second.ToString("00");

                return date + " " + time;
                */

                int[] pils = bitmap2.PropertyIdList;

                //Exif情報取得
                string ret_decode_exifid;

                if ((ret_decode_exifid = decode_exifid(bitmap2, 0x9003)) == "-1")
                {
                    return dt;    //取得失敗
                }

                string[] datetime = ret_decode_exifid.Split(' ');
                string date = datetime[0];
                string time = datetime[1];
                date = date.Replace(':', '/');

                //Exif時間を　dt に投入してフォーマットをそろえる
                dt = DateTime.Parse(date + " " + time);
                date = dt.Year.ToString() + "/" + dt.Month.ToString("00") + "/" + dt.Day.ToString("00");
                time = dt.Hour.ToString("00") + ":" + dt.Minute.ToString("00") + ":" + dt.Second.ToString("00");

                dt = DateTime.Parse(date + " " + time);

                return dt;

            }
            catch
            {
                //return "-1";
                return dt;  //return "2000/01/01 00:00:00";                
            }
        }

        /// <summary>
        /// 書式済みの撮影日(原画像データの生成日時)を返す
        /// </summary>
        /// <returns></returns>
        public string Date
        {
            get
            {
                try
                {
                    /*
                    string[] datetime = DateTimeFormated().Split(' ');
                    string date = datetime[0];
                    string time = datetime[1];

                    return date;
                    */

                    DateTime dt = DateTimeFormated();
                    //    MessageBox.Show("A" + dt.Year.ToString() + "/" + dt.Month.ToString("00") + "/" + dt.Day.ToString("00") + "z", "Date");
                    return dt.Year.ToString() + "/" + dt.Month.ToString("00") + "/" + dt.Day.ToString("00");

                }
                catch
                {
                    return "2000/01/01";
                }
            }

        }

        /// <summary>
        /// 書式済みの撮影時刻(原画像データの生成日時)を返す
        /// </summary>
        /// <returns></returns>
        public string Time
        {
            get
            {
                try
                {
                    /*
                    string[] datetime = DateTimeFormated().Split(' ');
                    string date = datetime[0];
                    string time = datetime[1];

                    return time;
                    */
                    DateTime dt = DateTimeFormated();

                    return dt.Hour.ToString("00") + ":" + dt.Minute.ToString("00") + ":" + dt.Second.ToString("00");
                    //    return dt.TimeOfDay.ToString();
                }
                catch
                {
                    return "00:00:00";
                }
            }
        }



        /// <summary>
        /// JPG保存品質
        /// </summary>
        private int Quality;

        /// <summary>
        /// JPGの保存品質 0..100
        /// 但し0は設定忘れと見なして100としている
        /// </summary>
        public int SaveQuality
        {
            set { this.Quality = value; }
            get { return this.Quality; }
        }




        /// <summary>
        /// 撮影方位の基準を返す 真方位:T 磁方位:M  0x10
        /// エラー:返値　真方位:"T" 磁方位:"M" それ以外 ""
        /// </summary>
        /// <returns></returns>
        private string GPSImgDirectionRef()
        {
            string ret_decode_exifid;
            ret_decode_exifid = decode_exifid(bitmap2, 0x10);

            if (ret_decode_exifid.Equals("T") || ret_decode_exifid.Equals("M"))
            {
                return ret_decode_exifid;
            }
            return "";
        }




        /// <summary>
        /// T(真方向) M(磁方向) 
        /// </summary>
        private string ImgDirectionRef = "";

        /// <summary>
        /// 撮影方位単位 真方位:T 磁方位:M 
        /// </summary>
        public string 撮影方位基準
        {
            set
            {
                if (value.Equals("T") || value.Equals("M"))
                {
                    this.ImgDirectionRef = value;
                }
                else
                {
                    this.ImgDirectionRef = "";
                }
            }

            get { return GPSImgDirectionRef(); }
        }




        /// <summary>
        /// 撮影方向取得
        /// 返り値:000.000..359.999
        /// エラー:-1
        /// </summary>
        /// <returns></returns>
        private string GPSImgDirection()
        {
            /* 撮影方向取得 0x11
              Tag 0x11
              Type PropertyTagTypeRational type5
              Count 1   len=8  方角 xxx.xxx
            */
            string Direction = "";

            int index;
            PropertyItem pi;
            int[] pils = bitmap2.PropertyIdList;

            index = Array.IndexOf(pils, 0x11);
            if (index == -1)
                return "-1";

            pi = bitmap2.PropertyItems[index];

            double deg = BitConverter.ToUInt32(pi.Value, 0);
            uint deg_div = BitConverter.ToUInt32(pi.Value, 4);

            double m = 0;
            if (deg_div != 0 || deg != 0)
            {
                m = (deg / deg_div);
            }

            Direction = m.ToString();

            return Direction;
        }

        /// <summary>
        /// 撮影方向 
        /// 000.000..359.999
        /// </summary>
        private string ImgDirection = "";

        /// <summary>
        /// 撮影方位 0..359.99
        /// </summary>
        public string 撮影方向
        {
            set
            {
                /*
                if (double.Parse(value) >= 0.0 && double.Parse(value) <= 360.0)
                {
                    this.ImgDirection = value;
                }
                else
                {
                    this.ImgDirection = "";
                }
                */
                this.ImgDirection = value;
            }

            get { return GPSImgDirection(); }
        }




        //画像タイトル取得 270 0x10E
        private string ImageDescription()
        {
            /*
            撮影日時
            Tag 0x10E
            Type PropertyTagTypeASCII 2
             * ASCII 形式でエンコードされた Byte オブジェクトの配列
            Count ANY 
            */

            string ret_decode_exifid;

            if ((ret_decode_exifid = decode_exifid(bitmap2, 0x10E)) != "-1")
            {
                int[] pils = bitmap2.PropertyIdList;
                int index = Array.IndexOf(pils, 0x10E);
                //Exif情報取得
                PropertyItem pi = bitmap2.PropertyItems[index];
                //    return Encoding.GetEncoding("Shift_JIS").GetString(pi.Value, 0, pi.Len - 1);
                return Encoding.GetEncoding("utf-8").GetString(pi.Value, 0, pi.Len - 1);
            }
            else
            {
                return "";
            }
        }

        private string Description = "";

        /// <summary>
        /// 画像の題を表す文字列
        /// 2 バイトコードは記録できない。
        /// 2 バイトコードの記録が必要な場合には、コメント(Exif Private タグの UserComment )を使用
        /// </summary>
        public string タイトル
        {
            set { this.Description = value; }
            get
            {
                return ImageDescription();
            }
        }




        /// <summary>
        /// 写真の縦横を設定 Tag = 274
        /// </summary>
        /// <param name="Ref"></param>
        private bool SetOrientation(int roll)
        {

            if (roll < 1 || roll > 8)
            {
                return false;
            }

            byte[] byteArray = BitConverter.GetBytes(roll);

            PropertyItem[] PropertyItems;
            PropertyItems = bitmap2.PropertyItems;

            PropertyItems[0].Id = 0x112; // 写真の回転方向
            PropertyItems[0].Type = 3;
            PropertyItems[0].Len = 2;
            PropertyItems[0].Value = byteArray;
            bitmap2.SetPropertyItem(PropertyItems[0]);
            return true;
        }

        /// <summary>
        /// 写真の縦横を取得 Tag = 274(0x112)
        /// </summary>
        /// <returns></returns>
        private int GetOrientation()
        {
            /* 画像方向
            Tag = 274 （112.H）
            Type = SHORT 3
            Count = 1
            */

            /*
            Default = 1
            1 = 0 番目の行が目で見たときの画像の上（visual top）、0 番目の列が左側（visual left-hand side）とな
            る。
            2 = 0 番目の行が目で見たときの画像の上、0 番目の列が右側（visual right-hand side）となる。
            3 = 0 番目の行が目で見たときの画像の下（visual bottom）、0 番目の列が右側となる。
            4 = 0 番目の行が目で見たときの画像の下、0 番目の列が左側となる。
            5 = 0 番目の行が目で見たときの画像の左側、0 番目の列が上となる。
            6 = 0 番目の行が目で見たときの画像の右側、0 番目の列が上となる。
            7 = 0 番目の行が目で見たときの画像の右側、0 番目の列が下となる。
            8 = 0 番目の行が目で見たときの画像の左側、0 番目の列が下となる。
            */

            string ret_decode_exifid;
            if ((ret_decode_exifid = decode_exifid(bitmap2, 0x112)) != "-1")
                return int.Parse(ret_decode_exifid);

            return -1;
        }

        /// <summary>
        /// 画像方向を示す
        /// Default = 1
        /// 1 = 0 番目の行が目で見たときの画像の上（visual top）、0 番目の列が左側（visual left-hand side）となる。
        /// 2 = 0 番目の行が目で見たときの画像の上、0 番目の列が右側（visual right-hand side）となる。
        /// 3 = 0 番目の行が目で見たときの画像の下（visual bottom）、0 番目の列が右側となる。
        /// 4 = 0 番目の行が目で見たときの画像の下、0 番目の列が左側となる。
        /// 5 = 0 番目の行が目で見たときの画像の左側、0 番目の列が上となる。
        /// 6 = 0 番目の行が目で見たときの画像の右側、0 番目の列が上となる。
        /// 7 = 0 番目の行が目で見たときの画像の右側、0 番目の列が下となる。
        /// 8 = 0 番目の行が目で見たときの画像の左側、0 番目の列が下となる。
        /// </summary>
        /// 
        public int Orientation
        {
            set
            {
                SetOrientation(value);
            }
            get
            {
                return GetOrientation();
            }
        }


        /// <summary>
        /// yyyy:MM:dd HH:mm:ss to yyyy/MM/dd HH:mm:ss
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        private string DateTime書式変換(string datetime)
        {
            string[] tmp = datetime.Split(' ');
            tmp[0] = tmp[0].Replace(':', '/');
            return tmp[0] + " " + tmp[1];
        }


        /// <summary>
        /// 画像が撮影後にデジタル化された日時 0x9004
        /// エラー:-1
        /// </summary>
        /// <returns></returns>
        private string DateTimeDigitized()
        {
            /*
           撮影日時 デジタルデータ作成日時
           Tag 0x9004
           Type PropertyTagTypeASCII 2
           ASCII 形式でエンコードされた Byte オブジェクトの配列
           Count 20 
           */

            //Exif情報取得
            string ret_decode_exifid;

            if ((ret_decode_exifid = decode_exifid(bitmap2, 0x9004)) != "-1")
            {
                return ret_decode_exifid;
            }
            else
            {
                return "-1";
            }
        }

        /// <summary>
        /// 0x9004 ディジタルデータ作成日時
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        private bool SetDateTimeDigitized(DateTime datetime)
        {
            try
            {
                //    date = date.Replace('/', ':');
                //    Exif_SetStrings(0x9004, date + " " + time);
                Exif_SetStrings(0x9004, datetime.ToString("yyyy/MM/dd HH:mm:ss"));
                return true;
            }
            catch
            {
                return false;
            }
        }

        private DateTime datetimedigitized; // = DateTime.Parse("2000/01/01 00:00:00"); 実際は 0001/01/01 0:00:00 で作成される

        /// <summary>
        /// 渡す日時の書式: DateTime
        /// </summary>
        public DateTime デジタルデータ作成日時
        {
            set
            {
                //   string[] datetime = value.  .Split(' ');
                //   string date = datetime[0].Replace("/", ":");
                //   string time = datetime[1];
                /*
                string date = value.Year.ToString() + ":" + value.Month.ToString("00") + ":" + value.Day.ToString("00");
                string time = value.Hour.ToString("00") + ":" + value.Minute.ToString("00") + ":" + value.Second.ToString("00");
                DateTime dt = DateTime.Parse(date + " " + time);
                MessageBox.Show(date + " " + time,"date + time");
                */
                this.datetimedigitized = value;
            }

            get
            {
                string ret = DateTimeDigitized();

                if (ret.Equals("-1"))
                {
                    return DateTime.Parse("2000/01/01 00:00:00");
                }

                /*
                DateTime dt = DateTime.Parse(ret);
                string date = dt.Year.ToString() + "/" + dt.Month.ToString("00") + "/" + dt.Day.ToString("00");
                string time = dt.Hour.ToString("00") + ":" + dt.Minute.ToString("00") + ":" + dt.Second.ToString("00");
              //  return DateTime.Parse(date + " " + time);
                */

                string f = "yyyy/MM/dd HH:mm:ss";
                return DateTime.ParseExact(DateTime書式変換(ret), f, null);
            }
        }




        /// <summary>
        /// 原画像データの生成日時 取得 36867 0x9003 
        /// エラー:-1
        /// </summary>
        private string DateTimeOriginal()
        {
            /*
             撮影日時
             Tag 0x9003 
             Type PropertyTagTypeASCII 2
             ASCII 形式でエンコードされた Byte オブジェクトの配列
             Count 20 
             */

            //Exif情報 原画像データの生成日時取得
            string ret_decode_exifid;

            if ((ret_decode_exifid = decode_exifid(bitmap2, 0x9003)) != "-1")
            {
                return ret_decode_exifid;
            }
            else
            {
                return "-1";
            }
        }


        /// <summary>
        /// 0x9003 原画像データの生成日時
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        private bool SetDateTimeOriginal(DateTime datetime)
        {
            try
            {
                Exif_SetStrings(0x9003, datetime.ToString("yyyy/MM/dd HH:mm:ss"));
                return true;
            }
            catch
            {
                return false;
            }
        }


        private DateTime datetimeoriginal;  // 実際は 0001/01/01 0:00:00 で作成される
        /// <summary>
        /// 撮影日時
        /// </summary>
        public DateTime 原画像データの生成日時
        {
            set
            {
                this.datetimeoriginal = value;
            }

            get
            {
                string ret = DateTimeOriginal();
                string f = "yyyy/MM/dd HH:mm:ss";

                if (ret.Equals("-1"))
                {
                    return DateTime.ParseExact("2000/01/01 00:00:00", f, null);
                }

                return DateTime.ParseExact(DateTime書式変換(ret), f, null);
            }
        }

        /// <summary>
        /// 原画像データの生成日時のエイリアス(別名)
        /// </summary>
        public DateTime 撮影日時
        {
            set
            {
                原画像データの生成日時 = value;
            }

            get
            {
                return 原画像データの生成日時;
            }
        }


        /// <summary>
        /// ファイル変更日時取得 0x132
        /// 返り値: 2008:08:02 12:52:32
        /// エラー: -1
        /// </summary>
        /// <returns></returns>
        private string GetDateTime()
        {
            /*
            ファイル変更日時
            Tag 0x132
            Type PropertyTagTypeASCII 2
            ASCII 形式でエンコードされた Byte オブジェクトの配列
            Count 20 
            */

            //Exif情報取得
            string ret_decode_exifid;

            if ((ret_decode_exifid = decode_exifid(bitmap2, 0x132)) != "-1")
            {
                return ret_decode_exifid;
            }
            else
            {
                return "-1";
            }
        }


        /// <summary>
        /// 0x132 ファイル変更日時  変更となっているが実際は撮影した本当の日時
        /// </summary>
        /// <param name="date"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        private bool SetDateTime(DateTime datetime)
        {
            /*
            Exif Version 2.1
            フォーマットは"YYYY:MM:DD HH:MM:SS"
            時間は24 時間表示し日付と時間の間に空白文字［20.H］を 一つ挿入する。
            */
            try
            {
                Exif_SetStrings(0x132, datetime.ToString("yyyy/MM/dd HH:mm:ss"));
                return true;
            }
            catch
            {
                return false;
            }
        }

        private DateTime Datetime;   //DateTimeと紛らわしいので注意  0001/01/01 0:00:00 で作成される

        /// <summary>
        /// ファイル変更日時
        /// </summary>
        public DateTime ファイル変更日時
        {
            set { this.Datetime = value; }
            get
            {
                string ret = GetDateTime();

                if (ret.Equals("-1"))
                {
                    return DateTime.Parse("2000/01/01 00:00:00");
                }

                string f = "yyyy/MM/dd HH:mm:ss";
                return DateTime.ParseExact(DateTime書式変換(ret), f, null);
            }
        }







        //以下作りかけで放置 or デバッグなし


        /// <summary>
        /// ISO スピードレート 0x8827 作りかけ
        /// </summary>
        private string ISOSpeedRatings
        {
            //34855 | ISOSpeedRatings         | ISO スピードレート               | ISO speed ratings

            /*ISO感度については、タイプ（Type）が
             * 「3：16bit符号なし整数」であるため、
             * BitConverterクラス（System名前空間）
             * のToUInt16メソッドにより、
             * 2bytesの値を整数に変換する必要がある。*/
            get
            {
                int[] pils = bitmap2.PropertyIdList;

                int index = Array.IndexOf(pils, 0x8827);
                PropertyItem pi = bitmap2.PropertyItems[index];

                int abc = 12500;
                pi.Value = BitConverter.GetBytes(abc);
                bitmap2.SetPropertyItem(pi);//  [index];

                //     MessageBox.Show(index.ToString(), "ISOSpeedRatings");

                int iso = BitConverter.ToUInt16(pi.Value, 0);
                return iso.ToString();
            }
        }



        /// <summary>
        /// 画像からコメント取得 0x9286
        /// エラー:-1
        /// データなし : ""
        /// </summary>
        /// <returns></returns>
        private string UserComment()
        {
            /*
            Tag 0x9286
            Type PropertyTagTypeASCII 2
            ASCII 形式でエンコードされた Byte オブジェクトの配列
            Count ANY 
            */

            //エンコードタイプ
            string type = "Shift_JIS";

            //Exif情報取得
            string ret_decode_exifid;
            ret_decode_exifid = decode_exifid(bitmap2, 0x9286);

            if (!ret_decode_exifid.Equals("-1"))
            {
                //先頭の8バイトの内容によって、デコードのタイプを変更する
                //Shift JISとして文字列に変換

                int[] pils = bitmap2.PropertyIdList;
                int index = Array.IndexOf(pils, 0x9286);
                //Exif情報取得
                PropertyItem pi = bitmap2.PropertyItems[index];


                if (ret_decode_exifid.StartsWith("UNICODE"))
                {
                    type = "utf-8";
                }

                else if (ret_decode_exifid.StartsWith(" ")) //UNDEFINED
                {
                    type = "Shift_JIS";
                }

                else if (ret_decode_exifid.StartsWith("SJIS"))
                {
                    type = "iso-2022-jp";
                }


                else if (ret_decode_exifid.StartsWith("ASCII"))
                {
                    type = "ASCII";
                }

                //格納されているデータが JISの場合
                Encoding typeEnc = Encoding.GetEncoding(type);
                byte[] bytes = pi.Value;

                //Unicodeに変換して返す
                string str = Encoding.GetEncoding(type).GetString(bytes);
                return str;
            }
            else
            {
                return "";
            }
        }

        private string Comment = "";
        public string コメント
        {
            set { this.Comment = value; }
            get { return UserComment(); }
        }


    }

    public class  Exif_Bitmap : Exif_Base
    {
        public Exif_Bitmap(Bitmap bitmap)
            : base(bitmap)
        {

        }
    }
}
