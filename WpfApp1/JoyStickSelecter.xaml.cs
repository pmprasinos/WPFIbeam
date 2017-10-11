using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for JoyStickSelecter.xaml
    /// </summary>
    public partial class JoyStickSelecter : Window
    {
        public int SelectedJoyStick;

        public JoyStickSelecter()
        {
            InitializeComponent();
        }

        private void RadioButtonjs_Clicked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            SelectedJoyStick = int.Parse(rb.Tag.ToString());
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void JSSelector_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
