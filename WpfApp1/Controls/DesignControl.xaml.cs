using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
//using System.Windows.Controls;
using System.Windows;
using System.Windows.Data;
using System.Data.SqlClient;
using System.Data;
using System.ComponentModel;

using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Diagnostics;
// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CustomControl
{
    public partial class AxisControl : System.Windows.Controls.UserControl
    {
        public UpdateSourceTrigger UpdateSourceTrigger { get; set; }
        protected bool isDragging;
        private Point clickPosition;
      
        public string CmdString;
        public bool HasKeyBoardFocus = false;
        public Point DraggedPosition = new Point(0, 0);
        public bool AllowDrag = false;
        public string SelectedQue;

        public AxisControl()
        {
            this.InitializeComponent();
            //DataContext = this;
            this.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            //this.ShadeRectangle_Selected.Visibility = Visibility.Hidden;
            this.MouseLeftButtonDown += new MouseButtonEventHandler(Control_MouseLeftButtonDown);
            this.MouseLeftButtonUp += new MouseButtonEventHandler(Control_MouseLeftButtonUp);
            this.MouseMove += new System.Windows.Input.MouseEventHandler(Control_MouseMove);
            

          //  for (int i = 0; i < this.Controls.Count; i++)
          //  {
          //      Controls[i].Click += Item_Click;
          //  }
        }

        public static List<T> GetLogicalChildCollection<T>(object parent) where T : DependencyObject
        {
            List<T> logicalCollection = new List<T>();
            GetLogicalChildCollection(parent as DependencyObject, logicalCollection);
            return logicalCollection;
        }

        private static void GetLogicalChildCollection<T>(DependencyObject parent, List<T> logicalCollection) where T : DependencyObject
        {
            IEnumerable children = LogicalTreeHelper.GetChildren(parent);
            foreach (object child in children)
            {
                if (child is DependencyObject)
                {
                    DependencyObject depChild = child as DependencyObject;
                    if (child is T)
                    {
                        logicalCollection.Add(child as T);
                    }
                    GetLogicalChildCollection(depChild, logicalCollection);
                }
            }
        }

        private void Control_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

            isDragging = true;
            var draggableControl = sender as System.Windows.Controls.UserControl;
            
            clickPosition = e.GetPosition(this.Parent as UIElement);
            if (sender == draggableControl)
            {
                draggableControl.CaptureMouse();
            }
        }
    

        private void Control_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
        
            isDragging = false;
            var draggable = sender as System.Windows.Controls.UserControl;
            if(sender == draggable) draggable.ReleaseMouseCapture();

            DraggedPosition = new Point(0, 0);

          
          
     
            CheckClick(sender, e);
            
        }

        private void Control_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var draggableControl = sender as System.Windows.Controls.UserControl;

            if (isDragging && draggableControl != null && AllowDrag)
            {
                Point currentPosition = e.GetPosition(this.Parent as UIElement);
                DraggedPosition = currentPosition;
                var transform = draggableControl.RenderTransform as System.Windows.Media.TranslateTransform;
                if (transform == null)
                {
                    transform = new System.Windows.Media.TranslateTransform();
                    draggableControl.RenderTransform = transform;
                }

                transform.X = currentPosition.X - clickPosition.X;
                transform.Y = currentPosition.Y - clickPosition.Y;
            }
        }
     
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
            set { SetValue(AxisNumberProperty, int.Parse(value.ToString())); }
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
            set { ADSQL.SqlWriteAxis(AxisNumber, "TARGETPOSITION", value); SetValue(TargetPositionProperty, value);   }
        }

        // Using a DependencyProperty as the backing store for TargetPosition.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TargetPositionProperty =
            DependencyProperty.Register("TargetPosition", typeof(int), typeof(AxisControl), new PropertyMetadata(0));




        public int Acceleration
        {
            get { return (int)GetValue(AccelerationProperty); }
            set { ADSQL.SqlWriteAxis(AxisNumber, "ACCELERATION", value); SetValue(AccelerationProperty, value); }
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
            get { return bool.Parse(GetValue(LiveValuesProperty).ToString()); }
            set { SetValue(LiveValuesProperty, value); }
        }
        public static readonly DependencyProperty LiveValuesProperty =
          DependencyProperty.Register("LiveValues", typeof(bool), typeof(AxisControl));

        // Using a DependencyProperty as the backing store for LiveValues.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(AxisControl));

        public bool IsSelected
        {
            get { return bool.Parse(GetValue(IsSelectedProperty).ToString()); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly DependencyProperty IsActiveProperty =
         DependencyProperty.Register("IsActive", typeof(bool), typeof(AxisControl));

        public bool IsActive
        {
            get { return bool.Parse(GetValue(IsActiveProperty).ToString()); }
            set {
                bool y = bool.Parse(value.ToString());
                SetValue(IsActiveProperty, bool.Parse(value.ToString()));
                using (SqlConnection MomCon = new SqlConnection("data source = MOM0\\MOMSQL; initial catalog = MomSQL; user id = pprasinos; password = Wyman123-; MultipleActiveResultSets = True; App = EntityFramework"))
                {
                    SqlCommand cmd = new SqlCommand("UPDATE MOMSQL..AXIS SET IsActive = 0 where AxisName = @AxisName", MomCon);
                    if (value) cmd.CommandText = "UPDATE MOMSQL..AXIS SET IsActive = 1 where AxisName = @AxisName";
                    cmd.Parameters.AddWithValue("@AxisName", this.AxisNameTextBox.Text);
                    MomCon.Open();
                    cmd.ExecuteNonQuery();
                    MomCon.Close(); 
                }
            }
        }
        // Using a DependencyProperty as the backing store for LiveValues.  This enables animation, styling, binding, etc...

            

        private void AxisControlTextBox_GotKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            this.HasKeyBoardFocus = true;
        }

        private void AxisControlTextBox_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            this.HasKeyBoardFocus = false;
  
        }

        private void AxisControlTextBox_LostKeyboardFocus(object sender, RoutedEventArgs e)
        {
            this.HasKeyBoardFocus = false;         
        }

        System.Diagnostics.Stopwatch ClickTimer = System.Diagnostics.Stopwatch.StartNew();
        private void AxisControl_Clicked(object sender, System.Windows.Input.TouchEventArgs e)
        {
            CheckClick(sender, (EventArgs)e);

        }

        private void AxisControl_Clicked(object sender, EventArgs e)
        {
            CheckClick(sender, (EventArgs)e);
            
        }

        private void CheckClick(object Sender, EventArgs e)
        {
            if (isDragging) return;
            using (SqlConnection MomCon = new SqlConnection("data source = MOM0\\MOMSQL; initial catalog = MomSQL; user id = pprasinos; password = Wyman123-; MultipleActiveResultSets = True; App = EntityFramework"))
            {
                if (ClickTimer.ElapsedMilliseconds > 10 && ClickTimer.ElapsedMilliseconds < 350)
                { this.IsActive = false;                }
                else                {                this.IsActive = true;                }
                ClickTimer = System.Diagnostics.Stopwatch.StartNew();
                Debug.Print(Sender.ToString());
                Debug.Print(e.ToString());
            }
        }
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedQue.ToString() != "")
            {
                System.Windows.MessageBoxResult g = MessageBox.Show("Are you sure you want to remove " + this.AxisNameTextBox.Text + " from the que: " + this.SelectedQue.ToString() + "?", "Remove Axis target from Que", MessageBoxButton.OKCancel);
                if (g != MessageBoxResult.OK) return;
            }else
            {
                this.IsActive = false; this.ShadeRectangle_Selected.Visibility = Visibility.Hidden;
            }
           
               
            }
        }
    }









