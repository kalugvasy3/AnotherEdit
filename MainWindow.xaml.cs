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
using System.Reflection;

using System.Diagnostics;

using Microsoft.Win32;

using System.Data;

using System.Threading;
using System.Threading.Tasks;

//using System.Threading.Tasks

using AnotherEdit;

namespace AnotherEdit
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // each listSB object has listSB+MEMO+SELECTED
        private List<StringBuilder> listSB_S1 = new List<StringBuilder>() { new StringBuilder() }; private memoScreen memoS1 = new memoScreen(); private List<listOfSelectedPositionInLine> selectedS1 = new List<listOfSelectedPositionInLine>();
        private List<StringBuilder> listSB_S2 = new List<StringBuilder>() { new StringBuilder() }; private memoScreen memoS2 = new memoScreen(); private List<listOfSelectedPositionInLine> selectedS2 = new List<listOfSelectedPositionInLine>();
        private List<StringBuilder> listSB_S3 = new List<StringBuilder>() { new StringBuilder() }; private memoScreen memoS3 = new memoScreen(); private List<listOfSelectedPositionInLine> selectedS3 = new List<listOfSelectedPositionInLine>();
        private List<StringBuilder> listSB_S4 = new List<StringBuilder>() { new StringBuilder() }; private memoScreen memoS4 = new memoScreen(); private List<listOfSelectedPositionInLine> selectedS4 = new List<listOfSelectedPositionInLine>();
        private List<StringBuilder> listSB_S5 = new List<StringBuilder>() { new StringBuilder() }; private memoScreen memoS5 = new memoScreen(); private List<listOfSelectedPositionInLine> selectedS5 = new List<listOfSelectedPositionInLine>();
        private List<StringBuilder> listSB_S6 = new List<StringBuilder>() { new StringBuilder() }; private memoScreen memoS6 = new memoScreen(); private List<listOfSelectedPositionInLine> selectedS6 = new List<listOfSelectedPositionInLine>();
        private List<StringBuilder> listSB_S7 = new List<StringBuilder>() { new StringBuilder() }; private memoScreen memoS7 = new memoScreen(); private List<listOfSelectedPositionInLine> selectedS7 = new List<listOfSelectedPositionInLine>();
        private List<StringBuilder> listSB_S8 = new List<StringBuilder>() { new StringBuilder() }; private memoScreen memoS8 = new memoScreen(); private List<listOfSelectedPositionInLine> selectedS8 = new List<listOfSelectedPositionInLine>();
        private List<StringBuilder> listSB_S9 = new List<StringBuilder>() { new StringBuilder() }; private memoScreen memoS9 = new memoScreen(); private List<listOfSelectedPositionInLine> selectedS9 = new List<listOfSelectedPositionInLine>();
        private List<StringBuilder> listSB_S0 = new List<StringBuilder>() { new StringBuilder() }; private memoScreen memoS0 = new memoScreen(); private List<listOfSelectedPositionInLine> selectedS0 = new List<listOfSelectedPositionInLine>();
        private List<StringBuilder> listSB_SS = new List<StringBuilder>() { new StringBuilder() }; private memoScreen memoSS = new memoScreen(); private List<listOfSelectedPositionInLine> selectedSS = new List<listOfSelectedPositionInLine>();

        private List<StringBuilder> listSB_CB = new List<StringBuilder>() { new StringBuilder() }; private memoScreen memoCB = new memoScreen(); private List<listOfSelectedPositionInLine> selectedCB = new List<listOfSelectedPositionInLine>();
        private List<StringBuilder> listSBLog = new List<StringBuilder>(); private memoScreen memoLog = new memoScreen(); private List<listOfSelectedPositionInLine> selectedLog = new List<listOfSelectedPositionInLine>();

        private string strScreen = ""; // Used for keep name Screen  ... S0,S1,....R0,R1,...


        private int intOS = 0;    // OS   32  or 64
        private System.Int64 intTotalMemory = 0;    // total memory which used by program



        //----------------------------------------------------------------------------------------------------------
        // MainWindow ----------------------------------------------------------------------------------------------

        public MainWindow()
        {

            InitializeComponent();

            DataBaseCommon dbc = new DataBaseCommon();
            dbc.LoadProviders(ref comboDB);

            ftb.MouseUp += (sender, e) => { e.Handled = true; };
            ftb.MouseDown += (sender, e) => { e.Handled = true; };

            systemInfo();

            if (!Environment.Is64BitOperatingSystem == true) { }

            txtFind.TextChanged += (s, e) => selection();

            ftb.canvasMain.AllowDrop = true;

            initComboFamilyFonts();
            initComboColor();
            userClk.Visibility = Visibility.Collapsed;
        }


        private void selection()
        {

            ftb.strFind = txtFind.Text;
            ftb.strReplace = txtReplace.Text;
            mainWindow.IsEnabled = false;
            ftb.updateBlock();
            mainWindow.IsEnabled = true;
        }



        public void systemInfo()
        {
            try
            {
                Microsoft.VisualBasic.Devices.ComputerInfo myComputer = new Microsoft.VisualBasic.Devices.ComputerInfo();
                ulong totalAvailable = myComputer.AvailablePhysicalMemory;

                intOS = 32;
                if (Environment.Is64BitOperatingSystem == true) { intOS = 64; };
                intTotalMemory = System.GC.GetTotalMemory(false);

                statusSystemBar.Content = "OS:X" + intOS.ToString() + " Used Memory - " + intTotalMemory.ToString("0,0") + " ( Available " + totalAvailable.ToString("0,0") + " )";
                statusDocSizeBar.Content = " ( " + ftb.longTotalDocSize.ToString("0,0") + " bytes) ";
            }
            catch (Exception ex)
            {
                ftb.blnError = true;
            }


        }

        //----------------------------------------------------------------------------------------------------------

        private void openFiles(String[] files)
        {

            //  int intElement = 0;

            menuTop.IsEnabled = false;

            try
            {

                ftb.blnAbort = false;

                TaskScheduler uiThread = TaskScheduler.FromCurrentSynchronizationContext();

                Dispatcher.Invoke(new Action(() =>
                {
                    userClk.Visibility = Visibility.Visible;
                    ftb.IsEnabled = false;
                }));

                Action MainThreadLoadFile = new Action(() =>
                {

                    // !!! Do not combine ALL in one Dispatcher !!!!!!!!!!!!!!!!!!!!!!!!!!!!

                    Dispatcher.Invoke(new Action(() =>
                    {

                        if (ftb.blnError == true && ftb.blnAppend == true)
                        {
                            MessageBox.Show("Could not Append new file ...\nPlease reduce file/s or use OS64 for load entire file(s) \nOR unselect [∑±] for load new file \nOR [☼]/[☼☼] - Clear - Current/All screen(s)...\nYou can still view/save loaded data but program may corrupt ...");
                            gridButtonRight.IsEnabled = false;
                            return;
                        }
                        else
                        {
                            gridButtonRight.IsEnabled = true;
                            ftb.blnError = false;
                        }
                        ftb.canvasMain.Focus();
                        //         ftb.blnLoad = true;
                        gridButtonLeft.IsEnabled = false;
                        gridButtonRight.IsEnabled = false;

                        //  mainWindow.ResizeMode = System.Windows.ResizeMode.NoResize;  //Need to allow re-size IF NOT EXCEPTION

                    }));



                    if (files == null)
                    {

                        return;
                    }

                    Dispatcher.Invoke(new Action(() => txtFind.Text = ""));
                    Dispatcher.Invoke(new Action(() => txtReplace.Text = ""));



                    if (ftb.blnAppend == false)
                    {   // if new file ... reset all below ...
                        ftb.listSBmain = null;
                        ftb.listSPMain = null;

                        GC.Collect();
                        Thread.Sleep(200);

                        ftb.listSBmain = new List<StringBuilder>();
                        ftb.listSPMain = new List<listOfSelectedPositionInLine>();

                        ftb.setTextBlockClear();

                        Dispatcher.Invoke(new Action(() => statusFile.Content = ""));

                        Dispatcher.Invoke(new Action(() => ftb.scrollX.Value = 0));
                        Dispatcher.Invoke(new Action(() => ftb.scrollY.Value = 0));

                        Dispatcher.Invoke(new Action(() => ftb.listSPMain = new List<listOfSelectedPositionInLine>()));
                        Dispatcher.Invoke(new Action(() => ftb.canvasSelected.Children.Clear()));
                        Dispatcher.Invoke(new Action(() => ftb.canvasSelecting.Children.Clear()));
                    }


                    Dispatcher.Invoke(new Action(() =>
                    {
                        if (ftb.listSPMain == null) ftb.listSPMain = new List<listOfSelectedPositionInLine>();
                    }
                        ));

                    foreach (string f in files)
                    {

                        FileInfo fileTmp = new FileInfo(f);
                        //------------------------------------------------------------------------------------------------------
                        //------------------------------------------------------------------------------------------------------

                        //if (fileTmp.CreationTime > DateTime.Parse("01/01/2013"))
                        //{
                        //    MessageBox.Show("Please visit   http:\\www.AnotherPart.Biz  for download NEW version! ... Thanks... ");
                        //    return;
                        //}
                        //------------------------------------------------------------------------------------------------------
                        //------------------------------------------------------------------------------------------------------

                        if ((fileTmp.Extension.ToUpper() == ".TXT") || (fileTmp.Extension.ToUpper() == ".SQL") || (fileTmp.Extension.ToUpper() == ".CSV")
                         || (fileTmp.Extension.ToUpper() == ".XML") || (fileTmp.Extension.ToUpper() == ".XAML") || (fileTmp.Extension.ToUpper() == ".LOG"))
                        {
                            Dispatcher.Invoke(new Action(() => statusFile.Content = "  " + fileTmp.FullName));
                            try
                            {
                                listSBLog.Add(new StringBuilder(strScreen + "   " + (ftb.blnAppend ? "Append  " : "Add New  ") + fileTmp.FullName + "    " + fileTmp.Length.ToString("0,0") + " (Bytes) " + "  " + DateTime.Now.ToString()));
                                selectedLog.Add(new listOfSelectedPositionInLine(0, 0, null));
                                using (StreamReader sr = new StreamReader(f))
                                {

                                    int intCount = 0;
                                    DateTime dt = new DateTime();
                                    dt = DateTime.Now;

                                    while (sr.Peek() >= 0)
                                    {

                                        if (ftb.blnAbort)
                                        {
                                            MessageBox.Show("Load was Aborted ...");
                                            Dispatcher.Invoke(new Action(() =>
                                            {
                                                ftb.updateBlock();
                                            }));
                                            break;
                                        }

                                        intCount++;
                                        StringBuilder sb = new StringBuilder(sr.ReadLine());
                                        ftb.intMaxLineSize = ftb.intMaxLineSize < sb.Length ? sb.Length : ftb.intMaxLineSize; // Count MAX line size
                                        ftb.listSBmain.Add(sb.Replace("�", "?").Replace(@"\'", "''").Replace("\t", "     ")); // Do not insert New Line
                                        
                                        ftb.listSPMain.Add(new listOfSelectedPositionInLine());
                                        
                                        if (intCount >= 10)
                                        {
                                            if (dt.AddSeconds(1) < DateTime.Now)
                                            {
                                                dt = DateTime.Now;
                                                intCount = 0;
                                                ftb.totalBytes();

                                                Dispatcher.Invoke(new Action(() => systemInfo()));
                                            }
                                            else { intCount = 0; }
                                        }
                                    }
                                    //                                    sr.Close();
                                }
                            }
                            catch (Exception ex)
                            {
                                if (listSBLog != null)
                                {
                                    listSBLog.Add(new StringBuilder(ex.Message + Environment.NewLine));
                                    selectedLog.Add(new listOfSelectedPositionInLine());
                                }

                                Dispatcher.Invoke(new Action(() =>
                                {
                                    userClk.Visibility = Visibility.Collapsed;
                                    rbLog.Foreground = new SolidColorBrush(Colors.Red);
                                    gridButtonRight.IsEnabled = false;
                                    ftb.blnError = true;

                                }));
                                if (ftb.blnAbort != true) MessageBox.Show("Could not Allocated in managed memory (MainThreadLoadFile)...\nPlease reduce file/s or use OS64 for load entire file(s) ...\nYou can still view/save loaded data but program may corrupt ...\n" + ex.Message);
                            }
                            finally
                            {
                                Dispatcher.Invoke(new Action(() =>
                                {
                                    ftb.blnAbort = false;
                                }));
                            }
                        }
                        else
                        {
                            listSBLog.Add(new StringBuilder(strScreen + "   " + fileTmp.FullName + " is not a .TXT|.SQL |.XML|.CSV |.XAML |.LOG   FILE(s) ... !!! ...  " + fileTmp.FullName + "   " + DateTime.Now.ToString()));
                            selectedLog.Add(new listOfSelectedPositionInLine());

                            MessageBox.Show(strScreen + "   " + fileTmp.FullName + " is not a .TXT|.SQL |.XML|.CSV |.XAML |.LOG");

                            Dispatcher.Invoke(new Action(() => rbLog.Foreground = new SolidColorBrush(Colors.Red)));
                        }
                    }

                    // ftb.init_VerticalScroll();
                    //  ftb.init_HorizontalScroll();

                });

                Action FinalThreadDoWOrk = new Action(() =>
                {
                    //           ftb.blnLoad = false;

                    gridButtonLeft.IsEnabled = true;
                    gridButtonRight.IsEnabled = true;

                    ftb.intAbsolutCaretHorizCurrent = 0;
                    ftb.intAbsolutCaretVertCurrent = 0;

                    ftb.intAbsolutSelectHorizStart = 0;
                    ftb.intAbsolutSelectVertStart = 0;


                    Dispatcher.Invoke(new Action(() => ftb.set_Caret(0, 0)));

                    //Dispatcher.Invoke(new Action(() => MainGrid.Children.RemoveAt(intElement)));  // It is Clock
                    Dispatcher.Invoke(new Action(() =>
                    {
                        userClk.Visibility = Visibility.Collapsed;
                        ftb.IsEnabled = true; ;
                    }));

                    Dispatcher.Invoke(new Action(() => MainGrid.IsEnabled = true));

                    //       Dispatcher.Invoke(new Action(() => mainWindow.ResizeMode = System.Windows.ResizeMode.CanResize));    // IF NOT EXCEPTION



                    Dispatcher.Invoke(new Action(() => ftb.addSelectedCanvas(false)));

                    Dispatcher.Invoke(new Action(() =>
                    {
                        ftb.totalBytes();
                        systemInfo();
                    }));

                    menuTop.IsEnabled = true; ;

                    Dispatcher.Invoke(new Action(() => ftb.updateBlock()));
                    Dispatcher.Invoke(new Action(() => ftb.canvasMain.Focus()));
                });


                //Action ExecuteProgressClock = new Action(() => {
                //    userClk.Visibility = Visibility.Visible;
                //    //userClock clk = new userClock();
                //    //Dispatcher.Invoke(new Action(() => intElement = MainGrid.Children.Add(clk)));
                //});

                Task MainThreadDoWorkTask = Task.Factory.StartNew(() => MainThreadLoadFile());

                //Task ExecuteProgressClkTask = new Task(ExecuteProgressClock);
                //ExecuteProgressClkTask.RunSynchronously();

                MainThreadDoWorkTask.ContinueWith(t => FinalThreadDoWOrk(), uiThread);

            }
            catch
            {
                MessageBox.Show("Could not Allocated in managed memory (openFiles)...\nYou can still view/save loaded data but program may corrupt ...");

                //               System.Windows.MessageBox.Show("Error", ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        //Drop------------------------------------------------------------------------------------------------------

        private void ftb_Drop(object sender, DragEventArgs e)
        {


            ftb.strHistory = "";
            IDataObject data = e.Data;
            String[] files = (String[])data.GetData(DataFormats.FileDrop);

            openFiles(files);

        }

        //----------------------------------------------------------------------------------------------------------
        //Find/Replace---------------------------------------------------------------------------------------------

        private void btnFindRightDown_Click(object sender, RoutedEventArgs e)
        {
            if (txtFind.Text == "") return;

            int intV = (int)ftb.scrollY.Value;
            int intH = (int)ftb.scrollX.Value;

            string strTmp = txtFind.Text.Split('~')[0];
            int intStrLength = strTmp.Length;

            int jFind = -1;
            int iLine = 0;

            for (iLine = intV; iLine < ftb.listSBmain.Count; iLine++)
            {

                if (intH + 1 >= ftb.listSBmain[iLine].Length)
                {
                    intH = -1;
                    continue;
                }
                if (ftb.listSBmain[iLine].ToString().Contains(strTmp))
                {                  // Significant increase performance if use "... .Contains()
                    jFind = ftb.listSBmain[iLine].ToString().IndexOf(strTmp, intH + 1);
                }
                else
                {
                    continue;
                }
                if (jFind == -1)
                {
                    intH = -1;
                    continue;
                }
                if (jFind != -1) break;
            }
            if ((jFind == -1) && (iLine == ftb.listSBmain.Count))
            {
                MessageBox.Show("No new Entrance ...");
                return;
            }
            ftb.scrollY.Value = iLine;
            ftb.scrollX.Value = jFind;
        }


        private void btnReplace_Click(object sender, RoutedEventArgs e)
        {
            if (txtFind.Text == "") return; // return if empty ...
            if (MessageBox.Show("Do you really  want to replace ALL?", "Replace ALL", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel) return;

            Dispatcher.Invoke(new Action(() => ftb.strFind = txtFind.Text));
            Dispatcher.Invoke(new Action(() => ftb.strReplace = txtReplace.Text));

            ftb.canvasSelected.Children.Clear();
           
            for (int i = 0; i < ftb.listSPMain.Count; i++)
            {
                ftb.listSPMain[i] = new listOfSelectedPositionInLine();
            }

            taskReplaceAllInParallel();   // Update Find according Replace

            systemInfo();

            ftb.updateBlock();
            ftb.totalBytes();

            ftb.init_VerticalScroll();
            ftb.init_HorizontalScroll();
        }


        private bool replaceAllInParallel()
        {

            try
            {
                if (ftb.strFind == "") return false;

                int N = ftb.listSBmain.Count;

                String[] find = ftb.strFind.Split('~');
                String[] replace = ftb.strReplace.Split('~');

                if (find.Length != replace.Length)
                {
                    MessageBox.Show(" [Find] and [Replace] MUST have same size", "Replace ALL", MessageBoxButton.OK);
                    return false;
                }


                Parallel.For(0, N, (int i, ParallelLoopState loop) =>
                {
                lGo:
                    for (int jj = 0; jj < find.Length; jj++ )
                    {
                        string strFind = find[jj];
                        string strReplace = replace[jj];
                        if (ftb.listSBmain[i].ToString().Contains("------")) continue;    // ------ three comments (any place in line) whole  line will not be changing (skipped)

                        ftb.listSBmain[i].Replace(strFind, strReplace);
                        if (ftb.listSBmain[i].ToString().Contains(strFind) && strFind == "  ") goto lGo;  // for example replace "  " to " " -> repeat until All -> " "
                      if (loop.IsStopped) return;
                    }
                });
            }
            catch
            {
                MessageBox.Show("Program has a problem allocating additional char(s)/byte(s) in memory (replaceAllInParallel) ...\nPlease save your  job ... ...");
                return false;
            }
              Dispatcher.Invoke(new Action(() => userClk.Visibility = Visibility.Collapsed));
            return true;
            
        }



        private void taskReplaceAllInParallel()
        {
            try
            {

                Dispatcher.Invoke(new Action(() => MainGrid.IsEnabled = false));
                TaskScheduler uiThread = TaskScheduler.FromCurrentSynchronizationContext();

                bool blnReturn = false;
                // task for find
                Action MainThreadFind = new Action(() => blnReturn = replaceAllInParallel());

                Action ExecuteProgressClock = new Action(() =>
                {
                    userClk.Visibility = Visibility.Visible;
                });

                // final task
                Action FinalThreadDoWOrk = new Action(() =>
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        userClk.Visibility = Visibility.Collapsed;
                        statusDocSizeBar.Content = ftb.longTotalDocSize.ToString("0,0");
                        if (blnReturn)
                        {
                            txtFind.Text = txtReplace.Text;
                            txtReplace.Text = "";
                        }
                    }));

                    //     ftb.updateBlock();

                    // if scroll value was changed - it automaticALY RUN UPDATE SCREEN
                    Dispatcher.Invoke(new Action(() => ftb.scrollX.Value = 0));
                    Dispatcher.Invoke(new Action(() => ftb.scrollY.Value = 0));
                    Dispatcher.Invoke(new Action(() => ftb.canvasMain.Focus()));
                    Dispatcher.Invoke(new Action(() => MainGrid.IsEnabled = true));

                });

                Task MainThreadDoWorkTask = Task.Factory.StartNew(() => MainThreadFind());
                Task ExecuteProgressClkTask = new Task(ExecuteProgressClock);

                ExecuteProgressClkTask.RunSynchronously();
                MainThreadDoWorkTask.ContinueWith(t => FinalThreadDoWOrk(), uiThread);

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error", ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                Dispatcher.Invoke(new Action(() => MainGrid.IsEnabled = true));
            }
        }

        //Helper-----------------------------------------------------------------------------------------------------

        private void initComboFamilyFonts()
        {
            string[,] strFontALL;

            strFontALL = new string[,] { 
                                            {"Courier New",  "1.665",   "0.884"}, 
                                            {"BatangChe",    "1.9976",  "0.87"},  
                                            {"Consolas",     "1.8166",  "0.853"}, 
                                            {"DFKai-listSB" ,    "1.997",  "0.834"},
                                            {"DotumChe",    "1.9979",  "0.87"},                                                
                                         //   {"FangSong" ,    "1.994",  "0.88"},                                                  
                                         //   {"GungsuhChe" ,  "1.994",  "0.88"},                                              
                                            {"KaiTi",        "1.9979",  "0.877"},
                                          
                                            {"Lucida Console", "1.659", "1.00"},
                                            {"Lucida Sans Typewriter", "1.6569",  "0.853"}, //
                                            {"Letter Gothic Std",  "1.665","0.823"},    
                                            
                                        //    {"MingLiU",  "1.995",  "0.83"}, 
                                        //    {"MingLiU_HKSCS",  "1.995",  "0.83"},
                                            {"MingLiU_HKSCS-ExtB",  "1.997",  "0.832"},
                                            {"MingLiU-ExtB",  "1.997",  "0.834"},
                                            
                                            {"NSimSun",  "1.999",  "0.877"}, //
                                           
                                            {"OCR A","1.655",  "0.967"},
                                            {"OCR A Std",  "1.387",  "0.835"}, 

                                            {"Orator Std",  "1.6642",  "0.75"},
                                            {"Segoe UI Mono",  "1.6646",  "0.752"},
                                        //    {"SimHei",  "1.995",  "0.87"},    
                                            {"Simplified Arabic Fixed","1.6646","0.917"},
                                        //    {"SimSun","1.995","0.87"},      
                                            {"SimSun-ExtB" ,"1.997","1.00"}
                                        };

            for (int i = 0; i < strFontALL.GetLength(0); i++)
            {

                ComboBoxItem cbi = new ComboBoxItem();
                selectedFonts sF = new selectedFonts();

                sF.strFontName = strFontALL[i, 0];
                sF.dHoriz = Double.Parse(strFontALL[i, 1].Trim());
                sF.dVert = Double.Parse(strFontALL[i, 2].Trim());

                // inherent from Label
                sF.Text = strFontALL[i, 0];
                sF.FontFamily = new FontFamily(strFontALL[i, 0]);
                sF.Visibility = System.Windows.Visibility.Visible;

                sF.FontSize = 13;
                sF.Height = 21;

                cbi.Content = sF;  // <- this item will be storing on comboBox
                cbi.ToolTip = strFontALL[i, 0];
                cbi.FontFamily = new FontFamily(strFontALL[i, 0]);

                foreach (FontFamily fontFamily in Fonts.SystemFontFamilies)
                {
                    if (fontFamily.ToString() == sF.FontFamily.ToString())
                    {
                        comboFontFamily.Items.Add(cbi);
                    }
                }
            }
            comboFontFamily.SelectedIndex = 0;
            comboFontSize.SelectedIndex = 2;
        }


        private void initComboColor()
        {
            // populate colors drop down (will work with other kinds of list controls) 

            Type colors = typeof(System.Drawing.Color);
            PropertyInfo[] colorInfo = colors.GetProperties(BindingFlags.Public | BindingFlags.Static);

            foreach (PropertyInfo info in colorInfo)
            {

                ComboBoxItem cbi = new ComboBoxItem();
                selectedColors sC = new selectedColors();

                sC.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                sC.strColor = info.Name;
                sC.Text = info.Name;
                sC.Visibility = System.Windows.Visibility.Visible;
                sC.FontFamily = new FontFamily("Courier New");
                sC.FontSize = 13;
                sC.Height = 21;


                if (info.Name != "Transparent")
                {
                    sC.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(info.Name));
                }
                else
                {
                    sC.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Black"));
                }
                cbi.ToolTip = sC.strColor;
                cbi.Content = sC;

                comboColor.Items.Add(cbi);
            }

            comboColor.SelectedIndex = 80;
        }


        private void btnClearEntireDoc_Click(object sender, RoutedEventArgs e)
        {

            gridButtonRight.IsEnabled = true;
            ftb.blnError = false;

            ftb.blnAbort = true;


            ftb.canvasSelected.Children.Clear();
            ftb.canvasSelecting.Children.Clear();

            ftb.intVertCountLinesOnPage = 0;  // depends from FontSize
            ftb.intHorizCountCharsOnPage = 0;

            //     ftb.intFirstLineOnPage = 0; // absolute number first line on current page  - if screen size was changed - need to re-calculate      
            //     ftb.intFirstCharOnPage = 0; // absolute number first char on current line on Current Page        

            //Caret section
            ftb.intAbsolutCaretVertCurrent = 0;
            ftb.intAbsolutCaretHorizCurrent = 0;

            ftb.blnSomthingSelected = false;

            Dispatcher.Invoke(new Action(() => txtFind.Text = ""));
            Dispatcher.Invoke(new Action(() => txtReplace.Text = ""));

            ftb.listSBmain = null;
            ftb.listSPMain = null;


            listSB_S1 = null;
            listSB_S2 = null;
            listSB_S3 = null;
            listSB_S4 = null;
            listSB_S5 = null;
            listSB_S6 = null;
            listSB_S7 = null;
            listSB_S8 = null;
            listSB_S9 = null;
            listSB_S0 = null;
            listSB_SS = null;

            listSBLog = null;
            listSB_CB = null;


            selectedS1 = null;
            selectedS2 = null;
            selectedS3 = null;
            selectedS4 = null;
            selectedS5 = null;
            selectedS6 = null;
            selectedS7 = null;
            selectedS8 = null;
            selectedS9 = null;
            selectedS0 = null;
            selectedSS = null;

            selectedLog = null;
            selectedCB = null;

            memoS0 = null;
            memoS1 = null;
            memoS2 = null;
            memoS3 = null;
            memoS4 = null;
            memoS5 = null;
            memoS6 = null;
            memoS7 = null;
            memoS8 = null;
            memoS9 = null;
            memoSS = null;

            memoCB = null;
            memoLog = null;

            ftb.listSB_A = null;
            ftb.listSB_B = null;
            ftb.listSB_C = null;
            ftb.listSB_D = null;
            ftb.listSB_E = null;

            GC.Collect();
            Thread.Sleep(500);
            txtMemo.Text = "";

            ftb.setTextBlockClear();

            Dispatcher.Invoke(new Action(() => rbLog.Foreground = new SolidColorBrush(Colors.Black)));

            ftb.listSBmain = new List<StringBuilder>();
            ftb.listSPMain = new List<listOfSelectedPositionInLine>();

            listSB_S1 = new List<StringBuilder>() { new StringBuilder("") };
            listSB_S2 = new List<StringBuilder>() { new StringBuilder("") };
            listSB_S3 = new List<StringBuilder>() { new StringBuilder("") };
            listSB_S4 = new List<StringBuilder>() { new StringBuilder("") };
            listSB_S5 = new List<StringBuilder>() { new StringBuilder("") };
            listSB_S6 = new List<StringBuilder>() { new StringBuilder("") };
            listSB_S7 = new List<StringBuilder>() { new StringBuilder("") };
            listSB_S8 = new List<StringBuilder>() { new StringBuilder("") };
            listSB_S9 = new List<StringBuilder>() { new StringBuilder("") };
            listSB_S0 = new List<StringBuilder>() { new StringBuilder("") };
            listSB_SS = new List<StringBuilder>() { new StringBuilder("") };

            selectedS1 = new List<listOfSelectedPositionInLine>();
            selectedS2 = new List<listOfSelectedPositionInLine>();
            selectedS3 = new List<listOfSelectedPositionInLine>();
            selectedS4 = new List<listOfSelectedPositionInLine>();
            selectedS5 = new List<listOfSelectedPositionInLine>();
            selectedS6 = new List<listOfSelectedPositionInLine>();
            selectedS7 = new List<listOfSelectedPositionInLine>();
            selectedS8 = new List<listOfSelectedPositionInLine>();
            selectedS9 = new List<listOfSelectedPositionInLine>();
            selectedS0 = new List<listOfSelectedPositionInLine>();
            selectedSS = new List<listOfSelectedPositionInLine>();

            selectedLog = new List<listOfSelectedPositionInLine>();
            selectedCB = new List<listOfSelectedPositionInLine>();

            listSBLog = new List<StringBuilder>() { new StringBuilder("") };
            listSB_CB = new List<StringBuilder>() { new StringBuilder("") };

            memoS0 = new memoScreen();
            memoS1 = new memoScreen();
            memoS2 = new memoScreen();
            memoS3 = new memoScreen();
            memoS4 = new memoScreen();
            memoS5 = new memoScreen();
            memoS6 = new memoScreen();
            memoS7 = new memoScreen();
            memoS8 = new memoScreen();
            memoS9 = new memoScreen();
            memoSS = new memoScreen();

            memoCB = new memoScreen();
            memoLog = new memoScreen();

            ftb.listSB_A = new List<StringBuilder>();
            ftb.listSB_B = new List<StringBuilder>();
            ftb.listSB_C = new List<StringBuilder>();
            ftb.listSB_D = new List<StringBuilder>();
            ftb.listSB_E = new List<StringBuilder>();


            ftb.intMaxLineSize = 0;

            Dispatcher.Invoke(new Action(() => ftb.setTextBlockClear()));
            Dispatcher.Invoke(new Action(() => statusFile.Content = ""));
            Dispatcher.Invoke(new Action(() => statusDocSizeBar.Content = ""));
            Dispatcher.Invoke(new Action(() => statusSystemBar.Content = ""));

            Dispatcher.Invoke(new Action(() => ftb.totalBytes()));
            Dispatcher.Invoke(new Action(() => systemInfo()));


            ftb.init_HorizontalScroll();
            ftb.init_VerticalScroll();

            Dispatcher.Invoke(new Action(() => ftb.set_Caret(0, 0)));

            ftb.blnAppend = true;
            appendClick();

            ftb.canvasMain.Focus();


        }


        private void btnClearFind_Click(object sender, RoutedEventArgs e)
        {

            Dispatcher.Invoke(new Action(() => txtFind.Text = ""));
            Dispatcher.Invoke(new Action(() => txtReplace.Text = ""));


        }

        //FIND-------------------------------------------------------------------------------------------BEGIN----------

        private void comboFontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ftb == null) return;
            ftb.listBoxHelper.Visibility = Visibility.Collapsed;

            ComboBox cb = sender as ComboBox;
            ComboBoxItem cbi = new ComboBoxItem();
            cbi = (ComboBoxItem)cb.SelectedItem;

            ftb.tb_FontSize = Double.Parse(cbi.Content.ToString());
            ftb.setTextBlockFontSize();

            ftb.init_VerticalScroll();
            ftb.init_HorizontalScroll();

            Dispatcher.Invoke(new Action(() => ftb.set_Caret(ftb.intAbsolutCaretVertCurrent, ftb.intAbsolutCaretHorizCurrent)));

            ftb.updateBlock();
        }

        //-------------------------------------------------------------------------------------------------

        private void initAfterSwitch(bool blnEnaRun, ref List<StringBuilder> objSB, ref memoScreen objMemo, ref List<listOfSelectedPositionInLine> objSelected)
        {

            try
            {
                if (ftb == null) return; // need keep - for nationalization process

                ftb.listBoxHelper.Visibility = Visibility.Collapsed;

                //Reset common objects...
                statusFile.Content = objMemo.statusFile;
                statusDocSizeBar.Content = "";
                ftb.longTotalDocSize = 0;
                ftb.intMaxLineSize = 0;
                txtFind.Text = objMemo.strFind;

                ftb.setTextBlockClear();

                ftb.blnAppend = objMemo.blnAppend;
                stAppend();

                ftb.listSBmain = objSB;
                ftb.listSPMain = objSelected;

                if (ftb.listSBmain.Count == 0)
                {
                    ftb.listSBmain = new List<StringBuilder>();
                    ftb.listSBmain.Add(new StringBuilder(""));
                }

                if (ftb.listSPMain.Count == 0)
                {
                    ftb.listSPMain = new List<listOfSelectedPositionInLine>();
                    ftb.listSPMain.Insert(0, new listOfSelectedPositionInLine());
                }

                // ftb.canvasSelected.Children.Clear();
                ftb.addSelectedCanvas(true);

                ftb.init_VerticalScroll();
                ftb.init_HorizontalScroll();

                ftb.scrollY.Value = objMemo.iLineBegin;       // When Update Value  - code Update screen automatically / see events..
                ftb.scrollX.Value = objMemo.iCharBegin;       // We must to use independent value - do not use intFist...

                if (strScreen == "Screen - LOG" || strScreen == "Screen - CB")
                {
                    btnRunAll.IsEnabled = false;  // for Result screen - unavailable RUN
                }
                else if (btnRunAll != null) btnRunAll.IsEnabled = true;

                if (btnRunCurrent != null) btnRunCurrent.IsEnabled = blnEnaRun;
                if (btnRunSelected != null) btnRunSelected.IsEnabled = blnEnaRun;
                if (btnRunCurrentSelected != null) btnRunCurrentSelected.IsEnabled = blnEnaRun;

                if (btnCut != null) btnCut.IsEnabled = blnEnaRun;
                if (btnCopy != null) btnCopy.IsEnabled = blnEnaRun;
                if (btnPaste != null) btnPaste.IsEnabled = blnEnaRun;

                menuRun.IsEnabled = blnEnaRun;
                comboFontSize.SelectedIndex = objMemo.indexSelectedFontSize;

                comboFontFamily.SelectedIndex = objMemo.indexSelectedFontFamily;

                if (chkBoxSame.IsChecked == false && strScreen != "Screen - SS")
                { // Do not change current Connection String for SS
                    // Sequence is important 
                    comboDB.SelectedIndex = objMemo.indexSelectedDB;
                    txtConnectionString.Text = objMemo.strConnectionString;

                }

                ftb.intAbsolutCaretHorizCurrent = objMemo.intAbsolutHorizCurrent;
                ftb.intAbsolutCaretVertCurrent = objMemo.intAbsolutVertCurrent;
                ftb.blnCaretInsert = objMemo.blnCaretInsert;

                txtMemo.Text = objMemo.strMemo;

                ftb.init_VerticalScroll();
                ftb.init_HorizontalScroll();


                ftb.totalBytes();
                systemInfo();

                ftb.updateBlock();

            }
            catch
            {
                MessageBox.Show("Program has a problem allocating additional char(s) in memory (initAfterSwitch) ...\nPlease save your  job ... ...");
            }



        }


        private void rbS0_Checked(object sender, RoutedEventArgs e)
        {

            strScreen = "Screen - S0";
            initAfterSwitch(true, ref listSB_S0, ref memoS0, ref selectedS0);


        }

        private void rbS1_Checked(object sender, RoutedEventArgs e)
        {
            strScreen = "Screen - S1";
            initAfterSwitch(true, ref  listSB_S1, ref memoS1, ref selectedS1);

        }

        private void rbS2_Checked(object sender, RoutedEventArgs e)
        {
            strScreen = "Screen - S2";
            initAfterSwitch(true, ref  listSB_S2, ref memoS2, ref selectedS2);

        }

        private void rbS3_Checked(object sender, RoutedEventArgs e)
        {
            strScreen = "Screen - S3";
            initAfterSwitch(true, ref  listSB_S3, ref memoS3, ref selectedS3);

        }

        private void rbS4_Checked(object sender, RoutedEventArgs e)
        {
            strScreen = "Screen - S4";
            initAfterSwitch(true, ref  listSB_S4, ref memoS4, ref selectedS4);

        }

        private void rbS5_Checked(object sender, RoutedEventArgs e)
        {
            strScreen = "Screen - S5";
            initAfterSwitch(true, ref  listSB_S5, ref memoS5, ref selectedS5);

        }

        private void rbS6_Checked(object sender, RoutedEventArgs e)
        {
            strScreen = "Screen - S6";
            initAfterSwitch(true, ref  listSB_S6, ref memoS6, ref selectedS6);

        }

        private void rbS7_Checked(object sender, RoutedEventArgs e)
        {
            strScreen = "Screen - S7";
            initAfterSwitch(true, ref  listSB_S7, ref memoS7, ref selectedS7);

        }

        private void rbS8_Checked(object sender, RoutedEventArgs e)
        {
            strScreen = "Screen - S8";
            initAfterSwitch(true, ref  listSB_S8, ref memoS8, ref selectedS8);

        }

        private void rbS9_Checked(object sender, RoutedEventArgs e)
        {
            strScreen = "Screen - S9";
            initAfterSwitch(true, ref  listSB_S9, ref memoS9, ref selectedS9);

        }

        private void rbSS_Checked(object sender, RoutedEventArgs e)
        {
            strScreen = "Screen - SS";
            ftb.canvasMain.AllowDrop = false;
            initAfterSwitch(false, ref  listSB_SS, ref memoSS, ref selectedSS);
            gridButtonLeft.IsEnabled = false;
            ftb.canvasSelected.Background = new SolidColorBrush(Colors.AliceBlue);
        }



        //---------------------------------------------------------------------------------------------

        private void saveSbCurrent(ref memoScreen objMemo)
        {
            // sequence is very important !!!
            // add support information to screen
            try
            {
                objMemo.iCharBegin = (int)ftb.scrollX.Value;
                objMemo.iLineBegin = (int)ftb.scrollY.Value;
                objMemo.statusFile = statusFile.Content == null ? "" : statusFile.Content.ToString();
                objMemo.strFind = txtFind.Text;
                objMemo.indexSelectedFontSize = comboFontSize.SelectedIndex;
                objMemo.strConnectionString = txtConnectionString.Text;
                objMemo.indexSelectedDB = comboDB.SelectedIndex == -1 ? 0 : comboDB.SelectedIndex;
                objMemo.indexSelectedFontFamily = comboFontFamily.SelectedIndex;
                objMemo.intAbsolutHorizCurrent = ftb.intAbsolutCaretHorizCurrent;
                objMemo.intAbsolutVertCurrent = ftb.intAbsolutCaretVertCurrent;
                objMemo.blnCaretInsert = ftb.blnCaretInsert;
                objMemo.strMemo = txtMemo.Text;

                ftb.canvasSelected.Children.Clear();
                objMemo.blnAppend = ftb.blnAppend;
            }
            catch
            {
                MessageBox.Show("Program has a problem allocating additional char(s) in memory (saveSbCurrent) ...\nPlease save your  job ... ...");
            }



        }


        private void rbS0_Unchecked(object sender, RoutedEventArgs e)
        {
            saveSbCurrent(ref memoS0);
            listSB_S0 = ftb.listSBmain;
            selectedS0 = ftb.listSPMain;
        }

        private void rbS1_Unchecked(object sender, RoutedEventArgs e)
        {
            saveSbCurrent(ref memoS1);
            listSB_S1 = ftb.listSBmain;
            selectedS1 = ftb.listSPMain;
        }

        private void rbS2_Unchecked(object sender, RoutedEventArgs e)
        {
            saveSbCurrent(ref memoS2);
            listSB_S2 = ftb.listSBmain;
            selectedS2 = ftb.listSPMain;
        }

        private void rbS3_Unchecked(object sender, RoutedEventArgs e)
        {
            saveSbCurrent(ref memoS3);
            listSB_S3 = ftb.listSBmain;
            selectedS3 = ftb.listSPMain;
        }

        private void rbS4_Unchecked(object sender, RoutedEventArgs e)
        {
            saveSbCurrent(ref memoS4);
            listSB_S4 = ftb.listSBmain;
            selectedS4 = ftb.listSPMain;
        }

        private void rbS5_Unchecked(object sender, RoutedEventArgs e)
        {
            saveSbCurrent(ref memoS5);
            listSB_S5 = ftb.listSBmain;
            selectedS5 = ftb.listSPMain;
        }

        private void rbS6_Unchecked(object sender, RoutedEventArgs e)
        {
            saveSbCurrent(ref memoS6);
            listSB_S6 = ftb.listSBmain;
            selectedS6 = ftb.listSPMain;
        }

        private void rbS7_Unchecked(object sender, RoutedEventArgs e)
        {
            saveSbCurrent(ref memoS7);
            listSB_S7 = ftb.listSBmain;
            selectedS7 = ftb.listSPMain;
        }


        private void rbS8_Unchecked(object sender, RoutedEventArgs e)
        {
            saveSbCurrent(ref memoS8);
            listSB_S8 = ftb.listSBmain;
            selectedS8 = ftb.listSPMain;
        }

        private void rbS9_Unchecked(object sender, RoutedEventArgs e)
        {
            saveSbCurrent(ref memoS9);
            listSB_S9 = ftb.listSBmain;
            selectedS9 = ftb.listSPMain;
        }

        private void rbSS_Unchecked(object sender, RoutedEventArgs e)
        {
            saveSbCurrent(ref memoSS);
            listSB_SS = ftb.listSBmain;
            selectedSS = ftb.listSPMain;
            ftb.canvasMain.AllowDrop = true;
            gridButtonLeft.IsEnabled = true;
            ftb.canvasSelected.Background = new SolidColorBrush(Colors.White);
        }


        // LOG ----------------------------------------------------------------------------------------------------------------

        private void rbLog_Checked(object sender, RoutedEventArgs e)
        {
            strScreen = "Screen - LOG";
            initAfterSwitch(false, ref  listSBLog, ref memoLog, ref selectedLog);
            ftb.canvasMain.AllowDrop = false;
            gridButtonLeft.IsEnabled = false;
            ftb.canvasSelected.Background = new SolidColorBrush(Colors.Lavender);
        }

        private void rbLog_Unchecked(object sender, RoutedEventArgs e)
        {
            saveSbCurrent(ref memoLog);
            listSBLog = ftb.listSBmain;
            ftb.canvasMain.AllowDrop = true;
            gridButtonLeft.IsEnabled = true;
            ftb.canvasSelected.Background = new SolidColorBrush(Colors.White);
        }

        private void rbCB_Checked(object sender, RoutedEventArgs e)
        {
            strScreen = "Screen - CB";
            initAfterSwitch(false, ref  listSB_CB, ref memoCB, ref selectedCB);
            ftb.canvasMain.AllowDrop = false;
            gridButtonLeft.IsEnabled = false;
            ftb.canvasSelected.Background = new SolidColorBrush(Colors.LightCyan);

        }

        private void rbCB_Unchecked(object sender, RoutedEventArgs e)
        {
            saveSbCurrent(ref memoCB);
            listSB_CB = ftb.listSBmain;
            selectedCB = ftb.listSPMain;
            ftb.canvasMain.AllowDrop = true;
            gridButtonLeft.IsEnabled = true;
            ftb.canvasSelected.Background = new SolidColorBrush(Colors.White);
        }

        // LOG ----------------------------------------------------------------------------------------------------------------

        private void btnReplaceRightDown_Click(object sender, RoutedEventArgs e)
        {
            if (ftb.listSBmain.Count == 0 || ftb.listSBmain == null || txtFind.Text == "" || txtReplace.Text == "") return;

            btnFindRightDown_Click(sender, e);
            ftb.listSBmain[(int)ftb.scrollY.Value].Replace(txtFind.Text, txtReplace.Text, (int)ftb.scrollX.Value, txtFind.Text.Length);

            ftb.updateBlock();
            //  blnStatusTyping = false;    // if not typing update from block

        }



        private void menu_MouseEnter(object sender, MouseEventArgs e)
        {

            var mi = sender as MenuItem;

            mi.Foreground = new SolidColorBrush(Colors.DarkBlue);
            mi.FontWeight = FontWeights.Bold;
        }

        private void menu_MouseLeave(object sender, MouseEventArgs e)
        {

            var mi = sender as MenuItem;

            //    mi.Foreground = new SolidColorBrush(Colors.DarkGray);
            mi.FontWeight = FontWeights.Normal;

        }

        private void comboFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ftb.listBoxHelper.Visibility = Visibility.Collapsed;

            ComboBoxItem cbi = (ComboBoxItem)(sender as ComboBox).SelectedItem;
            selectedFonts sF = (selectedFonts)cbi.Content;

            ftb.tb_FontFamily = new FontFamily(sF.strFontName.ToString());
            ftb.setTextBlockFontFamily();

            comboFontFamily.FontFamily = ftb.tb_FontFamily;

            ftb.coeffFont_Widh = sF.dHoriz;
            ftb.coeffFont_High = sF.dVert;

            txtFind.FontFamily = ftb.tb_FontFamily;
            txtReplace.FontFamily = ftb.tb_FontFamily;

            ftb.init_VerticalScroll();
            ftb.init_HorizontalScroll();

            Dispatcher.Invoke(new Action(() => ftb.set_Caret(ftb.intAbsolutCaretVertCurrent, ftb.intAbsolutCaretHorizCurrent)));

            ftb.updateBlock();
        }

        private void ftb_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {

            if (Keyboard.Modifiers == ModifierKeys.Control)
            { // if pressed Vertical
                if (e.Delta > 0)
                {
                    try { comboFontSize.SelectedIndex = comboFontSize.SelectedIndex - 1; }
                    catch { comboFontSize.SelectedIndex = 0; }
                }
                else
                {
                    try { comboFontSize.SelectedIndex = comboFontSize.SelectedIndex + 1; }
                    catch { }
                }
            }
            ftb.canvasMain.Focus();
        }

        // Remove Double Spaces in Selected


        private void btnRemoveDoubleSpace_Click(object sender, RoutedEventArgs e)
        {

            ftb.somthingSelected();
            if (ftb.blnSomthingSelected == false)
            {
                MessageBox.Show("You do not select something, please select line, area, ... etc and try again.");
                return;
            }

            if (ftb.listSBmain.Count == 0 || ftb.listSBmain == null) { return; }
            if (MessageBox.Show("Do you really want to replace all continues space to a single space ...?  \nOnly in Selected area. ", "Permanent space", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel) return;

            taskReplaceAllInParallelSelected("  ", " ", true);
            ftb.updateBlock();
        }

        private void replaceAllInParallel(string strOld, string strNew, bool blnMinimize)
        {

            int N = ftb.listSBmain.Count;
            Parallel.For(0, N, (int i) =>
            {

                if (ftb.blnAbort)
                {
                    MessageBox.Show("Replace was Aborted ...");
                    ftb.updateBlock();
                    ftb.blnAbort = false;
                    return;
                }

                for (int j = ftb.listSPMain[i].listSPinLine.Count - 1; j >= 0; j--)
                {
                    selectedPositions sp = ftb.listSPMain[i].listSPinLine[j];
                    int intOldLength = sp.iCharEnd - sp.iCharBegin;
                    string strTmp = ftb.listSBmain[i].ToString(sp.iCharBegin, intOldLength);
                lGo:
                    strTmp = strTmp.Replace(strOld, strNew);
                    if (strTmp.Contains(strOld) && blnMinimize && strOld != strNew) goto lGo;  // for example replace "  " to " " -> repeat until All -> " "

                    string strTmpOld = ftb.listSBmain[i].ToString(sp.iCharBegin, intOldLength);

                    if (strTmpOld != "")
                    {
                        ftb.listSBmain[i].Replace(strTmpOld, strTmp, sp.iCharBegin, intOldLength);
                        // Remove ----- Replace ???
                        //ftb.listSPMain[i].listSPinLine.RemoveAt(j);
                        int iDelta = intOldLength - strTmp.Length;
                        for (int jj = j; jj < ftb.listSPMain[i].listSPinLine.Count; jj++)
                        {
                            selectedPositions spp = ftb.listSPMain[i].listSPinLine[jj];
                            spp.iCharBegin -= jj != j ? iDelta : 0;
                            spp.iCharEnd -= iDelta;
                        }


                    }
                }
            });
            Dispatcher.Invoke(new Action(() =>
                  {
                      userClk.Visibility = Visibility.Collapsed;
                  }));
        }



        private void taskReplaceAllInParallelSelected(string strOld, string strNew, bool blnMinimize)
        {

            //    int intElement = 0; // use for save intUIE  for CLK
            try
            {

                Dispatcher.Invoke(new Action(() =>
                {
           //         mainWindow.ResizeMode = System.Windows.ResizeMode.NoResize;
                    MainGrid.IsEnabled = false;
                }));


                TaskScheduler uiThread = TaskScheduler.FromCurrentSynchronizationContext();

                // task for find
                Action MainThreadFind = new Action(() => replaceAllInParallel(strOld, strNew, blnMinimize));

                Action ExecuteProgressClock = new Action(() =>
                {
                    userClk.Visibility = Visibility.Visible;
                });

                // final task
                Action FinalThreadDoWOrk = new Action(() =>
                {

                    ftb.totalBytes();

                    ftb.init_HorizontalScroll();
                    ftb.init_VerticalScroll();

                    Dispatcher.Invoke(new Action(() =>
                    {
                        ftb.canvasMain.Focus();
                        userClk.Visibility = Visibility.Collapsed;
                        MainGrid.IsEnabled = true;
                        statusDocSizeBar.Content = ftb.longTotalDocSize.ToString("0,0");

                    }));

                    ftb.updateBlock();
                });

                Task MainThreadDoWorkTask = Task.Factory.StartNew(() => MainThreadFind());
                Task ExecuteProgressClkTask = new Task(ExecuteProgressClock);

                ExecuteProgressClkTask.RunSynchronously();
                MainThreadDoWorkTask.ContinueWith(t => FinalThreadDoWOrk(), uiThread);

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error", ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                Dispatcher.Invoke(new Action(() => MainGrid.IsEnabled = true));
            }
        }



        //--------------------------------------------------------------------------------------------

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            // Release MEMORY

            ftb.listSBmain = null;
            //.....................................
        }



        private void formatAllInParallel()
        {

            //int N = ftb.listSBmain.Count;
            //Parallel.For(0, N, (int i) => {

            //    for (int j = ftb.listSPMain[i].listSPinLine.Count - 1; j >= 0; j--) {
            //        selectedPositions sp = ftb.listSPMain[i].listSPinLine[j];
            //        int intOldLength = sp.iCharEnd - sp.iCharBegin;
            //        string strFirsLineSelected = ftb.listSBmain[i].ToString(sp.iCharBegin, intOldLength);
            //    lGo:
            //        strFirsLineSelected = strFirsLineSelected.Replace(strOld, strNew);
            //        if (strFirsLineSelected.Contains(strOld) && blnMinimize && strOld != strNew) goto lGo;  // for example replace "  " to " " -> repeat until All -> " "

            //        string strTmpOld = ftb.listSBmain[i].ToString(sp.iCharBegin, intOldLength);

            //        if (strTmpOld != "") {
            //            ftb.listSBmain[i].Replace(strTmpOld, strFirsLineSelected, sp.iCharBegin, intOldLength);
            //            // Remove ----- Replace ???
            //            //ftb.listSPMain[i].listSPinLine.RemoveAt(j);
            //            int iDelta = intOldLength - strFirsLineSelected.Length;
            //            for (int jj = j; jj < ftb.listSPMain[i].listSPinLine.Count; jj++) {
            //                selectedPositions spp = ftb.listSPMain[i].listSPinLine[jj];
            //                spp.iCharBegin -= jj != j ? iDelta : 0;
            //                spp.iCharEnd -= iDelta;
            //            }


            //        }
            //    }
            //});

        }




        //private void taskFormatAllInParallelSelected() {

        //    int intElement = 0; // use for save intUIE  for CLK
        //    try {

        //        Dispatcher.Invoke(new Action(() => MainGrid.IsEnabled = false));
        //        TaskScheduler uiThread = TaskScheduler.FromCurrentSynchronizationContext();

        //        // task for find
        //        Action MainThreadFind = new Action(() => formatAllInParallel());

        //        Action ExecuteProgressClock = new Action(() => {
        //            userClock clk = new userClock();
        //            Dispatcher.Invoke(new Action(() => intElement = MainGrid.Children.Add(clk)));
        //        });

        //        // final task
        //        Action FinalThreadDoWOrk = new Action(() => {
        //            Dispatcher.Invoke(new Action(() => MainGrid.Children.RemoveAt(intElement))); // Remove CLK

        //            ftb.totalBytes();

        //            // if scroll value was changed - it automaticALY RUN UPDATE SCREEN

        //            ftb.init_HorizontalScroll();
        //            ftb.init_VerticalScroll();

        //            ftb.canvasMain.Focus();

        //            Dispatcher.Invoke(new Action(() => {
        //                MainGrid.IsEnabled = true;
        //                statusDocSizeBar.Content = ftb.longTotalDocSize.ToString("0,0");

        //            }));

        //            ftb.updateBlock();
        //        });

        //        Task MainThreadDoWorkTask = Task.Factory.StartNew(() => MainThreadFind());
        //        Task ExecuteProgressClkTask = new Task(ExecuteProgressClock);

        //        ExecuteProgressClkTask.RunSynchronously();
        //        MainThreadDoWorkTask.ContinueWith(t => FinalThreadDoWOrk(), uiThread);

        //    }
        //    catch (Exception ex) {
        //        System.Windows.MessageBox.Show("Error", ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
        //        Dispatcher.Invoke(new Action(() => MainGrid.IsEnabled = true));
        //    }
        //}

        private void appendClick()
        {
            ftb.blnAppend = ftb.blnAppend ? false : true;
            stAppend();
            if (ftb.blnAppend == false)
            {
                ftb.DeSelectAllCurrent();
                //  listSBstandard.Clear();
            }
            ftb.updateBlock();
        }

        private void btnAppend_Click(object sender, RoutedEventArgs e)
        {
            appendClick();
        }

        private void stAppend()
        {
            btnAppend.Foreground = ftb.blnAppend ? new SolidColorBrush(Colors.Magenta) : new SolidColorBrush(Colors.DarkBlue);

            ftb.blnPlaceHolder = false;
            btnPlaceHolder.Foreground = new SolidColorBrush(Colors.DarkBlue);
            btnPlaceHolder.IsEnabled = ftb.blnAppend;

            ftb.blnPenDeSelect = false;
            btnPenDeSelect.Foreground = ftb.blnAppend ? new SolidColorBrush(Colors.DarkBlue) : new SolidColorBrush(Colors.Gray);
            btnPenDeSelect.IsEnabled = ftb.blnAppend;


            ftb.blnRecSelect = false;
            btnRecSelect.Foreground = ftb.blnAppend ? new SolidColorBrush(Colors.DarkBlue) : new SolidColorBrush(Colors.Gray);
            btnRecSelect.IsEnabled = ftb.blnAppend;

            ftb.blnRecDeSelect = false;
            btnRecDeSelect.Foreground = ftb.blnAppend ? new SolidColorBrush(Colors.DarkBlue) : new SolidColorBrush(Colors.Gray);
            btnRecDeSelect.IsEnabled = ftb.blnAppend;

            //btnDeSelectLeft
            btnDeSelectLeft.Foreground = ftb.blnAppend ? new SolidColorBrush(Colors.DarkBlue) : new SolidColorBrush(Colors.Gray);
            btnDeSelectLeft.IsEnabled = ftb.blnAppend;

            //btnDeSelectLeftUp
            btnDeSelectLeftUp.Foreground = ftb.blnAppend ? new SolidColorBrush(Colors.DarkBlue) : new SolidColorBrush(Colors.Gray);
            btnDeSelectLeftUp.IsEnabled = ftb.blnAppend;

            //btnDeSelectRight
            btnDeSelectRight.Foreground = ftb.blnAppend ? new SolidColorBrush(Colors.DarkBlue) : new SolidColorBrush(Colors.Gray);
            btnDeSelectRight.IsEnabled = ftb.blnAppend;

            //btnDeSelectRightDown
            btnDeSelectRightDown.Foreground = ftb.blnAppend ? new SolidColorBrush(Colors.DarkBlue) : new SolidColorBrush(Colors.Gray);
            btnDeSelectRightDown.IsEnabled = ftb.blnAppend;

            //btnDeSelectLine
            btnDeSelectLine.Foreground = ftb.blnAppend ? new SolidColorBrush(Colors.DarkBlue) : new SolidColorBrush(Colors.Gray);
            btnDeSelectLine.IsEnabled = ftb.blnAppend;

            //chkVert.IsEnabled = ftb.blnAppend;

        }





        private void btnPenDeSelect_Click(object sender, RoutedEventArgs e)
        {
            ftb.blnRecSelect = false;
            btnRecSelect.Foreground = new SolidColorBrush(Colors.DarkBlue);

            ftb.blnRecDeSelect = false;
            btnRecDeSelect.Foreground = new SolidColorBrush(Colors.DarkBlue);

            ftb.blnPlaceHolder = false;
            btnPlaceHolder.Foreground = new SolidColorBrush(Colors.DarkBlue);

            ftb.blnPenDeSelect = ftb.blnPenDeSelect ? false : true && ftb.blnAppend;
            btnPenDeSelect.Foreground = ftb.blnPenDeSelect ? new SolidColorBrush(Colors.Magenta) : new SolidColorBrush(Colors.DarkBlue);
        }


        private void btnRecDeSelect_Click(object sender, RoutedEventArgs e)
        {
            ftb.blnRecSelect = false;
            btnRecSelect.Foreground = new SolidColorBrush(Colors.DarkBlue);

            ftb.blnPenDeSelect = false;
            btnPenDeSelect.Foreground = new SolidColorBrush(Colors.DarkBlue);

            ftb.blnPlaceHolder = false;
            btnPlaceHolder.Foreground = new SolidColorBrush(Colors.DarkBlue);

            ftb.blnRecDeSelect = ftb.blnRecDeSelect ? false : true && ftb.blnAppend;
            btnRecDeSelect.Foreground = ftb.blnRecDeSelect ? new SolidColorBrush(Colors.Magenta) : new SolidColorBrush(Colors.DarkBlue);
        }

        private void btnRecSelect_Click(object sender, RoutedEventArgs e)
        {

            ftb.blnRecDeSelect = false;
            btnRecDeSelect.Foreground = new SolidColorBrush(Colors.DarkBlue);

            ftb.blnPenDeSelect = false;
            btnPenDeSelect.Foreground = new SolidColorBrush(Colors.DarkBlue);

            ftb.blnPlaceHolder = false;
            btnPlaceHolder.Foreground = new SolidColorBrush(Colors.DarkBlue);

            ftb.blnRecSelect = ftb.blnRecSelect ? false : true && ftb.blnAppend;
            btnRecSelect.Foreground = ftb.blnRecSelect ? new SolidColorBrush(Colors.Magenta) : new SolidColorBrush(Colors.DarkBlue);

        }


        private void btnPlaceHolder_Click(object sender, RoutedEventArgs e)
        {

            ftb.blnPenDeSelect = false;
            btnPenDeSelect.Foreground = new SolidColorBrush(Colors.DarkBlue);

            ftb.blnRecDeSelect = false;
            btnRecDeSelect.Foreground = new SolidColorBrush(Colors.DarkBlue);

            ftb.blnRecSelect = false;
            btnRecSelect.Foreground = new SolidColorBrush(Colors.DarkBlue);

            ftb.blnPlaceHolder = ftb.blnPlaceHolder ? false : true && ftb.blnAppend;
            btnPlaceHolder.Foreground = ftb.blnPlaceHolder ? new SolidColorBrush(Colors.Magenta) : new SolidColorBrush(Colors.DarkBlue);

        }


        private void ftb_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up || e.Key == Key.Down)
            {
            }
            else { ftb.canvasMain.Focus(); }
            systemInfo();
        }

        private void comboFontSize_MouseEnter(object sender, MouseEventArgs e)
        {
            comboFontSize.Foreground = new SolidColorBrush(Colors.DarkBlue);
        }

        private void comboFontSize_MouseLeave(object sender, MouseEventArgs e)
        {
            //   comboFontSize.Foreground = new SolidColorBrush(Colors.DarkGray);
        }

        private void btnClearCurrent_Click(object sender, RoutedEventArgs e)
        {

            gridButtonRight.IsEnabled = true;


            ftb.blnError = false;

            ftb.blnAbort = true;

            ftb.canvasSelected.Children.Clear();
            ftb.canvasSelecting.Children.Clear();

            ftb.intVertCountLinesOnPage = 0;  // depends from FontSize
            ftb.intHorizCountCharsOnPage = 0;

            //   ftb.intFirstLineOnPage = 0; // absolute number first line on current page  - if screen size was changed - need to re-calculate      
            //   ftb.intFirstCharOnPage = 0; // absolute number first char on current line on Current Page        

            //Caret section
            ftb.intAbsolutCaretVertCurrent = 0;
            ftb.intAbsolutCaretHorizCurrent = 0;

            ftb.listSB_A = null;
            ftb.listSB_B = null;
            ftb.listSB_C = null;
            ftb.listSB_D = null;
            ftb.listSB_E = null;

            ftb.blnSomthingSelected = false;

            Dispatcher.Invoke(new Action(() => txtFind.Text = ""));
            Dispatcher.Invoke(new Action(() => txtReplace.Text = ""));

            ftb.listSBmain = null;
            ftb.listSPMain = null;

            GC.Collect();
            Thread.Sleep(500);

            txtMemo.Text = "";

            ftb.setTextBlockClear();

            Dispatcher.Invoke(new Action(() => rbLog.Foreground = new SolidColorBrush(Colors.Black)));

            ftb.listSBmain = new List<StringBuilder>() { new StringBuilder() };
            ftb.listSPMain = new List<listOfSelectedPositionInLine>();

            Dispatcher.Invoke(new Action(() => statusFile.Content = ""));
            Dispatcher.Invoke(new Action(() => statusDocSizeBar.Content = ""));
            Dispatcher.Invoke(new Action(() => statusSystemBar.Content = ""));

            Dispatcher.Invoke(new Action(() => ftb.totalBytes()));
            Dispatcher.Invoke(new Action(() => systemInfo()));

            ftb.listSB_A = new List<StringBuilder>();
            ftb.listSB_B = new List<StringBuilder>();
            ftb.listSB_C = new List<StringBuilder>();
            ftb.listSB_D = new List<StringBuilder>();
            ftb.listSB_E = new List<StringBuilder>();

            ftb.init_HorizontalScroll();
            ftb.init_VerticalScroll();

            Dispatcher.Invoke(new Action(() => ftb.set_Caret(0, 0)));

            ftb.blnAppend = true;
            appendClick();

            ftb.canvasMain.Focus();


        }

        // --------------- --------------------------------------------------------------------------------------------------------------------------





        // Select/Deselect --------------------------------------------------------------------------------------------------------------------------

        private void btnSelectAll_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ftb.listSBmain.Count == 0 || ftb.listSBmain == null) { return; }
                ftb.correctLocation();
                ftb.resetCM();

                Parallel.For(0, ftb.listSBmain.Count, (int i) =>
                //   for (int i = 0; i < ftb.listSBmain.Count; i++)
                {
                    ftb.addSP(i, 0, ftb.listSBmain[i].Length, false);
                });

                //   ftb.addSelectedCanvas(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Program has a problem allocating additional char(s)/byte(s) in memory (btnSelectAll) ...\nPlease save your  job ... ...");
                ftb.blnError = true;
            }




            ftb.updateBlock();
        }

        private void btnDeSelectAll_Click(object sender, RoutedEventArgs e)
        {
            ftb.correctLocation();
            ftb.resetCM();

            if (ftb.listSBmain.Count == 0 || ftb.listSBmain == null) { return; }
            ftb.DeSelectAllCurrent();
        }

        private void btnSelectRight_Click(object sender, RoutedEventArgs e)
        {
            ftb.resetCM();

            if (ftb.listSBmain.Count == 0 || ftb.listSBmain == null) { return; }

            if (ftb.blnAppend == false)
            {
                ftb.correctLocation();
                ftb.DeSelectAllCurrent();
            }

            if (Keyboard.Modifiers == ModifierKeys.Control && ftb.blnAppend)
            {
                Parallel.For(0, ftb.listSBmain.Count, (int i) =>
              //for (int i = 0; i < ftb.listSPMain.Count; i++)
              {
                  ftb.addSP(i, ftb.intAbsolutCaretHorizCurrent, ftb.listSBmain[i].Length < ftb.intAbsolutCaretHorizCurrent ? ftb.intAbsolutCaretHorizCurrent : ftb.listSBmain[i].Length, ftb.blnAppend);   ////
              });
            }
            else
            {
                ftb.correctLocation();

                int iStop = ftb.listSBmain[ftb.intAbsolutCaretVertCurrent].Length < ftb.intAbsolutCaretHorizCurrent ? ftb.intAbsolutCaretHorizCurrent : ftb.listSBmain[ftb.intAbsolutCaretVertCurrent].Length;
                ftb.addSP(ftb.intAbsolutCaretVertCurrent, ftb.intAbsolutCaretHorizCurrent, iStop, ftb.blnAppend);
            }

            ftb.updateBlock();
        }

        private void btnDeSelectRight_Click(object sender, RoutedEventArgs e)
        {
            ftb.resetCM();

            if (ftb.listSBmain.Count == 0 || ftb.listSBmain == null) { return; }

            if (Keyboard.Modifiers == ModifierKeys.Control && ftb.blnAppend)
            {
                Parallel.For(0, ftb.listSBmain.Count, (int i) =>
              //for (int i = 0; i < ftb.listSPMain.Count; i++)
              {
                  ftb.deSelectSP(i, ftb.intAbsolutCaretHorizCurrent, ftb.listSBmain[i].Length < ftb.intAbsolutCaretHorizCurrent ? ftb.intAbsolutCaretHorizCurrent : ftb.listSBmain[i].Length, ftb.blnAppend);   ////
              });
            }
            else
            {
                ftb.correctLocation();

                int iStop = ftb.listSBmain[ftb.intAbsolutCaretVertCurrent].Length < ftb.intAbsolutCaretHorizCurrent ? ftb.intAbsolutCaretHorizCurrent : ftb.listSBmain[ftb.intAbsolutCaretVertCurrent].Length;
                ftb.deSelectSP(ftb.intAbsolutCaretVertCurrent, ftb.intAbsolutCaretHorizCurrent, iStop, ftb.blnAppend);
            }

            ftb.updateBlock();
        }



        private void btnSelectLine_Click(object sender, RoutedEventArgs e)
        {
            ftb.resetCM();
            if (ftb.listSBmain.Count == 0 || ftb.listSBmain == null) { return; }

            if (ftb.blnAppend == false)
            {
                ftb.DeSelectAllCurrent();
                ftb.correctLocation();
            }
            int intCurrentLine = (int)ftb.intAbsolutCaretVertCurrent;
            if (ftb.listSBmain.Count < intCurrentLine) return;

            ftb.addSP(intCurrentLine, 0, ftb.listSBmain[(int)ftb.intAbsolutCaretVertCurrent].Length, ftb.blnAppend);

            ftb.updateBlock();
        }

        private void btnDeSelectLine_Click(object sender, RoutedEventArgs e)
        {
            ftb.resetCM();
            if (ftb.listSBmain.Count == 0 || ftb.listSBmain == null) { return; }

            int intCurrentLine = (int)ftb.intAbsolutCaretVertCurrent;
            if (ftb.listSBmain.Count < intCurrentLine) return;

            ftb.deSelectSP(intCurrentLine, 0, ftb.listSBmain[(int)ftb.intAbsolutCaretVertCurrent].Length, ftb.blnAppend);

            ftb.updateBlock();
        }

        private void btnSelectLeft_Click(object sender, RoutedEventArgs e)
        {
            ftb.resetCM();
            if (ftb.listSBmain.Count == 0 || ftb.listSBmain == null) { return; }

            if (ftb.blnAppend == false)
            {
                ftb.DeSelectAllCurrent();
            }

            if (Keyboard.Modifiers == ModifierKeys.Control && ftb.blnAppend)
            {
                Parallel.For(0, ftb.listSBmain.Count, (int i) =>
              //for (int i = 0; i < ftb.listSPMain.Count; i++)
              {

                  int intSE = ftb.intAbsolutCaretHorizCurrent > ftb.listSBmain[i].Length ? ftb.listSBmain[i].Length : ftb.intAbsolutCaretHorizCurrent;
                  ftb.addSP(i, 0, intSE, ftb.blnAppend);
              });
            }
            else
            {
                ftb.correctLocation();
                ftb.addSP(ftb.intAbsolutCaretVertCurrent, 0, (int)ftb.intAbsolutCaretHorizCurrent, ftb.blnAppend);
            }

            ftb.addSelectedCanvas(false);
            ftb.updateBlock();
        }

        private void btnDeSelectLeft_Click(object sender, RoutedEventArgs e)
        {
            ftb.resetCM();
            if (ftb.listSBmain.Count == 0 || ftb.listSBmain == null) { return; }



            if (Keyboard.Modifiers == ModifierKeys.Control && ftb.blnAppend)
            {

                Parallel.For(0, ftb.listSBmain.Count, (int i) =>
              //for (int i = 0; i < ftb.listSPMain.Count; i++)
              {
                  int intSE = ftb.intAbsolutCaretHorizCurrent > ftb.listSBmain[i].Length ? ftb.listSBmain[i].Length : ftb.intAbsolutCaretHorizCurrent;
                  ftb.deSelectSP(i, 0, intSE, ftb.blnAppend);
              });
            }
            else
            {
                ftb.correctLocation();
                ftb.deSelectSP(ftb.intAbsolutCaretVertCurrent, 0, (int)ftb.intAbsolutCaretHorizCurrent, ftb.blnAppend);
            }

            //   ftb.addSelectedCanvas(false);
            ftb.updateBlock();
        }

        private void btnSelectRightDown_Click(object sender, RoutedEventArgs e)
        {
            ftb.resetCM();

            if (ftb.listSBmain.Count == 0 || ftb.listSBmain == null) { return; }

            ftb.intAbsolutSelectVertStart = ftb.listSBmain.Count - 1;
            ftb.intAbsolutSelectHorizStart = ftb.listSBmain[ftb.intAbsolutSelectVertStart].Length;

            if (ftb.blnAppend == false)
            {
                ftb.DeSelectAllCurrent();
                ftb.correctLocation();
            }

            int intCurrentLine = (int)ftb.intAbsolutCaretVertCurrent;
            if (ftb.listSBmain.Count < intCurrentLine) return;

            ftb.addSP(intCurrentLine, ftb.intAbsolutCaretHorizCurrent, ftb.listSBmain[intCurrentLine].Length < ftb.intAbsolutCaretHorizCurrent ? ftb.intAbsolutCaretHorizCurrent : ftb.listSBmain[intCurrentLine].Length, ftb.blnAppend);

            bool blnTmp = Keyboard.Modifiers == ModifierKeys.Control && ftb.blnAppend;

            Parallel.For(ftb.intAbsolutCaretVertCurrent + 1, ftb.listSBmain.Count, (int i) =>
          //for (int i = ftb.intAbsolutCaretVertCurrent + 1; i < ftb.listSPMain.Count; i++)
          {
              if (blnTmp)
              {
                  int iStop = ftb.listSBmain[i].Length < ftb.intAbsolutCaretHorizCurrent ? ftb.intAbsolutCaretHorizCurrent : ftb.listSBmain[i].Length;
                  ftb.addSP(i, ftb.intAbsolutCaretHorizCurrent, iStop, ftb.blnAppend); ///////
              }
              else
              {

                  ftb.addSP(i, 0, (int)ftb.listSBmain[i].Length, ftb.blnAppend);
              }

          });

            //    ftb.addSelectedCanvas(false);
            ftb.updateBlock();
        }

        private void btnDeSelectRightDown_Click(object sender, RoutedEventArgs e)
        {
            ftb.resetCM();
            if (ftb.listSBmain.Count == 0 || ftb.listSBmain == null) { return; }

            int intCurrentLine = ftb.intAbsolutCaretVertCurrent;
            if (ftb.listSBmain.Count < intCurrentLine) return;

            ftb.deSelectSP(intCurrentLine, ftb.intAbsolutCaretHorizCurrent, ftb.listSBmain[intCurrentLine].Length < ftb.intAbsolutCaretHorizCurrent ? ftb.intAbsolutCaretHorizCurrent : ftb.listSBmain[intCurrentLine].Length, ftb.blnAppend);

            bool blnTmp = Keyboard.Modifiers == ModifierKeys.Control && ftb.blnAppend;

            Parallel.For(ftb.intAbsolutCaretVertCurrent + 1, ftb.listSBmain.Count, (int i) =>
            //for (int i = (int)ftb.intAbsolutCaretVertCurrent + 1; i < ftb.listSPMain.Count; i++)
            {
                if (blnTmp)
                {
                    int iStop = ftb.listSBmain[i].Length < ftb.intAbsolutCaretHorizCurrent ? ftb.intAbsolutCaretHorizCurrent : ftb.listSBmain[i].Length;
                    ftb.deSelectSP(i, ftb.intAbsolutCaretHorizCurrent, iStop, ftb.blnAppend);////////////////
                }
                else
                {
                    ftb.deSelectSP(i, 0, ftb.listSBmain[i].Length, ftb.blnAppend);
                }

            });

            // ftb.addSelectedCanvas(false);
            ftb.updateBlock();
        }


        private void btnSelectLeftUp_Click(object sender, RoutedEventArgs e)
        {
            ftb.resetCM();
            if (ftb.listSBmain.Count == 0 || ftb.listSBmain == null) { return; }

            ftb.intAbsolutSelectVertStart = 0;
            ftb.intAbsolutSelectHorizStart = 0;

            if (ftb.blnAppend == false)
            {
                ftb.DeSelectAllCurrent();
                ftb.correctLocation();
            }

            int intCurrentLine = (int)ftb.intAbsolutCaretVertCurrent;

            if (ftb.listSBmain.Count < intCurrentLine) return;


            int intSE = ftb.intAbsolutCaretHorizCurrent > ftb.listSBmain[intCurrentLine].Length ? ftb.listSBmain[intCurrentLine].Length : ftb.intAbsolutCaretHorizCurrent;
            ftb.addSP(intCurrentLine, 0, intSE, ftb.blnAppend);

            bool blnTmp = Keyboard.Modifiers == ModifierKeys.Control && ftb.blnAppend;

            Parallel.For(0, ftb.intAbsolutCaretVertCurrent, (int i) =>
            //for (int i = 0; i < ftb.intAbsolutCaretVertCurrent; i++)
            {
                if (blnTmp)
                {
                    intSE = ftb.intAbsolutCaretHorizCurrent > ftb.listSBmain[i].Length ? ftb.listSBmain[i].Length : ftb.intAbsolutCaretHorizCurrent;
                    ftb.addSP(i, 0, intSE, ftb.blnAppend);
                }
                else
                {
                    ftb.addSP(i, 0, ftb.listSBmain[i].Length, ftb.blnAppend);
                }
            });

            //    ftb.addSelectedCanvas(false);
            ftb.updateBlock();
        }

        private void btnDeSelectLeftUp_Click(object sender, RoutedEventArgs e)
        {
            ftb.resetCM();
            if (ftb.listSBmain.Count == 0 || ftb.listSBmain == null) { return; }

            int intCurrentLine = (int)ftb.intAbsolutCaretVertCurrent;
            if (ftb.listSBmain.Count < intCurrentLine) return;

            int intSE = ftb.intAbsolutCaretHorizCurrent > ftb.listSBmain[intCurrentLine].Length ? ftb.listSBmain[intCurrentLine].Length : ftb.intAbsolutCaretHorizCurrent;
            ftb.deSelectSP(intCurrentLine, 0, intSE, ftb.blnAppend);

            bool blnTmp = Keyboard.Modifiers == ModifierKeys.Control && ftb.blnAppend;

            Parallel.For(0, intCurrentLine, (int i) =>
           //for (int i = 0; i < intCurrentLine; i++)
           {
               if (blnTmp)
               {
                   intSE = ftb.intAbsolutCaretHorizCurrent > ftb.listSBmain[i].Length ? ftb.listSBmain[i].Length : ftb.intAbsolutCaretHorizCurrent;
                   ftb.deSelectSP(i, 0, intSE, ftb.blnAppend);
               }
               else
               {
                   ftb.deSelectSP(i, 0, (int)ftb.listSBmain[i].Length, ftb.blnAppend);
               }
           });


            //    ftb.addSelectedCanvas(false);
            ftb.updateBlock();
        }

        private void MainGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // ftb.tb_PreviewMouseLeftButtonUp(sender, e); -- Do not Use here - Added 2 times record
        }

        private void MainGrid_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            //  ftb.tb_PreviewMouseRightButtonUp(sender, e); -- Do not Use here - Added 2 times record
        }


        public void selectionCopy()
        {
            listSB_CB = null;
            selectedCB = null;
            GC.Collect();

            listSB_CB = new List<StringBuilder>();
            selectedCB = new List<listOfSelectedPositionInLine>();

            if (ftb.blnSomthingSelected)
            {
                //    Parallel.For(0, ftb.listSPMain.Count, (int i) =>

                for (int i = 0; i < ftb.listSPMain.Count; i++)
                {
                    if (ftb.listSPMain[i].listSPinLine.Count > 0)
                    {
                        StringBuilder sbTmp = new StringBuilder();

                        foreach (selectedPositions sp in ftb.listSPMain[i].listSPinLine)
                        {
                            sbTmp.Append(ftb.listSBmain[i].ToString(sp.iCharBegin, sp.iCharEnd - sp.iCharBegin)).Append("║");
                        }

                        listSB_CB.Add(sbTmp.Replace("║", "", sbTmp.Length - 1, 1));
                        selectedCB.Add(new listOfSelectedPositionInLine());
                    }
                }
                // );
            }
        }




        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            selectionCopy();

            ftb.init_VerticalScroll();
            ftb.init_HorizontalScroll();
            systemInfo();

            ftb.totalBytes();
            ftb.updateBlock();

        }

        private void btnCut_Click(object sender, RoutedEventArgs e)
        {
            ftb.blnCut = true;
            selectionCopy();

            if (ftb.blnAppend)
            {
                ftb.deleteSelection();
            }
            else
            {
                ftb.deleteABCDE();
            }


            //ftb.addSelectedCanvas(false);

            ftb.init_VerticalScroll();
            ftb.init_HorizontalScroll();
            systemInfo();

            ftb.totalBytes();
            ftb.updateBlock();
        }

        private void btnPaste_Click(object sender, RoutedEventArgs e)
        {
            if (ftb.listSBmain == null)
            {
                ftb.listSBmain.Add(new StringBuilder(""));
                ftb.listSPMain.Add(new listOfSelectedPositionInLine());
            }
            ftb.somthingSelected();

            if (ftb.blnAppend)
            {
                if (!ftb.blnSomthingSelected)
                {
                    MessageBox.Show("Before using <Group Paste> please select something. \nData will be inserted from CB.");
                    return;
                }
                ftb.pasteGroup(listSB_CB);
            }
            else
            {
                ftb.deleteABCDE();
                ftb.pasteStandard(listSB_CB);
            }

            ftb.init_VerticalScroll();
            ftb.init_HorizontalScroll();
            systemInfo();

            ftb.totalBytes();

            ftb.addSelectedCanvas(true);
            ftb.updateBlock();
        }



        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ftb.blnMouseLeftButtonPressed = false;
            setHighlight();
        }



        private void txtMemo_MouseEnter(object sender, MouseEventArgs e)
        {

            var note = sender as TextBox;

            note.Foreground = new SolidColorBrush(Colors.DarkBlue);
            //    note.FontWeight = FontWeights.Bold;
            note.Background = new SolidColorBrush(Colors.White);
        }

        private void txtMemo_MouseLeave(object sender, MouseEventArgs e)
        {

            var note = sender as TextBox;

            note.Foreground = new SolidColorBrush(Colors.DeepSkyBlue);
            //note.FontWeight = FontWeights.Normal;
            //note.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xF1, 0xED, 0xED));
            note.Background = new SolidColorBrush(Colors.LightGray);
        }


        // RUN Section ------------------------------------------------------------------------------------------------------------

        private void btnRunAll_Click(object sender, RoutedEventArgs e)
        {
            if (comboDB.SelectedIndex == -1) return;

            string strDB = comboDB.SelectedValue.ToString();

            try
            {
                DataBaseCommon db = new DataBaseCommon();
                string strError = "";
                if (db.TestConnection(strDB, txtConnectionString.Text, out strError) == false)
                {
                    MessageBox.Show("Wrong Connection String ...");
                    return;
                }
            }
            catch
            {
                MessageBox.Show("Please install first SQL .net provider to DataBase ... (visit to microsoft.com | mysql.com | etc...");
                return;
            }

            listSB_SS = new List<StringBuilder>();
            StringBuilder sbRun = sqlSB(ftb.listSBmain);
            listSB_SS.Add(new StringBuilder("RUN ALL SQL -> see primary window."));

            // List<listOfSelectedPositionInLine> listSP = new List<listOfSelectedPositionInLine>();
            selectedSS = new List<listOfSelectedPositionInLine>();

            var newResult = new resultInGrid();
            newResult.Show();


            newResult.runSQL(strDB, txtConnectionString.Text, sbRun);
            //  Dispatcher.Invoke(new Action(() => ftb.canvasMain.Focus()));

            ftb.canvasMain.Focus();
        }

        private void btnRunCurrent_Click(object sender, RoutedEventArgs e)
        {
            if (comboDB.SelectedIndex == -1) return;

            string strDB = comboDB.SelectedValue.ToString();


            try
            {
                DataBaseCommon db = new DataBaseCommon();
                string strError = "";
                if (db.TestConnection(strDB, txtConnectionString.Text, out strError) == false)
                {
                    MessageBox.Show("Wrong Connection String ...");
                    return;
                }
            }
            catch
            {
                MessageBox.Show("Please install first SQL .net provider to DataBase ... (visit to microsoft.com | mysql.com | etc...");
                return;
            }

            listSB_SS = new List<StringBuilder>();
            listSB_SS = ftb.copyABCDE(true);

            //   if (listSB_SS == null) listSB_SS = new List<StringBuilder>() { new StringBuilder("") };

            selectedSS = new List<listOfSelectedPositionInLine>();
            foreach (StringBuilder sb in listSB_SS) selectedSS.Add(new listOfSelectedPositionInLine());

            StringBuilder sbRun = sqlSB(listSB_SS);

            var newResult = new resultInGrid();
            newResult.Show();

            newResult.runSQL(strDB, txtConnectionString.Text, sbRun);

            //   Dispatcher.Invoke(new Action(() => ftb.canvasMain.Focus()));
            ftb.canvasMain.Focus();
        }

        private void btnRunCurrentSelected_Click(object sender, RoutedEventArgs e)
        {
            if (comboDB.SelectedIndex == -1) return;

            string strDB = comboDB.SelectedValue.ToString();


            try
            {
                DataBaseCommon db = new DataBaseCommon();
                string strError = "";
                if (db.TestConnection(strDB, txtConnectionString.Text, out strError) == false)
                {
                    MessageBox.Show("Wrong Connection String ...");
                    return;
                }
            }
            catch
            {
                MessageBox.Show("Please install first SQL .net provider to DataBase ... (visit to microsoft.com | mysql.com | etc...");
                return;
            }

            listSB_SS = new List<StringBuilder>();
            listSB_SS = ftb.copyABCDESelected(false);

            //   if (listSB_SS == null) listSB_SS = new List<StringBuilder>() { new StringBuilder("") };

            selectedSS = new List<listOfSelectedPositionInLine>();
            foreach (StringBuilder sb in listSB_SS) selectedSS.Add(new listOfSelectedPositionInLine());

            StringBuilder sbRun = sqlSB(listSB_SS);


            var newResult = new resultInGrid();
            newResult.Show();

            newResult.runSQL(strDB, txtConnectionString.Text, sbRun);

            // Dispatcher.Invoke(new Action(() => ftb.canvasMain.Focus()));
            ftb.canvasMain.Focus();
        }



        private void btnRunSelected_Click(object sender, RoutedEventArgs e)
        {
            if (comboDB.SelectedIndex == -1) return;

            string strDB = comboDB.SelectedValue.ToString();


            try
            {
                DataBaseCommon db = new DataBaseCommon();
                string strError = "";
                if (db.TestConnection(strDB, txtConnectionString.Text, out strError) == false)
                {
                    MessageBox.Show("Wrong Connection String ...");
                    return;
                }
            }
            catch
            {
                MessageBox.Show("Please install first SQL .net provider to DataBase ... (visit to microsoft.com | mysql.com | etc...");
                return;
            }

            listSB_SS = new List<StringBuilder>();
            listSB_SS = ftb.copyABCDESelected(true);

            //   if (listSB_SS == null) listSB_SS = new List<StringBuilder>() { new StringBuilder("") };

            selectedSS = new List<listOfSelectedPositionInLine>();
            foreach (StringBuilder sb in listSB_SS) selectedSS.Add(new listOfSelectedPositionInLine());

            StringBuilder sbRun = sqlSB(listSB_SS);

            var newResult = new resultInGrid();
            newResult.Show();

            newResult.runSQL(strDB, txtConnectionString.Text, sbRun);

            // Dispatcher.Invoke(new Action(() => ftb.canvasMain.Focus()));
            ftb.canvasMain.Focus();
        }


        // RUN Section ------------------------------------------------------------------------------------------------------------


        private List<listOfSelectedPositionInLine> createNewSPlist(List<StringBuilder> sbList)
        {
            List<listOfSelectedPositionInLine> spList = new List<listOfSelectedPositionInLine>();
            foreach (StringBuilder sb in sbList)
            {
                spList.Add(new listOfSelectedPositionInLine());
            }

            return spList;
        }



        StringBuilder sqlSB(List<StringBuilder> sbSql)
        {
            if (sbSql == null) return new StringBuilder("");

            StringBuilder sbTmp = new StringBuilder();
            foreach (StringBuilder sb in sbSql)
            {
                sbTmp = sbTmp.Append(sb).Append(' ').Append(Environment.NewLine);     //if lines ended/started without space ....
            }
            return sbTmp;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (ftb.blnAppend)
            {
                ftb.deleteSelection();
            }
            else
            {
                ftb.deleteABCDE();
            }

            ftb.init_VerticalScroll();
            ftb.init_HorizontalScroll();
            systemInfo();

            ftb.totalBytes();
            ftb.updateBlock();
        }




        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {

            string strDB = comboDB.SelectedValue == null ? "" : comboDB.SelectedValue.ToString().Trim();
            string strError = "";
            try
            {
                DataBaseCommon db = new DataBaseCommon();
                if (db.TestConnection(strDB, txtConnectionString.Text, out strError) == false)
                {
                    MessageBox.Show(strError);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            try
            {
                var newTablesColumns = new selectedTablesColumns();
                newTablesColumns.Width = 500;

                newTablesColumns.runSCHEMA(strDB, txtConnectionString.Text);
                               
                newTablesColumns.Show();

                // Task.Factory.StartNew(() => ftb.listTablesHelper = listDBhelper());

                ftb.listTablesHelper = listTableHelper();
                ftb.listColumnsHelper = listColumnsHelper();
            }
            catch
            {
                MessageBox.Show("You do not have an access to INFORMATION_SCHEMA table ... Use different privilege ... ");
                return;
            }


        }

        // DB test connection
        public bool blnConnectedToDb = false;


        private List<string> listTableHelper()
        {
            List<string> listTmp = new List<string>();
            StringBuilder sbRun = new StringBuilder();

            try
            {
                int intEffected = 0;
                DataBaseCommon db = new DataBaseCommon();
                DataSet ds = new DataSet();

                string strDB = comboDB.SelectedValue.ToString();
                string strConnectionString = txtConnectionString.Text;

                if (strDB.ToUpper().IndexOf("SQLCLIENT") >= 0)
                {
                    sbRun.Append(" SELECT UPPER(TABLE_NAME) AS TABLE_NAME FROM INFORMATION_SCHEMA.TABLES ORDER BY TABLE_NAME;   ");
                    string strError = "";
                    ds = db.GetDataSet(sbRun, strDB, txtConnectionString.Text, out strError, out intEffected);
                }
                else if (strDB.ToUpper().IndexOf("IBM") >= 0)
                {
                    // find Data

                    sbRun.Append(" SELECT TABSCHEMA || '.' || UPPER(TABNAME)  TABLE_NAME  FROM SYSCAT.TABLES WHERE OWNERTYPE='U' ;");
                    string strError = "";
                    ds = db.GetDataSet(sbRun, strDB, txtConnectionString.Text, out strError, out intEffected);
                    ds = db.GetDataSet(sbRun, strDB, strConnectionString, out strError, out intEffected);
                }
                else { return listTmp; };


                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    listTmp.Add(row[0].ToString());
                }
                blnConnectedToDb = true;
            }
            catch
            {
                blnConnectedToDb = false;
            }
            return listTmp;
        }


        private List<string> listColumnsHelper()
        {
            int intEffected = 0;

            StringBuilder sbRun = new StringBuilder();
            List<string> listTmp = new List<string>();

            string strDB = comboDB.SelectedValue.ToString();
            string strConn = txtConnectionString.Text;

            try
            {
                DataBaseCommon db = new DataBaseCommon();
                DataSet ds = new DataSet();

                if (strDB.ToUpper().IndexOf("SQLCLIENT") >= 0)
                {
                    sbRun.Append(" SELECT UPPER(TABLE_NAME) + '.' + UPPER(COLUMN_NAME) AS FULL_NAME FROM INFORMATION_SCHEMA.COLUMNS  ORDER BY TABLE_NAME,COLUMN_NAME; ");
                    string strError = "";
                    ds = db.GetDataSet(sbRun, strDB, txtConnectionString.Text, out strError, out intEffected);
                }
                else if (strDB.ToUpper().IndexOf("IBM") >= 0)
                {
                    // find Data

                    sbRun.Append(" SELECT tT.TABSCHEMA || '.' || tC.TABNAME || '.' || tC.COLNAME AS FULL_NAME FROM SYSCAT.COLUMNS AS tC INNER JOIN SYSCAT.TABLES AS tT ON tT.TABNAME = tc.TABNAME AND tT.OWNERTYPE='U';");
                    string strError = "";
                    ds = db.GetDataSet(sbRun, strDB, txtConnectionString.Text, out strError, out intEffected);
                }
                else { return listTmp; };


                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    listTmp.Add(row[0].ToString());
                }
                blnConnectedToDb = true;
            }
            catch
            {
                blnConnectedToDb = false;
            }
            return listTmp;
        }


        //---------------------------------------------------------

        private void MainGrid_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            systemInfo();
            // if (ftb.blnMouseLeftButtonPressed) ftb.tb_PreviewMouseLeftButtonUp(sender, e);   // DO NOT DO THAT HERE / Multiply INSERT REC
        }

        private void chkBoxSame_Checked(object sender, RoutedEventArgs e)
        {
            txtConnectionString.IsEnabled = false;
        }

        private void chkBoxSame_Unchecked(object sender, RoutedEventArgs e)
        {
            txtConnectionString.IsEnabled = true;
        }

        private void comboDB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (comboDB != null && ftb != null)
            {

                ftb.strDataBaseName = comboDB.SelectedValue.ToString(); 
                ftb.strConnection = txtConnectionString.Text;


                //switch ((sender as ComboBox).SelectedIndex)
                //{
                //    case 0: ftb.strDataBaseName = "MSSQL"; break;
                //    case 1: ftb.strDataBaseName = "MSCOMP"; break;
                //    case 2: ftb.strDataBaseName = "MYSQL"; break;
                //    case 3: ftb.strDataBaseName = "ODBC"; break;
                //    default: ftb.strDataBaseName = ""; break;
                //}
            }

            if (ftb != null) btnHighlight.ToolTip = "Highlight SQL (" + ftb.strDataBaseName + ") Reserved words.";
            if (ftb != null) ftb.keySQL = new selectedSqlKeys(ftb.strConnection, ftb.strDataBaseName, ftb.blnODBCHighlight, ftb.blnFunctionHighlight);

            if (ftb != null) ftb.updateBlock();
        }


        private void comboColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem cbi = (ComboBoxItem)(sender as ComboBox).SelectedItem;
            selectedColors sC = (selectedColors)cbi.Content;

            ftb.selectedColor = (Color)ColorConverter.ConvertFromString(sC.strColor);
            txtFind.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(sC.strColor));
            ftb.updateBlock();
        }

        private void btnHighlight_Click(object sender, RoutedEventArgs e)
        {
            setHighlight();
        }

        private void setHighlight()
        {
            ftb.blnHighlight = ftb.blnHighlight ? false : true;
            btnHighlight.Foreground = ftb.blnHighlight ? new SolidColorBrush(Colors.Magenta) : new SolidColorBrush(Colors.DarkBlue);
            ftb.updateBlock();
        }



        private void chkODBC_Checked(object sender, RoutedEventArgs e)
        {
            ftb.blnODBCHighlight = true;
            ftb.keySQL = new selectedSqlKeys(ftb.strConnection, ftb.strDataBaseName, ftb.blnODBCHighlight, ftb.blnFunctionHighlight);
            ftb.updateBlock();
        }

        private void chkODBC_Unchecked(object sender, RoutedEventArgs e)
        {
            ftb.blnODBCHighlight = false;
            ftb.keySQL = new selectedSqlKeys(ftb.strConnection, ftb.strDataBaseName, ftb.blnODBCHighlight, ftb.blnFunctionHighlight);
            ftb.updateBlock();
        }

        private void chkFunction_Checked(object sender, RoutedEventArgs e)
        {
            ftb.blnFunctionHighlight = true;
            ftb.keySQL = new selectedSqlKeys(ftb.strConnection,ftb.strDataBaseName, ftb.blnODBCHighlight, ftb.blnFunctionHighlight);
            ftb.updateBlock();
        }

        private void chkFunction_Unchecked(object sender, RoutedEventArgs e)
        {
            ftb.blnFunctionHighlight = false;
            ftb.keySQL = new selectedSqlKeys(ftb.strConnection,ftb.strDataBaseName, ftb.blnODBCHighlight, ftb.blnFunctionHighlight);
            ftb.updateBlock();
        }

        private void chkApostrophe_Checked(object sender, RoutedEventArgs e)
        {
            ftb.blnApostropheHighlight = true;
            ftb.updateBlock();
        }

        private void chkApostrophe_Unchecked(object sender, RoutedEventArgs e)
        {
            ftb.blnApostropheHighlight = false;
            ftb.updateBlock();
        }




        private void btnCompress_Click(object sender, RoutedEventArgs e)
        {
            taskCompressAllInParallelSelected();
        }


        private void taskCompressAllInParallelSelected()
        {

            // int intElement = 0; // use for save intUIE  for CLK
            try
            {

                Dispatcher.Invoke(new Action(() =>
                {
                    MainGrid.IsEnabled = false;
                 //   mainWindow.ResizeMode = System.Windows.ResizeMode.NoResize;
                }));

                TaskScheduler uiThread = TaskScheduler.FromCurrentSynchronizationContext();

                // task for find
                Action MainThreadFind = new Action(() => ftb.compressABCDE());

                Action ExecuteProgressClock = new Action(() =>
                {

                    userClk.Visibility = Visibility.Visible;
                    //  userClock clk = new userClock();
                    //  Dispatcher.Invoke(new Action(() => intElement = MainGrid.Children.Add(clk)));

                });

                // final task
                Action FinalThreadDoWOrk = new Action(() =>
                {

                    userClk.Visibility = Visibility.Collapsed;
                    //Dispatcher.Invoke(new Action(() => MainGrid.Children.RemoveAt(intElement))); // Remove CLK

                 //   mainWindow.ResizeMode = System.Windows.ResizeMode.CanResize;

                    ftb.totalBytes();

                    // if scroll value was changed - it automaticALY RUN UPDATE SCREEN

                    ftb.init_HorizontalScroll();
                    ftb.init_VerticalScroll();

                    ftb.canvasMain.Focus();

                    Dispatcher.Invoke(new Action(() =>
                    {
                        MainGrid.IsEnabled = true;
                        statusDocSizeBar.Content = ftb.longTotalDocSize.ToString("0,0");

                    }));

                    ftb.updateBlock();
                });

                Task MainThreadDoWorkTask = Task.Factory.StartNew(() => MainThreadFind());
                Task ExecuteProgressClkTask = new Task(ExecuteProgressClock);

                ExecuteProgressClkTask.RunSynchronously();
                MainThreadDoWorkTask.ContinueWith(t => FinalThreadDoWOrk(), uiThread);

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error", ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                Dispatcher.Invoke(new Action(() => MainGrid.IsEnabled = true));
            }
        }

        private void btnAbort_Click(object sender, RoutedEventArgs e)
        {
            ftb.blnAbort = true;
        }

        private void btnFormat_Click(object sender, RoutedEventArgs e)
        {
            taskFormatAllInParallelSelected("");
        }


        public void taskFormatAllInParallelSelected(string strSplit)
        {

            //    int intElement = 0; // use for save intUIE  for CLK
            try
            {

                Dispatcher.Invoke(new Action(() =>
                {
                    MainGrid.IsEnabled = false;
              //      mainWindow.ResizeMode = System.Windows.ResizeMode.NoResize;
                }));

                TaskScheduler uiThread = TaskScheduler.FromCurrentSynchronizationContext();

                // task for find
                Action MainThreadFind = new Action(() => ftb.formatABCDE(strSplit));

                Action ExecuteProgressClock = new Action(() =>
                {
                    userClk.Visibility = Visibility.Visible;

                    // userClock clk = new userClock();
                    // Dispatcher.Invoke(new Action(() => intElement = MainGrid.Children.Add(clk)));

                });

                // final task
                Action FinalThreadDoWOrk = new Action(() =>
                {

                    userClk.Visibility = Visibility.Collapsed;
                    //Dispatcher.Invoke(new Action(() => MainGrid.Children.RemoveAt(intElement))); // Remove CLK

             //       mainWindow.ResizeMode = System.Windows.ResizeMode.CanResize;
                    ftb.totalBytes();

                    ftb.correctLocation();
                    ftb.scrollX.Value = 0;
                    ftb.scrollY.Value = ftb.intAbsolutCaretVertCurrent;
                    // if scroll value was changed - it automaticALY RUN UPDATE SCREEN

                    ftb.init_HorizontalScroll();
                    ftb.init_VerticalScroll();

                    ftb.blnAbort = false;

                    ftb.canvasMain.Focus();

                    Dispatcher.Invoke(new Action(() =>
                    {
                        MainGrid.IsEnabled = true;
                        statusDocSizeBar.Content = ftb.longTotalDocSize.ToString("0,0");

                    }));

                    ftb.scrollX.Value = ftb.intAbsolutCaretHorizCurrent;
                    ftb.scrollY.Value = ftb.intAbsolutCaretVertCurrent;

                    ftb.updateBlock();
                });

                Task MainThreadDoWorkTask = Task.Factory.StartNew(() => MainThreadFind());
                Task ExecuteProgressClkTask = new Task(ExecuteProgressClock);

                ExecuteProgressClkTask.RunSynchronously();
                MainThreadDoWorkTask.ContinueWith(t => FinalThreadDoWOrk(), uiThread);

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error", ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                Dispatcher.Invoke(new Action(() => MainGrid.IsEnabled = true));
            }
        }

        private void menuOpenAs_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".sql";
            ofd.Filter = " Text files |*.txt| SQL files |*.sql| CSV files |*.csv| XML files |*.xml| XAML files |*.xaml";

            string filePath = "";

            if (ofd.ShowDialog() == true)
            {
                filePath = ofd.FileName;
                string safeFilePath = ofd.SafeFileName;
            }else{
                return;
            }

            openFiles(filePath.Split(';'));
        }


        private void menuSaveAs_Click(object sender, RoutedEventArgs e)
        {

            string filename = "";
            try
            {

                string fileName = "";
                if (statusFile.Content != null) fileName = statusFile.Content.ToString();

                SaveFileDialog sfd = new SaveFileDialog();
                if (fileName != "")
                {
                    sfd.FileName = fileName;
                    sfd.InitialDirectory = System.IO.Path.GetDirectoryName(fileName);
                    sfd.RestoreDirectory = true;
                    sfd.DefaultExt = System.IO.Path.GetExtension(fileName);
                    sfd.Filter = " Current Extension |*" + sfd.DefaultExt;
                }
                else
                {
                    sfd.Filter = " Text files |*.txt| SQL files |*.sql| CSV files |*.csv| XML files |*.xml| XAML files |*.xaml";
                }

                userClk.Visibility = Visibility.Visible;

                Nullable<bool> result = sfd.ShowDialog();

                if (result == true)
                {
                    MainGrid.IsEnabled = false;
                    filename = sfd.FileName;
                }
                else
                {
                    userClk.Visibility = Visibility.Collapsed;
                    return;
                }

                mainSaveFile(filename);
            }
            catch (Exception ex)
            {
                ftb.blnError = true;
                Dispatcher.Invoke(new Action(() =>
                {

                    MainGrid.IsEnabled = true;
                    gridButtonLeft.IsEnabled = true;
                    gridButtonRight.IsEnabled = true;
                    userClk.Visibility = Visibility.Collapsed;
                }));
                MessageBox.Show("Not enough available memory (SaveDialog)...\n reduce file, try again, ...");
                return;
            }
        }

        private void mainSaveFile(string filename)
        {

            Dispatcher.Invoke(new Action(() =>
            {
                ftb.canvasMain.Focus();
                gridButtonLeft.IsEnabled = false;
                gridButtonRight.IsEnabled = false;

            }));

            TextWriter streamWriter = new StreamWriter(filename);

            if (ftb.blnError == true)
            {
                ftb.clear_ABCD();
            }

            try
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    statusFile.Content = "  " + filename;

                    foreach (StringBuilder sb in ftb.listSBmain)
                    {
                        streamWriter.WriteLine(sb.ToString());
                    }

                    Dispatcher.Invoke(new Action(() =>
                    {
                        gridButtonLeft.IsEnabled = true;
                        gridButtonRight.IsEnabled = true;

                        MainGrid.IsEnabled = true;
                        userClk.Visibility = Visibility.Collapsed;
                    }));

                    streamWriter.Flush();
                    streamWriter.Close();

                }));
            }
            catch (Exception ex)
            {
                throw ;
            }
        }




        private void menuExit_Click(object sender, RoutedEventArgs e)
        {
            // Configure the message box to be displayed 
            string messageBoxText = "Do you want to exit from AnotheEdit ...?\n";
            string caption = "Exit";
            MessageBoxButton button = MessageBoxButton.YesNoCancel;
            MessageBoxImage icon = MessageBoxImage.Warning;
            // Display message box
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

            // Process message box results 
            switch (result)
            {

                case MessageBoxResult.Yes:
                    Environment.Exit(0);
                    break;

                case MessageBoxResult.No:
                    break;

                case MessageBoxResult.Cancel:
                    break;
            }
        }


        private void mainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            // Configure the message box to be displayed 
            string messageBoxText = "Do you want to close AnotheEdit ...?\n";
            string caption = "Close";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Warning;
            // Display message box
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

            // Process message box results 
            switch (result)
            {

                case MessageBoxResult.Yes:
                    Environment.Exit(0);
                    break;

                case MessageBoxResult.No:
                    e.Cancel = true;
                    break;
            }
        }

        private void menuDonate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=KYGTH3Y3MF5SS");
            }
            catch
            {
                MessageBox.Show("Please use URL - https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=KYGTH3Y3MF5SS ");
            }

        }

        private void menuAbout_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("http://anotherpart.biz/AnotherEdit.aspx");
            }
            catch
            {
                MessageBox.Show("http://anotherpart.biz/AnotherEdit.aspx");
            }



        }

        private void menuHelpItems_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Please visit - http://anotherpart.biz/AnotherEdit.aspx");  // http://www.anotherpart.biz/AnotherEditSetup/publish.htm
        }

        private void btnSplit_Click(object sender, RoutedEventArgs e)
        {

            ftb.correctLocation();
            ftb.findABCDE();

            if (ftb.iLineStartABCDE == null)
            {
                MessageBox.Show("You do not select something ...");
                return;
            }

            int intTmpLength = ftb.listSBmain[(int)ftb.iLineStartABCDE].Length;

            if (ftb.iCharStartABCDE == intTmpLength)
            {
                MessageBox.Show("Please Select at least one symbol on first selected line ...");
                return;
            }

            string strSplit = "";
            string strFirsLineSelected = ftb.listSBmain[(int)ftb.iLineStartABCDE].ToString((int)ftb.iCharStartABCDE, 1); // first char from first line

            if (strFirsLineSelected == " ") strSplit = strFirsLineSelected;
            if (strFirsLineSelected == ",") strSplit = strFirsLineSelected;
            if (strFirsLineSelected == "[") strSplit = strFirsLineSelected;
            if (strFirsLineSelected == "]") strSplit = strFirsLineSelected;
            if (strFirsLineSelected == "}") strSplit = strFirsLineSelected;
            if (strFirsLineSelected == "{") strSplit = strFirsLineSelected;
            if (strFirsLineSelected == ")") strSplit = strFirsLineSelected;
            if (strFirsLineSelected == "(") strSplit = strFirsLineSelected;
            if (strFirsLineSelected == ":") strSplit = strFirsLineSelected;
            if (strFirsLineSelected == ";") strSplit = strFirsLineSelected;
            if (strFirsLineSelected == "?") strSplit = strFirsLineSelected;
            if (strFirsLineSelected == "!") strSplit = strFirsLineSelected;


            strFirsLineSelected = ftb.listSBmain[(int)ftb.iLineStartABCDE].ToString((int)ftb.iCharStartABCDE, intTmpLength - (int)ftb.iCharStartABCDE > 20 ? 20 : intTmpLength - (int)ftb.iCharStartABCDE);
            // Added space in first place ..
            strFirsLineSelected = (" " + strFirsLineSelected + " ").Replace(',', ' ').Replace('[', ' ').Replace(']', ' ').Replace('}', ' ').Replace('{', ' ').Replace(')', ' ').Replace('(', ' ').Replace(':', ' ').Replace(';', ' ').Replace('?', ' ').Replace('!', ' ').Replace('=', ' ').Replace('+', ' ').Replace('-', ' ').Replace('*', ' ').Replace('@', ' ');
            // Find first key word if split null
            if (strSplit == "")
            {
                if (ftb.keySQL == null) ftb.keySQL = new selectedSqlKeys(ftb.strConnection, ftb.strDataBaseName, ftb.blnODBCHighlight, ftb.blnFunctionHighlight);

                foreach (string str in ftb.keySQL.listSqlReserved)
                {
                    if (strFirsLineSelected.IndexOf(" " + str + " ") == 0)
                    {
                        strSplit = " " + str + " ";
                    }
                }
            }

            if (strSplit == "")
            {
                MessageBox.Show("First char is not a punctuation symbols ...\nOR\nFIRST WORD is not a Reserved Word ...");
                return;
            }

            taskFormatAllInParallelSelected(strSplit);

        }

        private void ftb_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {


            //treeContext.Visibility = Visibility.Visible;
            //nameTriStart.IsSelected = true;  // reset previous selectionMain

            //Point pnt = Mouse.GetPosition(canvasMain);

            //double myAH = treeContext.ActualHeight > 200 ? treeContext.ActualHeight : 200;
            //double myAW = treeContext.ActualWidth > 100 ? treeContext.ActualWidth : 100;

            //if (pnt.X <= canvasMain.ActualWidth / 2 && pnt.Y <= canvasMain.ActualHeight / 2) {
            //    tr.X = pnt.X;
            //    tr.Y = pnt.Y;
            //}
            //if (pnt.X <= canvasMain.ActualWidth / 2 && pnt.Y > canvasMain.ActualHeight / 2) {
            //    tr.X = pnt.X;
            //    tr.Y = pnt.Y - myAH - treeContext.Margin.Top / 2;
            //}
            //if (pnt.X > canvasMain.ActualWidth / 2 && pnt.Y <= canvasMain.ActualHeight / 2) {
            //    tr.X = pnt.X - myAW - treeContext.Margin.Left / 2;
            //    tr.Y = pnt.Y;
            //}
            //if (pnt.X > canvasMain.ActualWidth / 2 && pnt.Y > canvasMain.ActualHeight / 2) {
            //    tr.X = pnt.X - myAW - treeContext.Margin.Left / 2;
            //    tr.Y = pnt.Y - myAH - treeContext.Margin.Top / 2;
            //}


        }

        private void ftb_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //treeContext.Visibility = Visibility.Collapsed;
        }

        private void ftb_MouseUp(object sender, MouseButtonEventArgs e)
        {
            systemInfo();
        }

        private void statusSystem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ftb.totalBytes();
            systemInfo();
        }

        private void txtConnectionString_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (ftb != null)
            {
                ftb.strDataBaseName = comboDB.SelectedValue == null ? "" : comboDB.SelectedValue.ToString();
                ftb.strConnection = txtConnectionString.Text;
            }
        }

    }
}



