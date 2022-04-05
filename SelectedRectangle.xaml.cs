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

namespace AnotherEdit
{
    /// <summary>
    /// Interaction logic for selectedRectangle.xaml
    /// </summary>
    /// 
    
    public partial class selectedRectangle : UserControl
    {
        
        public selectedRectangle() {
            
            InitializeComponent();
        }
 
        public double recW {
            get {
                return myRec.Width;
            }
            set {
                myRec.Width = value;
            }
        }

        public double recH {
            get {
                return myRec.Height;
            }
            set {
                myRec.Height = value;
            }
        }


        public Brush recBrush {
            get {
                return myRec.Fill ;
            }
            set {
                myRec.Fill = value;
            }
        }


        public double recX {
            get {
                return tr.X;
            }
            set {
                tr.X = value;
            }
        }

        public double recY {
            get {
                return tr.Y;
            }
            set {
                tr.Y = value;
            }
        }

        
        public selectedRectangle(double dW, double dH,double X, double Y ,Brush br) {
            myRec.Width = dW;
            myRec.Height = dH;
            myRec.Fill = br;
            
            tr.X = X;
            tr.Y = Y;

            myRec.Fill = br;
        }


    }
}
