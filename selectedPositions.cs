using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnotherEdit
{
    // this class we used for store "position" in  parallel calculation 

    public class selectedPositions
    {
        private int icharB;
        private int icharE;
        private int? id_UIElement;

        public int iCharBegin {
            get {
                return icharB;
            }
            set {
                icharB = value;
            }
        }

        public int iCharEnd {
            get {
                return icharE;
            }
            set {
                icharE = value;
            }
        }

        public int? iCollection
        {
            get
            {
                return id_UIElement;
            }
            set
            {
                id_UIElement = value;
            }
        }

        public selectedPositions() {
            icharB = 0;
            icharE = 0;
            id_UIElement = null;
        }

        public selectedPositions(int iCharB, int iCharE, int? idUIElement) {
            icharB = iCharB;
            icharE = iCharE;
            id_UIElement = idUIElement;
        }
    }


    public class selectedPositionsCurrent   // For temporary holding selectionMain process ... 
    {
        private int i_charCurrent;
        private int i_lineCurrent;
        private int i_DeltaX;
        private int i_DeltaY;

        public int iCharCurrent {
            get {
                return i_charCurrent;
            }
            set {
                i_charCurrent = value;
            }
        }

        public int iLineCurrent {
            get {
                return i_lineCurrent;
            }
            set {
                i_lineCurrent = value;
            }
        }

        public int iDeltaX {
            get {
                return i_DeltaX;
            }
            set {
                i_DeltaX = value;
            }
        }

        public int iDeltaY {
            get {
                return i_DeltaY;
            }
            set {
                i_DeltaY = value;
            }
        }

        public selectedPositionsCurrent(selectedPositionsCurrent sp) {
            i_charCurrent = sp.iCharCurrent;
            i_lineCurrent = sp.iLineCurrent;
            i_DeltaX = sp.iDeltaX;
            i_DeltaY = sp.iDeltaY;
        }

        public selectedPositionsCurrent() {
            i_charCurrent = 0;
            i_lineCurrent = 0;
            i_DeltaX = 0;
            i_DeltaY = 0;
        }

        public selectedPositionsCurrent(int iLineCurrent, int iCharCurrent) {
            i_charCurrent = iCharCurrent;
            i_lineCurrent = iLineCurrent;
        }
    }


    public class listOfSelectedPositionInLine
    {

        public List<selectedPositions> listSPinLine = new List<selectedPositions>();

        public listOfSelectedPositionInLine() {
            listSPinLine.Clear();
        }

        public listOfSelectedPositionInLine(selectedPositions sp) {
            listSPinLine.Add(sp);
        }

        public listOfSelectedPositionInLine(int iCharBegin, int iCharEnd, int? iUIE) {
            listSPinLine.Add(new selectedPositions(iCharBegin, iCharEnd, iUIE));

        }

    }

}
