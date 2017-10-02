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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
namespace CustomControl
{
    /// <summary>
    /// Interaction logic for QueControl.xaml
    /// </summary>
    public partial class QueControl : System.Windows.Controls.UserControl
    {
        public int QueNumber;
        public QueControl()
        {
            this.InitializeComponent();

        }

        public int TextSize
        {
            get { return (int)GetValue(TextSizeProperty); }
            set { SetValue(TextSizeProperty, value); }
        }
        
        // Using a DependencyProperty as the backing store for TextSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextSizeProperty =
            DependencyProperty.Register("TextSize", typeof(int), typeof(QueControl), new PropertyMetadata(0));

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            //if (this.SelectedQue.ToString() != "")
            //{
            //    System.Windows.MessageBoxResult g = MessageBox.Show("Are you sure you want to remove " + this.AxisNameTextBox.Text + " from the que: " + this.SelectedQue.ToString() + "?", "Remove Axis target from Que", MessageBoxButton.OKCancel);
            //    if (g != MessageBoxResult.OK) return;
            //}
            //else
            //{
            //    this.IsActive = false; this.ShadeRectangle_Selected.Visibility = Visibility.Hidden;
            //}


        }
    



    public bool LiveValues
        {
            get { return bool.Parse(GetValue(LiveValuesProperty).ToString()); }
            set { SetValue(LiveValuesProperty, value); }
        }
        public static readonly DependencyProperty LiveValuesProperty =
          DependencyProperty.Register("LiveValues", typeof(bool), typeof(QueControl));

        // Using a DependencyProperty as the backing store for LiveValues.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(QueControl));

        public bool IsSelected
        {
            get { return bool.Parse(GetValue(IsSelectedProperty).ToString()); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly DependencyProperty IsActiveProperty =
         DependencyProperty.Register("IsActive", typeof(bool), typeof(QueControl));

        public bool IsActive
        {
            get { return bool.Parse(GetValue(IsActiveProperty).ToString()); }
            set
            {
                bool y = bool.Parse(value.ToString());
                SetValue(IsActiveProperty, bool.Parse(value.ToString()));
                using (SqlConnection MomCon = new SqlConnection("data source = MOM0\\MOMSQL; initial catalog = MomSQL; user id = pprasinos; password = Wyman123-; MultipleActiveResultSets = True; App = EntityFramework"))
                {
                    SqlCommand cmd = new SqlCommand("UPDATE MOMSQL..AXIS SET IsActive = 0 where AxisName = @AxisName", MomCon);
                    if (value) cmd.CommandText = "UPDATE MOMSQL..AXIS SET IsActive = 1 where AxisName = @AxisName";
                  
                    MomCon.Open();
                    cmd.ExecuteNonQuery();
                    MomCon.Close();
                }
            }
        }
        // Using a DependencyProperty as the backing store for LiveValues.  This enables animation, styling, binding, etc...

    }
}
