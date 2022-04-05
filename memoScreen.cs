using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnotherEdit
{

    public class memoScreen
    {
        private int i_LineB; // first line
        private int i_CharB; // first char

        private string str_Find;
        private string str_StatusFile; // StatusFile - full path to file ...
        private string str_ConnectionString;

        int index_SelectedFontFamily;
        int index_SelectedFontSize;
        int index_SelectedDB;

        private int int_AbsolutVertCurrent;
        private int int_AbsolutHorizCurrent;
        private bool bln_CaretInsert;
        private string str_Memo;
        private bool bln_Append;

        public bool blnAppend
        {
            get
            {
                return bln_Append; ;
            }
            set
            {
                bln_Append = value;
            }
        }

        public int iLineBegin {
            get {
                return i_LineB;
            }
            set {
                i_LineB = value;
            }
        }

        public int iCharBegin {
            get {
                return i_CharB;
            }
            set {
                i_CharB = value;
            }
        }


        public string strFind {
            get {
                return str_Find;
            }
            set {
                str_Find = value;
            }
        }



        public string statusFile {
            get {
                return str_StatusFile;
            }
            set {
                str_StatusFile = value;
            }
        }

        public int indexSelectedFontSize {
            get {
                return index_SelectedFontSize;
            }
            set {
                index_SelectedFontSize = value;
            }
        }

        public int indexSelectedFontFamily {
            get {
                return index_SelectedFontFamily;
            }
            set {
                index_SelectedFontFamily = value;
            }
        }

        public int indexSelectedDB {
            get {
                return index_SelectedDB;
            }
            set {
                index_SelectedDB = value;
            }
        }

        public string strConnectionString {
            get {
                return str_ConnectionString;
            }
            set {
                str_ConnectionString = value;
            }
        }
        //--------------

        public int intAbsolutVertCurrent {
            get {
                return int_AbsolutVertCurrent;
            }
            set {
                int_AbsolutVertCurrent = value;
            }
        }

        public int intAbsolutHorizCurrent {
            get {
                return int_AbsolutHorizCurrent;
            }
            set {
                int_AbsolutHorizCurrent = value;
            }
        }

        public bool blnCaretInsert {
            get {
                return bln_CaretInsert;
            }
            set {
                bln_CaretInsert = value;
            }
        }

        public string strMemo {
            get {
                return str_Memo;
            }
            set {
                str_Memo = value;
            }
        }


        // Default constructor. If a derived class does not invoke a base-
        // class constructor explicitly, the default constructor is called
        // implicitly.

        public memoScreen() {
            i_LineB = 0;
            i_CharB = 0;
            str_StatusFile = "";
            str_Find = "";
            index_SelectedFontSize = 2; // Font Size default - 12
            index_SelectedFontFamily = 0;
            index_SelectedDB = 0;   // Default DB  MS SQL
            str_ConnectionString = "";
            int_AbsolutVertCurrent = 0;
            int_AbsolutHorizCurrent = 0;
            bln_CaretInsert = false;
            str_Memo = "";
            bln_Append = false;
        }

        public memoScreen(int iLineB, int iCharB, string strStatusFile, string strFind,
                          string strConnectionString, int indexSelectedFontSize,
                          int indexSelectedFontFamily, int indexSelectedDB,
                          int intAbsolutVertCurrent, int intAbsolutHorizCurrent,
                          bool blnCaretInsert, string strMemo, bool blnAppend) {
            i_LineB = iLineB;
            i_CharB = iCharB;
            str_StatusFile = strStatusFile;
            index_SelectedFontSize = indexSelectedFontSize;
            str_ConnectionString = strConnectionString;
            index_SelectedDB = indexSelectedDB;
            index_SelectedFontFamily = indexSelectedFontFamily;
            int_AbsolutVertCurrent = intAbsolutVertCurrent;
            int_AbsolutHorizCurrent = intAbsolutHorizCurrent;
            bln_CaretInsert = blnCaretInsert;
            str_Memo = strMemo;
            bln_Append = blnAppend;
        }


    }


}
