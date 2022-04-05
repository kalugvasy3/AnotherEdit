using System;
using System.Collections.Generic;
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

using System.Threading;
using System.Threading.Tasks;

using System.Data;

namespace AnotherEdit
{
    /// <summary>
    /// Interaction logic for selectedTablesColumns.xaml
    /// </summary>
    public partial class selectedTablesColumns : Window
    {
        public selectedTablesColumns() {
            InitializeComponent();
            userClk.Visibility = Visibility.Collapsed;
        }


        private DataBaseCommon db = new DataBaseCommon();

        public void runSCHEMA(string strDB, string strConnectionString) {

          //  int? intElement = null;
            List<StringBuilder> sbRun = new List<StringBuilder>();
            strDB = strDB.ToUpper();
            string strError = "";

            if (strDB.IndexOf("SQLCLIENT") >= 0 || strConnectionString.IndexOf("SQL") >= 0)
            {
                sbRun.Add(new StringBuilder(" SELECT UPPER(TABLE_NAME) TABLE_NAME  FROM INFORMATION_SCHEMA.TABLES ORDER BY TABLE_NAME; "));
                sbRun.Add(new StringBuilder(" SELECT UPPER(TABLE_NAME) TABLE_NAME, UPPER(COLUMN_NAME) AS COLUMN_NAME  FROM INFORMATION_SCHEMA.COLUMNS ORDER BY TABLE_NAME; "));
                sbRun.Add(new StringBuilder(" SELECT UPPER(TABLE_NAME) + '.' + UPPER(COLUMN_NAME)  AS FULL_NAME FROM INFORMATION_SCHEMA.COLUMNS  ORDER BY TABLE_NAME; "));
                sbRun.Add(new StringBuilder(" SELECT * FROM INFORMATION_SCHEMA.COLUMNS;"));
                sbRun.Add(new StringBuilder(" SELECT * FROM SYS.PROCEDURES ORDER BY CREATE_DATE DESC ;"));
                sbRun.Add(new StringBuilder(" SELECT * FROM SYS.VIEWS ORDER BY CREATE_DATE DESC ;"));
 
            }
            else if (strDB.IndexOf("IBM") >= 0 || strConnectionString.IndexOf("DB2") >= 0)
            {
                // find Data

                int intStart =  strConnectionString.ToUpper().IndexOf("DATABASE");
                int intStop = strConnectionString.IndexOf(";", intStart);

                string strDataBase = strConnectionString.Substring(intStart + 8,intStop-(intStart + 8)).Replace("=","").Trim();

                sbRun.Add(new StringBuilder(" SELECT TABSCHEMA || '.' || UPPER(TABNAME)  TABLE_NAME  FROM SYSCAT.TABLES WHERE OWNERTYPE='U' ;"));
                sbRun.Add(new StringBuilder(" SELECT tT.TABSCHEMA || '.' || tC.TABNAME AS TABLE_NAME,tC.COLNAME AS COLUMN_NAME FROM SYSCAT.COLUMNS AS tC INNER JOIN SYSCAT.TABLES AS tT ON tT.TABNAME = tc.TABNAME AND tT.OWNERTYPE='U';"));
                sbRun.Add(new StringBuilder(" SELECT tT.TABSCHEMA || '.' || tC.TABNAME || '.' || tC.COLNAME AS FULL_NAME FROM SYSCAT.COLUMNS AS tC INNER JOIN SYSCAT.TABLES AS tT ON tT.TABNAME = tc.TABNAME AND tT.OWNERTYPE='U';"));
                sbRun.Add(new StringBuilder(" SELECT tC.* FROM SYSCAT.COLUMNS AS tC INNER JOIN SYSCAT.TABLES AS tT ON tT.TABNAME = tc.TABNAME AND tT.OWNERTYPE='U'; "));
                sbRun.Add(new StringBuilder(" SELECT ROUTINENAME, TEXT  FROM SYSCAT.ROUTINES WHERE OWNERTYPE = 'U' AND ROUTINESCHEMA = '" + strDataBase + "' ; "));  
                sbRun.Add(new StringBuilder(" SELECT VIEWNAME, TEXT  FROM SYSCAT.VIEWS   WHERE OWNERTYPE = 'U' ;"));
                sbRun.Add(new StringBuilder(" SELECT FUNCSCHEMA, FUNCNAME ,IMPLEMENTATION FROM SYSCAT.FUNCTIONS WHERE ORIGIN = 'U' ;"));
                sbRun.Add(new StringBuilder(" SELECT TRIGSCHEMA, TRIGNAME, TABSCHEMA, TABNAME, TEXT FROM SYSCAT.TRIGGERS WHERE OWNERTYPE = 'U' ;"));

            }

            else if (strDB.IndexOf("COMPACT") >= 0 )
            {
               // find Data for COMPACT

            }
            else if (strDB.IndexOf("ORA") >= 0)
            {
                // find Data for ORACLE

            }

            else if (strDB.IndexOf("MYSQL") >= 0)
            {
                // find Data for MYSQL

            }

            else return;


            try {

                int intEffected = 0;

                DataSet ds = new DataSet();

                TextBlock txtException = new TextBlock();
                txtException.TextWrapping = TextWrapping.Wrap;
                TextBlock txtSQL = new TextBlock();
                txtSQL.TextWrapping = TextWrapping.Wrap;

                Dispatcher.Invoke(new Action(() =>userClk.Visibility = Visibility.Visible)); 
                Thread.Sleep(500);

                TaskScheduler uiThread = TaskScheduler.FromCurrentSynchronizationContext();

                // DataSet
                Action MainThreadLoadSQL = new Action(() => {
                         
                            try {

                                if (sbRun.Count == 0) {
                                    Dispatcher.Invoke(new Action(() => txtException.Text = "No SQL Codes / SQL  is empty."));
                                    ds = null;
                                }
                                else {
                                    int iTable = 0;
                                    foreach (StringBuilder sb in sbRun) 
                                    {
                                        try
                                        {
                                            DataTable dt = new DataTable();
                                            dt = db.GetDataSet(sb, strDB, strConnectionString, out strError, out intEffected).Tables[0];
                                            dt.TableName = iTable.ToString();
                                            ds.Tables.Add(dt.Copy());
                                            iTable++;
                                        }
                                        catch (Exception ex) 
                                        {
                                            String str = ex.Message;
                                        }
                                        
                                    }
                                    
                                }
                            }
                            catch (Exception ex) {
                                Dispatcher.Invoke(new Action(() => {
                                    txtException.Text = ex.Message + "\n\r" + ex.InnerException.Message;
                                    txtException.Focus();
                                }));

                            }

                   
                    });
             



                // Grid
                Action FinalThreadDoWOrk = new Action(() => {
                    //   if (intElement != null) Dispatcher.Invoke(new Action(() => gridResult.Children.RemoveAt((int)intElement)));  // It is Clock

                    Dispatcher.Invoke(new Action(() => userClk.Visibility = Visibility.Collapsed));

                    if (txtException.Text == "") {

                        for (int i = 0; i < ds.Tables.Count; i++) {
                            DataTable dt = ds.Tables[i];
                            TabItem ti = new TabItem();
                            if (i == 0) ti.Header = "TABLES";
                            if (i == 1) ti.Header = "TABLE/COLUMN NAME";
                            if (i == 2) ti.Header = "FULL COLUMN NAME";
                            if (i == 3) ti.Header = "FULL TABLES INFORMATION";
                            if (i == 4) ti.Header = "PROCEDURES";
                            if (i == 5) ti.Header = "VIEWS";
                            if (i == 6) ti.Header = "FUNCTION";
                            if (i == 7) ti.Header = "TRIGGERS";

                            DataGrid dG = new DataGrid();
                            dG.FontFamily = new FontFamily("Consolas");
                            dG.AutoGenerateColumns = true;
                            dG.ItemsSource = dt.DefaultView;
                            ti.Content = dG;

                            tabMain.Items.Add(ti);
                        }

                    }
                    else {
                        //TabItem ti = new TabItem();
                        //ti.Header = "Exception";
                        //ti.Content = txtException;
                        gridResult.Children.Add(txtException);

                    }

                });

                // Clock
                //Action ExecuteProgressClock = new Action(() => {

                //    Dispatcher.Invoke(new Action(() => userClk.Visibility = Visibility.Visible));

                //    //userClock clk = new userClock();
                //    //Dispatcher.Invoke(new Action(() => intElement = gridResult.Children.Add(clk)));
                //});



                Task MainThreadDoWorkTask = Task.Factory.StartNew(() => MainThreadLoadSQL());

                //Task ExecuteProgressClkTask = new Task(ExecuteProgressClock);
                //ExecuteProgressClkTask.RunSynchronously();

                MainThreadDoWorkTask.ContinueWith(t => FinalThreadDoWOrk(), uiThread);

            }
            catch (Exception ex) {
                System.Windows.MessageBox.Show("Error", ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MainWindow_Unloaded(object sender, RoutedEventArgs e) {
            try {
            //    db.connToDB.Close();
                db = null;
            }
            catch {
            }

        }



        // foreach (var data in dataGridMain.SelectedItems)
        //{
        // MyObservableCollection myData = data as MyObservableCollection;
        // MessageBox.Show(myData.author);
        //}

        //private void btnProcessMedia_Click(object sender, RoutedEventArgs e) {
        //    if (dgProjects.SelectedItems.Count > 0) {
        //        for (int i = 0; i < dgProjects.SelectedItems.Count; i++) {
        //            System.Data.DataRowView selectedFile = (System.Data.DataRowView)dgProjects.SelectedItems[i];
        //            string str = Convert.ToString(selectedFile.Row.ItemArray[10]);
        //        }
        //    }

        //}

    }
}
