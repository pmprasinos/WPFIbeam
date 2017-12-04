﻿using System;
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
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Diagnostics;
// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CustomControl
{
    public partial class AxisControl : System.Windows.Controls.UserControl
    {
        //public UpdateSourceTrigger UpdateSourceTrigger { get; set; }
        protected bool isDragging;
        private Point clickPosition;
        public bool queControl;
        public string CmdString;
        public bool HasKeyBoardFocus = false;
        public Point DraggedPosition = new Point(0, 0);
        public bool AllowDrag = false;
        public string SelectedQue;
        public int AssignedJoyStick;

        public AxisControl()
        { this.InitializeComponent();
            this.AssignedJoyStick = -1;
           
            //DataContext = this;
            //this.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            //this.ShadeRectangle_Selected.Visibility = Visibility.Hidden;
            this.MouseLeftButtonDown += new MouseButtonEventHandler(Control_MouseLeftButtonDown);
            this.MouseLeftButtonUp += new MouseButtonEventHandler(Control_MouseLeftButtonUp);
            this.MouseMove += new System.Windows.Input.MouseEventHandler(Control_MouseMove);
            this.IsSelected = false;

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
            if (sender == draggable) draggable.ReleaseMouseCapture();

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
            set { SetValue(TargetPositionProperty, value); }
        }

       //  Using a DependencyProperty as the backing store for TargetPosition.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TargetPositionProperty =
            DependencyProperty.Register("TargetPosition", typeof(int), typeof(AxisControl), new PropertyMetadata(0));




        public int Acceleration
        {
            get { return (int)GetValue(AccelerationProperty); }
            set {  SetValue(AccelerationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Acceleration.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AccelerationProperty =
            DependencyProperty.Register("Acceleration", typeof(int), typeof(AxisControl), new PropertyMetadata(0));




        public int Deceleration
        {
            get { return (int)GetValue(DecelerationProperty); }
            set {  SetValue(DecelerationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Deceleration.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DecelerationProperty =
            DependencyProperty.Register("Deceleration", typeof(int), typeof(AxisControl), new PropertyMetadata(0));




        public int Velocity
        {
            get { return (int)GetValue(VelocityProperty); }
            set {  SetValue(VelocityProperty, value); }
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


        // Using a DependencyProperty as the backing store for LiveValues.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(AxisControl));

        public bool IsSelected
        {
            get { return bool.Parse(GetValue(IsSelectedProperty).ToString()); }
            set { SetValue(IsSelectedProperty, value);
                if (!IsSelected)
                {
                    this.ShadeRectangle_Selected.Visibility = Visibility.Hidden;
                    this.RemoveButton.Visibility = Visibility.Hidden;
                } else
                {
                    this.ShadeRectangle_Selected.Visibility = Visibility.Visible;
                    this.RemoveButton.Visibility = Visibility.Visible;
                    if (queControl) this.RemoveButton.Content = "Remove From Cue"; else this.RemoveButton.Content = "Remove From Selection";
                }
            }
            }

        public static readonly DependencyProperty IsActiveProperty =
         DependencyProperty.Register("IsActive", typeof(bool), typeof(AxisControl));

        public bool IsActive
        {
            get { return bool.Parse(GetValue(IsActiveProperty).ToString()); }
            set
            {
                bool y = bool.Parse(value.ToString());
                try { 
                using (SqlConnection MomCon = new SqlConnection("data source = MOM0\\MOMSQL;  Connection Timeout=10; initial catalog = MomSQL; user id = pprasinos; password = Wyman123-; MultipleActiveResultSets = True; App = EntityFramework"))
                {
                    SqlCommand cmd = new SqlCommand("UPDATE MOMSQL..AXIS SET IsActive = 0 where AxisName = @AxisName", MomCon);
                    if (y) cmd.CommandText = "UPDATE MOMSQL..AXIS SET IsActive = 1 where AxisName = @AxisName";
                        cmd.CommandTimeout = 500;
                    cmd.Parameters.AddWithValue("@AxisName", this.AxisNameTextBox.Text);
                    MomCon.Open();
                        SetValue(IsActiveProperty, bool.Parse(value.ToString()));
                        cmd.ExecuteNonQuery();
                    MomCon.Close();
                }
                } catch
                {
                    SetValue(IsActiveProperty, false);
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
           // this.HasKeyBoardFocus = false;
            System.Windows.Controls.TextBox t = (System.Windows.Controls.TextBox)sender;

            if (SelectedQue != "" && SelectedQue != null)
            {
             //   ADSQL.SqlWriteAxis(this.AxisNumber, t.Tag.ToString(), t.Text);
            }
            else
            {
               // ADSQL.SqlWriteAxis(this.AxisNumber, t.Tag.ToString(), t.Text);
            }


        }

        private void AxisControlTextBox_LostKeyboardFocus(object sender, RoutedEventArgs e)
        {
          //  this.HasKeyBoardFocus = false;
            System.Windows.Controls.TextBox t = (System.Windows.Controls.TextBox)sender;
           
            if(SelectedQue != "" && SelectedQue !=null)
            {
              //  ADSQL.SqlWriteAxis(this.AxisNumber, t.Tag.ToString(), t.Text);
            }
            else
            {
               // ADSQL.SqlWriteAxis(this.AxisNumber, t.Tag.ToString(), t.Text);
            }               
           
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
            WpfApp1.JoyStickSelecter js = new WpfApp1.JoyStickSelecter();
            if (!queControl)
            {
                this.IsSelected = true;
                js.ShowDialog();
                this.AssignedJoyStick = js.SelectedJoyStick;

            }
            if (isDragging) return;
        }


        public void PullSelection()
        {
            using (SqlConnection MomCon = new SqlConnection("data source = MOM0\\MOMSQL;  Connection Timeout=10; initial catalog = MomSQL; user id = pprasinos; password = Wyman123-; MultipleActiveResultSets = True; App = EntityFramework"))
            {
                try
                {
                    using (SqlCommand CMD = new SqlCommand("Update momsql..axis set isselected = 1 where AxisName = @AxisName and IsSelected = 1", MomCon))
                    {
                        CMD.CommandTimeout = 500;
                        CMD.Parameters.AddWithValue("@AxisName", this.AxisNameTextBox.Text);
                        MomCon.Open();
                        this.IsSelected = CMD.ExecuteNonQuery() > 0;
                        MomCon.Close();
                    }
                }catch
                {
                    this.IsSelected = false;
                }
                
            }
        }
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {

            if (this.queControl && this.SelectedQue.ToString() != "")
            {
                System.Windows.MessageBoxResult g = MessageBox.Show("Are you sure you want to remove " + this.AxisNameTextBox.Text + " from the que: " + this.SelectedQue.ToString() + "?", "Remove Axis target from Que", MessageBoxButton.OKCancel);
                if (g != MessageBoxResult.OK) return;
                using (SqlConnection MomCon = new SqlConnection("data source = MOM0\\MOMSQL;  Connection Timeout=10; initial catalog = MomSQL; user id = pprasinos; password = Wyman123-; MultipleActiveResultSets = True; App = EntityFramework"))
                {
                    using (SqlCommand CMD = new SqlCommand("Delete from momsql..Ques where AxisNum =@AxisNum and QueName = @QueName", MomCon))
                    {
                        CMD.CommandTimeout = 500;
                        CMD.Parameters.AddWithValue("@AxisNum", this.AxisNumber);
                        CMD.Parameters.AddWithValue("@QueName", this.SelectedQue);
                        MomCon.Open();
                        CMD.ExecuteNonQuery();
                        MomCon.Close();
                        this.ShadeRectangle_Selected.Visibility = Visibility.Hidden;
                    }
                }
            }
            else { this.IsSelected = false; this.ShadeRectangle_Selected.Visibility = Visibility.Hidden; }
        }

        private void AxisTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            System.Windows.Controls.TextBox t = (System.Windows.Controls.TextBox)sender;
            if (e.Key == Key.Enter )
            {
                if (SelectedQue != "" && SelectedQue != null)
                {            
                }
                else
                {
                    //ADSQL.SqlWriteAxis(this.AxisNumber, t.Tag.ToString(), t.Text);
                }
                this.RemoveButton.Focus();
            }
        }
    }
}








