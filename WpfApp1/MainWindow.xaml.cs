using System;
using System.Collections.Generic;
//using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Runtime.InteropServices;
//using System.Windows.Documents;
using System.Windows.Input;

using System.Data.SqlClient;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Windows.Shapes;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using devDept.Eyeshot.Triangulation;
using devDept.Eyeshot.Translators;
using devDept.Eyeshot.Labels;
using System.IO;
using System.Diagnostics;
//using System.Drawing;
using System.Data;
using System.Windows.Media;
using System.Threading;
//using System.Configuration;
using CustomControl;

namespace WpfApp1
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        internal struct LASTINPUTINFO
        {
            public uint cbSize;

            public uint dwTime;
        }
        [DllImport("User32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        public static int IdleTime() //In seconds
        {
            LASTINPUTINFO lastinputinfo = new LASTINPUTINFO();
            lastinputinfo.cbSize = (uint)Marshal.SizeOf(lastinputinfo);
            GetLastInputInfo(ref lastinputinfo);
            return (((Environment.TickCount & int.MaxValue) - ((int)lastinputinfo.dwTime & int.MaxValue)) & int.MaxValue) / 1000;
        }

        //Task BackTask;
        int tickCounter = 0;
        string MomConStr = "data source = MOM0\\MOMSQL; initial catalog = MomSQL; MultipleActiveResultSets = True; user id = pprasinos; password = Wyman123-;";
        DataTable dt = new DataTable();
        private Vector3D[][] IBeamTrans = new Vector3D[3][];
        private double[] KidPositions = new double[6];
        private const string EYESHOT_SERIAL = "ULTWPF-94GF-N1277-FNLR3-1PPHF";
        static System.Windows.Forms.Timer t = new System.Windows.Forms.Timer(); static System.Windows.Forms.Timer StatesGridTimer = new System.Windows.Forms.Timer();
        bool disabled = false; bool SerialConnected = true;
        double[] AxisSpeed = { 0, 0, 0, 0, 0, 0 };
        // QueControl[] JoyStickDisp;

        int LiveDisplay = 1; //1=Live mode, 2 = Virtual Mode, 3 = Fault Active, 4 = Estop Active
        Point4D[] JoySticks = new Point4D[4];
        static System.IO.Ports.SerialPort sp = new System.IO.Ports.SerialPort();
        SerialRemote sr;
        ToolTip TT = new System.Windows.Controls.ToolTip();
        int loopCount = 0;
      //  private System.Drawing.Point _mouseLocation;
        private int SelectedLayer = -1;
        bool ModeSelDisabled; bool t_Busy = false;
        bool UserInTextBox = false;
        AxisControl[] axControl; QueControl[] qControl;

        public MainWindow() : base()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandeledEx);
            InitializeComponent();
            ErrorIcon.Visibility = Visibility.Hidden;
            viewPortLayout1.Unlock(EYESHOT_SERIAL);

            if (!UserInTextBox) sr = null;
            //  JoyStickDisp[4] = { QueControl_1, QueControl_2, QueControl_3, QueControl_4};
            for (int i = 0; i < 100; i++) if (i < JoySticks.Length) JoySticks[i] = new Point4D(0, 0, 0, 0);
            axControl = new AxisControl[6];
            for (int i = 0; i < 6; i++)
            {
                AxisControl AX = new AxisControl();
                AX.Name = "AX" + (i + 1).ToString();
                System.Windows.Controls.Grid.SetRow(AX, i / 4);
                System.Windows.Controls.Grid.SetColumn(AX, i % 4);
                AxisGrid.Children.Add(AX);
                axControl[i] = AX;
            }
            qControl = new QueControl[4];
            for (int i = 0; i < 4; i++)
            {
                QueControl QC = new QueControl();
                QC.Name = "QC" + (i + 1).ToString();
                System.Windows.Controls.Grid.SetRow(QC, 4);
                System.Windows.Controls.Grid.SetColumn(QC, i * 2);
                JoyStickGrid.Children.Add(QC);
                qControl[i] = QC;
            }

            viewPortLayout1.ToolBar.Visible = false;            //JoySticks[0].X = 20;
            QuesGrid_Update(QueGrid_Selection);
           // qControl[0].QueName = "Top Position";
        }

        private void PopulateJoystickSelections(bool IsAxis, int JoyStickAssignment, string h)
        {
            if (IsAxis)
            {         

                for (int axs = 0; axs < 6; axs++)
                {
                    KidPositions[axs] = KidPositions[axs] + AxisSpeed[axs];

                    if (axControl[axs].AssignedJoyStick != -1 )
                    {
                       
                        qControl[axControl[axs].AssignedJoyStick].IsAxis = true; qControl[axControl[axs].AssignedJoyStick].QueName = axControl[axs].AxisNameTextBox.Text;
                        qControl[axControl[axs].AssignedJoyStick].AxisIndex = axs; qControl[axControl[axs].AssignedJoyStick].IsActive = true;
                        // ADSQL.SqlWriteAxis(axControl[axs].AssignedJoyStick, "ISACTIVE", 1);
                        if (qControl[axControl[axs].AssignedJoyStick].QueNotes == "CLEAR") {
                            qControl[axControl[axs].AssignedJoyStick].QueNotes = ""; qControl[axControl[axs].AssignedJoyStick].QueName = "";  qControl[axControl[axs].AssignedJoyStick].IsActive = false; qControl[axControl[axs].AssignedJoyStick].AxisIndex = -1; axControl[axs].AssignedJoyStick = -1; axControl[axs].IsSelected = false;  }


                    }
        
                }
            }
            else
            {
                qControl[JoyStickAssignment].IsAxis = false; qControl[JoyStickAssignment].QueName = h;
                qControl[JoyStickAssignment].AxisIndex = -1;
                qControl[JoyStickAssignment].IsActive = true;
            }
        }

        private void Joystick_Setup(object sender, RoutedEventArgs e)
        {
            JoyStickSetup js = new JoyStickSetup();
            js.ShowDialog();
        }

        private void StatesGridTimer_tick(object myObject, EventArgs e)
        {
            // UpdateSQL.Refresh();
            int x = 0;
            foreach (AxisControl AX in axControl)
            {
                AX.queControl = (bool)QueMode.IsChecked;
                if (!(bool)QueMode.IsChecked && AX.AssignedJoyStick != -1)
                {
                    if(qControl[AX.AssignedJoyStick].QueName != AX.AxisNameTextBox.Text )qControl[AX.AssignedJoyStick].QueName = AX.AxisNameTextBox.Text;
                }
                if (UpdateSQL.ADT.Rows.Count > x)
                {
                    if (!AX.HasKeyBoardFocus)
                    {
                        AX.DataContext = UpdateSQL.ADT.Rows[x];
                        AX.UpdateLayout();
                        AX.AxisNumber = (int)UpdateSQL.ADT.Rows[x].ItemArray[1];
                    }
                    else
                    {
                        Debug.Print("EAT SHIT");
                    }
                }
                x++;
            }
            Task k = Task.Run(() => UpdateSQL.Refresh());
        }

        private void T_tick(object myObject, EventArgs e)
        {
          

           Stopwatch s = Stopwatch.StartNew();
         //   try
         //   {
                t.Interval = 60;
                if (disabled || t_Busy) { return; }
                t_Busy = true;
                tickCounter++;

                // t.Stop();
              //  ConnectMom();
                ConnectSerial();
              
                if (sr.DeadmanRightPressed || sr.DeadmanLeftPressed) ADSQL.SqlWriteAxis(1, "LiveValues", true);
                for (int x = 0; x < 6; x++)
                {
                    KidPositions[x] = int.Parse(ADSQL.SqlReadAxis(x+1, "CurrentPosition").ToString());
                }

            ADSQL.SqlWriteAxis(1, "LiveValues", 1);
                for (int x = 0; x < 4; x++)
                {
                 
                    if (sr !=null)if ((sr.DeadmanRightPressed || sr.DeadmanLeftPressed) && qControl[x].IsActive && sr.w[x]==1)
                    {
                            ADSQL.ExecuteQue(qControl[x].QueName, Math.Abs(int.Parse(sr.y[x].ToString())+100) ); TextBlock1.Text = sr.y[x].ToString();
                    }
                    else if(sr != null) if ((sr.DeadmanRightPressed || sr.DeadmanLeftPressed) && qControl[x].QueNameTextBox.Text.Contains("iBeamHoist"))
                    {
                              //  if (sr.DeadmanRightPressed || sr.DeadmanLeftPressed) ADSQL.SqlWriteAxis(1, "isActive", true);
                                TextBlock1.Text = "JS: " + JoySticks[x].Y.ToString() + "  " + JoySticks[x].X.ToString() + "  " + JoySticks[x].Z.ToString();
                                Debug.Print("AXIS: " + qControl[x].AxisIndex.ToString());
                                Debug.Print("JOYSTICK:  " +(3 * JoySticks[x].Y).ToString());
                                Debug.Print("POSITION: " +KidPositions[qControl[x].AxisIndex].ToString());
                               Debug.Print("TARGET: " + int.Parse(Math.Round((5 * JoySticks[x].Y) + KidPositions[qControl[x].AxisIndex]).ToString()).ToString());
                         ADSQL.SqlWriteAxis(qControl[x].AxisIndex + 1, "TargetPosition", int.Parse(Math.Round(5 * JoySticks[x].Y).ToString()) + KidPositions[qControl[x].AxisIndex]);
                    }
                  
                }
                UpdateCad(SelectedLayer);

                for (int i = 0; i < JoySticks.Length; i++)
                {
                    if (sp.IsOpen) { JoySticks[i].X = sr.x[i]; JoySticks[i].Y = sr.y[i]; JoySticks[i].Z = sr.z[i]; JoySticks[i].W = sr.w[i]; }
                }
               
                loopCount++;
                loopCount = loopCount % 100;
                if (loopCount % 2 == 1) ModeSelect();
                if (loopCount == 99) QuesGrid_Update(QueGrid_Selection);
                if (LiveDisplay != 2)
                {
                    
                       // if (Mom != null && sr != null && loopCount % 10 == 3) if (sr.DeadmanRightPressed || sr.DeadmanLeftPressed) Mom.WriteValue(Mom.DeadMan, true, "DEADMAN"); else if (loopCount % 10 == 3) Mom.WriteValue(Mom.DeadMan, false, "DEADMAN");
                   // if (Mom != null && sr != null) if (loopCount % 50 == 49 && Mom.Connected) Mom.WriteValue(Mom.MomControl, true, "MOMCONTROL");
                }
                t_Busy = false;
           // }
            //catch  {  }
           // finally { t_Busy = false; }
        }

   

        private void MaximizeToSecondaryMonitor(Window window)
        {
            System.Drawing.Point p = new System.Drawing.Point(-1000, 0);
            var secondaryScreen = System.Windows.Forms.Screen.FromPoint(p);
            if (secondaryScreen != null)
            {
                if (!window.IsLoaded)
                    window.WindowStartupLocation = WindowStartupLocation.Manual;
                var workingArea = secondaryScreen.WorkingArea;
                window.Left = workingArea.Left;
                window.Top = workingArea.Top;
                window.Width = workingArea.Width;
                window.Height = workingArea.Height;
                // If window isn't loaded then maxmizing will result in the window displaying on the primary monitor
                if (window.IsLoaded)
                {
                    window.WindowState = WindowState.Maximized;
                }
            }
        }

        private void FullScreen(object sender, RoutedEventArgs e)
        {
            //loadSTL();
            // BackTask = Task.Run(() => {  ConnectMom(); });
            MaximizeToSecondaryMonitor(this);
            viewPortLayout1.Layers.Add(new Layer("0")); viewPortLayout1.Layers.Add(new Layer("1")); viewPortLayout1.Layers.Add(new Layer("2")); viewPortLayout1.Layers.Add(new Layer("3"));
            OpenCADFile();
            t = new System.Windows.Forms.Timer();
            t.Interval = 2000; StatesGridTimer.Interval = 100; StatesGridTimer.Start();
            t.Start();
            t.Tick += new EventHandler(T_tick); StatesGridTimer.Tick += new EventHandler(StatesGridTimer_tick);
            JoyStick_Activate();
            UpdateCad(2);
            QuesGrid_Update(QueGrid_Selection);
        }

        private void OpenCADFile()
        {
            if (disabled) return;
            ReadFileAsynch rfa;
            rfa = new ReadSTL(new MemoryStream(Properties.Resources.iBeam), false);                    //    break;
            rfa.Unlock(EYESHOT_SERIAL);
            rfa.LoadingText = "iBeam";
            viewPortLayout1.DoWork(rfa);
            rfa.AddToSceneAsSingleObject(viewPortLayout1, "iBeamParent", 0);
        }

        private void Edit_State_Clicked(object sender, RoutedEventArgs e)
        {
            MoveSetup d = new MoveSetup();
            d.ShowDialog();
        }

        private void ViewPortLayout1_OnLoad(object sender, RoutedEventArgs e)
        {
            if (disabled) return;
            for (int i = 0; i < 3; i++)
            {
                Entity E = (Entity)viewPortLayout1.Blocks["iBeamParent"].Entities[0].Clone();
                // Entity P = (Entity)viewPortLayout1.Blocks["PulleyParent"].Entities[0].Clone();

                E.Color = System.Drawing.Color.Black;
                viewPortLayout1.Entities.Add(E, i + 1);
                viewPortLayout1.Entities[(i * 2) + 1].Color = System.Drawing.Color.Black;
                viewPortLayout1.Entities[(i * 2) + 1].Visible = true;
                IBeamTrans[i] = new Vector3D[2];
                IBeamTrans[i][0] = new Vector3D(0, 0, 0);
                IBeamTrans[i][1] = new Vector3D(0, 0, 0);

            }
            UpdateCad(0); UpdateCad(1); UpdateCad(2);
            UpdateCad(0); UpdateCad(1); UpdateCad(2);
            viewPortLayout1.SetView(viewType.Trimetric, true, viewPortLayout1.AnimateCamera);
            viewPortLayout1.Invalidate();
            viewPortLayout1.ZoomFit();
        }

        private void UpdateCad(int BeamIndex)
        {            
            Vector3D d = new Vector3D(0, 0, 1);
            viewPortLayout1.Blocks["iBeamParent"].Entities[0].Visible = false;
            Entity e0 = (Entity)viewPortLayout1.Blocks["iBeamParent"].Entities[0].Clone();
            for (int k = 1; k < viewPortLayout1.Entities.Count; k++) if (viewPortLayout1.Entities[k].LayerIndex != 0) viewPortLayout1.Entities.RemoveAt(k);
            for (int i = 0; i < 3; i++)
            {
                KidPositions[(i * 2)] = int.Parse(ADSQL.SqlReadAxis((i * 2) + 1, "CurrentPosition").ToString());
                KidPositions[(i * 2) + 1] = int.Parse(ADSQL.SqlReadAxis((i * 2) + 2, "CurrentPosition").ToString());
                Entity e = (Entity)e0.Clone();
                //  Entity P = (Entity)P0.Clone();
                e.Rotate(1.5708, new Vector3D(1, 0, 0));
                if (e.BoxMax != null) e.Rotate(System.Math.Tan((KidPositions[i * 2] - KidPositions[(i * 2) + 1]) / (120 * 25.3)), new Vector3D(0, 1), boxCenter(e));
                e.Translate(0, 1000 * i, 0);
                e.Color = System.Drawing.Color.Black;
                e.Translate(0, 0, (KidPositions[i * 2] + KidPositions[i * 2 + 1]) / 2);
                viewPortLayout1.Entities.Add(e, i + 1);

                if (SelectedLayer != -1) PopulateAxisInfo((SelectedLayer * 2) - 1, false);
                if (SelectedLayer != -1) PopulateAxisInfo(SelectedLayer * 2, false);

                viewPortLayout1.Invalidate();
            }
            foreach (Entity en in viewPortLayout1.Entities)
            {
                if (en.LayerIndex != 0) en.Visible = true;
                if (en.LayerIndex == SelectedLayer) en.Selected = true;
            }
        }

        private Point3D boxCenter(Entity en)
        {
            Point3D result = new Point3D((en.BoxMax.X + en.BoxMin.X) / 2, (en.BoxMax.Y + en.BoxMin.Y) / 2, (en.BoxMax.Z + en.BoxMin.Z) / 2);
            return result;
        }


        private void viewportLayout1_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            //viewPortLayout1.Entities.ClearSelection();
            //_mouseLocation = devDept.Graphics.RenderContextUtility.ConvertPoint(e.GetPosition(viewPortLayout1));
            //viewPortLayout1.ProcessSelection(new System.Drawing.Rectangle(_mouseLocation.X - 10, _mouseLocation.Y - 10, 20, 20), true, false, new ViewportLayout.SelectionChangedEventArgs());
            //viewPortLayout1.UpdateVisibleSelection();
            //viewPortLayout1.Entities.SetSelectionAsCurrent();
            //foreach (Entity en in viewPortLayout1.Entities)
            //{
            //    if (en.Selected && SelectedLayer != en.LayerIndex)
            //    {
            //        QueGrid_Selection = -1;
            //        this.SelectedQueTextBox.Text = "Unknown";
            //      //  SelectedLayer = en.LayerIndex;
            //      //  PopulateAxisInfo((SelectedLayer * 2) - 1, true);
            //      //  foreach (AxisControl ax in axControl) ax.IsActive = false;
                   
            //        PopulateAxisInfo(SelectedLayer * 2, false);
            //    }
            //}

        }

        private void viewportLayout1_SelectionChanged(object sender, ViewportLayout.SelectionChangedEventArgs e)
        {
            //if (viewPortLayout1.Entities.CurrentBlockReference == null)
            //{
            //    // If the user clicks on a BlockReference then we want to set it as current.
            //    int currentEntity = viewPortLayout1.GetEntityUnderMouseCursor(_mouseLocation);
            //    if (currentEntity != -1)
            //    {
            //        viewPortLayout1.Entities.SetCurrent(viewPortLayout1.Entities[currentEntity] as BlockReference);
            //        if (viewPortLayout1.Entities.CurrentBlockReference != null)
            //            viewPortLayout1.Entities.CurrentBlockReference.Selected = true;
            //    }
            //}
            //else
            //{
            //    // Set the BlockReference as selected.
            //    // viewPortLayout1.Entities.CurrentBlockReference.Selected = true;
            //    // Selection();
            //    viewPortLayout1.Invalidate();
            //}
            //JoyStick_Activate();
        }



        private void JoyStick_Activate()
        {
            if (SelectedLayer != -1)
            {
                JS1.Source = ImageFilter.SetJoyStick(true, true, false, false); JS1.Opacity = 100;
                JS2.Source = ImageFilter.SetJoyStick(true, true, false, false); JS2.Opacity = 20;
            }

            // JS3.Source = ImageFilter.SetJoyStick(false, true, true, true); JS3.Opacity = 20;
            // JS4.Source = ImageFilter.SetJoyStick(false, true, true, true); JS4.Opacity = 20;
        }

     
    

        private void PopulateAxisInfo(int AxisNum, bool clear = false)
        {
            // AxisControl_1.AxisNumber = 1 + ((SelectedLayer - 1) * 2);
            //AxisControl_2.AxisNumber = 2 + ((SelectedLayer - 1) * 2);
            //AxisControl_1.Visibility = Visibility.Visible;
            //AxisControl_2.Visibility = Visibility.Visible;

        }

        private void QuesGrid_Update(int SelectedIndex)
        {
            this.QueGrid.SelectionChanged -= QueGrid_SelectionChanged;
            QueGrid.BeginEdit();
            bool WasFocused = QueGrid.IsFocused;
            using (SqlConnection MomCon = new SqlConnection(MomConStr))
            {
                int j = QueGrid.Items.Count;
                using (SqlCommand acmd = new SqlCommand("Exec momsql.dbo.GetQueData", MomCon))
                {
                    acmd.CommandTimeout = 50;
                    dt.Clear();
                    MomCon.Open();
                    dt.Load(acmd.ExecuteReader());
                    this.Dispatcher.Invoke(() => QueGrid.ItemsSource = dt.DefaultView);
                    if (j == QueGrid.Items.Count) this.Dispatcher.Invoke(() => { QueGrid.SelectedIndex = SelectedIndex; if (WasFocused) QueGrid.Focus(); });
                    MomCon.Close();
                }
            }
            this.QueGrid.SelectionChanged += QueGrid_SelectionChanged;
        }

        private void StatesGrid_PullSelected(int SelectedIndex)
        {
            this.QueGrid.SelectionChanged -= QueGrid_SelectionChanged;

            int j = QueGrid.Items.Count;
            try
            {
                using (SqlConnection MomCon = new SqlConnection(MomConStr))
                {
                    if (SelectedIndex != -1)
                    {
                        DataRowView g = (DataRowView)QueGrid.Items[SelectedIndex];
                        string h = g.Row.ItemArray[0].ToString();
                        int JoyStickAssignment = int.Parse(g.Row.ItemArray[2].ToString());
                        using (SqlCommand cmd = new SqlCommand("momsql..PullAxisTargets", MomCon))
                        {
                            cmd.Parameters.AddWithValue("@QueName", h);
                            cmd.CommandType = CommandType.StoredProcedure;
                            MomCon.Open();
                            cmd.ExecuteNonQuery();
                            MomCon.Close();
                        }
                        foreach (AxisControl AX in axControl)
                        {
                            // this.Dispatcher.Invoke(() => AX.TargetPositionTextBox.Text = AX.TargetPosition.ToString());
                            this.Dispatcher.Invoke(() => AX.queControl = (bool)this.QueMode.IsChecked);
                            this.Dispatcher.Invoke(() => AX.PullSelection());
                        }

                        if (JoyStickAssignment != -1)
                        {
                            this.Dispatcher.Invoke(() => SelectedQueTextBox.Text = h);
                            Thread.Sleep(1);
                            this.Dispatcher.Invoke(() => PopulateJoystickSelections(!(bool)QueMode.IsChecked, JoyStickAssignment-1, SelectedQueTextBox.Text));
                        }

                    }
                }
            }
            catch { }
            finally { this.QueGrid.SelectionChanged += QueGrid_SelectionChanged; }
        }


        private void UnhandeledEx(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            MessageBox.Show(e.ToString(), "Unhandled Exception");
        }


        private void closed(object sender, RoutedEventArgs e)
        {
          
                Application.Current.Shutdown();
        }



        private void ResetSaftey(object sender, RoutedEventArgs e)
        {
            ADSQL.SqlWriteAxis(1, "AxisStatus", "RESET");
            VirtualModeButton.Opacity = 20;
            SafteyResetButton.Visibility = Visibility.Hidden;
            ModeLabel.Content = "Resetting Saftey System...";
            LiveDisplay = 1;
            loopCount = 80;
        }

        private void EnableVirtualMode(object sender, RoutedEventArgs e)
        {
            ModeSelDisabled = true;
            if ((string)VirtualModeButton.Content == "Enable Live Mode")
            {
                do
                {
                    MessageBoxResult c = MessageBox.Show("Console E-Stop must be released to enter Live Mode", "Enter Live Mode", MessageBoxButton.OKCancel);
                    if (c == MessageBoxResult.Cancel) { ModeSelDisabled = false; return; }
                } while (sr.EstopPressed);
                LiveDisplay = 1;
            }
            else
            {
                do
                {
                    MessageBoxResult c = MessageBox.Show("Console E-Stop must be active to enter Simulation Mode", "Enter Simulation", MessageBoxButton.OKCancel);
                    ModeSelDisabled = false;
                    if (c == MessageBoxResult.Cancel) { ModeSelDisabled = false; return; }
                } while (!sr.EstopPressed);
                LiveDisplay = 2;
            }

            ModeSelDisabled = false;
            ModeSelect();
        }

        private void ErrorIcon_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            ReConnect();
        }

        private void ReConnect()
        {
            SerialConnected = true;
        }

        private void ErrorIcon_MouseEnter(object sender, MouseEventArgs e)
        {
            SerialConnected = true;
            Thread.Sleep(t.Interval);
            TT.Content = ErrorIcon.ToolTip;
            TT.IsOpen = true;
        }

        private void ErrorIcon_MouseLeave(object sender, MouseEventArgs e)
        {
            TT.IsOpen = false;
        }
        int QueGrid_Selection = -1;

        private void QueGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            QueGrid_Selection = QueGrid.SelectedIndex;

            DataRowView dataRow = (DataRowView)QueGrid.SelectedItem;
            //int h = int.Parse(dataRow.Row.ItemArray[2].ToString());
            var k = Task.Run(() => StatesGrid_PullSelected(QueGrid_Selection));
        }

        private void GenerateQueButton_Clicked(object sender, RoutedEventArgs e)
        {
            NameStateDialog nsd = new NameStateDialog();
            nsd.TextBoxName.Text = this.SelectedQueTextBox.Text;
            nsd.ShowDialog();
            string notesStr = nsd.NotesResult;

            string CmdString = "Select * from Momsql..Ques where QueName = @Quename";


            using (SqlConnection MomCon = new SqlConnection(MomConStr))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(CmdString, MomCon);
                    MomCon.Open();
                    cmd.Parameters.AddWithValue("@QueName", nsd.NameResult);
                    while (cmd.ExecuteReader().HasRows)
                    {
                        MomCon.Close();
                        MessageBoxResult r = MessageBox.Show("A STATE WITH THIS NAME ALREADY EXISTS, WOULD YOU LIKE TO OVERWRITE?", "SAVE ERROR", MessageBoxButton.YesNoCancel);
                        if (r == MessageBoxResult.Cancel) return;
                        if (r == MessageBoxResult.OK) { MessageBox.Show("HAVENT DONE THAT YET, SORRY");  }
                        nsd = new NameStateDialog();
                        nsd.TextBoxNotes.Text = notesStr;
                        nsd.ShowDialog();
                        notesStr = nsd.NotesResult;
                        cmd.Parameters["@QueName"].Value = nsd.NameResult;
                        //nsd.TextBoxName.SelectAll();
                        MomCon.Open();
                    }
                    foreach(AxisControl AX in axControl)
                    {
                        if(AX.IsSelected)
                        {
                            cmd = new SqlCommand("MomSQL..StoreGroupQue", MomCon);
                            cmd.Parameters.AddWithValue("@QueName", nsd.NameResult);
                            cmd.Parameters.AddWithValue("@JoyStickNumber", nsd.JoyStickSelected + 1);
                            cmd.Parameters.AddWithValue("@AxisNumber", AX.AxisNumber);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Notes", nsd.NotesResult);
                            cmd.CommandTimeout = 100;
                            cmd.ExecuteNonQuery();
                        }
                    }
                   
                }
                catch { }
                finally { MomCon.Close(); }
                QuesGrid_Update(QueGrid_Selection);
            }
        }
       
        void ConnectSerial()
        {
            string s = (string)ErrorIcon.ToolTip;
            if (this.SerialConnected && !sp.IsOpen)
            {
                foreach (string ComPort in System.IO.Ports.SerialPort.GetPortNames())
                {
                    if (!sp.IsOpen && SerialConnected)
                    {
                        sp = new System.IO.Ports.SerialPort(ComPort, 19200);
                        try
                        {
                            sp.Open();
                            Thread.Sleep(100);
                            if (sp.BytesToRead < 10) sp.Close(); else { sr = new SerialRemote(ref sp); }
                        }
                        catch
                        {
                            SerialConnected = false;
                        }
                    }
                }
                if (!sp.IsOpen)
                {
                    ErrorIcon.Visibility = Visibility.Visible;
                    if (!s.Contains("Error connecting to analog controls")) ErrorIcon.ToolTip = ErrorIcon.ToolTip + "Error connecting to analog controls. Click to troubleshoot.\r";
                    SerialConnected = false;
                }
                else { sr.start(); SerialConnected = true; ErrorIcon.Visibility = Visibility.Hidden; }
            }
        }

        private void ModeSelect()
        {
            if(!(bool)QueMode.IsChecked) PopulateJoystickSelections(true, -1, "");

        
                    if (!SerialConnected || sr is null || ModeSelDisabled) return;
                    ModeSelDisabled = true;
                    if (sr.EstopPressed  && LiveDisplay != 2)
                    {
                        ADSQL.SqlWriteAxis(1, "AxisStatus", "FAULT"); ShadeRectangle1.Stroke = new SolidColorBrush(System.Windows.Media.Colors.Red);
                    }

                    if ((string)ADSQL.SqlReadAxis(1, "AxisStatus") =="FAULT" && LiveDisplay != 2) LiveDisplay = 3;
                    if (sr.EstopPressed && LiveDisplay != 2) LiveDisplay = 4;
                    // if (!sr.EstopPressed && LiveDisplay == 2) { Mom.WriteValue(Mom.GlobalEstop, true); LiveDisplay = 3; }

                

            if (LiveDisplay == 3)
            {
                VirtualModeButton.Content = "Enable Simulation";
                ShadeRectangle1.Stroke = new SolidColorBrush(System.Windows.Media.Colors.Orange);
                ModeLabel.Content = "System Fault Active (Live)";
                ModeLabel.Foreground = new SolidColorBrush(System.Windows.Media.Colors.Orange);
                if (!sr.EstopPressed) SafteyResetButton.Visibility = Visibility.Visible;
            }

            if (LiveDisplay == 4)
            {
                ShadeRectangle1.Stroke = new SolidColorBrush(System.Windows.Media.Colors.Red);
                ModeLabel.Foreground = new SolidColorBrush(System.Windows.Media.Colors.Red);
                ModeLabel.Content = "Console E-Stop active (Live)";
                VirtualModeButton.Opacity = 20;
                if (sr.EstopPressed) SafteyResetButton.Visibility = Visibility.Hidden;
            }

          
            if (LiveDisplay == 2)
            {
                ShadeRectangle1.Stroke = new SolidColorBrush(System.Windows.Media.Colors.Yellow);
                ModeLabel.Foreground = new SolidColorBrush(System.Windows.Media.Colors.Yellow);
                ModeLabel.Content = "Simulation Mode Active\r";

                VirtualModeButton.Opacity = 20;
                VirtualModeButton.Content = "Enable Live Mode";
                SafteyResetButton.Visibility = Visibility.Hidden;
                if (!sr.EstopPressed)
                {
                    ModeSelDisabled = true;
                    do
                    {
                        MessageBoxResult c = MessageBox.Show("Console E-Stop Released, Press E-Stop to remain in Virtual Mode.\r Or press Ok to enter Live Mode.", "Enter Live Mode", MessageBoxButton.OKCancel);
                        if (c == MessageBoxResult.Cancel) { ModeSelDisabled = false; return; }
                        if (c == MessageBoxResult.OK) { ModeSelDisabled = false; LiveDisplay = 1; return; }
                    } while (!sr.EstopPressed);

                    ModeSelDisabled = false;
                }
            }
            else if (LiveDisplay == 1)
            {
                ShadeRectangle1.Stroke = new SolidColorBrush(System.Windows.Media.Colors.Gray);
                ModeLabel.Foreground = new SolidColorBrush(System.Windows.Media.Colors.Gray);
                ModeLabel.Content = "Live mode Active";
                VirtualModeButton.Content = "Enable Simulation";
            }
            ModeSelDisabled = false;
        }

        private void QueModeCheckBox_Clicked(object sender, RoutedEventArgs e)
        {
            foreach (AxisControl ax in axControl) { ax.AssignedJoyStick = -1; ax.IsSelected = false; }
            foreach (QueControl q in qControl) { q.ClearQueControl(); }
            
            SelectedQueTextBox.Text = "";
        }
    }


}
