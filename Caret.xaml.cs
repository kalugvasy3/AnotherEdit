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
    /// Interaction logic for Caret.xaml
    /// </summary>
    public partial class Caret : UserControl 
    {
        public Caret() {
            InitializeComponent();
        }

        private double dw;
        private double dh;
        private Color clr;

        private int i_NumLine; // absolute Line
        private int i_NumChar; // absolute Char


        public int iNumLine {
            get {
                return i_NumLine;
            }
            set {
                i_NumLine = value;
            }
        }

        public int iNumChar {
            get {
                return i_NumChar;
            }
            set {
                i_NumChar = value;
            }
        }


        public double careteW {
            get {
                return dw;
            }
            set {
                dw = value;
                myCaret.Width = dw;
            }
        }

        public double careteH {
            get {
                return dh;
            }
            set {
                dh = value;
                myCaret.Height = dh;
            }
        }


        public Color careteClr {
            get {
                return clr;
            }
            set {
                clr = value;
                myCaret.Fill = new SolidColorBrush(clr);
            }
        }


        public Caret(double dW, double dH, Color clR) {
            myCaret.Width = dW;
            myCaret.Height = dH;
            myCaret.Fill = new SolidColorBrush(clR);

            dw = dW;
            dh = dH;
            clr = clR;

        }
    }
}





//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Windows.Shapes;
//using System.Windows.Media.Animation;

//namespace AnotherEdit
//{
//    public partial class Caret : UserControl
//    {
//        private Storyboard myStoryboard;
//        private Rectangle rec;

//        public Caret() {

//            StackPanel myPanel = new StackPanel();
//            myPanel.Margin = new Thickness(10);

//            rec = new Rectangle();

//            rec.Name = "rec";
//       //     this.RegisterName(rec.Name, rec);
//            rec.Width = 2;
//            rec.Height = 13;
//            rec.Fill = Brushes.Blue;

//            DoubleAnimation myDoubleAnimation = new DoubleAnimation();
//            myDoubleAnimation.From = 1.0;
//            myDoubleAnimation.To = 0.0;
//            myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.3));
//            myDoubleAnimation.AutoReverse = true;
//            myDoubleAnimation.RepeatBehavior = RepeatBehavior.Forever;

//            myStoryboard = new Storyboard();
//            myStoryboard.Children.Add(myDoubleAnimation);
//            Storyboard.SetTargetName(myDoubleAnimation, rec.Name);
//            Storyboard.SetTargetProperty(myDoubleAnimation, new PropertyPath(Rectangle.OpacityProperty));

//            // Use the Loaded event to start the Storyboard.
//            rec.Loaded += new RoutedEventHandler(myRectangleLoaded);
//            myPanel.Children.Add(rec);
//          //  this.Content = myPanel;
//        }

//        private void myRectangleLoaded(object sender, RoutedEventArgs e_in) {
//            myStoryboard.Begin(this);
//        }



//    }
//}

