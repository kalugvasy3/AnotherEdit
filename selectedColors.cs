using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Controls;

namespace AnotherEdit
{
    class selectedColors : TextBlock
  {
        private string str_Color;

        public string strColor {
            get {
                return str_Color;
            }
            set {
                str_Color = value;
            }
        }

 
        public selectedColors() {
            str_Color = "Black";
        }

    }
}


