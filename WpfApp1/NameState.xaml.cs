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
    /// Interaction logic for Window1.xaml
    /// </summary>
    //
    public partial class NameStateDialog : Window
    {
        public string NotesResult;
        public string NameResult;
        public bool SaveClicked;
        public int JoyStickSelected = 1;

        public NameStateDialog()
        {
            InitializeComponent();
            Application curApp = Application.Current;
            Window mainWindow = curApp.MainWindow;
            this.Left = mainWindow.Left + (mainWindow.Width - this.ActualWidth - 400) / 2;
            this.Top = mainWindow.Top + (mainWindow.Height - this.ActualHeight - 600) / 2;
            this.Topmost = true;
            TextBoxName.SelectAll();
        }

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            SaveClicked = true;
            NotesResult = TextBoxNotes.Text;
            NameResult = TextBoxName.Text;
            this.Hide();
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            SaveClicked = false;
            this.Close();
        }

        private void RadioButton_Clicked(object sender, RoutedEventArgs e)
        {
            RadioButton[] rbs = { RB0, RB1, RB2, RB3 };
            RadioButton rb = sender as RadioButton;
         
                int y = int.Parse(rb.Tag.ToString());
                foreach (RadioButton r in rbs) { r.IsChecked = false; r.IsThreeState = false; }
                rbs[y].IsChecked = true;
                JoyStickSelectLabel.Content = "Joystick_" + y.ToString();
            JoyStickSelected = y;
           
        }
    }
}
