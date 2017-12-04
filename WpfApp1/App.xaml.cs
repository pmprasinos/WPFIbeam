using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public bool DoHandle = false;
        private void DispatcherUnhandeledEx(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            if (this.DoHandle)
            {
                //Handling the exception within the UnhandledException handler.
             //   MessageBox.Show(e.Exception.Message, "Exception Caught",
              //                          MessageBoxButton.OK, MessageBoxImage.Error);
                e.Handled = true;
            }
            else
            {
                //If you do not set e.Handled to true, the application will close due to crash.
               // MessageBox.Show("Application is going to close! ", "Uncaught Exception");
                e.Handled = false;
                MainWindow.Close();
            }
        }


    }
}
