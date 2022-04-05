using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


using System.Collections.Concurrent;

using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;


using AnotherEdit;



namespace AnotherEdit
{
    /// <summary>
    /// Interaction logic for FastTextBox.xaml
    /// </summary>
    public partial class FastTextBox : UserControl
    {

        public List<StringBuilder> listSBmain = new List<StringBuilder>();

        public List<StringBuilder> listSB_A = new List<StringBuilder>();
        public List<StringBuilder> listSB_B = new List<StringBuilder>();
        public List<StringBuilder> listSB_C = new List<StringBuilder>();
        public List<StringBuilder> listSB_D = new List<StringBuilder>();
        public List<StringBuilder> listSB_E = new List<StringBuilder>();

        public bool blnError = false;

        private int intLastLineOnPage = 0;

        public List<listOfSelectedPositionInLine> listSPMain = new List<listOfSelectedPositionInLine>();

        //     public List<StringBuilder> listSBstandard = new List<StringBuilder>();
        //     public List<listOfSelectedPositionInLine> selectedCB = new List<listOfSelectedPositionInLine>();

        public bool blnPenDeSelect = false;
        public bool blnRecDeSelect = false;
        public bool blnRecSelect = false;
        public bool blnPlaceHolder = false;

        //     public bool blnAllowVertical = false;

        public string strFind = "";           // for  store  Find string
        public string strReplace = "";

        public bool blnMouseLeftButtonPressed = false;
        public bool blnMouseRightButtonPressed = false;

        //Font coefficients 

        public double coeffFont_Widh = 1.823;  
        public double coeffFont_High = 0.840;

        public long longTotalDocSize = 0;
        public int intMaxLineSize = 0; // Count/Re-count during EDIT and LOADING

        //     private List<TextEffect>[] listTE;


        // ----------------------------------------------------------------------------------------------------

        public int intVertCountLinesOnPage = 0;  // depends from FontSize
        public int intHorizCountCharsOnPage = 0;



        //    public int intFirstLineOnPage = 0; // absolute number first line on current page  - if screen size was changed - need to re-calculate      
        //    public int intFirstCharOnPage = 0; // absolute number first char on current line on Current Page        

        //Caret section
        public int intAbsolutCaretVertCurrent = 0;
        public int intAbsolutCaretHorizCurrent = 0;

        public bool blnCaretInsert = false;
        public bool blnSomthingSelected = false;

        public bool blnAppend = false; // flag (we store flag Append - like check box)

        public int intNextAbsolutSelectionLine = 0;
        public int intNextAbsolutSelectionHorizont = 0;

        public Color selectedColor = (Color)ColorConverter.ConvertFromString("Magenta");

        //Temporary value

        //      private static string[] strOriginal;    // array store original string - used for update listSBmain together with

        private Caret crt;

        public bool blnHighlight = false;
        public string strDataBaseName = "MSSQL"; // Just Name
        public string strConnection = "";

        //    public bool blnLoad = false;

        public FontFamily tb_FontFamily = new FontFamily("Courier New");
        public double tb_FontSize = 10;

        public TextBlock[] tb = new TextBlock[222];
        private TextEffectCollection[] tec = new TextEffectCollection[222];

        public FastTextBox() {
            InitializeComponent();

            treeContext.Visibility = Visibility.Collapsed;
            listBoxHelper.Visibility = Visibility.Collapsed;

            listSPMain.Add(new listOfSelectedPositionInLine());

            //MouseRightButtonDown += (sender, e) => { e.Handled = true; };
            //MouseRightButtonUp += (sender, e) => { e.Handled = true; };

            //     MouseUp += (sender, e) => { e.Handled = true; };
            //     MouseDown += (sender, e) => { e.Handled = true; };

            // tb.AddHandler(TextBlock.DragOverEvent, new DragEventHandler(tb_DragOver), true);  //RichTextBox

            // tb.AddHandler(Keyboard.KeyDownEvent, new KeyEventHandler(canvasMain_KeyDown), true);

            //canvasMain.AddHandler(Canvas.SizeChangedEvent, new SizeChangedEventHandler (txtBlock_SizeChanged), true);

            canvasMain.Focusable = true;
            //    canvasMain.PreviewKeyUp += (s, e_in) => canvasMain_KeyDown(s, e_in);

            pointMouseLeftButtonPressed.X = 0;
            pointMouseLeftButtonPressed.Y = 0;

            if (crt == null) {
                crt = new Caret();
                canvasMain.Children.Add(crt);    // Add to GridMain - It should not depends from Map !!
                //set_Caret(13, 2, 2, 13, Colors.Black);
                Dispatcher.Invoke(new Action(() => set_Caret(0, 0)));
            }

            listSBmain = new List<StringBuilder>();
            listSBmain.Add(new StringBuilder(""));

            listSPMain = new List<listOfSelectedPositionInLine>();
            listSPMain.Add(new listOfSelectedPositionInLine());

            initTextBlocks();

        }

        //------------------------------------------------------------------------------------------------------------------

        public bool blnAbort = false;


        private void initTextBlocks() {
            StackPanel s = new StackPanel();
            s = tbStackPanel;

            for (int i = 0; i < tb.Length; i++) {

                StackPanel ss = new StackPanel();

                tb[i] = new TextBlock();

                tb[i].FontFamily = tb_FontFamily;
                tb[i].FontSize = tb_FontSize;
                tb[i].FontWeight = FontWeights.Normal;  // new FontWeight();
                tb[i].Visibility = Visibility.Collapsed;

                tb[i].TextEffects = new TextEffectCollection();

                s.Children.Add(tb[i]);
                s.Children.Add(ss);
                s = ss;
            }
        }


        public void setTextBlockClear() {
            Dispatcher.Invoke(new Action(() => {
                for (int i = 0; i < tb.Length; i++) {
                    tb[i].Text = "";
                    tb[i].Visibility = Visibility.Collapsed;
                    tb[i].TextEffects.Clear();
                    tec[i] = new TextEffectCollection();
                }
            }));
        }

        public void setTextBlockFontSize() {
            Dispatcher.Invoke(new Action(() => {
                for (int i = 0; i < tb.Length; i++) {
                    tb[i].FontSize = tb_FontSize;
                }
            }));
        }

        public void setTextBlockFontFamily() {
            Dispatcher.Invoke(new Action(() => {
                for (int i = 0; i < tb.Length; i++) {
                    tb[i].FontFamily = tb_FontFamily;
                }
            }));
        }

        //------------------------------------------------------------------------------------------------------------------




        //private void initTextBlocks() {
        //    for (int i = 0; i < tb.Length; i++) {

        //        tb[i] = new TextBlock();

        //        tb[i].FontFamily = tb_FontFamily;
        //        tb[i].FontSize = tb_FontSize;
        //        tb[i].FontWeight = FontWeights.Normal;  // new FontWeight();
        //        tb[i].Visibility = Visibility.Collapsed;

        //        tb[i].TextEffects = new TextEffectCollection();

        //        tbStackPanel.Children.Add(tb[i]);
        //    }
        //}


        //public void setTextBlockClear() {
        //    Dispatcher.Invoke(new Action(() => {
        //        foreach (TextBlock tb in tbStackPanel.Children) {
        //            tb.Text = "";
        //            tb.Visibility = Visibility.Collapsed;
        //            tb.TextEffects.Clear();
        //        }
        //    }));
        //}

        //public void setTextBlockFontSize() {
        //    Dispatcher.Invoke(new Action(() => {
        //        foreach (TextBlock tb in tbStackPanel.Children) {
        //            tb.FontSize = tb_FontSize;

        //        }
        //    }));
        //}

        //public void setTextBlockFontFamily() {
        //    Dispatcher.Invoke(new Action(() => {
        //        foreach (TextBlock tb in tbStackPanel.Children) {
        //            tb.FontFamily = tb_FontFamily;
        //        }
        //    }));
        //}






        private void tb_DragOver(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                e.Effects = DragDropEffects.All;
            }
            else {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = false;
        }



        //---------------------------------------------------------------------------------------------------------

        public void set_Caret(double Y, double X, double H, double W, Color clr) {
            crt.careteH = H;
            crt.careteW = W;
            crt.careteClr = clr;
            crt.tr.X = X - 1;
            crt.tr.Y = Y;
        }

        public void set_Caret(int intLine, int intChar) {
            int iChar = (int)(intChar - intFirstCharOnPageUpdate);
            int iLine = (int)(intLine - intFirstLineOnPageUpdate);

            crt.tr.X = tb_FontSize * iChar / coeffFont_Widh - 1;
            crt.tr.Y = tb_FontSize * iLine / coeffFont_High;

            crt.careteH = tb_FontSize / coeffFont_High;
            crt.careteW = blnCaretInsert ? tb_FontSize / coeffFont_Widh : tb_FontSize / coeffFont_Widh / 3;

            if (iLine < 0 ||
                iChar < 0 ||
                iLine > intVertCountLinesOnPage - 1 ||
                iChar > intHorizCountCharsOnPage - 1) {
                crt.careteClr = Colors.Transparent;
            }
            else {
                crt.careteClr = blnCaretInsert ? Colors.Magenta : Colors.Magenta;
            }
        }



        //scroll  INIT ---------------------------------------------------------------------------------------------

        public void init_VerticalScroll() {
            //         if (blnLoad) return;

            //     Dispatcher.Invoke(new Action(() =>
            //     {

            intVertCountLinesOnPage = (int)((canvasMain.ActualHeight - 17) * coeffFont_High / tb_FontSize);
            intVertCountLinesOnPage = intVertCountLinesOnPage < 0 ? 0 : intVertCountLinesOnPage;

            // intFirstLineOnPage = (int)scrollY.Value;

            int intTmp = listSBmain.Count - 1; // for scrolling we Use Maximum line
            scrollY.Minimum = 0;
            scrollY.Maximum = intTmp < 0 ? 0 : intTmp;  // prevent scrolling if listSBmain is empty

            scrollY.SmallChange = 1;   // one line
            scrollY.LargeChange = intVertCountLinesOnPage / 2;  // number lines per page/2
            tbY.Text = "Y: " + intFirstLineOnPageUpdate.ToString("0,0") + " of " + ((int)scrollY.Maximum).ToString("0,0");

            //   }));
        }

        public void init_HorizontalScroll() {
            //         if (blnLoad) return;

            //  totalBytes();

            //    Dispatcher.Invoke(new Action(() =>
            //    {

            intHorizCountCharsOnPage = (int)((canvasMain.ActualWidth - 17) * coeffFont_Widh / tb_FontSize);
            intHorizCountCharsOnPage = intHorizCountCharsOnPage < 0 ? 0 : intHorizCountCharsOnPage;

            //    intFirstCharOnPage = (int)scrollX.Value;

            scrollX.Minimum = 0;
            scrollX.Maximum = intMaxLineSize == 0 ? 0 : intMaxLineSize - 1; // size of longest line

            scrollX.SmallChange = 1;   // one line
            scrollX.LargeChange = intHorizCountCharsOnPage / 2;   // number lines per page/2
            tbX.Text = "X: " + intFirstCharOnPageUpdate.ToString("0,0") + " of " + ((int)scrollX.Maximum).ToString("0,0");


            //    }));
        }

        //---------------------------------------------------------------------------------------------------------
        //SCROLL VALUE CHAGED -------------------------------------------------------------------------------------

        private void scrollY_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            init_VerticalScroll();

            updateBlock();

        }


        private void scrollX_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            init_HorizontalScroll();

