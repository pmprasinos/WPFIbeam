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
using System.Data;

namespace CustomControl
{
    /// <summary>
    /// Interaction logic for QueControl.xaml
    /// </summary>

    public partial class QueControl : System.Windows.Controls.UserControl
    {
        public DataTable dtItemSource = new DataTable();
        public int AxisIndex;
        public int QueNumber;
        public bool IsAxis;
       
        public QueControl()
        {
            this.InitializeComponent();
            this.SetValue(QueNameProperty, ("").ToString());
            //this.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
        }

        public int AxisQuantity
        {
            get { return (int)GetValue(AxisQuantityProperty); }
            set { SetValue(AxisQuantityProperty, value); }
        }
        
        // Using a DependencyProperty as the backing store for TextSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AxisQuantityProperty =
            DependencyProperty.Register("AxisQuantity", typeof(int), typeof(QueControl), new PropertyMetadata(0));

     

        public double TrimFactor
        {
            get { return (double)GetValue(TrimFactorProperty); }
            set { SetValue(TrimFactorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TrimFactor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TrimFactorProperty =
            DependencyProperty.Register("TrimFactor", typeof(double), typeof(QueControl), new PropertyMetadata(default(double)));



        public string QueNotes
        {
            get { return (string)GetValue(QueNotesProperty); }
            set { SetValue(QueNotesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for QueNotes.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty QueNotesProperty =
            DependencyProperty.Register("QueNotes", typeof(string), typeof(QueControl), new PropertyMetadata(default(string)));



        public string QueName
        {
            get { return GetValue(QueNameProperty).ToString(); }
            set
            {   if (GetValue(QueNameProperty).ToString() == value.ToString() || value.ToString().Length<3 ) return;
                SetValue(QueNameProperty, value);
                if (value.ToString().Contains("iBeamHoist")) IsAxis = true;
                    using (SqlConnection MomCon = new SqlConnection("data source = MOM0\\MOMSQL; initial catalog = MomSQL; user id = pprasinos; password = Wyman123-; MultipleActiveResultSets = True; App = EntityFramework"))
                    {
                        string cmdstr = "MOMSQL.dbo.GetQueData";
                    if (this.IsAxis) cmdstr = "Select AxisName as QueName, AxisType as QueNotes, 1 as TrimFactor, '1' as AxisQuantity, 'LOAD' as Load from momsql..axis where AxisName = @QueName";
                        using (SqlCommand CMD = new SqlCommand(cmdstr, MomCon))
                        {
                            CMD.Parameters.AddWithValue("@QueName", this.QueName);
                            MomCon.Open();
                        if (!this.IsAxis) CMD.CommandType = CommandType.StoredProcedure;
                           using (SqlDataReader sdr= CMD.ExecuteReader())
                            {
                               if(sdr.HasRows)
                            {
                                sdr.Read();

                                QueNameTextBox.Text = sdr["QueName"].ToString();
                                NotesTextBox.Text = sdr["QueNotes"].ToString();
                                AxisQuantityTextBox.Text = sdr["AxisQuantity"].ToString();
                                this.IsActive = true;
                            }
                            else { QueNameTextBox.Clear(); NotesTextBox.Clear(); AxisQuantityTextBox.Clear(); TrimFactorTextBox.Clear(); this.IsActive = false; }
                                                             
                           }
                           MomCon.Close();
                    }
                    }

                
              
            }
        }
        // Using a DependencyProperty as the backing store for StateName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty QueNameProperty =
            DependencyProperty.Register("QueName", typeof(string), typeof(QueControl), new PropertyMetadata(default(string)));




      

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
            get { return bool.Parse(GetValue(IsActiveProperty).ToString());
    }
    set
            {
                SetValue(IsActiveProperty, bool.Parse(value.ToString())); 
            }
        }

        private void RemoveButton_Clicked(object sender, RoutedEventArgs e)
        {
            ClearQueControl();
         
        }
        public void ClearQueControl()
        {
            this.QueName = ""; this.TrimFactor = 0; QueNameTextBox.Clear(); NotesTextBox.Clear(); AxisQuantityTextBox.Clear(); TrimFactorTextBox.Clear(); this.IsActive = false; this.IsAxis = false;
            this.QueNotes = "CLEAR";
        }
        // Using a DependencyProperty as the backing store for LiveValues.  This enables animation, styling, binding, etc...

    }
}
