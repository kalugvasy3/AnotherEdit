using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnotherEdit
{


    // Class for store Absolute Path 

    public class absolutPosition
    {
        private int i_Line;
        private int i_Char;
        private int i_Absolut;
        private string str_Current;

        public int iLine {
            get {
                return i_Line;
            }
            set {
                i_Line = value;
            }
        }
        public int iChar {
            get {
                return i_Char;
            }
            set {
                i_Char = value;
            }
        }

        public int iAbsolut {
            get {
                return i_Absolut;
            }
            set {
                i_Absolut = value;
            }
        }

        public string strCurrent {
            get {
                return str_Current;
            }
            set {
                str_Current = value;
            }
        }


        public absolutPosition() {
            i_Line = 0;
            i_Char = 0;
            i_Absolut = 0;
            str_Current = "";
        }
    }




}