            updateBlock();

        }

        //Drop------------------------------------------------------------------------------------------------------


        private int intFirstLineOnPageUpdate = 0; //Uses only inside Update - ALL Sub Functions
        private int intFirstCharOnPageUpdate = 0;

        private int intOldFirstLineOnPageUpdate = 0; //Uses only inside Update - ALL Sub Functions
        private int intOldFirstCharOnPageUpdate = 0;

        public void updateBlock() {

            if (listSBmain == null) return;

            init_HorizontalScroll();
            init_VerticalScroll();

            intOldFirstLineOnPageUpdate = intFirstLineOnPageUpdate;
            intOldFirstCharOnPageUpdate = intFirstCharOnPageUpdate;

            intFirstLineOnPageUpdate = (int)scrollY.Value; ;
            intFirstCharOnPageUpdate = (int)scrollX.Value; ;

            intLastLineOnPage = (intFirstLineOnPageUpdate + intVertCountLinesOnPage) < listSBmain.Count ? (intFirstLineOnPageUpdate + intVertCountLinesOnPage) : (listSBmain.Count);

            //      if (blnLoad) return;
            Dispatcher.Invoke(new Action(() => {
                if (listSBmain.Count == 0) set_Caret(0, 0);
                else set_Caret(intAbsolutCaretVertCurrent, intAbsolutCaretHorizCurrent);
            }));

            if (listSBmain.Count == 0 || listSBmain == null) {

                setTextBlockClear();

                listSBmain.Add(new StringBuilder(""));
                listSPMain.Insert(0, new listOfSelectedPositionInLine());

                intAbsolutCaretHorizCurrent = 0;
                intAbsolutCaretVertCurrent = 0;

                canvasSelected.Children.Clear();
                canvasSelecting.Children.Clear();
            }


            string pText = "";
            int intLastCharOnPage = intFirstCharOnPageUpdate + intHorizCountCharsOnPage;

            //--------------------------------------------------------------------------------------------


            somthingSelected(); // Calculate blnSomthingSelected

            // We must update Selected Rectangle each time ...

            Color clr = Colors.LightBlue;
            if (blnAppend) clr = Colors.Aquamarine;
            Brush myBrush = new SolidColorBrush(clr);
            myBrush.Freeze();


            var t = Task.Factory.StartNew(() => this.Dispatcher.BeginInvoke(new Action(() => updateCanvasSelected(myBrush))));

            //--------------------------------------------------------------------------------------------------------------
            // Drawing something New // Do not use Parallel/Task in currentCanvasSelecting
            Dispatcher.Invoke(new Action(() => canvasSelecting.Children.Clear()));

            if (blnMouseLeftButtonPressed && !blnMouseRightButtonPressed) currentCanvasSelecting(); // Making selectionMain for Moving Cursor

            //------------------------------------------------------------------------------------------
            //------------------------------------------------------------------------------------------

            setTextBlockClear();

            //SolidColorBrush scbg = new SolidColorBrush(Colors.LimeGreen);
            //scbg.Freeze();


            for (int i = 0; i < intLastLineOnPage - intFirstLineOnPageUpdate; i++) {

                int intTmp = intLastCharOnPage <= listSBmain[i + intFirstLineOnPageUpdate].Length ? intHorizCountCharsOnPage : (listSBmain[i + intFirstLineOnPageUpdate].Length - intFirstCharOnPageUpdate);
                // Add paragraphs to the FlowDocument. 

                tb[i].Height = tb_FontSize / coeffFont_High;


                if (intFirstCharOnPageUpdate < listSBmain[i + intFirstLineOnPageUpdate].Length) {
                    pText = (intTmp != 0 ? listSBmain[i + intFirstLineOnPageUpdate].ToString(intFirstCharOnPageUpdate, intTmp) + ((i < intLastLineOnPage - intFirstLineOnPageUpdate - 1) ? Environment.NewLine : "") : (i < intLastLineOnPage - intFirstLineOnPageUpdate - 1) ? Environment.NewLine : "");

                    tb[i].Text = pText;
                    tb[i].Visibility = Visibility.Visible;
                }
                else {
                    if (i < intLastLineOnPage - intFirstLineOnPageUpdate - 1) {
                        pText = Environment.NewLine;

                        tb[i].Text = pText;
                        tb[i].Visibility = Visibility.Visible;
                    }
                }

                //int indexGreen = -1;
                //if (listSBmain[i + intFirstLineOnPageUpdate].ToString().Contains("--"))
                //{
                //    indexGreen = listSBmain[i + intFirstLineOnPageUpdate].ToString().IndexOf("--");
                //    int index = indexGreen - intFirstCharOnPageUpdate;

                //    if (index < intHorizCountCharsOnPage)
                //    {
                //        index = index < 0 ? 0 : index;
                //        int intEnd = tb[i].Text.Length;

                //        TextEffect te = new TextEffect(null, scbg, null, index, intEnd - index);
                //        tec[i].Add(te);
                //    }
                //}


            }

            if (blnMouseLeftButtonPressed) return;

            // var tt = Task.Factory.StartNew(() =>  Do not T A S K  !!!!!!!!!!!!!!!!!!!
            Dispatcher.BeginInvoke(new Action(() => {

                if (blnHighlight) currentHighlight();
                if (strFind != "") currentTextEffect(strFind);
               
                setTE();
            }));

            //);

            canvasMain.Focus();

            //------------------------------------------

        }

        public void setTE() {
            for (int i = 0; i < intLastLineOnPage - intFirstLineOnPageUpdate; i++) {
                tb[i].TextEffects = tec[i];
            }
        }


        public void currentTextEffect(string strSelected) {

            try {


                //    Parallel.For (0,intLastLineOnPage - intFirstLineOnPage,(int i) => { 




                for (int i = 0; i < intLastLineOnPage - intFirstLineOnPageUpdate; i++) {
                    string strOrig = "";

                    Dispatcher.Invoke(new Action(() => {
                        //   tb[i].TextEffects.Clear();
                        strOrig = tb[i].Text;
                    }));

                    if (strOrig == null) goto lexit;

                    int iCurrentLineLength = strOrig.Length;


                    foreach (string str in strFind.Split('~')) {
                        int iFindLength = str.Trim().Length;
                        int index = -iFindLength;
                        SolidColorBrush scb = new SolidColorBrush(selectedColor);
                        scb.Freeze();


                        if (iFindLength > 0) {
                            index = strOrig.IndexOf(str, index + iFindLength);
                        lContinue:
                            if (index > -1) {
                                TextEffect te = new TextEffect(null, scb, null, index, iFindLength);

                                tec[i].Add(te);

                                if (index + iFindLength < iCurrentLineLength) {
                                    index = strOrig.IndexOf(str, index + iFindLength);
                                    goto lContinue;
                                }
                            }
                        }
                    }
                lexit: ;
                }


            }
            catch (Exception ex) {
                MessageBox.Show("Program has a problem allocating additional char(s)/byte(s) in memory (currentTextEffect) ...\nPlease save your  job ... ..." + ex.Message);
                blnError = true;
            }

        }

        public bool blnODBCHighlight = false;
        public bool blnFunctionHighlight = false;
        public bool blnApostropheHighlight = false;


        public selectedSqlKeys keySQL;

        public void currentHighlight() {

            try {


                SolidColorBrush scbk = new SolidColorBrush(Colors.Blue);
                scbk.Freeze();
                SolidColorBrush scbg = new SolidColorBrush(Colors.LimeGreen);
                scbg.Freeze();

                if (keySQL == null) keySQL = new selectedSqlKeys(strConnection,strDataBaseName, blnODBCHighlight, blnFunctionHighlight);

                //   intFirstLineOnPage = (int)scrollY.Value;


                //  Parallel.For(0, intLastLineOnPage - intFirstLineOnPageUpdate, (int i) => { 

                for (int i = 0; i < intLastLineOnPage - intFirstLineOnPageUpdate; i++) {

                    if (tb[i].Text == null) goto lexit;

                    string strOrig = " " + tb[i].Text.ToUpper().Replace(Environment.NewLine, "") + " ";
                    strOrig = strOrig.Replace('[', ' ').Replace(']', ' ').Replace(',', ' ').Replace('(', ' ').Replace(')', ' ').Replace(';', ' ').Replace('|', ' ').Replace('{', ' ').Replace('}', ' ').Replace('-', ' ');
                    int iCurrentLineLength = strOrig.Length;


                    //   Parallel.For(0, keySQL.listSqlReserved.Count, (int iii) => {
                    foreach (string str in keySQL.listSqlReserved) {
                        //       string str = keySQL.listSqlReserved[iii];

                        string strTmp = " " + str + " ";
                        int iFindLength = strTmp.Length;
                        int index = -iFindLength;


                        if (iFindLength > 0) {
                            index = strOrig.IndexOf(strTmp, index + iFindLength);
                        lContinue:
                            if (index > -1) {
                                TextEffect te = new TextEffect(null, scbk, null, index, iFindLength - 1);

                                tec[i].Add(te);

                                if (index + iFindLength < iCurrentLineLength) {
                                    if (strOrig.Substring(index).Contains(strTmp)) index = strOrig.IndexOf(strTmp, index + iFindLength - 1);
                                    goto lContinue;
                                }
                            }
                        }
                    }


                    if (blnApostropheHighlight) {

                        //  intFirstCharOnPage = (int)scrollX.Value;
                        string strApost = "";
                        List<int> listApost = new List<int>();
                        if (listSBmain != null) {
                            strApost = listSBmain[i + intFirstLineOnPageUpdate].ToString();
                        }
                        else {
                            continue;
                        }



                        int intAstart = intFirstCharOnPageUpdate;
                        int intAstop = intFirstCharOnPageUpdate + intHorizCountCharsOnPage;
                        int intLength = listSBmain[i + intFirstLineOnPageUpdate].Length;

                        intAstart = intAstart < intLength ? intAstart : intLength - 1;
                        intAstop = intAstop < intLength ? intAstop : intLength;

                        bool blnStartStop = false;

                        for (int jjj = 0; jjj <= intAstart; jjj++) {
                            if (strApost[jjj] == '\'') blnStartStop = !blnStartStop;
                        }

                        int indexStart = 0;
                        if (blnStartStop == true) indexStart = intAstart;

                        SolidColorBrush cgbA = new SolidColorBrush(Colors.OrangeRed);
                        cgbA.Freeze();

                        for (int jjj = intAstart + 1; jjj < intAstop; jjj++) {

                            if (strApost[jjj] == '\'') {

                                if (blnStartStop == false) {
                                    indexStart = jjj;
                                }
                                else {

                                    TextEffect te = new TextEffect(null, cgbA, null, indexStart - intFirstCharOnPageUpdate, jjj - indexStart + 1);
                                    tec[i].Add(te);
                                }

                                blnStartStop = !blnStartStop;
                            }

                            if (jjj == intFirstCharOnPageUpdate + intHorizCountCharsOnPage - 1 && blnStartStop) {
                                TextEffect te = new TextEffect(null, cgbA, null, indexStart - intFirstCharOnPageUpdate, jjj - indexStart + 1);
                                tec[i].Add(te);
                            }
                        }
                    }

                    int indexGreen = -1;
                    if (listSBmain != null) {
                        if (listSBmain[i + intFirstLineOnPageUpdate].ToString().Contains("--")) {
                            indexGreen = listSBmain[i + intFirstLineOnPageUpdate].ToString().IndexOf("--");
                            int index = indexGreen - intFirstCharOnPageUpdate;

                            if (index < intHorizCountCharsOnPage) {
                                index = index < 0 ? 0 : index;
                                int intEnd = tb[i].Text.Length;

                                TextEffect te = new TextEffect();

                                if (intEnd - index >= 0) {
                                    te = new TextEffect(null, scbg, null, index, intEnd - index);
                                }
                                else return;

                                tec[i].Add(te);
                            }
                        }
                    }


     //        tb[i].TextEffects = tec[i];

             lexit: ;

                }  // end for
                //   });

            }
            catch (Exception ex)
            {

                MessageBox.Show("Program has a problem allocating additional char(s)/byte(s) in memory (currentHighlight) ...\nPlease save your  job ... ..."+ " (1)" + ex.Message);
                blnError = true;
            }
        }


        public void updateCanvasSelected(Brush myBrush) {

            try {



                int N = canvasSelected.Children.Count;
                for (int i = 0; i < N; i++) {
                    var sr = canvasSelected.Children[i] as selectedRectangle;
                    sr.myRec.Fill = null;
                };


                for (int i = 0; i < intLastLineOnPage - intFirstLineOnPageUpdate; i++) {
                    int isp = 0;

                    if (listSPMain.Count <= i + intFirstLineOnPageUpdate) continue;
                    if (listSPMain[i + intFirstLineOnPageUpdate].listSPinLine == null) continue;

                    foreach (selectedPositions sp in listSPMain[i + intFirstLineOnPageUpdate].listSPinLine) {
                        if (sp.iCollection == null || canvasSelected.Children.Count == 0) {
                            listSPMain[i + intFirstLineOnPageUpdate].listSPinLine[isp].iCollection = canvasSelected.Children.Add(set_Rectangle(i + intFirstLineOnPageUpdate, sp.iCharBegin, sp.iCharEnd, myBrush));
                        }
                        else {
                            try {
                                var recc = (canvasSelected.Children[(int)sp.iCollection]) as selectedRectangle;
                                if (recc != null) set_Rectangle(ref recc, i + intFirstLineOnPageUpdate, sp.iCharBegin, sp.iCharEnd, myBrush);
                                else listSPMain[i + intFirstLineOnPageUpdate].listSPinLine[isp].iCollection = canvasSelected.Children.Add(set_Rectangle(i + intFirstLineOnPageUpdate, sp.iCharBegin, sp.iCharEnd, myBrush));

                            }
                            catch {
                                sp.iCollection = null;
                            }
                        }
                        isp++;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Program has a problem allocating additional char(s)/byte(s) in memory (updateCanvasSelected) ...\nPlease save your  job ... ..." + " (2)" + ex.Message);
                blnError = true;
            }
        }


        public int intAbsolutSelectVertStart = 0;
        public int intAbsolutSelectHorizStart = 0;

        public void currentCanvasSelecting() // Making selectionMain for Moving Cursor // Do not Use Parallel here 
        {

            try {



                if (intAbsolutCaretHorizCurrent == intAbsolutSelectHorizStart && intAbsolutCaretVertCurrent == intAbsolutSelectVertStart && blnPlaceHolder == false) return;

                Color clr = Colors.Indigo;

                int intStartLine = intAbsolutSelectVertStart;     // Caret Selected Position 
                int intStartChar = intAbsolutSelectHorizStart;    // Caret Selected Position 

                int intStopLine = spMoveCurrent.iLineCurrent;      // spMoveCurrent
                int intStopChar = spMoveCurrent.iCharCurrent;      // spMoveCurrent

                if (blnPlaceHolder == false) {
                    if (intStartLine >= listSBmain.Count) intStartLine = listSBmain.Count - 1;
                    if (intStartChar >= listSBmain[intStartLine].Length) intStartChar = intAbsolutSelectHorizStart;
                    if (intStopLine >= listSBmain.Count) intStopLine = listSBmain.Count - 1;
                    if (intStopChar >= listSBmain[intStopLine].Length) intStopChar = intAbsolutCaretHorizCurrent;
                }




                if (intStartLine > intStopLine) {
                    int intTmp = intStartLine;
                    intStartLine = intStopLine;
                    intStopLine = intTmp;

                    intTmp = intStartChar;
                    intStartChar = intStopChar;
                    intStopChar = intTmp;
                }

                if (intStartLine == intStopLine && intStartChar > intStopChar) {
                    int intTmp = intStartChar;
                    intStartChar = intStopChar;
                    intStopChar = intTmp;
                }

                if (intStartLine < intFirstLineOnPageUpdate) {
                    intStartLine = intFirstLineOnPageUpdate;
                    intStartChar = 0;
                }
                if (intStopLine > intFirstLineOnPageUpdate + intVertCountLinesOnPage) {
                    intStopLine = intFirstLineOnPageUpdate + intVertCountLinesOnPage;
                    intStopChar = intFirstCharOnPageUpdate + intHorizCountCharsOnPage;
                }

                if (blnPenDeSelect == false && blnRecSelect == false && blnRecDeSelect == false && blnPlaceHolder == false) {
                    clr = Colors.Indigo;

                    selectedRectangle rec = new selectedRectangle();

                    Brush myBrush = new SolidColorBrush(clr);
                    // myBrush.Freeze();

                    Canvas c = new Canvas();  // This approach increase performance in 4..6 times // do not need to invalidate Rec which we already added
                    c = canvasSelecting;

                    for (int i = intStartLine; i <= intStopLine; i++) {

                        if (i == intStartLine && intStartLine != intStopLine) {
                            if (intStartChar >= listSBmain[i].Length) intStartChar = listSBmain[i].Length;

                            rec = set_Rectangle(i, intStartChar, listSBmain[i].Length, myBrush);
                            rec.Opacity = 0.2;
                        }

                        if (i == intStartLine && intStartLine == intStopLine) {

                            if (intStopChar >= listSBmain[i].Length) intStopChar = listSBmain[i].Length;
                            if (intStartChar >= listSBmain[i].Length) intStartChar = listSBmain[i].Length;

                            rec = set_Rectangle(i, intStartChar, intStopChar, myBrush);
                            rec.Opacity = 0.2;
                        }

                        if (i == intStopLine && intStartLine != intStopLine) {
                            if (intStopChar >= listSBmain[i].Length) intStopChar = listSBmain[i].Length;
                            rec = set_Rectangle(i, 0, intStopChar, myBrush);
                            rec.Opacity = 0.2;
                        }

                        if (intStartLine < i && i < intStopLine) {
                            rec = set_Rectangle(i, 0, listSBmain[i].Length, myBrush);
                            rec.Opacity = 0.2;
                        }

                        Canvas cc = new Canvas();

                        Dispatcher.Invoke(new Action(() => {
                            c.Children.Add(rec);
                            c.Children.Add(cc);
                            c = cc;
                        }));

                    }
                }




                if (blnPenDeSelect == true && blnRecSelect == false && blnRecDeSelect == false && blnPlaceHolder == false) {
                    clr = Colors.Gainsboro;

                    selectedRectangle rec = new selectedRectangle();

                    Brush myBrush = new SolidColorBrush(clr);
                    //myBrush.Freeze();

                    Canvas c = new Canvas();
                    c = canvasSelecting;

                    for (int i = intStartLine; i <= intStopLine; i++) {

                        if (i == intStartLine && intStartLine != intStopLine) {
                            if (intStartChar >= listSBmain[i].Length) intStartChar = listSBmain[i].Length;

                            rec = set_Rectangle(i, intStartChar, listSBmain[i].Length, myBrush);
                            rec.Opacity = 0.9;
                        }

                        if (i == intStartLine && intStartLine == intStopLine) {

                            if (intStopChar >= listSBmain[i].Length) intStopChar = listSBmain[i].Length;
                            if (intStartChar >= listSBmain[i].Length) intStartChar = listSBmain[i].Length;

                            rec = set_Rectangle(i, intStartChar, intStopChar, myBrush);
                            rec.Opacity = 0.9;
                        }

                        if (i == intStopLine && intStartLine != intStopLine) {
                            if (intStopChar >= listSBmain[i].Length) intStopChar = listSBmain[i].Length;
                            rec = set_Rectangle(i, 0, intStopChar, myBrush);
                            rec.Opacity = 0.9;
                        }

                        if (intStartLine < i && i < intStopLine) {
                            rec = set_Rectangle(i, 0, listSBmain[i].Length, myBrush);
                            rec.Opacity = 0.9;
                        }

                        Canvas cc = new Canvas();

                        Dispatcher.Invoke(new Action(() => {
                            c.Children.Add(rec);
                            c.Children.Add(cc);
                            c = cc;
                        }));

                    }
                }

                if (blnPenDeSelect == false && blnRecSelect == true && blnRecDeSelect == false && blnPlaceHolder == false) {
                    clr = Colors.Indigo;

                    if (intStartLine == intStopLine) {
                        intStartLine = intFirstLineOnPageUpdate;
                        intStopLine = intFirstLineOnPageUpdate + intVertCountLinesOnPage - 1;
                        intStopLine = intStopLine >= listSBmain.Count ? listSBmain.Count - 1 : intStopLine;
                    }

                    if (intStartChar > intStopChar) {

                        int intTmp = intStartChar;
                        intStartChar = intStopChar;
                        intStopChar = intTmp;
                    }


                    selectedRectangle rec = new selectedRectangle();

                    Brush myBrush = new SolidColorBrush(clr);
                    // myBrush.Freeze();

                    Canvas c = new Canvas();
                    c = canvasSelecting;

                    for (int i = intStartLine; i <= intStopLine; i++) {
                        if (intStartChar == intStopChar) {
                            rec = set_Rectangle(i, 0, listSBmain[i].Length, myBrush);

                        }
                        else {
                            rec = set_Rectangle(i, intStartChar, intStopChar, myBrush);  // intAbsolutCaretHorizCurrent
                        }


                        if (rec == null) continue;
                        rec.Opacity = 0.2;

                        Canvas cc = new Canvas();

                        Dispatcher.Invoke(new Action(() => {
                            c.Children.Add(rec);
                            c.Children.Add(cc);
                            c = cc;
                        }));

                    }
                }

                if (blnPenDeSelect == false && blnRecSelect == false && blnRecDeSelect == true && blnPlaceHolder == false) {
                    clr = Colors.Gainsboro;

                    if (intStartLine == intStopLine) {
                        intStartLine = intFirstLineOnPageUpdate;
                        intStopLine = intFirstLineOnPageUpdate + intVertCountLinesOnPage - 1;
                        intStopLine = intStopLine >= listSBmain.Count ? listSBmain.Count - 1 : intStopLine;
                    }

                    if (intStartChar > intStopChar) {

                        int intTmp = intStartChar;
                        intStartChar = intStopChar;
                        intStopChar = intTmp;
                    }


                    selectedRectangle rec = new selectedRectangle();
                    Brush myBrush = new SolidColorBrush(clr);
                    //myBrush.Freeze();


                    Canvas c = new Canvas();
                    c = canvasSelecting;

                    for (int i = intStartLine; i <= intStopLine; i++) {

                        if (intStartChar == intStopChar) {
                            rec = set_Rectangle(i, 0, listSBmain[i].Length, myBrush);
                        }
                        else {
                            rec = set_Rectangle(i, intStartChar, intStopChar, myBrush);
                        }


                        rec.Opacity = 0.9;

                        Canvas cc = new Canvas();

                        Dispatcher.Invoke(new Action(() => {
                            c.Children.Add(rec);
                            c.Children.Add(cc);
                            c = cc;
                        }));

                    }
                }

                if (blnPenDeSelect == false && blnRecSelect == false && blnRecDeSelect == false && blnPlaceHolder == true) {
                    clr = Colors.Indigo;

                    if (intStartLine == intStopLine) {
                        intStartLine = intFirstLineOnPageUpdate;
                        intStopLine = intFirstLineOnPageUpdate + intVertCountLinesOnPage - 1;
                        intStopLine = intStopLine >= listSBmain.Count ? listSBmain.Count - 1 : intStopLine;
                    }

                    if (intStartChar > intStopChar) {
                        int intTmp = intStartChar;
                        intStartChar = intStopChar;
                        intStopChar = intTmp;
                    }


                    if (intStartLine > listSBmain.Count - 1) intStartLine = listSBmain.Count;


                    selectedRectangle rec = new selectedRectangle();

                    Brush myBrush = new SolidColorBrush(clr);
                    // myBrush.Freeze();

                    Canvas c = new Canvas();
                    c = canvasSelecting;

                    for (int i = intStartLine; i <= intStopLine; i++) {


                        rec = set_Rectangle(i, intAbsolutCaretHorizCurrent, intAbsolutCaretHorizCurrent, myBrush);  // intAbsolutCaretHorizCurrent


                        if (rec == null) continue;
                        rec.Opacity = 0.2;

                        Canvas cc = new Canvas();

                        Dispatcher.Invoke(new Action(() => {
                            c.Children.Add(rec);
                            c.Children.Add(cc);
                            c = cc;
                        }));

                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Program has a problem allocating additional char(s)/byte(s) in memory (currentCanvasSelecting) ...\nPlease save your  job ... ..." + " (3)" + ex.Message);
                blnError = true;
            }

        }



        public void currentSelectionAddtoSP() // Making selectionMain for Moving Cursor
        {
            try {



                if (listSBmain == null) return;

                if (listSBmain.Count == 0) return;

                //if (intAbsolutCaretHorizCurrent == intAbsolutSelectHorizStart &&
                //    intAbsolutCaretVertCurrent == intAbsolutSelectVertStart && blnAppend == false) return;

                if (intAbsolutCaretHorizCurrent == intAbsolutSelectHorizStart &&
                    intAbsolutCaretVertCurrent == intAbsolutSelectVertStart) return;
                if (Keyboard.Modifiers == ModifierKeys.Control) return; // Do nothing if CTRL



                int intStartLine = intAbsolutSelectVertStart;     // Caret Selected Position 
                int intStartChar = intAbsolutSelectHorizStart;    // Caret Selected Position 

                int intStopLine = spMoveCurrent.iLineCurrent;      // spMoveCurrent
                int intStopChar = spMoveCurrent.iCharCurrent;      // spMoveCurrent

                if (blnPlaceHolder == false) {
                    if (intStartLine >= listSBmain.Count) intStartLine = listSBmain.Count - 1;
                    //   if (intStartChar >= listSBmain[intStartLine].Length) intStartChar = listSBmain[intStartLine].Length;
                    if (intStartChar >= listSBmain[intStartLine].Length) intStartChar = intAbsolutSelectHorizStart;
                    if (intStopLine >= listSBmain.Count) intStopLine = listSBmain.Count - 1;

                    if (intStartLine > listSBmain.Count - 1 && blnPlaceHolder == true) intStartLine = listSBmain.Count;
                    //  if (intStopChar >= listSBmain[intStopLine].Length) intStopChar = listSBmain[intStopLine].Length;
                    if (intStopChar >= listSBmain[intStopLine].Length) intStopChar = intAbsolutCaretHorizCurrent;
                }



                if (intStartLine > intStopLine) {
                    int intTmp = intStartLine;
                    intStartLine = intStopLine;
                    intStopLine = intTmp;

                    intTmp = intStartChar;
                    intStartChar = intStopChar;
                    intStopChar = intTmp;
                }

                if (intStartLine == intStopLine && intStartChar > intStopChar) {
                    int intTmp = intStartChar;
                    intStartChar = intStopChar;
                    intStopChar = intTmp;
                }

                //-----------------------------------------------------------------------------------------------
                if (blnPenDeSelect == false && blnRecSelect == false && blnRecDeSelect == false && blnPlaceHolder == false) {

                    Parallel.For(intStartLine, intStopLine + 1, (int i) =>
                        //for (int i = intStartLine; i <= intStopLine; i++) 
                    {

                        if (i == intStartLine && intStartLine != intStopLine) {
                            if (intStartChar >= listSBmain[i].Length) intStartChar = listSBmain[i].Length;

                            addSP(i, intStartChar, listSBmain[i].Length, blnAppend);
                        }

                        if (i == intStartLine && intStartLine == intStopLine) {

                            if (intStopChar >= listSBmain[i].Length) intStopChar = listSBmain[i].Length;
                            if (intStartChar >= listSBmain[i].Length) intStartChar = listSBmain[i].Length;

                            addSP(i, intStartChar, intStopChar, blnAppend);
                        }

                        if (i == intStopLine && intStartLine != intStopLine) {
                            if (intStopChar >= listSBmain[i].Length) intStopChar = listSBmain[i].Length;
                            addSP(i, 0, intStopChar, blnAppend);
                        }

                        if (intStartLine < i && i < intStopLine) {
                            addSP(i, 0, listSBmain[i].Length, blnAppend);
                        }

                    });
                }
                //-----------------------------------------------------------------------------------------------
                //-----------------------------------------------------------------------------------------------
                if (blnPenDeSelect == true && blnRecSelect == false && blnRecDeSelect == false && blnPlaceHolder == false) {

                    Parallel.For(intStartLine, intStopLine + 1, (int i) =>

                 //   for (int i = intStartLine; i <= intStopLine; i++)
                    {

                        if (i == intStartLine && intStartLine != intStopLine) {
                            if (intStartChar >= listSBmain[i].Length) intStartChar = listSBmain[i].Length;

                            deSelectSP(i, intStartChar, listSBmain[i].Length, blnAppend);
                        }

                        if (i == intStartLine && intStartLine == intStopLine) {

                            if (intStopChar >= listSBmain[i].Length) intStopChar = listSBmain[i].Length;
                            if (intStartChar >= listSBmain[i].Length) intStartChar = listSBmain[i].Length;

                            deSelectSP(i, intStartChar, intStopChar, blnAppend);
                        }

                        if (i == intStopLine && intStartLine != intStopLine) {
                            if (intStopChar >= listSBmain[i].Length) intStopChar = listSBmain[i].Length;
                            deSelectSP(i, 0, intStopChar, blnAppend);
                        }

                        if (intStartLine < i && i < intStopLine) {
                            deSelectSP(i, 0, listSBmain[i].Length, blnAppend);
                        }

                    });
                }
                //-----------------------------------------------------------------------------------------------
                //-----------------------------------------------------------------------------------------------

                if (blnPenDeSelect == false && blnRecSelect == true && blnRecDeSelect == false && blnPlaceHolder == false) {
                    if (intStartLine == intStopLine) {
                        intStartLine = intFirstLineOnPageUpdate;
                        intStopLine = intFirstLineOnPageUpdate + intVertCountLinesOnPage - 1;
                        intStopLine = intStopLine >= listSBmain.Count ? listSBmain.Count - 1 : intStopLine;
                    }

                    if (intStartChar > intStopChar) {

                        int intTmp = intStartChar;
                        intStartChar = intStopChar;
                        intStopChar = intTmp;
                    }

                    int intB = intStartLine;
                    int intE = intStopLine;

                    if (intAbsolutCaretVertCurrent == intAbsolutSelectVertStart) {
                        intB = 0;
                        intE = listSBmain.Count - 1;
                    }

                    Parallel.For(intB, intE + 1, (int i) =>

                   // for (int i = intB; i <= intE; i++) 
                    {
                        if (intStartChar > listSBmain[i].Length && intStartChar != intStopChar) goto lexit;


                        if (intStartChar == intStopChar) {
                            addSP(i, 0, listSBmain[i].Length, blnAppend);
                        }
                        else {
                            int intTmp = intStopChar > listSBmain[i].Length ? listSBmain[i].Length : intStopChar;
                            addSP(i, intStartChar, intTmp, blnAppend);   // intAbsolutCaretHorizCurrent
                        }

                    lexit: ;

                    });

                }
                //-----------------------------------------------------------------------------------------------

                //-----------------------------------------------------------------------------------------------
                if (blnPenDeSelect == false && blnRecSelect == false && blnRecDeSelect == true && blnPlaceHolder == false) {
                    if (intStartLine == intStopLine) {
                        intStartLine = intFirstLineOnPageUpdate;
                        intStopLine = intFirstLineOnPageUpdate + intVertCountLinesOnPage - 1;
                        intStopLine = intStopLine >= listSBmain.Count ? listSBmain.Count - 1 : intStopLine;
                    }

                    if (intStartChar > intStopChar) {

                        int intTmp = intStartChar;
                        intStartChar = intStopChar;
                        intStopChar = intTmp;
                    }


                    int intB = intStartLine;
                    int intE = intStopLine;

                    if (intAbsolutCaretVertCurrent == intAbsolutSelectVertStart) {
                        intB = 0;
                        intE = listSBmain.Count - 1;
                    }

                    Parallel.For(intB, intE + 1, (int i) =>
                       //for (int i = intB; i <= intE; i++)
                   {
                       if (intStartChar > listSBmain[i].Length && intStartChar != intStopChar) goto lexit;

                       if (intStartChar == intStopChar) {
                           deSelectSP(i, 0, listSBmain[i].Length, blnAppend);
                       }
                       else {
                           int intTmp = intStopChar > listSBmain[i].Length ? listSBmain[i].Length : intStopChar;
                           deSelectSP(i, intStartChar, intTmp, blnAppend); ;
                       }

                   lexit: ;
                   });

                }

                if (blnPenDeSelect == false && blnRecSelect == false && blnRecDeSelect == false && blnPlaceHolder == true) {

                    if (intStartLine == intStopLine) {
                        intStartLine = intFirstLineOnPageUpdate;
                        intStopLine = intFirstLineOnPageUpdate + intVertCountLinesOnPage - 1;
                        intStopLine = intStopLine >= listSBmain.Count ? listSBmain.Count - 1 : intStopLine;
                    }
                    if (intStartLine > listSBmain.Count - 1) intStartLine = listSBmain.Count;

                    int intB = intStartLine;
                    int intE = intStopLine;

                    if (intAbsolutCaretVertCurrent == intAbsolutSelectVertStart) {


                        intB = 0;
                        intE = listSBmain.Count - 1;





                    }

                    // No Parallel

                    for (int i = intB; i <= intE; i++) {
                        intStartChar = intAbsolutCaretHorizCurrent;
                        addSP(i, intStartChar, intStartChar, blnAppend);
                    }

                    //totalBytes();
                    //     init_HorizontalScroll();
                    //     init_VerticalScroll();


                }
                //-----------------------------------------------------------------------------------------------




                //-----------------------------------------------------------------------------------------------
            }
            catch (Exception ex)
            {
                MessageBox.Show("Program has a problem allocating additional char(s)/byte(s) in memory (currentSelectionAddtoSP) ...\nPlease save your  job ... ..." + " (4)" + ex.Message);
                blnError = true;
            }

        }


        public void addSelectedCanvas(bool blnForceAdd) {  //each SelectedPosition must has a selectedRectangle
            try {

                canvasSelecting.Children.Clear();
                if (blnForceAdd) canvasSelected.Children.Clear();

                foreach (object obj in canvasSelected.Children) {
                    selectedRectangle sr = obj as selectedRectangle;
                    //Brush br = new SolidColorBrush(Colors.Transparent);
                    sr.myRec.Fill = null;
                }


                for (int i = 0; i < listSPMain.Count; i++) {

                    for (int isp = 0; isp < listSPMain[i].listSPinLine.Count; isp++) {
                        selectedPositions sp = listSPMain[i].listSPinLine[isp];
                        if (sp.iCollection == null || blnForceAdd) {
                            Dispatcher.Invoke(new Action(() => {
                                listSPMain[i].listSPinLine[isp].iCollection = canvasSelected.Children.Add(set_Rectangle(i, sp.iCharBegin, sp.iCharEnd, null));
                            }));
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Program has a problem allocating additional char(s)/byte(s) in memory (addSelectedCanvas) ...\nPlease save your  job ... ..." + " (5)" + ex.Message);
                blnError = true;
            }

        }


        public void somthingSelected() {
            blnSomthingSelected = false;

            if (listSPMain == null || listSPMain.Count == 0) return;
            int intLastLineOnPage = (intFirstLineOnPageUpdate + intVertCountLinesOnPage) < listSPMain.Count ? (intFirstLineOnPageUpdate + intVertCountLinesOnPage) : (listSPMain.Count);

            //      Parallel.For(intFirstLineOnPageUpdate, intLastLineOnPage, (int i, ParallelLoopState loop) =>
            //      {
            Parallel.For(0, listSPMain.Count, (int i, ParallelLoopState loop) => {
                foreach (selectedPositions sp in listSPMain[i].listSPinLine) {
                    if (listSPMain[i].listSPinLine.Count == 0) continue;
                    else {
                        blnSomthingSelected = true;
                        loop.Break();
                    }

                }
            });
        }



        public long totalBytes() {

            longTotalDocSize = 0;
            intMaxLineSize = 0;
            foreach (StringBuilder sb in listSBmain) {
                longTotalDocSize += sb.Length;
                intMaxLineSize = intMaxLineSize < sb.Length ? sb.Length : intMaxLineSize; // Count MAX line size
            }
            return longTotalDocSize;
        }

        private void txtBlock_SizeChanged(object sender, SizeChangedEventArgs e) {

            blnMouseLeftButtonPressed = true;

            e.Handled = true;
            //    init_HorizontalScroll();
            //    init_VerticalScroll();

            updateBlock(); //++++
        }

        private void scrollV_MouseWheel(object sender, MouseWheelEventArgs e) {
            if (Keyboard.Modifiers == ModifierKeys.Control) return; // Do nothing if CTRL


            if (e.Delta > 0) {
                scrollY.Value = intFirstLineOnPageUpdate - 3;
            }
            else {
                scrollY.Value = intFirstLineOnPageUpdate + 3;
            }

            if (blnMouseLeftButtonPressed && !blnMouseRightButtonPressed) spCurrentSelectionByMouse();

            //    updateBlock(); //++++
            //    canvasMain.Focus();
        }

        private void scrollH_MouseWheel(object sender, MouseWheelEventArgs e) {
            if (Keyboard.Modifiers == ModifierKeys.Control) return;

            if (e.Delta > 0) {
                scrollX.Value = intFirstCharOnPageUpdate - 7;
            }
            else {
                scrollX.Value = intFirstCharOnPageUpdate + 7;
            }

            if (blnMouseLeftButtonPressed && !blnMouseRightButtonPressed) spCurrentSelectionByMouse();
            //  updateBlock();   //++++
            //  canvasMain.Focus();
        }

        private void tb_MouseMove(object sender, MouseEventArgs e) {
            e.Handled = true;

            int intRelativeX = (int)((Mouse.GetPosition(canvasMain).X + tb_FontSize / 4) * coeffFont_Widh / tb_FontSize);
            int intRelativeY = (int)(Mouse.GetPosition(canvasMain).Y * coeffFont_High / tb_FontSize);

            tbXY.Text = "X:" + intRelativeX.ToString() + "   Y:" + intRelativeY.ToString();


            if (blnMouseLeftButtonPressed == true && e.LeftButton == MouseButtonState.Released) {
                mouseLeftUp();
                return;
            }

            double x = (Mouse.GetPosition(canvasMain).X - pointMouseLeftButtonPressed.X);
            double y = (Mouse.GetPosition(canvasMain).Y - pointMouseLeftButtonPressed.Y);

            int intHorizCurrent = (int)((x + tb_FontSize / 4) * coeffFont_Widh / tb_FontSize);
            int intVertCurrent = (int)(y * coeffFont_High / tb_FontSize);

            if (e.MiddleButton == MouseButtonState.Pressed) {

                canvasMain.Cursor = Cursors.Hand;
                scrollX.Value = intPressedPositionX - intHorizCurrent;
                scrollY.Value = intPressedPositionY - intVertCurrent;
            }



            //Select Section - when mouse move

            if (blnMouseLeftButtonPressed && Keyboard.Modifiers != ModifierKeys.Control) { //#FFF2F5F8   spMoveCurrent  

                spCurrentSelectionByMouse();
                if (intAbsolutCaretVertCurrent == intFirstLineOnPageUpdate + intVertCountLinesOnPage) scrollY.Value += 3;
                if (intAbsolutCaretVertCurrent == intFirstLineOnPageUpdate) scrollY.Value -= 3;
                if (intAbsolutCaretHorizCurrent == intFirstCharOnPageUpdate + intHorizCountCharsOnPage) scrollX.Value += 3;
                if (intAbsolutCaretHorizCurrent == intFirstCharOnPageUpdate) scrollX.Value -= 3;
            }

            canvasMain.Cursor = Cursors.IBeam;
        }


        private void mouseLeftUp() {
            currentSelectionAddtoSP();
            blnSomthingSelected = true;
            blnMouseLeftButtonPressed = false;

            updateBlock(); //++++
        }

        private void spCurrentSelectionByMouse() {
            try {

                Point p = Mouse.GetPosition(canvasMain);
                // Current Cursor

                spMoveCurrent.iDeltaY = (int)(p.Y / tb_FontSize * coeffFont_High);
                spMoveCurrent.iLineCurrent = intFirstLineOnPageUpdate + spMoveCurrent.iDeltaY;

                spMoveCurrent.iDeltaX = (int)((p.X + tb_FontSize / 4) * coeffFont_Widh / tb_FontSize);
                spMoveCurrent.iCharCurrent = intFirstCharOnPageUpdate + spMoveCurrent.iDeltaX;
                if (p.X <= 0) spMoveCurrent.iCharCurrent = 0;

                if (!spMoveCurrent.Equals(spMovePrevious)) {

                    intAbsolutCaretVertCurrent = spMoveCurrent.iLineCurrent;
                    intAbsolutCaretHorizCurrent = spMoveCurrent.iCharCurrent;

                    updateBlock();  //++++
                    spMovePrevious = new selectedPositionsCurrent(spMoveCurrent);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Program has a problem allocating additional char(s)/byte(s) in memory (spCurrentSelectionByMouse) ...\nPlease save your  job ... ..." + " (6)" + ex.Message);
                blnError = true;
            }
        }

        selectedPositionsCurrent spMoveCurrent = new selectedPositionsCurrent();
        selectedPositionsCurrent spMovePrevious = new selectedPositionsCurrent();

        Point pointMouseLeftButtonPressed;
        int intPressedPositionY = 0;
        int intPressedPositionX = 0;


        private void ftb_PreviewMouseUp(object sender, MouseButtonEventArgs e) {
            //    blnMouseLeftButtonPressed = false;
            canvasMain.Cursor = Cursors.IBeam;
        }

        private void tbX_MouseEnter(object sender, MouseEventArgs e) {
            scrollX.Focus();
        }

        private void tbY_MouseEnter(object sender, MouseEventArgs e) {
            scrollY.Focus();
        }

        //----------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------

        private void Cut_Click(object sender, RoutedEventArgs e) {

        }

        private void tb_LayoutUpdated(object sender, EventArgs e) {

        }

        public void tb_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            mouseLeftUp();

        }




        private void tb_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e) {
            blnMouseRightButtonPressed = true;
            //set_Caret(intAbsolutCaretVertCurrent, intAbsolutCaretHorizCurrent);
            //correctLocation();
        }



        private void tb_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            try {
                blnAbort = false;

                resetCM();
                listBoxHelper.Visibility = Visibility.Collapsed;

                if (blnMouseLeftButtonPressed == true) return;


                if (listSBmain.Count == 0 || listSBmain == null) {
                    Dispatcher.Invoke(new Action(() => set_Caret(0, 0)));
                    return;
                }


                blnMouseLeftButtonPressed = true;
                if (blnAppend == false) DeSelectAllCurrent();

                setMousePositionForMoving();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Program has a problem allocating additional char(s)/byte(s) in memory (tb_PreviewMouseLeftButtonDown) ...\nPlease save your  job ... ..." + " (7)" + ex.Message);
                blnError = true;
            }


        }


        private void setMousePositionForMoving() {
            try {

                pointMouseLeftButtonPressed = Mouse.GetPosition(canvasMain);
                Point p = Mouse.GetPosition(canvasMain);

                intAbsolutCaretVertCurrent = intFirstLineOnPageUpdate + (int)(p.Y / tb_FontSize * coeffFont_High); // Do NOT combine first and second on same (long)
                intAbsolutCaretHorizCurrent = intFirstCharOnPageUpdate + (int)((p.X + tb_FontSize / 4) / tb_FontSize * coeffFont_Widh);

                intPressedPositionY = intFirstLineOnPageUpdate;
                intPressedPositionX = intFirstCharOnPageUpdate;

                if (Keyboard.Modifiers != ModifierKeys.Shift) {
                    intAbsolutSelectVertStart = intAbsolutCaretVertCurrent;   // Save/Select Start Position for Selection
                    intAbsolutSelectHorizStart = intAbsolutCaretHorizCurrent; // Save/Select Start Position for Selection
                }

                Dispatcher.Invoke(new Action(() => set_Caret(intAbsolutCaretVertCurrent, intAbsolutCaretHorizCurrent)));
                canvasMain.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Program has a problem allocating additional char(s)/byte(s) in memory (setMousePositionForMoving) ...\nPlease save your  job ... ..." + " (8)" + ex.Message);
                blnError = true;
            }
        }


        private void updateSelection() {
            try {
                currentSelectionAddtoSP();
                blnSomthingSelected = true;
                blnMouseLeftButtonPressed = false;
                updateBlock();  // +++++
            }
            catch (Exception ex)
            {
                MessageBox.Show("Program has a problem allocating additional char(s)/byte(s) in memory (updateSelection) ...\nPlease save your  job ... ..." + " (9)" + ex.Message);
                blnError = true;
            }

        }



        // Key Down ----------------------------------------------------------------------------------------------------------------
        private string strOEM = "";





        private void tb_PreviewKeyDown(object sender, KeyEventArgs e) {
            e.Handled = true;
            strOEM = "";
            blnAbort = false;

            try {


                if (listSBmain.Count == 0 || listSBmain == null) {
                    listSBmain.Add(new StringBuilder(""));
                    listSPMain.Insert(0, new listOfSelectedPositionInLine());

                    intAbsolutCaretHorizCurrent = 0;
                    intAbsolutCaretVertCurrent = 0;
                    goto lexit;
                }

                somthingSelected();

                //blnSomthingSelected = true;


                if (e.Key == Key.Insert) {
                    blnCaretInsert = !blnCaretInsert;
                    listBoxHelper.Visibility = Visibility.Collapsed;
                    goto lexit;
                }

                if (e.Key == Key.Left) {

                    //if (Keyboard.Modifiers == ModifierKeys.Shift) {  // ??????????????????????????
                    //    spMoveCurrent.iLineCurrent = intAbsolutCaretVertCurrent;
                    //    spMoveCurrent.iCharCurrent = intAbsolutCaretHorizCurrent;
                    //}


                    if (intFirstCharOnPageUpdate < intAbsolutCaretHorizCurrent) {
                        intAbsolutCaretHorizCurrent -= 1;
                    }
                    else {
                        scrollX.Value -= 1;
                        intAbsolutCaretHorizCurrent = intFirstCharOnPageUpdate;
                    }
                    listBoxHelper.Visibility = Visibility.Collapsed;
                    goto lexit;
                }

                if (e.Key == Key.Right) {

                    //if (Keyboard.Modifiers == ModifierKeys.Shift) {  // ??????????????????????????
                    //    spMoveCurrent.iLineCurrent = intAbsolutCaretVertCurrent;
                    //    spMoveCurrent.iCharCurrent = intAbsolutCaretHorizCurrent;
                    //}

                    if (intFirstCharOnPageUpdate + intHorizCountCharsOnPage - 1 > intAbsolutCaretHorizCurrent) {
                        intAbsolutCaretHorizCurrent += 1;
                    }
                    else {
                        scrollX.Value += 1;
                        intAbsolutCaretHorizCurrent = intFirstCharOnPageUpdate + intHorizCountCharsOnPage - 1;
                    }
                    listBoxHelper.Visibility = Visibility.Collapsed;
                    goto lexit;
                }

                if (e.Key == Key.Up) {

                    //if (Keyboard.Modifiers == ModifierKeys.Shift) {  // ??????????????????????????
                    //    spMoveCurrent.iLineCurrent = intAbsolutCaretVertCurrent;
                    //    spMoveCurrent.iCharCurrent = intAbsolutCaretHorizCurrent;
                    //}
                    if (strHistory.Trim() != "") {
                        listBoxHelper.Focus();
                        return;
                    }

                    if (intFirstLineOnPageUpdate < intAbsolutCaretVertCurrent) {
                        intAbsolutCaretVertCurrent -= 1;
                    }
                    else {
                        scrollY.Value -= 1;
                        intAbsolutCaretVertCurrent = intFirstLineOnPageUpdate;
                    }

                    ///////////////////////////////////////////////

                    goto lexit;
                }

                if (e.Key == Key.Down) {

                    //if (Keyboard.Modifiers == ModifierKeys.Shift) {  // ??????????????????????????
                    //    spMoveCurrent.iLineCurrent = intAbsolutCaretVertCurrent;
                    //    spMoveCurrent.iCharCurrent = intAbsolutCaretHorizCurrent;
                    //}

                    if (strHistory.Trim() != "") {
                        listBoxHelper.Focus();
                        return;
                    }

                    if (intFirstLineOnPageUpdate + intVertCountLinesOnPage - 1 > intAbsolutCaretVertCurrent) {
                        intAbsolutCaretVertCurrent += 1;
                    }
                    else {
                        scrollY.Value += 1;
                        intAbsolutCaretVertCurrent = intFirstLineOnPageUpdate + intVertCountLinesOnPage - 1;
                    }

                    ///////////////////////////////////////////////


                    goto lexit;
                }

                if (e.Key == Key.Home) {

                    //if (Keyboard.Modifiers == ModifierKeys.Shift) {  // ??????????????????????????
                    //    spMoveCurrent.iLineCurrent = intAbsolutCaretVertCurrent;
                    //    spMoveCurrent.iCharCurrent = intAbsolutCaretHorizCurrent;
                    //}


                    correctLocation();
                    if (Keyboard.Modifiers == ModifierKeys.Control) {
                        intAbsolutCaretVertCurrent = 0;
                        scrollY.Value = 0;
                    }

                    intAbsolutCaretHorizCurrent = 0;
                    scrollX.Value = 0;

                    listBoxHelper.Visibility = Visibility.Collapsed;
                    goto lexit;
                }

                if (e.Key == Key.End) {
                    //if (Keyboard.Modifiers == ModifierKeys.Shift) {  // ??????????????????????????
                    //    spMoveCurrent.iLineCurrent = intAbsolutCaretVertCurrent;
                    //    spMoveCurrent.iCharCurrent = intAbsolutCaretHorizCurrent;
                    //}


                    correctLocation();
                    if (Keyboard.Modifiers == ModifierKeys.Control) {
                        intAbsolutCaretHorizCurrent = listSBmain[listSBmain.Count - 1].Length;
                        intAbsolutCaretVertCurrent = listSBmain.Count - 1;
                        scrollX.Value = listSBmain[listSBmain.Count - 1].Length - intHorizCountCharsOnPage + 1;
                        scrollY.Value = listSBmain.Count - intVertCountLinesOnPage;
                    }

                    intAbsolutCaretHorizCurrent = listSBmain[(int)intAbsolutCaretVertCurrent].Length;
                    scrollX.Value = listSBmain[(int)intAbsolutCaretVertCurrent].Length - intHorizCountCharsOnPage + 1;

                    listBoxHelper.Visibility = Visibility.Collapsed;
                    goto lexit;
                }


                if (Keyboard.Modifiers == ModifierKeys.Control && e.Key.ToString() == "F") {
                    scrollY.Value = intAbsolutCaretVertCurrent;
                    scrollX.Value = intAbsolutCaretHorizCurrent;

                    listBoxHelper.Visibility = Visibility.Collapsed;
                    goto lexit;
                }

                // Added 
                if (Keyboard.Modifiers == ModifierKeys.Control && blnMouseLeftButtonPressed && blnAppend) {


                    if (intAbsolutCaretVertCurrent < listSBmain.Count) {
                        if (intAbsolutCaretHorizCurrent <= listSBmain[intAbsolutCaretVertCurrent].Length) {

                            intAbsolutSelectHorizStart = intAbsolutCaretHorizCurrent;
                            intAbsolutSelectVertStart = intAbsolutCaretVertCurrent;

                            addSP(intAbsolutCaretVertCurrent, intAbsolutCaretHorizCurrent, intAbsolutCaretHorizCurrent, blnAppend);

                            updateBlock();
                        }
                    }

                }



                if (Keyboard.Modifiers == ModifierKeys.Control && e.Key.ToString() == "S") {

                    intNextAbsolutSelectionLine = (int)scrollY.Value;
                    intNextAbsolutSelectionHorizont = (int)scrollX.Value;

                    for (int i = intNextAbsolutSelectionLine; i < listSPMain.Count; i++) {
                        if (listSPMain[i].listSPinLine.Count == 0) continue;
                        for (int j = 0; j < listSPMain[i].listSPinLine.Count; j++) {
                            selectedPositions sp = listSPMain[i].listSPinLine[j];
                            if ((sp.iCharBegin > intNextAbsolutSelectionHorizont && i == intNextAbsolutSelectionLine) ||
                                 (i > intNextAbsolutSelectionLine)) {
                                intNextAbsolutSelectionLine = i;
                                intNextAbsolutSelectionHorizont = sp.iCharBegin;

                                scrollY.Value = intNextAbsolutSelectionLine;
                                scrollX.Value = intNextAbsolutSelectionHorizont;

                                goto lexit;
                            }
                        }
                    }

                    listBoxHelper.Visibility = Visibility.Collapsed;
                    goto lexit;
                }


                if (e.Key == Key.PageUp) {

                    //if (Keyboard.Modifiers == ModifierKeys.Shift) {  // ??????????????????????????
                    //    spMoveCurrent.iLineCurrent = intAbsolutCaretVertCurrent;
                    //    spMoveCurrent.iCharCurrent = intAbsolutCaretHorizCurrent;
                    //}



                    if (Keyboard.Modifiers == ModifierKeys.Control) {
                        intAbsolutCaretVertCurrent = 0;
                        scrollY.Value = 0;
                    }

                    scrollY.Value -= intVertCountLinesOnPage;

                    if ((intAbsolutCaretVertCurrent - intVertCountLinesOnPage) > 0 && (intFirstLineOnPageUpdate - intVertCountLinesOnPage) > 0) {
                        intAbsolutCaretVertCurrent -= intVertCountLinesOnPage;
                    }
                    else {
                        scrollY.Value = 0;
                        intAbsolutCaretVertCurrent = 0;
                    }

                    listBoxHelper.Visibility = Visibility.Collapsed;
                    goto lexit;
                }

                if (e.Key == Key.PageDown) {

                    //if (Keyboard.Modifiers == ModifierKeys.Shift) {  // ??????????????????????????
                    //    spMoveCurrent.iLineCurrent = intAbsolutCaretVertCurrent;
                    //    spMoveCurrent.iCharCurrent = intAbsolutCaretHorizCurrent;
                    //}


                    if (Keyboard.Modifiers == ModifierKeys.Control) {
                        scrollY.Value = listSBmain.Count - intVertCountLinesOnPage + 1;
                        intAbsolutCaretVertCurrent = listSBmain.Count - 1;
                    }

                    scrollY.Value += intVertCountLinesOnPage;

                    if (listSBmain.Count - 1 > intFirstLineOnPageUpdate) {
                        intAbsolutCaretVertCurrent += intVertCountLinesOnPage;
                    }
                    else {
                        intAbsolutCaretVertCurrent = intFirstLineOnPageUpdate; // -2 because restriction always show last line
                    }

                    listBoxHelper.Visibility = Visibility.Collapsed;
                    goto lexit;
                }

                if (e.Key == Key.Delete) {
                    correctLocation();

                    if (blnSomthingSelected == false || blnAppend == true) {
                        if (intAbsolutCaretHorizCurrent == listSBmain[(int)intAbsolutCaretVertCurrent].Length && intAbsolutCaretVertCurrent < listSBmain.Count - 1) {
                            listSBmain[(int)intAbsolutCaretVertCurrent].Append(listSBmain[(int)intAbsolutCaretVertCurrent + 1]);
                            listSBmain.RemoveAt((int)intAbsolutCaretVertCurrent + 1);

                            foreach (selectedPositions sp in listSPMain[(int)intAbsolutCaretVertCurrent + 1].listSPinLine) ///////////////////////////////////////////
                        {
                                //     if (sp.iCollection != null) canvasSelected.Children.RemoveAt((int)sp.iCollection);
                                if (sp.iCollection != null) {
                                    var recc = (canvasSelected.Children[(int)sp.iCollection]) as selectedRectangle;
                                    set_Rectangle(ref recc, 0, 0, 0, new SolidColorBrush(Colors.Transparent));
                                }

                            }


                            listSPMain.RemoveAt((int)intAbsolutCaretVertCurrent + 1);
                        }
                        else {
                            if (intAbsolutCaretHorizCurrent < listSBmain[(int)intAbsolutCaretVertCurrent].Length) {
                                listSBmain[(int)intAbsolutCaretVertCurrent].Remove((int)intAbsolutCaretHorizCurrent, 1);

                                correctSPwhenLineWasModified(-1, intAbsolutCaretHorizCurrent, intAbsolutCaretVertCurrent);

                            }
                        }

                    }
                    else {
                        // Delete Selection
                        //deleteSelection();
                        deleteABCDE();
                    }

                    listBoxHelper.Visibility = Visibility.Collapsed;
                    goto lexit;
                }


                if (e.Key == Key.Back) {
                    correctLocation();

                    if (blnSomthingSelected == false || blnAppend == true) {

                        if (intAbsolutCaretHorizCurrent == 0) {
                            if (intAbsolutCaretVertCurrent > 0) {
                                intAbsolutCaretHorizCurrent = listSBmain[(int)intAbsolutCaretVertCurrent - 1].Length;
                                listSBmain[(int)intAbsolutCaretVertCurrent - 1].Append(listSBmain[(int)intAbsolutCaretVertCurrent]);

                                listSBmain.RemoveAt((int)intAbsolutCaretVertCurrent);

                                foreach (selectedPositions sp in listSPMain[(int)intAbsolutCaretVertCurrent].listSPinLine) {
                                    if (sp.iCollection != null) {
                                        var recc = (canvasSelected.Children[(int)sp.iCollection]) as selectedRectangle;
                                        set_Rectangle(ref recc, 0, 0, 0, new SolidColorBrush(Colors.Transparent));
                                    }


                                }
                                listSPMain.RemoveAt((int)intAbsolutCaretVertCurrent);

                                intAbsolutCaretVertCurrent = intAbsolutCaretVertCurrent - 1;

                                if (intAbsolutCaretHorizCurrent > intFirstCharOnPageUpdate + intHorizCountCharsOnPage - 1) scrollX.Value = intAbsolutCaretHorizCurrent;
                                if (intAbsolutCaretVertCurrent < intFirstLineOnPageUpdate) scrollY.Value -= 1;
                            }

                        }
                        else {
                            if (intAbsolutCaretHorizCurrent > listSBmain[(int)intAbsolutCaretVertCurrent].Length) {
                                intAbsolutCaretHorizCurrent = listSBmain[(int)intAbsolutCaretVertCurrent].Length;
                            }
                            else {
                                listSBmain[(int)intAbsolutCaretVertCurrent].Remove((int)intAbsolutCaretHorizCurrent - 1, 1);
                                intAbsolutCaretHorizCurrent -= 1;
                                correctSPwhenLineWasModified(-1, intAbsolutCaretHorizCurrent, intAbsolutCaretVertCurrent);

                            }
                        }
                        if (intAbsolutCaretHorizCurrent <= intFirstCharOnPageUpdate) scrollX.Value -= 1;
                    }
                    else {
                        // Delete Selection
                        //deleteSelection();

                        deleteABCDE();
                    }


                    listBoxHelper.Visibility = Visibility.Collapsed;
                    goto lexit;
                }

                if (e.Key == Key.Enter) {

                    if (blnSomthingSelected == false || blnAppend == true) {
                        correctLocation();
                        if (listSPMain.Count == 0) listSPMain.Add(new listOfSelectedPositionInLine());
                        string strTmp = listSBmain[(int)intAbsolutCaretVertCurrent].ToString((int)intAbsolutCaretHorizCurrent, (int)(listSBmain[(int)intAbsolutCaretVertCurrent].Length - (int)intAbsolutCaretHorizCurrent));
                        if (strTmp != "") listSBmain[(int)intAbsolutCaretVertCurrent].Remove((int)intAbsolutCaretHorizCurrent, (int)(listSBmain[(int)intAbsolutCaretVertCurrent].Length - (int)intAbsolutCaretHorizCurrent));

                        listSBmain.Insert((int)intAbsolutCaretVertCurrent + 1, new StringBuilder(strTmp));
                        listSPMain.Insert((int)intAbsolutCaretVertCurrent + 1, new listOfSelectedPositionInLine());


                        foreach (selectedPositions sp in listSPMain[(int)intAbsolutCaretVertCurrent].listSPinLine) {
                            if (sp.iCollection != null) {
                                var recc = (canvasSelected.Children[(int)sp.iCollection]) as selectedRectangle;
                                set_Rectangle(ref recc, 0, 0, 0, new SolidColorBrush(Colors.Transparent));
                            }
                        }
                        listSPMain[(int)intAbsolutCaretVertCurrent].listSPinLine.Clear();

                        intAbsolutCaretHorizCurrent = 0;
                        intAbsolutCaretVertCurrent += 1;

                    }
                    else {
                        // Delete Selection
                        //deleteSelection();
                        deleteABCDE();
                    }

                    if (intAbsolutCaretVertCurrent >= intFirstLineOnPageUpdate + intVertCountLinesOnPage) scrollY.Value += 1;
                    if (intAbsolutCaretHorizCurrent == 0) scrollX.Value = 0;

                    listBoxHelper.Visibility = Visibility.Collapsed;
                    goto lexit;
                }

                // INSERT SECTION  ------------------------------------------------------------------------------------------------------------------

                listBoxHelper.Visibility = Visibility.Collapsed;

                strOEM = OemKeyToChar(e, Keyboard.Modifiers);

                if (blnSomthingSelected == true && blnAppend == false && strOEM != "" && Keyboard.Modifiers != ModifierKeys.Control && e.Key.ToString() != "Tab") {
                    deleteABCDE();
                }

                if (strOEM == "" && Keyboard.Modifiers == ModifierKeys.Shift) goto lexit;

                //--------------------------------------------------


                if ((strOEM == "C" || strOEM == "c") && Keyboard.Modifiers == ModifierKeys.Control) {
                    selectedCopy();
                }
                if ((strOEM == "X" || strOEM == "x") && Keyboard.Modifiers == ModifierKeys.Control) {
                    selectedCut();
                }
                if ((strOEM == "V" || strOEM == "v") && Keyboard.Modifiers == ModifierKeys.Control) {
                    selectedPaste();
                }
                if ((strOEM == "D" || strOEM == "d") && Keyboard.Modifiers == ModifierKeys.Control) {
                    deleteABCDE();
                }

                if (Keyboard.Modifiers == ModifierKeys.Control) goto lexit;


                correctLocation();
                findABCDE();

                // words helper --------------------------------------------------

                if (strOEM == " " && strHistory.Trim() != "" && blnHelperEna) { listEnter(); }
                if (blnHelperEna == false && strOEM == " " && Keyboard.Modifiers != ModifierKeys.Shift) {
                    strHistory = "";
                }
                // if (blnHighlight) {
                findWordCurrent(strOEM);
                // }


                // ---------------------------------------------------------------


                if (blnCaretInsert && strOEM != "" && e.Key.ToString() != "Tab") {

                    listSBmain[intAbsolutCaretVertCurrent].Remove(intAbsolutCaretHorizCurrent, strOEM.Length);
                    listSBmain[intAbsolutCaretVertCurrent].Insert(intAbsolutCaretHorizCurrent, strOEM);

                }
                else {
                    if (listSBmain.Count == 0) {
                        listSBmain.Add(new StringBuilder(""));
                        listSPMain.Add(new listOfSelectedPositionInLine());
                        intAbsolutCaretVertCurrent = 0;
                        intAbsolutCaretHorizCurrent = 0;
                    }

                    if (iLineStartABCDE == null) {
                        iLineStartABCDE = intAbsolutCaretVertCurrent;
                        iLineStopABCDE = intAbsolutCaretVertCurrent;
                        iCharStartABCDE = intAbsolutCaretHorizCurrent;
                    }

                    if (e.Key.ToString() != "Tab") { //|| blnAppend == true  iLineStartABCDE == null || 
                        listSBmain[intAbsolutCaretVertCurrent].Insert(intAbsolutCaretHorizCurrent, strOEM);
                        correctSPwhenLineWasModified(strOEM.Length, intAbsolutCaretHorizCurrent, intAbsolutCaretVertCurrent);
                    }
                    else {
                        //                           TAB    TAB     TAB   

                        if (Keyboard.Modifiers == ModifierKeys.Shift) {

                            for (int i = (int)iLineStartABCDE; i <= (int)iLineStopABCDE; i++) {
                                int intLengthSelected = listSBmain[i].Length - (int)iCharStartABCDE;

                                if (i == (int)iLineStartABCDE && listSBmain[i].ToString((int)iCharStartABCDE, intLengthSelected >= strOEM.Length ? strOEM.Length : 0).IndexOf(strOEM) != 0) continue;
                                if (i != (int)iLineStartABCDE && listSBmain[i].ToString(0, listSBmain[i].Length >= strOEM.Length ? strOEM.Length : 0).IndexOf(strOEM) != 0) continue;

                                if (i == (int)iLineStartABCDE) {
                                    listSBmain[i].Replace(strOEM, "", (int)iCharStartABCDE, strOEM.Length);
                                    correctSPwhenLineWasModified(-strOEM.Length, (int)iCharStartABCDE, i);
                                    continue;
                                }
                                else {
                                    if (listSBmain[i].ToString().IndexOf(strOEM) != 0) continue;

                                    listSBmain[i].Replace(strOEM, "", 0, strOEM.Length);
                                    correctSPwhenLineWasModified(-strOEM.Length, 0, i);
                                }

                            }
                            //  intAbsolutCaretHorizCurrent = intAbsolutCaretHorizCurrent > strOEM.Length ? intAbsolutCaretHorizCurrent - strOEM.Length : 0;

                        }
                        else {
                            for (int i = (int)iLineStartABCDE; i <= (int)iLineStopABCDE; i++) {

                                if (i == (int)iLineStartABCDE) {
                                    listSBmain[i].Insert((int)iCharStartABCDE, strOEM);
                                    correctSPwhenLineWasModified(strOEM.Length, (int)iCharStartABCDE, i);
                                    continue;
                                }

                                listSBmain[i] = (new StringBuilder(strOEM)).Append(listSBmain[i]);
                                correctSPwhenLineWasModified(strOEM.Length, 0, i);
                            }
                            //   intAbsolutCaretHorizCurrent += strOEM.Length;
                        }
                    }
                }

                if (strOEM != "" && e.Key.ToString() != "Tab") { //&& iLineStartABCDE != null listSPMain.Count > 0

                    if (blnAppend == true) {
                        if (iLineStartABCDE != null) { // something Selected
                            //  correctSPwhenLineWasModified(strOEM.Length, intAbsolutCaretHorizCurrent, intAbsolutCaretVertCurrent);
                        }
                        else {
                            listSPMain.Add(new listOfSelectedPositionInLine());
                        }
                    }


                    intAbsolutCaretHorizCurrent += strOEM.Length;
                    intAbsolutSelectHorizStart = intAbsolutCaretHorizCurrent;
                    intAbsolutSelectVertStart = intAbsolutCaretVertCurrent;

                    spMoveCurrent.iLineCurrent = intAbsolutCaretVertCurrent;
                    spMoveCurrent.iCharCurrent = intAbsolutCaretHorizCurrent;


                }

                if (intFirstCharOnPageUpdate + intHorizCountCharsOnPage - 1 < intAbsolutCaretHorizCurrent) scrollX.Value += strOEM.Length;




            lexit:

                if (strOEM.Trim() == "" && Keyboard.Modifiers != ModifierKeys.Shift) strHistory = "";

                if (blnAppend == false && e.Key.ToString() != "Tab" && Keyboard.Modifiers != ModifierKeys.Control && Keyboard.Modifiers != ModifierKeys.Shift) DeSelectAllCurrent(); //     Keyboard.Modifiers == ModifierKeys.None && 

                // totalBytes();  DO NOT COUNT EACH TIME ... PERFORMENCE ...

                if (Keyboard.Modifiers == ModifierKeys.None || strOEM != "") {
                    spMoveCurrent.iLineCurrent = intAbsolutCaretVertCurrent;
                    spMoveCurrent.iCharCurrent = intAbsolutCaretHorizCurrent;

                    intAbsolutSelectHorizStart = intAbsolutCaretHorizCurrent;
                    intAbsolutSelectVertStart = intAbsolutCaretVertCurrent;

                    //intPressedPositionY = intAbsolutCaretVertCurrent;
                    //intPressedPositionX = intAbsolutCaretHorizCurrent;
                }

                if (Keyboard.Modifiers == ModifierKeys.Shift && strOEM == "") {  // strOEM == ""
                    spMoveCurrent.iLineCurrent = intAbsolutCaretVertCurrent;
                    spMoveCurrent.iCharCurrent = intAbsolutCaretHorizCurrent;
                    updateSelection();
                }


                //   }));

                if (e.Key == Key.Up || e.Key == Key.Down || e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.PageDown || e.Key == Key.PageUp || e.Key == Key.Home || e.Key == Key.End) {
                    if (listSBmain.Count == 0) set_Caret(0, 0);
                    else set_Caret(intAbsolutCaretVertCurrent, intAbsolutCaretHorizCurrent);
                }
                else {
                    updateBlock();
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Program has a problem allocating additional char(s)/byte(s) in memory (tb_PreviewKeyDown)...\nPlease save your  job ..." + " (10)" + ex.Message);
                blnError = true;
            }


            canvasMain.Focus();
        }

        public string strHistory = "";
        public List<string> listTablesHelper = new List<string>();
        public List<string> listColumnsHelper = new List<string>();

        public bool blnHelperEna = false;

        public void findWordCurrent(string strOEM) {

            correctLocation();

            strOEM = strOEM.ToUpper();
            listBoxHelper.Visibility = Visibility.Collapsed;
            string strTmp = listSBmain[intAbsolutCaretVertCurrent].ToString().Replace('(', ' ').Replace(')', ' ').Replace("[", "").Replace("]", "").Replace(',', ' ').Replace(';', ' ').ToUpper();
            bool blnContinue = false;

            if (intAbsolutCaretHorizCurrent > strTmp.Length) return;

            if (strOEM.Length == 1 && intAbsolutCaretHorizCurrent > 0) {
                blnContinue = blnContinue || (strTmp.Substring(intAbsolutCaretHorizCurrent - 1, 1) == " " && intAbsolutCaretHorizCurrent == strTmp.Length);
            }

            if (strOEM.Length > 0 && intAbsolutCaretHorizCurrent > 0) {
                blnContinue = blnContinue || intAbsolutCaretHorizCurrent == strTmp.Length - 1;
            }

            if (strOEM.Length > 0 && intAbsolutCaretHorizCurrent > 0 && intAbsolutCaretHorizCurrent < strTmp.Length) {
                blnContinue = blnContinue || (strTmp.Substring(intAbsolutCaretHorizCurrent - 1, 1) == " " && intAbsolutCaretHorizCurrent < strTmp.Length && strTmp.Substring(intAbsolutCaretHorizCurrent, 1) == " ");
            }

            if (strOEM.Length > 0 && intAbsolutCaretHorizCurrent == 0 && intAbsolutCaretHorizCurrent < strTmp.Length - 1) {
                blnContinue = blnContinue || strTmp.Substring(intAbsolutCaretHorizCurrent, 1) == " ";
            }

            double dNum = 0;
            bool isNum = double.TryParse(strHistory, out dNum);

            if (strHistory.Trim() != "" || strTmp.Length == 0 || strOEM == ".") {
                blnContinue = true;
            }

            if (blnContinue) {
                strHistory += strOEM;
                if (strHistory.Length > 1 || (strHistory.Contains('.') && !isNum)) returnWord(strHistory);
            }
            else blnHelperEna = false;
        }

        private void returnWord(string strHistory) {

            selectedSqlKeys sqlHelper = new selectedSqlKeys(strConnection,strDataBaseName, blnODBCHighlight, blnHighlight);
            List<string> listHelper = new List<string>();

            int iSelected = 0;
            int iTmp = 0;
            int iWmax = 0;
            int iMinLength = 1000;

            if (!strHistory.Contains('.'))   // not contain dot
            {   //reserved words + tables

                foreach (string str in sqlHelper.listSqlReserved) {
                    if (str.Contains(strHistory.ToUpper())) {
                        listHelper.Add(str);
                        iWmax = str.Length > iWmax ? str.Length : iWmax;
                        if (str.IndexOf(strHistory.ToUpper()) == 0 && iMinLength > str.Length) {
                            iMinLength = str.Length;
                            iSelected = iTmp;
                        }
                        iTmp++;
                    }
                }

                foreach (string str in listTablesHelper) {
                    if (str.Contains(strHistory)) {
                        listHelper.Add(str);
                        iWmax = str.Length > iWmax ? str.Length : iWmax;
                        if (str.IndexOf(strHistory.ToUpper()) == 0 && iMinLength > str.Length) {
                            iMinLength = str.Length;
                            iSelected = iTmp;
                        }
                        iTmp++;
                    }
                }
            }
            else {  //if dot - all full columns name 



                if (strHistory.Contains("...")) {
                    foreach (string str in sqlHelper.listSqlReserved) {

                        listHelper.Add(str);
                        iWmax = str.Length > iWmax ? str.Length : iWmax;
                        listHelper.Sort();
                    }
                    iSelected = 0;
                    goto lcontinue;
                }

                string strTmp = listSBmain[intAbsolutCaretVertCurrent].ToString(0, intAbsolutCaretHorizCurrent).Replace('(', ' ').Replace(')', ' ').Replace("[", "").Replace("]", "").Replace(',', ' ').Replace(';', ' ').ToUpper();
                int intStart = strTmp.LastIndexOf(' ');
                intStart = intStart < 0 ? 0 : intStart;
                strHistory = strTmp.Substring(intStart < 0 ? 0 : intStart).Trim();

                if (!strHistory.Contains(".")) strHistory += ".";
                //    if (strHistory.Contains(".")) {
                foreach (string str in listColumnsHelper) {
                    if (str.Contains(strHistory)) {
                        listHelper.Add(str);
                        iWmax = str.Length > iWmax ? str.Length : iWmax;
                        if (str.IndexOf(strHistory.ToUpper()) == 0 && iMinLength > str.Length) {
                            iMinLength = str.Length;
                            iSelected = iTmp;
                        }
                        iTmp++;
                    }
                }
                //    }

            }

        lcontinue:



            double dH = 0;
            double dHsingle = 0;

            if (listHelper.Count > 0) {
                listBoxHelper.FontFamily = tb_FontFamily;
                listBoxHelper.FontSize = tb_FontSize;
                listBoxHelper.ItemsSource = listHelper;
                listBoxHelper.Visibility = Visibility.Visible;

                dHsingle = tb_FontSize / coeffFont_High;
                dH = dHsingle * (listHelper.Count > 30.0 ? 31.0 : listHelper.Count) + 0.5 * dHsingle;

                listBoxHelper.MaxHeight = dH;
                listBoxHelper.SelectedIndex = iSelected;
            }
            else {
                blnHelperEna = false;
                listBoxHelper.Visibility = Visibility.Collapsed;
                return;
            }

            double myAH = listBoxHelper.ActualHeight;
            double myAW = 0;
            // different size for small and .... box
            if (listHelper.Count > 10.0) myAW = tb_FontSize / coeffFont_Widh * (iWmax + 3);
            else myAW = tb_FontSize / coeffFont_Widh * (iWmax + 1);

            myAW = myAW < canvasMain.ActualWidth / 2 ? myAW : canvasMain.ActualWidth / 2;
            dH = dH < canvasMain.ActualHeight / 2 ? dH : canvasMain.ActualHeight / 2;

            listBoxHelper.Width = myAW;
            listBoxHelper.Height = dH;

            if (crt.tr.X <= canvasMain.ActualWidth / 2 && crt.tr.Y <= canvasMain.ActualHeight / 2) {
                trList.X = crt.tr.X + 10;
                trList.Y = crt.tr.Y + dHsingle;
            }
            if (crt.tr.X <= canvasMain.ActualWidth / 2 && crt.tr.Y > canvasMain.ActualHeight / 2) {
                trList.X = crt.tr.X + 10;
                trList.Y = crt.tr.Y - dH;
            }
            if (crt.tr.X > canvasMain.ActualWidth / 2 && crt.tr.Y <= canvasMain.ActualHeight / 2) {
                trList.X = crt.tr.X - myAW;
                trList.Y = crt.tr.Y + dHsingle;
            }
            if (crt.tr.X > canvasMain.ActualWidth / 2 && crt.tr.Y > canvasMain.ActualHeight / 2) {
                trList.X = crt.tr.X - myAW;
                trList.Y = crt.tr.Y - dH;
            }

            blnHelperEna = true;
        }





        public void correctSPwhenLineWasModified(int intShift, int intX, int intY) { // Modified 

            if (listSPMain[intY].listSPinLine.Count != 0) {

                for (int j = 0; j < listSPMain[intY].listSPinLine.Count; j++) {
                    selectedPositions sp = listSPMain[intY].listSPinLine[j];

                    if (sp.iCharEnd < intX) continue;
                    if (sp.iCharBegin <= intX && intX < sp.iCharEnd) { sp.iCharEnd += intShift; }; //sp.iCharBegin  <=
                    if (sp.iCharBegin == intX && sp.iCharBegin == sp.iCharEnd) { listSPMain[intY].listSPinLine.RemoveAt(j); j--; }

                    if (intX < sp.iCharBegin)  /////////////if (intX <= sp.iCharBegin) 
                    {
                        sp.iCharBegin += intShift;
                        sp.iCharEnd += intShift;
                    }

                    sp.iCharBegin = sp.iCharBegin > 0 ? sp.iCharBegin : 0;
                    sp.iCharEnd = sp.iCharEnd > 0 ? sp.iCharEnd : 0;


                }
            }
        }


        //Delete Selection ---Uses For Group --------------------------------------------------------------------------

        public void deleteSelection() {

            if (blnSomthingSelected) {

                //inversion
                int intStart = listSBmain.Count - 1;
                int intStop = 0;

                bool blnLastLineDel = false;

                int intCount = listSPMain.Count - 1;
                //Count Down
                for (int i = intCount; i >= 0; i--) // Do not use  listSPMain.Count - 1  here 
                {
                    if (listSPMain[i].listSPinLine.Count > 0) {   // Delete only for Standard Selection

                        intStart = intStart > i ? i : intStart;
                        intStop = intStop < i ? i : intStop;

                        for (int j = listSPMain[i].listSPinLine.Count - 1; j >= 0; j--) {
                            //   int j = listSPMain[i].listSPinLine.Count - 1 - iminus;

                            selectedPositions sp = listSPMain[i].listSPinLine[j];

                            if (i == intStart) blnLastLineDel = blnLastLineDel && sp.iCharEnd == listSBmain[i].Length && sp.iCharBegin > 0 && intStart != intStop;
                            if (i == intStop) blnLastLineDel = sp.iCharEnd != listSBmain[i].Length && intStart != intStop;

                            if (blnAppend == false) listSBmain[i].Remove(sp.iCharBegin, sp.iCharEnd - sp.iCharBegin);
                            else {
                                listSBmain[i].Remove(sp.iCharBegin, sp.iCharEnd - sp.iCharBegin);

                                if (blnCut) listSBmain[i].Insert(sp.iCharBegin, " ", sp.iCharEnd - sp.iCharBegin);

                            }

                            intAbsolutCaretVertCurrent = i;
                            intAbsolutCaretHorizCurrent = sp.iCharBegin;

                            // Remove Empty String - if selected
                            if ((listSBmain[i].Length == 0 && blnAppend == false) || (!blnCut && listSBmain[i].Length == 0)) { //&& intStart != intStop
                                listSBmain.RemoveAt(i);
                                listSPMain.RemoveAt(i);
                            }
                        }
                    }
                }


                if (!blnCut) DeSelectAllCurrent();

                if (blnLastLineDel && intStop != intStart) {
                    listSBmain[intStart].Append(listSBmain[intStart + 1]);
                    listSBmain.RemoveAt(intStart + 1);

                    foreach (selectedPositions spp in listSPMain[intStart + 1].listSPinLine) {
                        var recc = (canvasSelected.Children[(int)spp.iCollection]) as selectedRectangle;
                        set_Rectangle(ref recc, 0, 0, 0, new SolidColorBrush(Colors.Transparent));
                    }

                    listSPMain.RemoveAt(intStart + 1);
                }

                if (!blnCut || blnAppend == false) DeSelectAllCurrent();
                blnCut = false;

                if (listSBmain.Count == 0) {
                    canvasSelected.Children.Clear();
                    listSBmain = null;
                    listSPMain = null;

                    GC.Collect();
                    Thread.Sleep(100);

                    listSBmain = new List<StringBuilder>();
                    listSPMain = new List<listOfSelectedPositionInLine>();
                }
            }
        }

        public bool blnCut = false;



        //----------------------------------------------------------------------------------------------------

        public void correctLocation() {    // tied to end line
            try {
                if (listSBmain.Count == 0 || listSBmain == null) {
                    Dispatcher.Invoke(new Action(() => set_Caret(0, 0)));


                    return;
                }

                if (listSBmain.Count <= intAbsolutCaretVertCurrent) {
                    intAbsolutCaretVertCurrent = listSBmain.Count - 1;
                    intAbsolutCaretHorizCurrent = listSBmain[(int)intAbsolutCaretVertCurrent].Length;
                    scrollX.Value = intAbsolutCaretHorizCurrent - intHorizCountCharsOnPage + 1;

                }

                if (listSBmain[(int)intAbsolutCaretVertCurrent].Length < intAbsolutCaretHorizCurrent) {
                    intAbsolutCaretHorizCurrent = listSBmain[(int)intAbsolutCaretVertCurrent].Length;
                    scrollX.Value = intAbsolutCaretHorizCurrent - intHorizCountCharsOnPage + 1;
                }

                intAbsolutSelectHorizStart = intAbsolutCaretHorizCurrent;
                intAbsolutSelectVertStart = intAbsolutCaretVertCurrent;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Program has a problem allocating additional char(s)/byte(s) in memory (correctLocation) ...\nPlease save your  job ... ..." + " (11)" + ex.Message);
                blnError = true;
            }



        }

        //----------------------------------------------------------------------------------------------------------------------------------------------

        private string OemKeyToChar(KeyEventArgs e, ModifierKeys k) {
            string str = "";

            //      if (Keyboard.Modifiers == ModifierKeys.Control) return "";

            switch (e.Key.ToString()) {

                case "LWin":
                str = "";
                break;

                case "System":
                str = "";
                break;

                //--------------------------------------------------------------------------------
                case "Oem3":
                if (Keyboard.Modifiers == ModifierKeys.Shift || e.IsUp) str = "~"; else str = "`";
                break;

                case "D1":
                if (Keyboard.Modifiers == ModifierKeys.Shift || e.IsUp) str = "!"; else str = "1";
                break;

                case "D2":
                if (Keyboard.Modifiers == ModifierKeys.Shift || e.IsUp) str = "@"; else str = "2";
                break;

                case "D3":
                if (Keyboard.Modifiers == ModifierKeys.Shift || e.IsUp) str = "#"; else str = "3";
                break;

                case "D4":
                if (Keyboard.Modifiers == ModifierKeys.Shift || e.IsUp) str = "$"; else str = "4";
                break;
                case "D5":
                if (Keyboard.Modifiers == ModifierKeys.Shift || e.IsUp) str = "%"; else str = "5";
                break;

                case "D6":
                if (Keyboard.Modifiers == ModifierKeys.Shift || e.IsUp) str = "^"; else str = "6";
                break;

                case "D7":
                if (Keyboard.Modifiers == ModifierKeys.Shift || e.IsUp) str = "&"; else str = "7";
                break;

                case "D8":
                if (Keyboard.Modifiers == ModifierKeys.Shift || e.IsUp) str = "*"; else str = "8";
                break;

                case "D9":
                if (Keyboard.Modifiers == ModifierKeys.Shift || e.IsUp) str = "("; else str = "9";
                break;

                case "D0":
                if (Keyboard.Modifiers == ModifierKeys.Shift || e.IsUp) str = ")"; else str = "0";
                break;

                case "OemMinus":
                if (Keyboard.Modifiers == ModifierKeys.Shift || e.IsUp) str = "_"; else str = "-";
                break;

                case "OemPlus":
                if (Keyboard.Modifiers == ModifierKeys.Shift || e.IsUp) str = "+"; else str = "=";
                break;

                //---------------------------------------------------------------------------------

                case "Tab":
                //str = "\t";
                str = "    ";  //4 spaces
                break;

                case "Q":
                if (Keyboard.Modifiers == ModifierKeys.Shift || e.IsUp) str = "Q"; else str = "q";
                break;

                case "W":
                if (Keyboard.Modifiers == ModifierKeys.Shift || e.IsUp) str = "W"; else str = "w";
                break;

                case "E":
                if (Keyboard.Modifiers == ModifierKeys.Shift || e.IsUp) str = "E"; else str = "e";
                break;

                case "R":
                if (Keyboard.Modifiers == ModifierKeys.Shift || e.IsUp) str = "R"; else str = "r";
                break;

                case "T":
                if (Keyboard.Modifiers == ModifierKeys.Shift || e.IsUp) str = "T"; else str = "t";
                break;

                case "Y":
                if (Keyboard.Modifiers == ModifierKeys.Shift || e.IsUp) str = "Y"; else str = "y";
                break;

                case "U":
                if (Keyboard.Modifiers == ModifierKeys.Shift || e.IsUp) str = "U"; else str = "u";
                break;

                case "I":
                if (Keyboard.Modifiers == ModifierKeys.Shift || e.IsUp) str = "I"; else str = "i";
                break;

                case "O":
                if (Keyboard.Modifiers == ModifierKeys.Shift || e.IsUp) str = "O"; else str = "o";
                break;

                case "P":
                if (Keyboard.Modifiers == ModifierKeys.Shift || e.IsUp) str = "P"; else str = "p";
                break;

                case "OemOpenBrackets":
                if (Keyboard.Modifiers == ModifierKeys.Shift || e.IsUp) str = "{"; else str = "[";
                break;

                case "Oem6":
                if (Keyboard.Modifiers == ModifierKeys.Shift || e.IsUp) str = "}"; else str = "]";
                break;

                case "Oem5":
                if (Keyboard.Modifiers == ModifierKeys.Shift || e.IsUp) str = "|"; else str = "\\";
                break;

                //----------------------------------------------------------------------------------
                case "Capital":
                if (Keyboard.Modifiers == ModifierKeys.Shift || e.IsUp) str = ""; else str = "";
                break;

                case "A":
                if (Keyboard.Modifiers == ModifierKeys.Shift || e.IsUp) str = "A"; else str = "a";
                break;

                case "S":
                if (Keyboard.Modifiers == ModifierKeys.Shift || e.IsUp) str = "S"; else str = "s";
                break;

                case "D":
                if (Keyboard.Modifiers == ModifierKeys.Shift || e.IsUp) str = "D"; else str = "d";
                break;

                case "F":
                if (Keyboard.Modifiers == ModifierKeys.Shift || e.IsUp) str = "F"; else str = "f";
                break;

                case "G":
                if (Keyboard.Modifiers == ModifierKeys.Shift || e.IsUp) str = "G"; else str = "g";
                break;

                case "H":
                if (Keyboard.Modifiers == ModifierKeys.Shift || e.IsUp) str = "H"; else str = "h";
                break;

                case "J":
                if (Keyboard.Modifiers == ModifierKeys.Shift || e.IsUp) str = "J"; else str = "j";
                break;

                case "K":
                if (Keyboard.Modifiers == ModifierKeys.Shift || e.IsUp) str = "K"; else str = "k";
                break;

                case "L":
                if (Keyboard.Modifiers == ModifierKeys.Shift || e.IsUp) str = "L"; else str = "l";
                break;

                case "Oem1":
                if (Keyboard.Modifiers == ModifierKeys.Shift || e.IsUp) str = ":"; else str = ";";
                break;

                case "OemQuotes":
                if (Keyboard.Modifiers == ModifierKeys.Shift || e.IsUp) str = "\""; else str = "'";
                break;

                //---------------------------------------------------------------------------------


                case "Z":
                if (Keyboard.Modifiers == ModifierKeys.Shift || e.IsUp) str = "Z"; else str = "z";
                break;

                case "X":
                if (Keyboard.Modifiers == ModifierKeys.Shift || e.IsUp) str = "X"; else str = "x";
                break;


                case "C":
                if (Keyboard.Modifiers == ModifierKeys.Shift || e.IsUp) str = "C"; else str = "c";
                break;

                case "V":
                if (Keyboard.Modifiers == ModifierKeys.Shift || e.IsUp) str = "V"; else str = "v";
                break;


                case "B":
                if (Keyboard.Modifiers == ModifierKeys.Shift || e.IsUp) str = "B"; else str = "b";
                break;

                case "N":
                if (Keyboard.Modifiers == ModifierKeys.Shift || e.IsUp) str = "N"; else str = "n";
                break;


                case "M":
                if (Keyboard.Modifiers == ModifierKeys.Shift || e.IsUp) str = "M"; else str = "m";
                break;

                case "OemComma":
                if (Keyboard.Modifiers == ModifierKeys.Shift || e.IsUp) str = "<"; else str = ",";
                break;

                case "OemPeriod":
                if (Keyboard.Modifiers == ModifierKeys.Shift || e.IsUp) str = ">"; else str = ".";
                break;

                case "OemQuestion":
                if (Keyboard.Modifiers == ModifierKeys.Shift || e.IsUp) str = "?"; else str = "/";
                break;

                case "Space":
                str = " ";
                break;

                //--------------------------------------------------------------------------------
                case "NumPad7":
                str = "7";
                break;

                case "NumPad8":
                str = "8";
                break;

                case "NumPad9":
                str = "9";
                break;

                case "NumPad4":
                str = "4";
                break;

                case "NumPad5":
                str = "5";
                break;
                case "NumPad6":
                str = "6";
                break;

                case "NumPad1":
                str = "1";
                break;

                case "NumPad2":
                str = "2";
                break;

                case "NumPad3":
                str = "3";
                break;

                case "NumPad0":
                str = "0";
                break;

                case "Decimal":
                str = ".";
                break;

                case "Divide":
                str = "/";
                break;

                case "Multiply":
                str = "*";
                break;

                case "Subtract":
                str = "-";
                break;

                case "Add":
                str = "+";
                break;

                case "NumLock":
                str = "";
                break;

                case "Clear":
                str = "";
                break;


                case "F2":
                str = "…";
                break;

                case "F3":
                str = "⁞";
                break;

                case "F4":
                str = "║";
                break;




                //---------------------------------------------------------------------------------

                default:
                str = "";
                break;
            }

            return str;

        }


        //private Rectangle rectSelect

        //-----------------------------------------------------------------------------------------------------------------------------------------
        // Create selectedRectangle
        public selectedRectangle set_Rectangle(int intLine, int intBeginChar, int intEndChar, Brush myBrush) {

            try {
                selectedRectangle rec = new selectedRectangle();

                if (intBeginChar > intEndChar) {
                    int intTmp = intBeginChar;
                    intBeginChar = intEndChar;
                    intEndChar = intTmp;
                }

                if ((intBeginChar > intFirstCharOnPageUpdate + intHorizCountCharsOnPage) ||
                    (intEndChar < intFirstCharOnPageUpdate) ||
                    (intLine < intFirstLineOnPageUpdate) ||
                    (intLine > intFirstLineOnPageUpdate + intVertCountLinesOnPage - 1)) {

                    rec.myRec.Width = 0;
                    rec.myRec.Height = 0;
                    rec.myRec.Fill = new SolidColorBrush(Colors.Transparent);
                    return rec;
                }

                intBeginChar = intBeginChar > intFirstCharOnPageUpdate - 1 ? intBeginChar : intFirstCharOnPageUpdate;
                intEndChar = intEndChar < intFirstCharOnPageUpdate + intHorizCountCharsOnPage + 1 ? intEndChar : intFirstCharOnPageUpdate + intHorizCountCharsOnPage;// +(blnOnScrollX ? 1 : 0);

                int iBChar = intBeginChar - intFirstCharOnPageUpdate;
                int iEChar = intEndChar - intFirstCharOnPageUpdate;

                int iLine = intLine - intFirstLineOnPageUpdate;

                rec.tr.X = tb_FontSize * iBChar / coeffFont_Widh;
                rec.tr.Y = tb_FontSize * iLine / coeffFont_High;

                rec.myRec.Height = tb_FontSize / coeffFont_High;
                if (iBChar - iEChar == 0) {
                    rec.myRec.Width = tb_FontSize / coeffFont_Widh / 3;
                }
                else {
                    rec.myRec.Width = (iEChar - iBChar) * tb_FontSize / coeffFont_Widh;
                }

                rec.myRec.Fill = myBrush;

                return rec;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Program has a problem allocating additional char(s)/byte(s) in memory (set_Rectangle) ...\nPlease save your  job ..." + " (12)" + ex.Message);
                blnError = true;
                return null;
            }

        }


        //  NOT CREATE REC - ONLY MODIFY
        public void set_Rectangle(ref selectedRectangle rec, int intLine, int intBeginChar, int intEndChar, Brush myBrush) {

            if (intBeginChar > intEndChar) {
                int intTmp = intBeginChar;
                intBeginChar = intEndChar;
                intEndChar = intTmp;
            }

            if ((intBeginChar > intFirstCharOnPageUpdate + intHorizCountCharsOnPage) ||
                (intEndChar < intFirstCharOnPageUpdate) ||
                (intLine < intFirstLineOnPageUpdate) ||
                (intLine > intFirstLineOnPageUpdate + intVertCountLinesOnPage - 1)) {

                rec.myRec.Width = 0;
                rec.myRec.Height = 0;
                rec.myRec.Fill = new SolidColorBrush(Colors.Transparent);
                return;
            }

            intBeginChar = intBeginChar > intFirstCharOnPageUpdate - 1 ? intBeginChar : intFirstCharOnPageUpdate;
            intEndChar = intEndChar < intFirstCharOnPageUpdate + intHorizCountCharsOnPage + 1 ? intEndChar : intFirstCharOnPageUpdate + intHorizCountCharsOnPage;// +(blnOnScrollX ? 1 : 0);

            int iBChar = intBeginChar - intFirstCharOnPageUpdate;
            int iEChar = intEndChar - intFirstCharOnPageUpdate;

            int iLine = intLine - intFirstLineOnPageUpdate;

            rec.tr.X = tb_FontSize * iBChar / coeffFont_Widh;
            rec.tr.Y = tb_FontSize * iLine / coeffFont_High;

            rec.myRec.Height = tb_FontSize / coeffFont_High;
            if (iBChar - iEChar == 0) {
                rec.myRec.Width = tb_FontSize / coeffFont_Widh / 3;
            }
            else {
                rec.myRec.Width = (iEChar - iBChar) * tb_FontSize / coeffFont_Widh;
            }

            rec.myRec.Fill = myBrush;

        }



        public void addSP(int lineSp, int beginSp, int endSp, bool blnAppend) {

            try {



                if (beginSp > endSp) {
                    int intTmp = beginSp;
                    beginSp = endSp;
                    endSp = intTmp;
                }

                if (lineSp > listSBmain.Count - 1 && blnPlaceHolder == true) {
                    for (int ii = 0; ii < (lineSp - listSBmain.Count + 1); ii++) {
                        listSBmain.Add(new StringBuilder());
                        listOfSelectedPositionInLine lsp = new listOfSelectedPositionInLine();
                        listSPMain.Add(lsp);
                    }
                }

                if (beginSp > listSBmain[lineSp].Length && blnPlaceHolder == false) return;
                if (endSp > listSBmain[lineSp].Length) endSp = intAbsolutSelectHorizStart;             ///////////////------------------

                if (listSPMain.Count == 0 && blnPlaceHolder == false) {
                    listSPMain.Add(new listOfSelectedPositionInLine(beginSp, endSp, null));
                    return;
                }

                if (beginSp > listSBmain[lineSp].Length && blnPlaceHolder == true) {
                    listSBmain[lineSp].Append(new StringBuilder("".PadLeft(beginSp - listSBmain[lineSp].Length)));
                    listSPMain[lineSp].listSPinLine.Add(new selectedPositions(beginSp, beginSp, null));
                    return;
                }

                listOfSelectedPositionInLine listSPnew = new listOfSelectedPositionInLine();
                listOfSelectedPositionInLine listSPold = new listOfSelectedPositionInLine();

                deSelectSP(lineSp, beginSp, endSp, blnAppend);
                listSPMain[lineSp].listSPinLine.Add(new selectedPositions(beginSp, endSp, null));

                // Selected rectangle will be added below

                listSPold.listSPinLine = listSPMain[lineSp].listSPinLine;

                int iCycle = listSPold.listSPinLine.Count;

                for (int i = 0; i < iCycle; i++)  // iLine just count - do not use in logic ...
            {
                    int jmin = 0;
                    selectedPositions spMin = new selectedPositions();
                    spMin = listSPold.listSPinLine[jmin];

                    for (int j = 0; j < listSPold.listSPinLine.Count; j++) {
                        if (spMin.iCharBegin > listSPold.listSPinLine[j].iCharBegin) {
                            spMin = listSPold.listSPinLine[j];
                            jmin = j;
                        }
                    }
                    if (i == 0) listSPnew.listSPinLine.Add(spMin);
                    else {
                        int jLast = listSPnew.listSPinLine.Count - 1;
                        if (listSPnew.listSPinLine[jLast].iCharEnd == spMin.iCharBegin) {
                            listSPnew.listSPinLine[jLast].iCharEnd = spMin.iCharEnd;
                        }
                        else listSPnew.listSPinLine.Add(spMin);
                    }
                    listSPold.listSPinLine.RemoveAt(jmin);
                }

                listSPMain[lineSp] = listSPnew;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Program has a problem allocating additional char(s)/byte(s) in memory (addSP) ...\nPlease save your  job ..." + " (13)" + ex.Message);
                blnError = true;
            }
        }


        public void deSelectSP(int lineDeSp, int beginDeSp, int endDeSp, bool blnAppend) {

            if (beginDeSp > endDeSp) {
                int intTmp = beginDeSp;
                beginDeSp = endDeSp;
                endDeSp = intTmp;
            }

            if (beginDeSp > listSBmain[lineDeSp].Length) return;


            if (endDeSp > listSBmain[lineDeSp].Length) endDeSp = intAbsolutSelectHorizStart;      ////////////////////////////----------------------



            if (beginDeSp == 0 && endDeSp == listSBmain[lineDeSp].Length) {
                listSPMain[lineDeSp].listSPinLine.Clear(); // Clear Current Line
                return;
            }

            if (listSPMain[lineDeSp].listSPinLine.Count == 0) {   // If have not selected yet ....
                return;
            }


            listOfSelectedPositionInLine listSpNew = new listOfSelectedPositionInLine(); //  (beginDeSp, endDeSp);


            for (int i = 0; i < listSPMain[lineDeSp].listSPinLine.Count; i++) {
                selectedPositions spTmpOld = listSPMain[lineDeSp].listSPinLine[i];

                //  0
                //  0 
                if (beginDeSp == 0 && endDeSp == 0 && listSBmain[lineDeSp].Length == 0) {
                    listSpNew.listSPinLine = new List<selectedPositions>();
                    continue;
                }

                //  _ _ _
                //   Old
                if ((beginDeSp <= spTmpOld.iCharBegin) && (endDeSp >= spTmpOld.iCharEnd)) {
                    continue;
                }

                //      ___                    Old___
                //  Old 
                if (spTmpOld.iCharEnd <= beginDeSp && spTmpOld.iCharEnd <= endDeSp) {
                    listSpNew.listSPinLine.Add(spTmpOld);
                    continue;
                }

                //  ___                        ___Old
                //      Old
                if (endDeSp <= spTmpOld.iCharBegin && endDeSp < spTmpOld.iCharEnd) {
                    listSpNew.listSPinLine.Add(spTmpOld);
                    continue;
                }


                //   ___
                //  O l d
                if ((beginDeSp > spTmpOld.iCharBegin) && (endDeSp < spTmpOld.iCharEnd)) {
                    listSpNew.listSPinLine.Add(new selectedPositions(spTmpOld.iCharBegin, beginDeSp, null));
                    listSpNew.listSPinLine.Add(new selectedPositions(endDeSp, spTmpOld.iCharEnd, null));
                    continue;
                }

                //    ___ 
                //  Old 
                if (spTmpOld.iCharBegin < beginDeSp && spTmpOld.iCharEnd <= endDeSp && spTmpOld.iCharEnd > beginDeSp) {
                    listSpNew.listSPinLine.Add(new selectedPositions(spTmpOld.iCharBegin, beginDeSp, null));
                    continue;
                }

                //  ___ 
                //    Old
                if (beginDeSp <= spTmpOld.iCharBegin && endDeSp < spTmpOld.iCharEnd && spTmpOld.iCharBegin < endDeSp) {
                    listSpNew.listSPinLine.Add(new selectedPositions(endDeSp, spTmpOld.iCharEnd, null));
                    continue;
                }

            }

            listSPMain[lineDeSp].listSPinLine = listSpNew.listSPinLine;
        }



        public void DeSelectAllCurrent() {
            if (listSPMain.Count == 0 || listSPMain == null || blnSomthingSelected == false) { return; }

            Parallel.For(0, listSPMain.Count, (int i) =>
                //for (int i = 0; i < listSPMain.Count; i++) 
            {
                listSPMain[i] = new listOfSelectedPositionInLine();   // for each line need to Clear content...   // All be NULL
            });

            canvasSelected.Children.Clear();

            updateBlock();
            blnSomthingSelected = false;
        }

        private void tb_LostFocus(object sender, RoutedEventArgs e) {
            blnMouseLeftButtonPressed = false;
            blnMouseRightButtonPressed = false;
        }

        private void tb_MouseLeave(object sender, MouseEventArgs e) {
            blnMouseRightButtonPressed = false;
        }

        private void tb_MouseDown(object sender, MouseButtonEventArgs e) {
            setMousePositionForMoving();
        }


        //-----------------------------------------------------------------------------------------------------------------------------------------



        //-----------------------------------------------------------------------------------------------------------------------------------------

        private void selectedCopy(object sender, RoutedEventArgs e) {
            selectedCopy();
        }

        public void selectedCopy() {
            try {
                List<StringBuilder> listSBcopy = copyABCDE(false);
                if (listSBcopy == null) goto lexit;
                int iTotal = 0;

                String strTmp = "";
                for (int i = 0; i < listSBcopy.Count; i++) {
                    strTmp += listSBcopy[i].ToString();
                    iTotal += listSBcopy[i].Length;
                    if (iTotal > 30000000) {
                        resetCM();
                        MessageBox.Show("You reach limit 30M for copy/cut data to clipboard!\nPlease use CopyTo/CutTo (limit 0.5G)");
                        strTmp = null;

                        return;
                    }
                    if (i != listSBcopy.Count - 1) strTmp += Environment.NewLine;
                }

                Clipboard.SetText(strTmp);

            lexit:

                resetCM();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Program has a problem allocating additional char(s)/byte(s) in memory (selectedCopy) ...\nPlease save your  job ..." + " (14)" + ex.Message);
                blnError = true;
            }
        }


        private void selectedCut(object sender, RoutedEventArgs e) {
            selectedCut();
        }

        public void selectedCut() {

            try {
                List<StringBuilder> listSBcopy = copyABCDE(false);
                if (listSBcopy == null) goto lexit;

                int iTotal = 0;

                String strTmp = "";
                for (int i = 0; i < listSBcopy.Count; i++) {
                    strTmp += listSBcopy[i].ToString();
                    if (i != listSBcopy.Count - 1) strTmp += Environment.NewLine;
                    iTotal += listSBcopy[i].Length;

                    if (iTotal > 50000000) {
                        MessageBox.Show("You reach limit 50M for copy/cut data to clipboard!\nPlease use CopyTo/CutTo (limit 0.5G)");
                        strTmp = null;
                        return;
                    }

                }

                System.Windows.Forms.Clipboard.SetText(strTmp);
                deleteABCDE();

            lexit:
                resetCM();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Program has a problem allocating additional char(s)/byte(s) in memory (selectedCut) ...\nPlease save your  job ..." + " (15)" + ex.Message);
                blnError = true;
            }
        }


        private void selectedPaste(object sender, RoutedEventArgs e) {
            selectedPaste();
        }


        public void selectedPaste() {
            try {
                string strTmp = "";
                strTmp = System.Windows.Forms.Clipboard.GetText();

                List<StringBuilder> listSb = new List<StringBuilder>();

                foreach (string str in strTmp.Split('\n')) {
                    listSb.Add(new StringBuilder(str.Replace("\r", "")));
                }

                deleteABCDE();
                pasteStandard(listSb);
                resetCM();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Program has a problem allocating additional char(s)/byte(s) in memory (selectedPaste) ...\nPlease save your  job ..." + " (16)" + ex.Message);
                blnError = true;
            }

        }

        private void selectedDelete(object sender, RoutedEventArgs e) {
            try {
                deleteABCDE();
                resetCM();
            }
            catch (Exception ex) {
                MessageBox.Show("Program has a problem allocating additional char(s)/byte(s) in memory (selectedDelete) ...\nPlease save your  job ..." + " (17)" + ex.Message);
                blnError = true;
            }


        }

        public void resetCM() {

            nameCopyTo.IsExpanded = false;
            nameCutTo.IsExpanded = false;
            namePasteFrom.IsExpanded = false;
            nameSQL.IsExpanded = false;

            CopyToA.IsSelected = false;
            CopyToB.IsSelected = false;
            CopyToC.IsSelected = false;
            CopyToD.IsSelected = false;
            CopyToE.IsSelected = false;

            CutToA.IsSelected = false;
            CutToB.IsSelected = false;
            CutToC.IsSelected = false;
            CutToD.IsSelected = false;
            CutToE.IsSelected = false;

            PasteFromA.IsSelected = false;
            PasteFromB.IsSelected = false;
            PasteFromC.IsSelected = false;
            PasteFromD.IsSelected = false;
            PasteFromE.IsSelected = false;

            strHistory = "";

            treeContext.Visibility = Visibility.Collapsed;
            updateBlock();
            canvasMain.Focus();
        }

        private void nameCopyTo_Selected(object sender, RoutedEventArgs e) {
            nameCutTo.IsExpanded = false;
            namePasteFrom.IsExpanded = false;

            nameSQL.IsExpanded = false;
            nameSELECT.IsExpanded = false;
            nameDELETE.IsExpanded = false;
            nameUPDATE.IsExpanded = false;
            nameINSERT.IsExpanded = false;
        }

        private void nameCutTo_Selected(object sender, RoutedEventArgs e) {
            nameCopyTo.IsExpanded = false;
            namePasteFrom.IsExpanded = false;

            nameSQL.IsExpanded = false;
            nameSELECT.IsExpanded = false;
            nameDELETE.IsExpanded = false;
            nameUPDATE.IsExpanded = false;
            nameINSERT.IsExpanded = false;
        }

        private void namePasteFrom_Selected(object sender, RoutedEventArgs e) {
            nameCopyTo.IsExpanded = false;
            nameCutTo.IsExpanded = false;

            nameSQL.IsExpanded = false;
            nameSELECT.IsExpanded = false;
            nameDELETE.IsExpanded = false;
            nameUPDATE.IsExpanded = false;
            nameINSERT.IsExpanded = false;
        }

        private void nameCopyTo_Expanded(object sender, RoutedEventArgs e) {
            nameCutTo.IsExpanded = false;
            namePasteFrom.IsExpanded = false;

            nameSQL.IsExpanded = false;
            nameSELECT.IsExpanded = false;
            nameDELETE.IsExpanded = false;
            nameUPDATE.IsExpanded = false;
            nameINSERT.IsExpanded = false;
        }

        private void nameCutTo_Expanded(object sender, RoutedEventArgs e) {
            nameCopyTo.IsExpanded = false;
            namePasteFrom.IsExpanded = false;

            nameSQL.IsExpanded = false;
            nameSELECT.IsExpanded = false;
            nameDELETE.IsExpanded = false;
            nameUPDATE.IsExpanded = false;
            nameINSERT.IsExpanded = false;
        }

        private void namePasteFrom_Expanded(object sender, RoutedEventArgs e) {
            nameCopyTo.IsExpanded = false;
            nameCutTo.IsExpanded = false;

            nameSQL.IsExpanded = false;
            nameSELECT.IsExpanded = false;
            nameDELETE.IsExpanded = false;
            nameUPDATE.IsExpanded = false;
            nameINSERT.IsExpanded = false;
        }

        private string strToolTip(List<StringBuilder> listSBtmp) {
            string strTmp = "";
            if (listSBtmp != null) {
                int intLimit = 0;
                foreach (StringBuilder sb in listSBtmp) {
                    if (strTmp != "") strTmp += Environment.NewLine;
                    intLimit++;
                    if (intLimit > 5) {
                        strTmp += "... ... ...";
                        return strTmp;
                    }
                    if (sb.ToString().Length > 100) {
                        strTmp += sb.ToString().Substring(0, 100) + " ... ... ...";
                    }
                    else {
                        strTmp += sb.ToString();
                    }
                }
            }
            else {
                strTmp = "-null-";
            }
            return strTmp;
        }



        // Copy To    Selected ---------------------------------------------------------------------------
        private void CopyToA_Selected(object sender, RoutedEventArgs e) {

            listSB_A = null;
            GC.Collect();
            listSB_A = new List<StringBuilder>();

            listSB_A = copyABCDE(false);
            PasteFromA.ToolTip = strToolTip(listSB_A);

            resetCM();
        }

        private void CopyToB_Selected(object sender, RoutedEventArgs e) {

            listSB_B = null;
            GC.Collect();
            listSB_B = new List<StringBuilder>();

            listSB_B = copyABCDE(false);
            PasteFromB.ToolTip = strToolTip(listSB_B);

            resetCM();
        }

        private void CopyToC_Selected(object sender, RoutedEventArgs e) {

            listSB_C = null;
            GC.Collect();
            listSB_C = new List<StringBuilder>();

            listSB_C = copyABCDE(false);
            PasteFromC.ToolTip = strToolTip(listSB_C);

            resetCM();
        }

        private void CopyToD_Selected(object sender, RoutedEventArgs e) {

            listSB_D = null;
            GC.Collect();
            listSB_D = new List<StringBuilder>();

            listSB_D = copyABCDE(false);
            PasteFromD.ToolTip = strToolTip(listSB_D);

            resetCM();
        }

        private void CopyToE_Selected(object sender, RoutedEventArgs e) {
            listSB_E = null;
            GC.Collect();
            listSB_E = new List<StringBuilder>();

            listSB_E = copyABCDE(false);
            PasteFromE.ToolTip = strToolTip(listSB_E);

            resetCM();
        }

        // Cut To    Selected ---------------------------------------------------------------------------

        private void CutToA_Selected(object sender, RoutedEventArgs e) {
            listSB_A = null;
            GC.Collect();
            listSB_A = new List<StringBuilder>();

            listSB_A = copyABCDE(false);
            if (listSB_A == null) return;
            PasteFromA.ToolTip = strToolTip(listSB_A);
            deleteABCDE();

            resetCM();
        }

        private void CutToB_Selected(object sender, RoutedEventArgs e) {
            listSB_B = null;
            GC.Collect();
            listSB_B = new List<StringBuilder>();


            listSB_B = copyABCDE(false);
            PasteFromB.ToolTip = strToolTip(listSB_B);
            if (listSB_B == null) return;
            deleteABCDE();

            resetCM();
        }

        private void CutToC_Selected(object sender, RoutedEventArgs e) {
            listSB_C = null;
            GC.Collect();
            listSB_C = new List<StringBuilder>();

            listSB_C = copyABCDE(false);
            if (listSB_C == null) return;
            PasteFromC.ToolTip = strToolTip(listSB_C);
            deleteABCDE();

            resetCM();
        }

        private void CutToD_Selected(object sender, RoutedEventArgs e) {
            listSB_D = null;
            GC.Collect();
            listSB_D = new List<StringBuilder>();

            listSB_D = copyABCDE(false);
            if (listSB_D == null) return;
            PasteFromD.ToolTip = strToolTip(listSB_D);
            deleteABCDE();

            resetCM();
        }

        private void CutToE_Selected(object sender, RoutedEventArgs e) {
            listSB_E = null;
            GC.Collect();
            listSB_E = new List<StringBuilder>();

            listSB_E = copyABCDE(false);
            if (listSB_E == null) return;
            PasteFromE.ToolTip = strToolTip(listSB_E);
            deleteABCDE();

            resetCM();
        }


        // Paste From    Selected ---------------------------------------------------------------------------

        private void PasteFromA_Selected(object sender, RoutedEventArgs e) {
            deleteABCDE();
            pasteStandard(listSB_A);
            resetCM();
        }

        private void PasteFromB_Selected(object sender, RoutedEventArgs e) {
            deleteABCDE();
            pasteStandard(listSB_B);
            resetCM();
        }

        private void PasteFromC_Selected(object sender, RoutedEventArgs e) {
            deleteABCDE();
            pasteStandard(listSB_C);
            resetCM();
        }

        private void PasteFromD_Selected(object sender, RoutedEventArgs e) {
            deleteABCDE();
            pasteStandard(listSB_D);
            resetCM();
        }

        private void PasteFromE_Selected(object sender, RoutedEventArgs e) {
            deleteABCDE();
            pasteStandard(listSB_E);
            resetCM();
        }

        // -----------------------------------------------------------------------------------------------------

        private void initContextMenu() {
            resetCM();
            treeContext.Visibility = Visibility.Collapsed;
        }


        public void tb_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e) {
            blnMouseRightButtonPressed = false;


            treeContext.Visibility = Visibility.Visible;
            nameTriStart.IsSelected = true;  // reset previous selectionMain

            Point pnt = Mouse.GetPosition(canvasMain);

            double myAH = treeContext.ActualHeight > 200 ? treeContext.ActualHeight : 200;
            double myAW = treeContext.ActualWidth > 100 ? treeContext.ActualWidth : 100;

            if (pnt.X <= canvasMain.ActualWidth / 2 && pnt.Y <= canvasMain.ActualHeight / 2) {
                tr.X = pnt.X;
                tr.Y = pnt.Y;
            }
            if (pnt.X <= canvasMain.ActualWidth / 2 && pnt.Y > canvasMain.ActualHeight / 2) {
                tr.X = pnt.X;
                tr.Y = pnt.Y - myAH - treeContext.Margin.Top / 2;
            }
            if (pnt.X > canvasMain.ActualWidth / 2 && pnt.Y <= canvasMain.ActualHeight / 2) {
                tr.X = pnt.X - myAW - treeContext.Margin.Left / 2;
                tr.Y = pnt.Y;
            }
            if (pnt.X > canvasMain.ActualWidth / 2 && pnt.Y > canvasMain.ActualHeight / 2) {
                tr.X = pnt.X - myAW - treeContext.Margin.Left / 2;
                tr.Y = pnt.Y - myAH - treeContext.Margin.Top / 2;
            }



        }

        public int? iLineStartABCDE = null;
        public int? iCharStartABCDE = null;

        public int? iLineStopABCDE = null;
        public int? iCharStopABCDE = null;



        public void findABCDE() {

            try {

                iLineStartABCDE = null;
                iCharStartABCDE = null;

                iLineStopABCDE = null;
                iCharStopABCDE = null;

                bool blnValid = false;

                if (intAbsolutCaretVertCurrent >= listSBmain.Count) return;

                if (intAbsolutCaretHorizCurrent > listSBmain[intAbsolutCaretVertCurrent].Length) return;

                if (listSPMain.Count == 0) listSPMain.Add(new listOfSelectedPositionInLine());
                if (listSPMain[intAbsolutCaretVertCurrent].listSPinLine.Count == 0) return;

                for (int i = intAbsolutCaretVertCurrent; i < listSPMain.Count; i++) {

                    if (listSPMain[i].listSPinLine.Count == 0 && i > intAbsolutCaretVertCurrent && blnValid) {
                        iLineStopABCDE = i - 1;
                        iCharStopABCDE = listSBmain[i - 1].Length;
                        goto lmin;
                    }

                    for (int j = 0; j < listSPMain[i].listSPinLine.Count; j++) {
                        selectedPositions sp = listSPMain[i].listSPinLine[j];

                        //for first line
                        if (sp.iCharBegin <= intAbsolutCaretHorizCurrent && intAbsolutCaretHorizCurrent <= sp.iCharEnd && i == intAbsolutCaretVertCurrent) {
                            blnValid = true;

                            if (i == 0) {
                                iLineStartABCDE = 0;
                                iCharStartABCDE = sp.iCharBegin;
                            }

                            if (sp.iCharBegin > 0) {
                                iLineStartABCDE = intAbsolutCaretVertCurrent;
                                iCharStartABCDE = sp.iCharBegin;

                            }

                            if (sp.iCharEnd < listSBmain[intAbsolutCaretVertCurrent].Length ||
                                intAbsolutCaretVertCurrent == listSBmain.Count - 1) {
                                iLineStopABCDE = intAbsolutCaretVertCurrent;
                                iCharStopABCDE = sp.iCharEnd;
                                goto lmin;
                            }

                            if (intAbsolutCaretVertCurrent == listSBmain.Count - 1) {
                                iLineStopABCDE = intAbsolutCaretVertCurrent;
                                iCharStopABCDE = sp.iCharEnd;
                                goto lmin;
                            }
                        }



                        // ----------

                        if (i > intAbsolutCaretVertCurrent && blnValid) {
                            if (sp.iCharEnd == listSBmain[i].Length &&
                                sp.iCharBegin == 0 &&
                                i < listSBmain.Count - 1) continue;

                            if (sp.iCharEnd == listSBmain[i].Length &&
                                sp.iCharBegin == 0 &&
                                i == listSBmain.Count - 1
                                ) {
                                iLineStopABCDE = i;
                                iCharStopABCDE = listSBmain[i].Length;
                                goto lmin;
                            }


                            if (sp.iCharBegin == 0 && i > intAbsolutCaretVertCurrent && sp.iCharEnd < listSBmain[i].Length) {
                                iLineStopABCDE = i;
                                iCharStopABCDE = sp.iCharEnd;
                                goto lmin;
                            }


                            if (sp.iCharBegin != 0 && i > intAbsolutCaretVertCurrent) {
                                iLineStopABCDE = i - 1;
                                iCharStopABCDE = listSBmain[i - 1].Length;
                                goto lmin;
                            }
                        }

                        //-----------

                    }

                }

            lmin:

                if (iLineStartABCDE != null || blnValid == false) return;

                for (int i = intAbsolutCaretVertCurrent - 1; i >= 0; i--) {

                    if (listSPMain[i].listSPinLine.Count == 0 && i < intAbsolutCaretVertCurrent && blnValid) {
                        iLineStartABCDE = i + 1;
                        iCharStartABCDE = 0;
                        return;
                    }

                    for (int j = listSPMain[i].listSPinLine.Count - 1; j >= 0; j--) {
                        selectedPositions sp = listSPMain[i].listSPinLine[j];

                        if (sp.iCharEnd == listSBmain[i].Length && sp.iCharBegin == 0 && i != 0) continue;

                        if (sp.iCharEnd == listSBmain[i].Length && sp.iCharBegin == 0 && i == 0) {
                            iLineStartABCDE = 0;
                            iCharStartABCDE = 0;
                        }

                        if (sp.iCharEnd == listSBmain[i].Length && sp.iCharBegin > 0) {
                            iLineStartABCDE = i;
                            iCharStartABCDE = sp.iCharBegin;
                            return;
                        }

                        if (sp.iCharEnd != listSBmain[i].Length) {
                            iLineStartABCDE = i + 1;
                            iCharStartABCDE = 0;
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Program has a problem allocating additional char(s)/byte(s) in memory (findABCDE) ...\nPlease save your  job ..." + " (1)" + ex.Message);
                blnError = true;
            }
        }


        private void findCurrentCommand() {

            iLineStartABCDE = null;
            iCharStartABCDE = null;

            iLineStopABCDE = null;
            iCharStopABCDE = null;

            if (intAbsolutCaretHorizCurrent > listSBmain[intAbsolutCaretVertCurrent].Length ||
                intAbsolutCaretVertCurrent >= listSBmain.Count) return;

            for (int i = intAbsolutCaretVertCurrent; i < listSBmain.Count; i++) {
                string strTmpEnd = "";
                string strTmpBegin = "";

                if (i == intAbsolutCaretVertCurrent) {
                    strTmpBegin = listSBmain[i].ToString(0, intAbsolutCaretHorizCurrent);

                    if (strTmpBegin.Contains(";")) {
                        iLineStartABCDE = i;
                        iCharStartABCDE = strTmpBegin.IndexOf(";") + 1;
                    }
                }

                if (i == intAbsolutCaretVertCurrent) strTmpEnd = listSBmain[i].ToString(intAbsolutCaretHorizCurrent, listSBmain[i].ToString().Length - intAbsolutCaretHorizCurrent);
                else strTmpEnd = listSBmain[i].ToString();

                if (strTmpEnd.Contains(";")) {
                    iLineStopABCDE = i;
                    iCharStopABCDE = strTmpEnd.IndexOf(";") + 1;
                    if (i == intAbsolutCaretVertCurrent) iCharStopABCDE += intAbsolutCaretHorizCurrent;
                    goto lmin;
                }
            }

            iLineStopABCDE = listSBmain.Count - 1;
            iCharStopABCDE = listSBmain[(int)iLineStopABCDE].Length;


        lmin:

            if (iLineStartABCDE != null && iLineStopABCDE != null) return;

            for (int i = intAbsolutCaretVertCurrent - 1; i >= 0; i--) {

                string strTmpEnd = "";
                strTmpEnd = listSBmain[i].ToString();

                if (strTmpEnd.Contains(";")) {
                    iLineStartABCDE = i;
                    iCharStartABCDE = strTmpEnd.LastIndexOf(";") + 1;
                    if (iCharStartABCDE >= listSBmain[i].Length) {
                        iCharStartABCDE = 0;
                        iLineStartABCDE += 1;
                    }
                    return;
                }
            }

            if (iLineStartABCDE == null) {
                iLineStartABCDE = 0;
                iCharStartABCDE = 0;
            }
        }



        private StringBuilder sbLineStart() {

            StringBuilder sb = new StringBuilder();

            foreach (selectedPositions sp in listSPMain[(int)iLineStartABCDE].listSPinLine) {
                int? intStart = null;
                int? intStop = null;

                if (sp.iCharBegin < iCharStartABCDE && iCharStartABCDE < sp.iCharEnd) intStart = iCharStartABCDE;
                if (sp.iCharBegin < iCharStopABCDE && iCharStopABCDE < sp.iCharEnd) intStop = iCharStopABCDE;
                if (sp.iCharBegin >= (int)iCharStartABCDE) {
                    intStart = sp.iCharBegin;
                    intStop = sp.iCharEnd;
                }
                if (intStart == null || intStop == null) continue;
                sb.Append(listSBmain[(int)iLineStartABCDE].ToString((int)intStart, (int)(intStop - intStart))).Append(" ");
            }
            return sb;
        }

        private StringBuilder sbLineStop() {

            StringBuilder sb = new StringBuilder();

            foreach (selectedPositions sp in listSPMain[(int)iLineStopABCDE].listSPinLine) {
                int? intStart = null;
                int? intStop = null;

                if (sp.iCharBegin < iCharStopABCDE && iCharStopABCDE < sp.iCharEnd) intStop = iCharStopABCDE;
                if (sp.iCharBegin >= 0 && sp.iCharEnd <= (int)iCharStopABCDE) {
                    intStart = sp.iCharBegin;
                    intStop = sp.iCharEnd;
                }
                if (intStart == null || intStop == null) continue;
                sb.Append(listSBmain[(int)iLineStopABCDE].ToString((int)intStart, (int)(intStop - intStart))).Append(" ");
            }
            return sb;
        }

        public List<StringBuilder> copyABCDESelected(bool blnAllSQL) {
            try {


                correctLocation();
                List<StringBuilder> listSbTmp = new List<StringBuilder>();

                if (blnAllSQL) {
                    iLineStartABCDE = 0;
                    iCharStartABCDE = 0;
                    iLineStopABCDE = listSBmain.Count - 1;
                    iCharStopABCDE = listSBmain[(int)iLineStopABCDE].Length;
                }
                else findCurrentCommand();


                if (iLineStartABCDE != null &&
                    iCharStartABCDE != null &&
                    iLineStopABCDE != null &&
                    iCharStopABCDE != null) {

                    if (iLineStartABCDE == iLineStopABCDE) {
                        listSbTmp.Add(sbLineStart());
                        return listSbTmp;
                    }
                    else {
                        for (int i = (int)iLineStartABCDE; i <= (int)iLineStopABCDE; i++) {

                            if (i == iLineStartABCDE) {
                                StringBuilder sb = sbLineStart();
                                if (sb.ToString() != "") listSbTmp.Add(sb);
                            }
                            if (i != iLineStartABCDE && i != iLineStopABCDE) {
                                StringBuilder sb = new StringBuilder();

                                foreach (selectedPositions sp in listSPMain[i].listSPinLine) {
                                    sb.Append(listSBmain[i].ToString(sp.iCharBegin, sp.iCharEnd - sp.iCharBegin)).Append(" ");
                                }
                                if (sb.ToString() != "") listSbTmp.Add(sb);
                            }

                            if (i == iLineStopABCDE && iLineStartABCDE != iLineStopABCDE) {
                                StringBuilder sb = sbLineStop();
                                if (sb.ToString() != "") listSbTmp.Add(sbLineStop());
                            }
                        }
                        return listSbTmp;
                    }
                }
                else {
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Program has a problem allocating additional char(s)/byte(s) in memory (copyABCDESelected) ...\nPlease save your  job ..." + " (18)" + ex.Message);
                blnError = true;
                return null;
            }
        }


        public List<StringBuilder> copyABCDE(bool blnCurrentSQL) {

            try {
                long longGC = GC.GetTotalMemory(false);
                Microsoft.VisualBasic.Devices.ComputerInfo myComputer = new Microsoft.VisualBasic.Devices.ComputerInfo();
                ulong totalAvailable = myComputer.AvailablePhysicalMemory;

                correctLocation();
                List<StringBuilder> listSbTmp = new List<StringBuilder>();

                if (blnCurrentSQL) findCurrentCommand();
                else findABCDE();

                ulong uTotal = 0;

                if (iLineStartABCDE != null &&
                    iCharStartABCDE != null &&
                    iLineStopABCDE != null &&
                    iCharStopABCDE != null) {

                    if (iLineStartABCDE == iLineStopABCDE) {
                        string strTmp = listSBmain[(int)iLineStartABCDE].ToString((int)iCharStartABCDE, (int)iCharStopABCDE - (int)iCharStartABCDE );
                        listSbTmp.Add(new StringBuilder(strTmp));
                        return listSbTmp;
                    }
                    else {
                        for (int i = (int)iLineStartABCDE; i <= (int)iLineStopABCDE; i++) {

                            if (i == iLineStartABCDE) {
                                string strTmp = listSBmain[i].ToString((int)iCharStartABCDE, listSBmain[i].Length - (int)iCharStartABCDE);
                                uTotal += (ulong)strTmp.Length;
                                listSbTmp.Add(new StringBuilder(strTmp));
                            }
                            if (i != iLineStartABCDE && i != iLineStopABCDE) {
                                string strTmp = listSBmain[i].ToString(0, listSBmain[i].Length);
                                uTotal += (ulong)strTmp.Length;
                                listSbTmp.Add(new StringBuilder(strTmp));
                            }

                            if (i == iLineStopABCDE && iLineStartABCDE != iLineStopABCDE) {
                                string strTmp = listSBmain[i].ToString(0, (int)iCharStopABCDE);
                                uTotal += (ulong)strTmp.Length;
                                listSbTmp.Add(new StringBuilder(strTmp));
                            }

                            if (uTotal > 500000000) {
                                resetCM();
                                MessageBox.Show("You reach limit 0.5G for copy data!");
                                listSbTmp = null;
                                return null;
                            }

                            if (uTotal > totalAvailable / 2) {
                                resetCM();
                                MessageBox.Show("Not enough available memory (" + totalAvailable.ToString("0,0") + " bytes.) \nUse smaller portion...");
                                listSbTmp = null;
                                return null;
                            }


                        }
                        return listSbTmp;
                    }
                }
                else {
                    return null;
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Program has a problem allocating additional char(s)/byte(s) in memory ( copyABCDE) ...\nPlease save your  job ..." + " (19)" + ex.Message);
                blnError = true;
                return null;
            }
        }


        public void deleteABCDE()  // Delete Current Selection
        {
            try {

                correctLocation();

                findABCDE();

                if (iLineStartABCDE != null &&
                    iCharStartABCDE != null &&
                    iLineStopABCDE != null &&
                    iCharStopABCDE != null) {

                    if (iLineStartABCDE == iLineStopABCDE) {
                        listSBmain[(int)iLineStartABCDE].Remove((int)iCharStartABCDE, (int)iCharStopABCDE - (int)iCharStartABCDE);
                    }
                    else {
                        for (int i = (int)iLineStartABCDE; i <= (int)iLineStopABCDE; i++) {

                            if (i == iLineStartABCDE) {
                                listSBmain[i].Remove((int)iCharStartABCDE, listSBmain[i].Length - (int)iCharStartABCDE);
                            }
                            if (i != iLineStartABCDE && i != iLineStopABCDE) {
                                listSBmain[i].Remove(0, listSBmain[i].Length);
                            }

                            if (i == iLineStopABCDE && iLineStartABCDE != iLineStopABCDE) {
                                listSBmain[i].Remove(0, (int)iCharStopABCDE);
                            }
                        }
                    }


                    for (int i = (int)iLineStopABCDE; i >= (int)iLineStartABCDE; i--) {
                        foreach (selectedPositions sp in listSPMain[i].listSPinLine) {
                            if (sp.iCollection == null) continue;

                            var recc = (canvasSelected.Children[(int)sp.iCollection]) as selectedRectangle;
                            set_Rectangle(ref recc, 0, 0, 0, null);
                            sp.iCharBegin = (int)iCharStartABCDE;
                            sp.iCharEnd = (int)iCharStartABCDE;
                            sp.iCollection = null;
                        }

                        listSPMain[i].listSPinLine.Clear();

                        if (listSBmain[i].Length == 0 && iLineStartABCDE != iLineStopABCDE && i != iLineStopABCDE) {
                            listSBmain.RemoveAt(i);
                            listSPMain.RemoveAt(i);
                        }
                    }

                    if (iCharStartABCDE != 0 && iLineStartABCDE != iLineStopABCDE && iLineStartABCDE != listSBmain.Count - 1) {

                        listSBmain[(int)iLineStartABCDE].Append(listSBmain[(int)iLineStartABCDE + 1]);
                        listSBmain.RemoveAt((int)iLineStartABCDE + 1);
                        listSPMain.RemoveAt((int)iLineStartABCDE + 1);
                    }

                    intAbsolutCaretVertCurrent = (int)iLineStartABCDE;
                    intAbsolutSelectVertStart = (int)iLineStartABCDE;
                    intAbsolutCaretHorizCurrent = (int)iCharStartABCDE;
                    intAbsolutSelectHorizStart = (int)iCharStartABCDE;
                }

                totalBytes();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Program has a problem allocating additional char(s)/byte(s) in memory (deleteABCDE) ...\nPlease save your  job ..." + " (20)" + ex.Message);
                blnError = true;
            }
        }


        //-----------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------
        public void pasteGroup(List<StringBuilder> listSBgroup) {

            try {



                int intListSPMainCount = listSPMain.Count; // Total Lines
                int intLinesSelected = 0; // <-- Line with selected position

                int iLinesCountCB = listSBgroup.Count; // count lines in CB

                //   Parallel.For(0, intListSPMainCount, (int i) =>

                for (int i = 0; i < intListSPMainCount; i++) {

                    int iHorizontSPCount = listSPMain[i].listSPinLine.Count; //Counted Selection Position in line...
                    if (iHorizontSPCount > 0) { // if selected per line is not null

                        string[] strArray;
                        if (listSBgroup.Count != 0) {
                            strArray = listSBgroup[intLinesSelected % iLinesCountCB].ToString().Split('║');   // horizontal strings // by module for cycle
                        }
                        else {
                            strArray = new string[1];
                            strArray[0] = "";
                        }


                        int iCBHorizontStringsCount = strArray.GetLength(0); // dimension  

                        for (int jHselected = iHorizontSPCount - 1; jHselected >= 0; jHselected--) {
                            string strCB = strArray[jHselected % iCBHorizontStringsCount]; // count down - 

                            selectedPositions sp = listSPMain[i].listSPinLine[jHselected];
                            listSBmain[i].Remove(sp.iCharBegin, sp.iCharEnd - sp.iCharBegin);
                            listSBmain[i].Insert(sp.iCharBegin, strCB);
                        }

                        int iDelta = 0;
                        for (int jHselected = 0; jHselected < iHorizontSPCount; jHselected++) {

                            int intBegin = listSPMain[i].listSPinLine[jHselected].iCharBegin;
                            int intEnd = listSPMain[i].listSPinLine[jHselected].iCharEnd;

                            int intCB = strArray[jHselected % iCBHorizontStringsCount].Length;

                            listSPMain[i].listSPinLine[jHselected].iCharBegin = intBegin + iDelta;
                            listSPMain[i].listSPinLine[jHselected].iCharEnd = intBegin + intCB + iDelta;
                            iDelta = iDelta + intCB - (intEnd - intBegin);
                        }

                        intLinesSelected++;
                    }
                }
                //   );
                totalBytes();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Program has a problem allocating additional char(s)/byte(s) in memory (pasteGroup) ...\nPlease save your  job ..." + " (21)" + ex.Message);
                blnError = true;
            }
        }


        public void pasteStandard(List<StringBuilder> listSBstandard) {

            try {


                if (listSBmain.Count == 0) {
                    listSBmain.Add(new StringBuilder());
                    listSPMain.Add(new listOfSelectedPositionInLine());
                }


                int iLinesCountCB = listSBstandard.Count; // count lines in CB
                if (iLinesCountCB == 0) return;

                correctLocation();
                string str0 = "";
                string str1 = "";

                if (intAbsolutCaretHorizCurrent > 0) {
                    str0 = listSBmain[intAbsolutCaretVertCurrent].ToString(0, intAbsolutCaretHorizCurrent);
                }

                if (intAbsolutCaretHorizCurrent < listSBmain[intAbsolutCaretVertCurrent].Length) {// -1
                    int intLength = listSBmain[intAbsolutCaretVertCurrent].Length - intAbsolutCaretHorizCurrent;
                    str1 = listSBmain[intAbsolutCaretVertCurrent].ToString(intAbsolutCaretHorizCurrent, intLength);
                }

                StringBuilder sb = new StringBuilder();
                sb.Append(str0).Append(listSBstandard[0].ToString().Replace("\t", "     "));

                listSBmain.RemoveAt(intAbsolutCaretVertCurrent);

                foreach (selectedPositions spp in listSPMain[intAbsolutCaretVertCurrent].listSPinLine) ///////////////////////////////////////////
            {
                    // Do not remove REC - just update to null
                    if (spp.iCollection == null) continue;
                    var recc = (canvasSelected.Children[(int)spp.iCollection]) as selectedRectangle;
                    set_Rectangle(ref recc, 0, 0, 0, null);
                }

                listSPMain.RemoveAt(intAbsolutCaretVertCurrent);


                listSBmain.Insert(intAbsolutCaretVertCurrent, sb);
                listSPMain.Insert(intAbsolutCaretVertCurrent, new listOfSelectedPositionInLine());

                //  Parallel.For(1, iLinesCountCB, (int i) =>
                for (int i = 1; i < iLinesCountCB; i++) {
                    sb = new StringBuilder(listSBstandard[i].ToString().Replace("\t", "     "));
                    listSBmain.Insert(intAbsolutCaretVertCurrent + i, sb);
                    listSPMain.Insert(intAbsolutCaretVertCurrent + i, new listOfSelectedPositionInLine());
                }
                // );

                int intH = listSBmain[intAbsolutCaretVertCurrent + iLinesCountCB - 1].Length;

                if (str1 != "") {
                    listSBmain[intAbsolutCaretVertCurrent + iLinesCountCB - 1].Append(str1);
                    //  listSBmain[].Insert(intAbsolutCaretVertCurrent + iLinesCountCB - 1, new StringBuilder(str1));
                    //  listSPMain.Insert(intAbsolutCaretVertCurrent + iLinesCountCB - 1, new listOfSelectedPositionInLine());
                }

                totalBytes();

                intAbsolutCaretHorizCurrent = intH;
                intAbsolutCaretVertCurrent = intAbsolutCaretVertCurrent + iLinesCountCB - 1;
            }
            catch (Exception ex) {
                MessageBox.Show("Program has a problem allocating additional char(s)/byte(s) in memory (pasteStandard) ...\nPlease save your  job ..." + " (22)" + ex.Message);
                blnError = true;
            }


        }

        private void SELECT_expanded(object sender, RoutedEventArgs e) {
            //   nameSELECT.IsExpanded = false;
            nameDELETE.IsExpanded = false;
            nameUPDATE.IsExpanded = false;
            nameINSERT.IsExpanded = false;
        }

        private void DELETE_expanded(object sender, RoutedEventArgs e) {
            nameSELECT.IsExpanded = false;
            //  nameDELETE.IsExpanded = false;
            nameUPDATE.IsExpanded = false;
            nameINSERT.IsExpanded = false;
        }

        private void UPDATE_expanded(object sender, RoutedEventArgs e) {
            nameSELECT.IsExpanded = false;
            nameDELETE.IsExpanded = false;
            //   nameUPDATE.IsExpanded = false;
            nameINSERT.IsExpanded = false;
        }

        private void INSERT_expanded(object sender, RoutedEventArgs e) {
            nameSELECT.IsExpanded = false;
            nameDELETE.IsExpanded = false;
            nameUPDATE.IsExpanded = false;
            //     nameINSERT.IsExpanded = false;
        }

        private void SQL_expanded(object sender, RoutedEventArgs e) {
            nameCopyTo.IsExpanded = false;
            namePasteFrom.IsExpanded = false;
            nameCutTo.IsExpanded = false;
        }


        public void syntaxPaste(string syntaxName, bool blnFull) {
            try {
                selectedSQLsyntax sqlSyntax = new selectedSQLsyntax(strDataBaseName, blnFull);
                string strTmp = "";

                if (syntaxName == "SELECT") strTmp = sqlSyntax.SelectSQL;
                if (syntaxName == "INSERT") strTmp = sqlSyntax.InsertSQL;
                if (syntaxName == "DELETE") strTmp = sqlSyntax.DeleteSQL;
                if (syntaxName == "UPDATE") strTmp = sqlSyntax.UpdateSQL;

                List<StringBuilder> listSb = new List<StringBuilder>();


                listSb.Add(new StringBuilder(""));
                listSb.Add(new StringBuilder(""));

                foreach (string str in strTmp.Split('\n')) {
                    listSb.Add(new StringBuilder(str.Replace("\r", "")));
                }

                listSb.Add(new StringBuilder(""));
                listSb.Add(new StringBuilder(""));

                deleteABCDE();
                pasteStandard(listSb);
                resetCM();

                scrollX.Value = 0;


            }
            catch (Exception ex)
            {
                MessageBox.Show("Program has a problem allocating additional char(s)/byte(s) in memory (syntaxPaste) ...\nPlease save your  job ..." + " (23)" + ex.Message);
                blnError = true;
            }

        }




        private void SelectedSelectMain(object sender, RoutedEventArgs e) {
            syntaxPaste("SELECT", false);
        }

        private void SelectedSelectFull(object sender, RoutedEventArgs e) {
            syntaxPaste("SELECT", true);
        }



        private void SelectedDeleteMain(object sender, RoutedEventArgs e) {
            syntaxPaste("DELETE", false);
        }

        private void SelectedDeleteFull(object sender, RoutedEventArgs e) {
            syntaxPaste("DELETE", true);
        }



        private void SelectedUpdateMain(object sender, RoutedEventArgs e) {
            syntaxPaste("UPDATE", false);
        }

        private void SelectedUpdateFull(object sender, RoutedEventArgs e) {
            syntaxPaste("UPDATE", true);
        }



        private void SelectedInsertMain(object sender, RoutedEventArgs e) {
            syntaxPaste("INSERT", false);
        }

        private void SelectedInsertFull(object sender, RoutedEventArgs e) {
            syntaxPaste("INSERT", true);
        }




        private void Comment_Selected(object sender, RoutedEventArgs e) {
            try {


                correctLocation();
                findABCDE();

                if (iLineStartABCDE != null) {
                    for (int i = (int)iLineStartABCDE; i <= (int)iLineStopABCDE; i++) {
                        if (i == (int)iLineStartABCDE) listSBmain[i].Insert((int)iCharStartABCDE, "--");
                        else {
                            listSBmain[i].Insert(0, "--");
                        }
                        listSPMain[i].listSPinLine.Clear();
                    }
                }
                else {
                    listSBmain[intAbsolutCaretVertCurrent] = listSBmain[intAbsolutCaretVertCurrent].Insert(0, "--");
                }

                resetCM();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Program has a problem allocating additional char(s)/byte(s) in memory (Comment_Selected) ...\nPlease save your  job ..." + " (24)" + ex.Message);
                blnError = true;
            }
        }



        private void upper_Selected(object sender, RoutedEventArgs e) {
            try {
                correctLocation();
                findABCDE();

                if (iLineStartABCDE == null) return;

                for (int i = (int)iLineStartABCDE; i <= (int)iLineStopABCDE; i++) {

                    if (iLineStartABCDE == iLineStopABCDE) {
                        string str0 = listSBmain[i].ToString(0, (int)iCharStartABCDE);
                        string str1 = listSBmain[i].ToString((int)iCharStartABCDE, (int)iCharStopABCDE - (int)iCharStartABCDE);
                        string str2 = listSBmain[i].ToString((int)iCharStopABCDE, listSBmain[i].Length - (int)iCharStopABCDE);
                        listSBmain[i] = new StringBuilder(str0 + str1.ToUpper() + str2);
                    }

                    if (i == (int)iLineStartABCDE && (iLineStartABCDE != iLineStopABCDE)) {
                        string str0 = listSBmain[i].ToString(0, (int)iCharStartABCDE);
                        string str1 = listSBmain[i].ToString((int)iCharStartABCDE, listSBmain[i].Length - (int)iCharStartABCDE);
                        listSBmain[i] = new StringBuilder(str0 + str1.ToUpper());
                    }
                    if (i != (int)iLineStartABCDE && i != (int)iLineStopABCDE) {
                        listSBmain[i] = new StringBuilder(listSBmain[i].ToString().ToUpper());
                    }
                    if (i == (int)iLineStopABCDE && (iLineStartABCDE != iLineStopABCDE)) {
                        string str0 = listSBmain[i].ToString(0, (int)iCharStopABCDE);
                        string str1 = listSBmain[i].ToString((int)iCharStopABCDE, listSBmain[i].Length - (int)iCharStopABCDE);
                        listSBmain[i] = new StringBuilder(str0.ToUpper() + str1);
                    }
                }


                resetCM();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Program has a problem allocating additional char(s)/byte(s) in memory (upper_Selected) ...\nPlease save your  job ..." + " (25)" + ex.Message);
                blnError = true;
            }

        }

        private void lower_Selected(object sender, RoutedEventArgs e) {

            try {
                correctLocation();
                findABCDE();

                if (iLineStartABCDE == null) return;

                for (int i = (int)iLineStartABCDE; i <= (int)iLineStopABCDE; i++) {
                    if (iLineStartABCDE == iLineStopABCDE) {
                        string str0 = listSBmain[i].ToString(0, (int)iCharStartABCDE);
                        string str1 = listSBmain[i].ToString((int)iCharStartABCDE, (int)iCharStopABCDE - (int)iCharStartABCDE);
                        string str2 = listSBmain[i].ToString((int)iCharStopABCDE, listSBmain[i].Length - (int)iCharStopABCDE);
                        listSBmain[i] = new StringBuilder(str0 + str1.ToLower() + str2);
                    }

                    if (i == (int)iLineStartABCDE && (iLineStartABCDE != iLineStopABCDE)) {
                        string str0 = listSBmain[i].ToString(0, (int)iCharStartABCDE);
                        string str1 = listSBmain[i].ToString((int)iCharStartABCDE, listSBmain[i].Length - (int)iCharStartABCDE);
                        listSBmain[i] = new StringBuilder(str0 + str1.ToLower());
                    }

                    if (i != (int)iLineStartABCDE && i != (int)iLineStopABCDE) {
                        listSBmain[i] = new StringBuilder(listSBmain[i].ToString().ToLower());
                    }

                    if (i == (int)iLineStopABCDE && (iLineStartABCDE != iLineStopABCDE)) {
                        string str0 = listSBmain[i].ToString(0, (int)iCharStopABCDE);
                        string str1 = listSBmain[i].ToString((int)iCharStopABCDE, listSBmain[i].Length - (int)iCharStopABCDE);
                        listSBmain[i] = new StringBuilder(str0.ToLower() + str1);
                    }
                }
                resetCM();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Program has a problem allocating additional char(s)/byte(s) in memory (lower_Selected) ...\nPlease save your  job ..." + " (26)" + ex.Message);
                blnError = true;
            }


        }




        private void UnComment_Selected(object sender, RoutedEventArgs e) {
            try {
                correctLocation();
                findABCDE();

                if (iLineStartABCDE != null) {
                    for (int i = (int)iLineStartABCDE; i <= (int)iLineStopABCDE; i++) {
                        listSBmain[i] = listSBmain[i].Replace("--", "");
                        listSPMain[i].listSPinLine.Clear();
                    }

                }
                else {
                    listSBmain[intAbsolutCaretVertCurrent] = listSBmain[intAbsolutCaretVertCurrent].Replace("--", "");
                }

                resetCM();
            }
            catch (Exception ex) {
                MessageBox.Show("Program has a problem allocating additional char(s)/byte(s) in memory (UnComment_Selected) ...\nPlease save your  job ..." + " (27)" + ex.Message);
                blnError = true;
            }


        }

        private void scrollY_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            blnMouseLeftButtonPressed = true;
        }

        private void scrollY_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            blnMouseLeftButtonPressed = false;
            updateBlock();
        }

        private void scrollX_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            blnMouseLeftButtonPressed = true;
        }

        private void scrollX_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            blnMouseLeftButtonPressed = false;
            updateBlock();
        }




        public void compressABCDE() {  // Step by step compressing ...

            try {
                Dispatcher.Invoke(new Action(() => correctLocation()));
                Dispatcher.Invoke(new Action(() => findABCDE()));

                if (iLineStartABCDE == null) {
                    MessageBox.Show("You do not select something ...");
                    return;
                }

                StringBuilder sbTmp = new StringBuilder();
                int intTmp = 0;

                for (int i = (int)iLineStartABCDE; i <= (int)iLineStopABCDE; i++) {
                    if (listSBmain[i].ToString().Contains("--") || listSBmain[i].ToString().Contains("/*")) {
                        MessageBox.Show("Please do not Select Comment ... compress was aborted ... no changes ...");
                        return;
                    };
                    sbTmp.Append(listSBmain[i]).Append(" ");

                    if (blnAbort) {
                        MessageBox.Show("Format was Aborted ..");
                        blnAbort = false;
                        return;
                    }
                }

                selectedSqlKeys sqlNewLine = new selectedSqlKeys();
                List<string> strNewLine = sqlNewLine.newLineBefore;

                sbTmp.Replace('\n', ' ').Replace('\r', ' ').Replace(" ,", ",").Replace(", ", ",").Replace(" (", "(").Replace("( ", "(").Replace(" )", ")").Replace(") ", ")").Replace("  ", " ").Replace(" [", "[").Replace("[ ", "[").Replace(" ]", "]").Replace("] ", "]");
                //            sbTmp.Replace('\n', ' ').Replace('\r', ' ').Replace(" ,", ",").Replace(", ", ",").Replace(" (", "(").Replace("( ", "(").Replace(" )", ")").Replace(") ", ")").Replace("  ", " ");

                listSBmain.RemoveRange((int)iLineStartABCDE + intTmp, (int)iLineStopABCDE - (int)iLineStartABCDE + 1);
                listSPMain.RemoveRange((int)iLineStartABCDE + intTmp, (int)iLineStopABCDE - (int)iLineStartABCDE + 1);

                listSBmain.Insert((int)iLineStartABCDE + intTmp, new StringBuilder(sbTmp.ToString().Trim()));
                listSPMain.Insert((int)iLineStartABCDE + intTmp, new listOfSelectedPositionInLine(0, sbTmp.Length, null));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Program has a problem allocating additional char(s)/byte(s) in memory (compressABCDE) ...\nPlease save your  job ..." + " (28)" + ex.Message);
                blnError = true;
            }


        }




        private bool compressInList(int intLine, int intStartChar, int StopChar) {
            int intTmp = listSBmain[intLine].Length;

            listSBmain[intLine].Replace("  ", " ");
            listSBmain[intLine].Replace(" , ", ",");
            listSBmain[intLine].Replace(", ", ",");
            listSBmain[intLine].Replace(" ,", ",");
            listSBmain[intLine].Replace(" )", ")");
            listSBmain[intLine].Replace("( ", "(");
            listSBmain[intLine].Replace(" ]", "]");
            listSBmain[intLine].Replace("[ ", "[");
            listSBmain[intLine].Replace("\n", " ");
            listSBmain[intLine].Replace("\r", " ");
            listSBmain[intLine].Replace(" ;", ";");
            listSBmain[intLine].Replace("; ", ";");

            if (listSBmain[intLine].ToString() == " ") {
                listSBmain[intLine] = new StringBuilder("");
            }

            if (listSBmain[intLine].Length != intTmp) return true;
            return false;
        }


        public void formatABCDE(string strSplit) {

            try {
                Dispatcher.Invoke(new Action(() => correctLocation()));
                Dispatcher.Invoke(new Action(() => findABCDE()));

                if (iLineStartABCDE == null) {
                    MessageBox.Show("You do not select something ...");
                    return;
                }

                StringBuilder sbTmp = new StringBuilder(" ");

                for (int i = (int)iLineStartABCDE; i <= (int)iLineStopABCDE; i++) {
                    if (listSBmain[i].ToString().Contains("--") || listSBmain[i].ToString().Contains("/*")) {
                        MessageBox.Show("Please do not Select Comment ... format was aborted ... no changes ...");
                        return;
                    };
                    if ((int)iCharStopABCDE == 0 && (int)iLineStopABCDE == i) continue;

                    sbTmp.Append(listSBmain[i]).Append(" ").Append('☼').Append(" ");  // make one string 

                    if (blnAbort) {
                        MessageBox.Show("Format was Aborted .");
                        blnAbort = false;
                        return;
                    }
                }

                selectedSqlKeys sqlNewLine = new selectedSqlKeys();

                List<string> strNewLine = new List<string>();

                if (strSplit == "") {
                    strNewLine = sqlNewLine.newLineBefore;
                }
                else {
                    if (strSplit.Contains('☼')) {
                        strNewLine.Add(strSplit);
                    }
                    else {
                        if (strSplit.Length == 1) strNewLine.Add(strSplit + "?" + strSplit + "☼");
                        else strNewLine.Add(strSplit + "?" + "☼" + strSplit);
                    }
                }

                sbTmp = sbTmp.Replace("(", " ( ").Replace(")", " ) ").Replace("[", " [ ").Replace("]", " ] ").Replace("\n", "").Replace("\r", "").Replace(",", " , ").Replace("  ", " ");


                foreach (string str in strNewLine) {

                    if (sbTmp.ToString().Contains(str.Split('?')[0])) sbTmp.Replace(str.Split('?')[0], str.Split('?')[1]).Replace("  ", " ");

                    if (blnAbort) {
                        MessageBox.Show("Format was Aborted ..");
                        blnAbort = false;
                        return;
                    }
                }

                int intTmp = 0;

                //before ADD remove OLD
                for (int i = (int)iLineStartABCDE; i <= (int)iLineStopABCDE; i++) {
                    if ((int)iCharStopABCDE == 0 && (int)iLineStopABCDE == i) continue;
                    if (listSBmain[(int)iLineStartABCDE + intTmp].ToString().Contains("--") || listSBmain[(int)iLineStartABCDE + intTmp].ToString().Contains("/*")) {
                        intTmp += 1;
                        continue;
                    }
                    listSBmain.RemoveAt((int)iLineStartABCDE + intTmp);
                    listSPMain.RemoveAt((int)iLineStartABCDE + intTmp);

                    if (blnAbort) {
                        MessageBox.Show("Format was Aborted ...");
                        blnAbort = false;
                        return;
                    }
                }
                //---------------------

                List<string> strFinal = sbTmp.ToString().Split('☼').ToList();
                List<StringBuilder> listSBTmp = new List<StringBuilder>();
                List<listOfSelectedPositionInLine> listSPTmp = new List<listOfSelectedPositionInLine>();

                for (int j = 0; j < strFinal.Count; j++) {
                    StringBuilder sb = new StringBuilder(strFinal[j]);
                    sb.Replace(" ,", ",").Replace(", ", ",").Replace(" (", "(").Replace("( ", "(").Replace(" )", ")").Replace(") ", ")").Replace(" [", "[").Replace("[ ", "[").Replace(" ]", "]").Replace("] ", "]").Replace("  ", " ");

                    listSBTmp.Add(sb);
                    listSPTmp.Add(new listOfSelectedPositionInLine(0, sb.Length, null));

                    if (blnAbort) {
                        MessageBox.Show("Format was Aborted ....");
                        blnAbort = false;
                        return;
                    }
                }

                listSBmain.InsertRange((int)iLineStartABCDE + intTmp, listSBTmp);
                listSPMain.InsertRange((int)iLineStartABCDE + intTmp, listSPTmp);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Program has a problem allocating additional char(s)/byte(s) in memory (formatABCDE) ...\nPlease save your  job ..." + " (29)" + ex.Message);
                blnError = true;
            }
        }


        private void listBoxHelper_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            listEnter();

        }

        private void listEnter() {
            try {
                // find again 
                if (strHistory.Contains(".")) {
                    string strTmp = listSBmain[intAbsolutCaretVertCurrent].ToString(0, intAbsolutCaretHorizCurrent).Replace('(', ' ').Replace(')', ' ').Replace("[", "").Replace("]", "").Replace(',', ' ').Replace(';', ' ').ToUpper();
                    if (strTmp.Length > 0) {
                        int intStart = strTmp.LastIndexOf(' ');
                        intStart = intStart < 0 ? 0 : intStart;
                        strHistory = strTmp.Substring(intStart < 0 ? 0 : intStart).Trim();
                    }
                }

                listBoxHelper.Visibility = Visibility.Collapsed;
                string strSelected = listBoxHelper.SelectedValue as string;

                listSBmain[intAbsolutCaretVertCurrent].Remove(intAbsolutCaretHorizCurrent - strHistory.Length, strHistory.Length);
                listSBmain[intAbsolutCaretVertCurrent].Insert(intAbsolutCaretHorizCurrent - strHistory.Length, strSelected, 1);


                intAbsolutCaretHorizCurrent = intAbsolutCaretHorizCurrent - strHistory.Length + strSelected.Length;
                intAbsolutSelectHorizStart = intAbsolutCaretHorizCurrent;

                int intShift = strSelected.Length - strHistory.Length;
                correctSPwhenLineWasModified(intShift, intAbsolutCaretHorizCurrent, intAbsolutCaretVertCurrent);

                strHistory = "";

                set_Caret(intAbsolutCaretVertCurrent, intAbsolutCaretHorizCurrent);
                if (intAbsolutCaretHorizCurrent > intFirstCharOnPageUpdate + intHorizCountCharsOnPage) scrollX.Value = intAbsolutCaretHorizCurrent - intHorizCountCharsOnPage + 2;
                else updateBlock();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Program has a problem allocating additional char(s)/byte(s) in memory (listEnter) ...\nPlease save your  job ..." + " (30)" + ex.Message);
                blnError = true;
            }

        }

        private void listBoxHelper_PreviewKeyDown(object sender, KeyEventArgs e) {

            if (e.Key == Key.Enter) {
                listEnter();
            }
            if (e.Key == Key.Up || e.Key == Key.Down) {

            }
            else {
                e.Handled = true;
                listBoxHelper.Visibility = Visibility.Collapsed;
                strHistory = "";
                updateBlock();
            }
        }

        public void clear_ABCD() {
            try {
                Dispatcher.Invoke(new Action(() => {
                    listSB_A = null;
                    listSB_B = null;
                    listSB_C = null;
                    listSB_D = null;
                    listSB_E = null;

                    GC.Collect();
                    Thread.Sleep(200);
                    
                    listSB_A = new List<StringBuilder>();
                    listSB_B = new List<StringBuilder>();
                    listSB_C = new List<StringBuilder>();
                    listSB_D = new List<StringBuilder>();
                    listSB_E = new List<StringBuilder>();

                    PasteFromA.ToolTip = "";
                    PasteFromB.ToolTip = "";
                    PasteFromC.ToolTip = "";
                    PasteFromD.ToolTip = "";
                    PasteFromE.ToolTip = "";

                    totalBytes();
                    resetCM();
                    Clipboard.SetText("");
                }));
            }
            catch {

            }
        }


        private void clear_ABCD(object sender, RoutedEventArgs e) {
            clear_ABCD();
        }
    }
}
