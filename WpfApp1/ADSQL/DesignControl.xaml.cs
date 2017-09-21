using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Data;
using System.Data.SqlClient;
using System.Data;
using System.ComponentModel;
//using Windows.Foundation;
//using Windows.Foundation.Collections;
//using Windows.UI.Xaml;
//using Windows.UI.Xaml.Controls;
//using Windows.UI.Xaml.Controls.Primitives;
//using Windows.UI.Xaml.Data;
//using Windows.UI.Xaml.Input;
//using Windows.UI.Xaml.Media;
//using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CustomControl
{
     public  partial class AxisControl : UserControl
    {
        public UpdateSourceTrigger UpdateSourceTrigger { get; set; }

        public static SqlConnection MomCon = new SqlConnection("data source = CONSOLE1; initial catalog = MomSQL; user id = pprasinos; password = Wyman123-; MultipleActiveResultSets = True; App = EntityFramework");
        public string CmdString;
        
        public AxisControl()
        {
            this.InitializeComponent();
            //DataContext = this;
            this.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
        }

        public void Refresh()
        {
            if (this.AxisNumber == -1) return;

            using (SqlConnection con = new SqlConnection("data source = CONSOLE1; initial catalog = MomSQL; user id = pprasinos; password = Wyman123-; MultipleActiveResultSets = True; App = EntityFramework"))
            {
                
                con.Open();
                CmdString = "exec getaxisdata @AxisNumber = " + this.AxisNumber.ToString();
                SqlCommand cmd = new SqlCommand(CmdString, con);
                cmd.CommandType = CommandType.StoredProcedure;
                //SqlDataAdapter sda = new SqlDataAdapter(cmd);
                //DataTable dt = new DataTable("Employee");
                //cmd.CommandTimeout = 1000;
                //sda.Fill(dt);
                //this.ItemsSource = dt.DefaultView;
                cmd = new SqlCommand(CmdString, con);
                SqlDataReader sdr = cmd.ExecuteReader();
                sdr.Read();

                do
                {
                    string s = sdr["CurrentPosition"].ToString().ToUpper();
                    //string v = sdr["Value"].ToString();
                   this.CurrentPosition = int.Parse(sdr["CurrentPosition"].ToString());
                    this.TargetPosition = int.Parse(sdr["TargetPosition"].ToString());
                    this.Acceleration = int.Parse(sdr["Acceleration"].ToString());
                    this.Deceleration = int.Parse(sdr["Deceleration"].ToString());
                    this.AxisName =sdr["AxisName"].ToString();
                    this.LiveValues = (bool)sdr["LiveValues"];

                    //this.SetBinding(AxisNameProperty, sdr["AxisName"].ToString());
                    //  if (sdr["Property"].ToString().ToUpper() == "TARGETPOSITION") this.SetValue(TargetPositionProperty, int.Parse(sdr["Value"].ToString()));
                    if (this.LiveValues) this.AxisStatusTextBox.Text = "Free Run"; else this.AxisStatusTextBox.Text = "Virtual Mode";

                    if (!this.AxisNameTextBox.IsKeyboardFocused) this.AxisNameTextBox.Text = this.AxisName;
                   if(!this.CurrentPositionTextBox.IsKeyboardFocused) this.CurrentPositionTextBox.Text = this.CurrentPosition.ToString();
                   if(!this.TargetPositionTextBox.IsKeyboardFocused) this.TargetPositionTextBox.Text = this.TargetPosition.ToString();
                    //  if (sdr["Property"].ToString().ToUpper() == "VELOCITY") this.SetValue(VelocityProperty, int.Parse(sdr["Value"].ToString()));
                    //   if (sdr["Property"].ToString().ToUpper() == "DECELERATION") this.SetValue(DecelerationProperty, int.Parse(sdr["Value"].ToString()));
                    //  if (sdr["Property"].ToString().ToUpper() == "AXISNAME") this.SetValue(AxisNameProperty, int.Parse(sdr["Value"].ToString()));
                    //  this.CurrentPositionTextBox.Text = this.GetValue(CurrentPositionProperty).ToString();
                    // this.TargetPositionTextBox.Text = this.GetValue(TargetPositionProperty).ToString();
                   // this.TargetPositionTextBox.BindingGroup.UpdateSources();
                    
                
                    //this.AxisNameTextBox.Text = this.GetValue(AxisNameProperty).ToString();
                }
                while (sdr.Read() && sdr.HasRows == true);
                con.Close();

            }
        }

        public string Details
        {
            get
            {
                return String.Format("{0} was born on {1} and this is a long description of the person.", this.AxisName, this.TargetPosition);
            }
        }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value);  }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable),
                typeof(AxisControl), new PropertyMetadata(null));

        public string AxisName
    {
        get { return (string)GetValue(AxisNameProperty); }
        set { SetValue(AxisNameProperty, value); }
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty AxisNameProperty =
        DependencyProperty.Register("AxisName", typeof(string), typeof(AxisControl), new PropertyMetadata(default(string)));

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this,
                    new PropertyChangedEventArgs(propertyName));
            }
        }

        public int AxisNumber
        {
            get { return (int)GetValue(AxisNumberProperty); }
            set { SetValue(AxisNumberProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AxisNumber.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AxisNumberProperty =
            DependencyProperty.Register("AxisNumber", typeof(int), typeof(AxisControl), new PropertyMetadata(0));



        public int CurrentPosition
        {
            get { return (int)GetValue(CurrentPositionProperty); }
            set { SetValue(CurrentPositionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentPosition.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentPositionProperty =
            DependencyProperty.Register("CurrentPosition", typeof(int), typeof(AxisControl), new PropertyMetadata(0));




        public int TargetPosition
        {
            get { return (int)GetValue(TargetPositionProperty); }
            set { SetValue(TargetPositionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TargetPosition.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TargetPositionProperty =
            DependencyProperty.Register("TargetPosition", typeof(int), typeof(AxisControl), new PropertyMetadata(0));




        public int Acceleration
        {
            get { return (int)GetValue(AccelerationProperty); }
            set { ADSQL.SqlWriteAxis(AxisNumber, "ACCELERATION", value);  SetValue(AccelerationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Acceleration.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AccelerationProperty =
            DependencyProperty.Register("Acceleration", typeof(int), typeof(AxisControl), new PropertyMetadata(0));




        public int Deceleration
        {
            get { return (int)GetValue(DecelerationProperty); }
            set { ADSQL.SqlWriteAxis(AxisNumber, "DECELERATION", value); SetValue(DecelerationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Deceleration.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DecelerationProperty =
            DependencyProperty.Register("Deceleration", typeof(int), typeof(AxisControl), new PropertyMetadata(0));




        public int Velocity
        {
            get { return (int)GetValue(VelocityProperty); }
            set { ADSQL.SqlWriteAxis(AxisNumber, "VELOCITY", value); SetValue(VelocityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Velocity.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VelocityProperty =
            DependencyProperty.Register("Velocity", typeof(int), typeof(AxisControl), new PropertyMetadata(0));



        public int JSOffset
        {
            get { return (int)GetValue(JSOffsetProperty); }
            set { SetValue(JSOffsetProperty, value); }
        }

        // Using a DependencyProperty as the backing store for JSOffset.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty JSOffsetProperty =
            DependencyProperty.Register("JSOffset", typeof(int), typeof(AxisControl), new PropertyMetadata(0));




        public int TextSize
        {
            get { return (int)GetValue(TextSizeProperty); }
            set { SetValue(TextSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextSizeProperty =
            DependencyProperty.Register("TextSize", typeof(int), typeof(AxisControl), new PropertyMetadata(0));





        public bool LiveValues
        {
            get { return (bool)GetValue(LiveValuesProperty); }
            set { SetValue(LiveValuesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LiveValues.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LiveValuesProperty =
            DependencyProperty.Register("LiveValues", typeof(bool), typeof(AxisControl));


        //public string ConString
        //{
        //    get { return (string)GetValue(ConStringProperty); }
        //    set { SetValue(ConStringProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty ConStringProperty =
        //    DependencyProperty.Register("MyProperty", typeof(string), typeof(AxisControl), new PropertyMetadata(0));





    }



}




