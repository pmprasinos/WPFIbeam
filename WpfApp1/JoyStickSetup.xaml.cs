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
    /// Interaction logic for JoyStickSetup.xaml
    /// </summary>
    public partial class JoyStickSetup : Window
    {
        public JoyStickSetup()
        {
            InitializeComponent();
            Application curApp = Application.Current;
            Window mainWindow = curApp.MainWindow;
            this.Left = mainWindow.Left + (mainWindow.Width - this.ActualWidth-400) / 2;
            this.Top = mainWindow.Top + (mainWindow.Height - this.ActualHeight-600) / 2;

            this.Activate();
        }


        private void OnDeactivated(object sender, EventArgs e)
        {
            Close();
            return;
        }
    }
}
