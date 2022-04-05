using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Controls;

namespace AnotherEdit
{

    public class selectedFonts : TextBlock
    {
        private double d_Horiz;
        private double d_Vert;
        private string str_FontName;

        public double dHoriz {
            get {
                return d_Horiz;
            }
            set {
                d_Horiz = value;
            }
        }

        public double dVert {
            get {
                return d_Vert;
            }
            set {
                d_Vert = value;
            }
        }

        public string strFontName {
            get {
                return str_FontName;
            }
            set {
                str_FontName = value;
            }
        }

        public selectedFonts() {
            d_Horiz = 1.67;
            d_Vert = 0.877;
            str_FontName = "Courier New";
        }

        public selectedFonts(string strFontName, double dHoriz, double dVert) {
            d_Horiz = dHoriz;
            d_Vert = dVert;
            str_FontName = strFontName;
        }
    }


}