// LOG ----------------------------------------------------------------------------------------------------------------





//TextRange allTextRange = new TextRange(myftb.Document.ContentStart, myftb.Document.ContentEnd);

//string allText = allTextRange.Text;

//// Create a FlowDocument to contain content for the RichTextBox. 
//        FlowDocument myFlowDoc = new FlowDocument(); 

//        // Add paragraphs to the FlowDocument. 
//        myFlowDoc.Blocks.Add(new Paragraph(new Run("Paragraph 1"))); 
//        myFlowDoc.Blocks.Add(new Paragraph(new Run("Paragraph 2"))); 
//        myFlowDoc.Blocks.Add(new Paragraph(new Run("Paragraph 3"))); 
//        RichTextBox myRichTextBox = new RichTextBox(); 

//        // Add initial content to the RichTextBox. 
//        myRichTextBox.Document = myFlowDoc; 

// http://msdn.microsoft.com/en-us/library/system.windows.forms.richtextbox.aspx


//Like this ?
// private void richTextBox1_MouseUp(object sender, MouseEventArgs e_in)
//{
//if (e_in.Button == MouseButtons.Right)
//{
//richTextBox1.SelectionStart = richTextBox1.GetCharIndexFromPosition(e_in.Location);
//richTextBox1.SelectionLength = 0;
//label1.Text = Convert.ToString(richTextBox1.SelectionStart); 

//}
//}





