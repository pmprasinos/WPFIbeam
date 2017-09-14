using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
//using System.Windows.Input;
//using System.Windows.Media;

namespace _treeListView
{
    public class SpecialTextBox : TextBox
    {
        public static readonly DependencyProperty AxisNum = DependencyProperty.RegisterAttached("AxisNumber", typeof(string), typeof(SpecialTextBox));
        public static readonly DependencyProperty UserEdit = DependencyProperty.RegisterAttached("UserCanEdit", typeof(bool), typeof(SpecialTextBox));

        public string AxisNumber
        {
            get { return (string)this.GetValue(AxisNum); }
            set { this.SetValue(AxisNum, value); }
        }
        public bool UserCanEdit
        {
            get { return (bool)this.GetValue(AxisNum); }
            set { this.SetValue(UserEdit, value); }

        }


        public SpecialTextBox()
        {
        
        }

    
    }

}