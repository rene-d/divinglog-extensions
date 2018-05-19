using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DL5GeoTag
{
    public class TextBoxUnit : System.Windows.Forms.TextBox
    {
        public bool UnitFeet { get; set; }

        public TextBoxUnit()
        {
            UnitFeet = false;
        }

        public double? ValueUnit
        {
            get
            {
                double d = 0;
                
                if (Text != "")
                    double.TryParse(Text, out d);
                
                return UnitFeet ? d * 0.3048 : d;                
            }
            set
            {
                if (value == null)
                {
                    Text = "";
                }
                else
                {
                    Text = string.Format("{0:.#}", UnitFeet ? value / 0.3048 : value);
                }
            }

        }
    }
}
