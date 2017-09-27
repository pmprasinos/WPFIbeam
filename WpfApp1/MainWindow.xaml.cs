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

using System.Data.SqlClient;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using devDept.Eyeshot.Triangulation;
using devDept.Eyeshot.Translators;
using devDept.Eyeshot.Labels;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Windows.Media;
using System.Threading;
using System.Configuration;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Task BackTask;
        int tickCounter = 0;
        SqlConnection MomCon = new SqlConnection("data source = MOM0\\MOMSQL; initial catalog = MomSQL; user id = pprasinos; password = Wyman123-; MultipleActiveResultSets = True; App = EntityFramework");
        SqlDataReader sda; DataTable dt;
        private Vector3D[][] IBeamTrans = new Vector3D[3][];
        private double[] KidPositions = new double[6];
        private const string EYESHOT_SERIAL = "ULTWPF-94GF-N1277-FNLR3-1PPHF";
        static System.Windows.Forms.Timer t = new System.Windows.Forms.Timer(); static System.Windows.Forms.Timer StatesGridTimer = new System.Windows.Forms.Timer();
        bool disabled = false; bool MomConnected = true; bool SerialConnected = true;
        double[] AxisSpeed = { 0, 0, 0, 0, 0, 0 };
        ADSClient Mom;
        List<AxisGroup> AxisData = new List<AxisGroup>();
        /// <summary>
        /// 1=Live mode, 2 = Virtual Mode, 3 = Fault Active, 4 = Estop Active
        /// </summary>
        int LiveDisplay = 1; //1=Live mode, 2 = Virtual Mode, 3 = Fault Active, 4 = Estop Active
        Point4D[] JoySticks = new Point4D[4];
        Dictionary<string, long[]> States = new Dictionary<string, long[]>();
        static System.IO.Ports.SerialPort sp = new System.IO.Ports.SerialPort();
        SerialRemote sr;
        List<_treeListView.TreeListViewItem> TviList = new List<_treeListView.TreeListViewItem>();
        ToolTip TT = new System.Windows.Controls.ToolTip();
        int loopCount = 0;
        private System.Drawing.Point _mouseLocation;
        private int SelectedLayer = -1;
        bool ModeSelDisabled; bool t_Busy = false;
        bool UserInTextBox = false;

        public MainWindow() : base()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandeledEx);
            InitializeComponent();
            ErrorIcon.Visibility = Visibility.Hidden;
            viewPortLayout1.Unlock(EYESHOT_SERIAL);

            if (!UserInTextBox) sr = null;

            for (int i = 0; i < 100; i++)
            {
                if (i < JoySticks.Length) JoySticks[i] = new Point4D(0, 0, 0, 0);
                if (i < 6)
                {
                    AxisData.Add(new AxisGroup("Axis_" + i, "Hoist", "10.0.0." + i + 1, 0, "3000", "0", "MM"));
                    AxisData[i].Foreground = System.Windows.Media.Brushes.White;
                }
            }

            viewPortLayout1.ToolBar.Visible = false;            //JoySticks[0].X = 20;
        }



        private void Joystick_Setup(object sender, RoutedEventArgs e)
        {
            JoyStickSetup js = new JoyStickSetup();

            js.ShowDialog();
        }

        private void StatesGridTimer_tick(object myObject, EventArgs e)
        {
            StatesGrid_Update();
        }
            private void T_tick(object myObject, EventArgs e)
        {
            // if(BackTask !=null) if (!BackTask.IsCompleted) BackTask = null;
            Stopwatch s = Stopwatch.StartNew();
            try
            {
                t.Interval = 50;
                if (disabled || t_Busy) { t.Interval = 10; return; }
                t_Busy = true;
                tickCounter++;

                // t.Stop();
                ConnectMom();
                ConnectSerial();
                UpdateCad(SelectedLayer);       

                for (int i = 0; i < JoySticks.Length; i++)
                {
                    if (sp.IsOpen) { JoySticks[i].X = sr.x[i]; JoySticks[i].Y = sr.y[i]; JoySticks[i].Z = sr.z[i]; JoySticks[i].W = sr.w[i]; }
                }
                AxisSpeed[Math.Abs(SelectedLayer - 1) * 2] = JoySticks[0].X / 2;
                AxisSpeed[1 + Math.Abs(SelectedLayer - 1) * 2] = JoySticks[1].X / 2;

                loopCount++;
                loopCount = loopCount % 100;
                if (loopCount % 10 == 9) ModeSelect();
                if (LiveDisplay != 2)
                {
                    if (Mom != null && sr != null && loopCount % 10 == 3) if (sr.DeadmanRightPressed || sr.DeadmanLeftPressed) Mom.WriteValue(Mom.DeadMan, true, "DEADMAN"); else if (loopCount % 10 == 3) Mom.WriteValue(Mom.DeadMan, false, "DEADMAN");
                    if (Mom != null && sr != null) if (loopCount % 50 == 49 && Mom.Connected) Mom.WriteValue(Mom.MomControl, true, "MOMCONTROL");
                }
                t_Busy = false;
            }
            catch { t_Busy = false; Debug.Print("ERROR"); }
            finally { }
            Debug.Print(loopCount.ToString() + "   " + s.ElapsedMilliseconds.ToString());
        }

        void ConnectMom()
        {
            if (MomConnected) try
            {
                    MomConnected = false;

                    if (Mom is null)
                    {
                        System.Net.NetworkInformation.Ping p = new System.Net.NetworkInformation.Ping();
                        string host = "";
                        int n = 0;
                        do { host = "10.99.1."; host = host + (n + 1).ToString(); try { host = p.Send(host).Address.ToString(); if (!host.Contains("10.99.1")) host = ""; } catch { n++; host = ""; } } while (host == "" && n < 2);
                        n = 0;
                        do { if (host == "") { host = "MOM"; host = host + n; }; try { host = p.Send(host).Address.ToString(); if (!host.Contains("10.99.1")) host = ""; } catch { n++; host = ""; } } while (host == "" && n < 2);
                        host = "10.99.1.1";
                        Mom = new ADSClient(host + ".1.1", true, 2);
                    }
                    System.Threading.Tasks.Task<bool> t = Task.Run(() => Mom.WriteValue(Mom.TimeOutSwitch, false));
            }
            catch
            {
                    string s = (string)ErrorIcon.ToolTip;
                    //Could not connect to mom instance
                    ErrorIcon.Visibility = Visibility.Visible;
                    s = (string)ErrorIcon.ToolTip;
                    MomConnected = false;
                    if (!s.Contains("Error connecing to Mom PLC Instance"))
                    {
                        ErrorIcon.ToolTip = ErrorIcon.ToolTip + "Error connecing to Mom PLC Instance. Click to troubleshoot.\r";
                    }
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

        static void MaximizeToSecondaryMonitor(Window window)
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
            rfa = new ReadSTL(new MemoryStream(Properties.Resources.Pulley), false);
            viewPortLayout1.DoWork(rfa);
            rfa.AddToSceneAsSingleObject(viewPortLayout1, "PulleyParent", 0);
            // FileName = "C:\\Users\\Phil\\source\\repos\\WpfApp1\\WpfApp1\\obj\\3D-Grid-and-ZeroDeck-pit.stl";
            // rfa = new ReadSTL(FileName);
            // viewPortLayout1.DoWork(rfa);
            // rfa.Entities[0].Scale(25.4, 25.4, 25.4);
            // rfa.Entities[0].Visible = true;

            // rfa.Entities[0].Selectable = false;
            //rfa.Entities[0].Translate(-13400, -13400, -1000);
            //rfa.AddToSceneAsSingleObject(viewPortLayout1, "PIT", 0);

        }


        private void ViewPortLayout1_OnLoad(object sender, RoutedEventArgs e)
        {
            if (disabled) return;

            Entity s = (Entity)viewPortLayout1.Blocks["PulleyParent"].Entities[0];
            s.Color = System.Drawing.Color.FromArgb(200, System.Drawing.Color.Aqua);

            for (int i = 0; i < 3; i++)
            {
                Entity E = (Entity)viewPortLayout1.Blocks["iBeamParent"].Entities[0].Clone();
                Entity P = (Entity)viewPortLayout1.Blocks["PulleyParent"].Entities[0].Clone();

                E.Color = System.Drawing.Color.Black;
                viewPortLayout1.Entities.Add(E, i + 1);
                viewPortLayout1.Entities[(i * 2) + 1].Color = System.Drawing.Color.Black;
                viewPortLayout1.Entities[(i * 2) + 1].Visible = true;
                // P.GroupIndex = i + 1;
                P.Rotate(Math.PI / 2, new Vector3D(0, 0, 1));
                P.Rotate(Math.PI / 2, new Vector3D(0, -1, 0));
                P.Translate(E.BoxMax.X, boxCenter(E).Y, E.BoxMax.Z - P.BoxMin.Z);
                viewPortLayout1.Entities.Add(P, i + 1, System.Drawing.Color.AliceBlue);
                P = (Entity)viewPortLayout1.Blocks["PulleyParent"].Entities[0].Clone();
                P.Rotate(Math.PI / 2, new Vector3D(0, 0, 1));
                P.Rotate(Math.PI / 2, new Vector3D(0, -1, 0));
                P.Translate(E.BoxMin.X - E.BoxSize.Z, boxCenter(E).Y, E.BoxMin.Z + 300);
                viewPortLayout1.Entities.Add(P, i + 1);
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

            //viewPortLayout1.Entities.Add(u);
            viewPortLayout1.Blocks["iBeamParent"].Entities[0].Visible = false;
            viewPortLayout1.Blocks["PulleyParent"].Entities[0].Visible = false;

            Entity e0 = (Entity)viewPortLayout1.Blocks["iBeamParent"].Entities[0].Clone();
            Entity P0 = (Entity)viewPortLayout1.Blocks["PulleyParent"].Entities[0].Clone();
            // Entity s0 = (Entity)viewPortLayout1.Blocks["Pit"].Entities[0].Clone();

            for (int k = 1; k < viewPortLayout1.Entities.Count; k++) if (viewPortLayout1.Entities[k].LayerIndex != 0) viewPortLayout1.Entities.RemoveAt(k);

            for (int i = 0; i < 3; i++)
            {
                if (LiveDisplay == 1 && i == SelectedLayer - 1 && Mom != null)
                {


                    ADSQL.SqlWriteAxis((i * 2) + 1, "LiveValues", true);

                    KidPositions[(i * 2)] = int.Parse(ADSQL.SqlReadAxis((i * 2) + 1, "CurrentPosition").ToString());
                    KidPositions[(i * 2) + 1] = int.Parse(ADSQL.SqlReadAxis((i * 2) + 2, "CurrentPosition").ToString());
                    // if (loopCount % 3 == 2) Mom.Kids[(i * 2) + 1].TargetPosition = Mom.Kids[(i * 2) + 1].CurrentPosition + int.Parse(Math.Round(3 * AxisSpeed[(i * 2) + 1], 0).ToString());

                    if (loopCount % 3 == 2) ADSQL.SqlWriteAxis((i * 2) + 1, "TargetPosition", int.Parse(Math.Round(3 * AxisSpeed[(i * 2)], 0).ToString()) + KidPositions[(i * 2)]);
                    if (loopCount % 3 == 2) ADSQL.SqlWriteAxis((i * 2) + 2, "TargetPosition", int.Parse(Math.Round(3 * AxisSpeed[(i * 2) + 1], 0).ToString()) + KidPositions[(i * 2) + 1]);

                }
                else if (i == SelectedLayer - 1 && Mom != null)
                {
                    ADSQL.SqlWriteAxis((i * 2) + 1, "LiveValues", false);
                    if (loopCount % 3 == 2) ADSQL.SqlWriteAxis((i * 2) + 1, "TargetPosition", int.Parse(Math.Round(3 * AxisSpeed[(i * 2)], 0).ToString()) + KidPositions[(i * 2)]);
                    if (loopCount % 3 == 2) ADSQL.SqlWriteAxis((i * 2) + 2, "TargetPosition", int.Parse(Math.Round(3 * AxisSpeed[(i * 2) + 1], 0).ToString()) + KidPositions[(i * 2) + 1]);
                    KidPositions[(i * 2)] = int.Parse(ADSQL.SqlReadAxis((i * 2) + 1, "TargetPosition").ToString());
                    KidPositions[(i * 2) + 1] = int.Parse(ADSQL.SqlReadAxis((i * 2) + 2, "TargetPosition").ToString());

                }
                else if (SelectedLayer == -1 && Mom != null)
                {
                    KidPositions[(i * 2)] = int.Parse(ADSQL.SqlReadAxis((i * 2) + 1, "CurrentPosition").ToString());
                    KidPositions[(i * 2) + 1] = int.Parse(ADSQL.SqlReadAxis((i * 2) + 2, "CurrentPosition").ToString());
                }
                Entity e = (Entity)e0.Clone();
                Entity P = (Entity)P0.Clone();
                e.Rotate(1.5708, new Vector3D(1, 0, 0));
                if (e.BoxMax != null) e.Rotate(System.Math.Tan((KidPositions[i * 2] - KidPositions[(i * 2) + 1]) / (120 * 25.3)), new Vector3D(0, 1), boxCenter(e));
                e.Translate(0, 1000 * i, 0);
                e.Color = System.Drawing.Color.Black;
                e.Translate(0, 0, (KidPositions[i * 2] + KidPositions[i * 2 + 1]) / 2);
                viewPortLayout1.Entities.Add(e, i + 1);
                viewPortLayout1.Entities[(i * 2) + 0].Color = System.Drawing.Color.Black;
                viewPortLayout1.Entities[(i * 2) + 0].Visible = true;
                P.Rotate(Math.PI / 2, d);
                P.Rotate(Math.PI / 2, new Vector3D(0, -1, 0));

                if (KidPositions[i * 2] - KidPositions[(i * 2) + 1] < 0)
                {
                    if (e.BoxMax != null && P.BoxMax != null) P.Translate(e.BoxMax.X + (System.Math.Sin((KidPositions[i * 2] - KidPositions[(i * 2) + 1]) / (120 * 25.3)) * e.BoxSize.Y), boxCenter(e).Y, e.BoxMax.Z - P.BoxMin.Z);
                }
                else
                {
                    if (e.BoxMax != null && P.BoxMax != null) P.Translate(e.BoxMax.X - (System.Math.Sin((KidPositions[i * 2] - KidPositions[(i * 2) + 1]) / (120 * 25.3)) * e.BoxSize.Y), boxCenter(e).Y, e.BoxMin.Z + 300);
                }

                viewPortLayout1.Entities.Add(P, i + 1, System.Drawing.Color.AliceBlue);
                P = (Entity)viewPortLayout1.Blocks["PulleyParent"].Entities[0].Clone();
                P.Rotate(Math.PI / 2, d);
                P.Rotate(Math.PI / 2, new Vector3D(0, -1, 0));
                if (KidPositions[i * 2] - KidPositions[(i * 2) + 1] < 0)
                {
                    if (e.BoxMax != null && P.BoxMax != null) P.Translate(e.BoxMin.X - (System.Math.Sin((KidPositions[i * 2] - KidPositions[(i * 2) + 1]) / (120 * 25.3)) * e.BoxSize.Y), boxCenter(e).Y, e.BoxMin.Z + 300);
                }
                else
                {
                    if (e.BoxMax != null && P.BoxMax != null) P.Translate(e.BoxMin.X + (System.Math.Sin((KidPositions[i * 2] - KidPositions[(i * 2) + 1]) / (120 * 25.3)) * e.BoxSize.Y), boxCenter(e).Y, e.BoxMax.Z - P.BoxMin.Z);
                }
                viewPortLayout1.Entities.Add(P, i + 1);
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
            viewPortLayout1.Entities.ClearSelection();
            _mouseLocation = devDept.Graphics.RenderContextUtility.ConvertPoint(e.GetPosition(viewPortLayout1));
            viewPortLayout1.ProcessSelection(new System.Drawing.Rectangle(_mouseLocation.X - 10, _mouseLocation.Y - 10, 20, 20), true, false, new ViewportLayout.SelectionChangedEventArgs());
            viewPortLayout1.UpdateVisibleSelection();
            viewPortLayout1.Entities.SetSelectionAsCurrent();
            foreach (Entity en in viewPortLayout1.Entities)
            {
                if (en.Selected && SelectedLayer != en.LayerIndex)
                {

                    SelectedLayer = en.LayerIndex;
                    PopulateAxisInfo((SelectedLayer * 2) - 1, true);
                    PopulateAxisInfo(SelectedLayer * 2, false);
                }
            }

        }

        private void viewportLayout1_SelectionChanged(object sender, ViewportLayout.SelectionChangedEventArgs e)
        {
            if (viewPortLayout1.Entities.CurrentBlockReference == null)
            {
                // If the user clicks on a BlockReference then we want to set it as current.
                int currentEntity = viewPortLayout1.GetEntityUnderMouseCursor(_mouseLocation);
                if (currentEntity != -1)
                {
                    viewPortLayout1.Entities.SetCurrent(viewPortLayout1.Entities[currentEntity] as BlockReference);
                    if (viewPortLayout1.Entities.CurrentBlockReference != null)
                        viewPortLayout1.Entities.CurrentBlockReference.Selected = true;
                }
            }
            else
            {
                // Set the BlockReference as selected.
                // viewPortLayout1.Entities.CurrentBlockReference.Selected = true;
                // Selection();
                viewPortLayout1.Invalidate();
            }
            JoyStick_Activate();
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
            AxisControl_1.AxisNumber = 1 + ((SelectedLayer - 1) * 2);
            AxisControl_2.AxisNumber = 2 + ((SelectedLayer - 1) * 2);
            AxisControl_1.Refresh();
            AxisControl_2.Refresh();
        }

        SqlCommand scmd;
        private void StatesGrid_Update()
        {
            String y = "";
            using (SqlConnection con = new SqlConnection(MomCon.ConnectionString))
            {
                System.Data.DataRowView currentRow = (System.Data.DataRowView)StatesGrid.SelectedItem;
                if (StatesGrid.SelectedCells.Count != 0) y = currentRow.Row.ItemArray[0].ToString();
                MomCon.Open();
                if (scmd == null) scmd = new SqlCommand("Exec momsql.dbo.GetStateData", MomCon);
                sda = scmd.ExecuteReader();
                if (dt == null) { dt = new DataTable("AxisControl"); StatesGrid.ItemsSource = dt.DefaultView; }
                dt.Clear();
                dt.Load(sda);
                StatesGrid.Items.Refresh();
                MomCon.Close();

                if (y == "") return;
               // StatesGrid.SelectAll();
            }

            foreach (object item in StatesGrid.ItemsSource)
            {
                try
                {
                    DataRowView drv = (DataRowView)item;
                    if (drv.Row.ItemArray[0].ToString() == y) StatesGrid.SelectedItem = item;
                }
                catch { }
            }
        }

        private void StoreAxisGrouState_Clicked(object sender, RoutedEventArgs e)
        {

            NameStateDialog nsd = new NameStateDialog();
            nsd.ShowDialog();
            double[] g = new double[] { SelectedLayer, KidPositions[SelectedLayer] };
            // if (nsd.SaveClicked) States.Add(nsd.NameResult, g);
            string[] row = new string[] { "THIS", "IS", "TEST" };

            string CmdString = "MomSQL..StoreGroupState";

            SqlCommand cmd = new SqlCommand(CmdString, MomCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AxisGroupNum", SelectedLayer);
            cmd.Parameters.AddWithValue("@StateName", nsd.NameResult);
            cmd.Parameters.AddWithValue("@Notes", nsd.NotesResult);


            cmd.CommandTimeout = 1000;

            try
            {
                MomCon.Open();
                cmd.ExecuteNonQuery();
            }
            catch { }
            finally { MomCon.Close(); }


        }

        private void UnhandeledEx(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            MessageBox.Show(e.ToString(), "Unhandled Exception");
        }


        private void closed(object sender, RoutedEventArgs e)
        {
            Mom.WriteValue(Mom.MomControl, false, "MOMCONTROL");
            this.Close();
        }

        private void ModeSelect()
        {
            if (Mom != null) if (!Mom.Connected)
                {
                    LiveDisplay = 2;
                }
                else
                {
                    if (!SerialConnected || sr is null || ModeSelDisabled) return;
                    ModeSelDisabled = true;
                    if (sr.EstopPressed && !(Mom is null) && LiveDisplay != 2)
                    {
                        Mom.WriteValue(Mom.GlobalEstop, true); ShadeRectangle1.Stroke = new SolidColorBrush(System.Windows.Media.Colors.Red);
                    }

                    if ((int)Mom.ReadValue(Mom.GlobalEstop) > 0 && LiveDisplay != 2) LiveDisplay = 3;
                    if (sr.EstopPressed && LiveDisplay != 2) LiveDisplay = 4;
                    // if (!sr.EstopPressed && LiveDisplay == 2) { Mom.WriteValue(Mom.GlobalEstop, true); LiveDisplay = 3; }

                }

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
                VirtualModeButton.Opacity = 80;
                if (sr.EstopPressed) SafteyResetButton.Visibility = Visibility.Hidden;

            }

            for (int axs = 0; axs < 6; axs++)
            {
                KidPositions[axs] = KidPositions[axs] + AxisSpeed[axs];
                AxisData[axs].Position = AxisData[axs].Position + AxisSpeed[axs];
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
                if (loopCount % 50 == 38) Mom.WriteValue(Mom.MomControl, true, "MOMCONTROL");
            }
            ModeSelDisabled = false;
        }

        private void ResetSaftey(object sender, RoutedEventArgs e)
        {
            if (!(Mom is null)) Mom.WriteValue(Mom.FaultReset, true, "FAULTRESET");
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
            MomConnected = true;
            SerialConnected = true;
        }

        private void ErrorIcon_MouseEnter(object sender, MouseEventArgs e)
        {
            MomConnected = true;
            SerialConnected = true;
            Thread.Sleep(t.Interval);
            TT.Content = ErrorIcon.ToolTip;
            TT.IsOpen = true;
        }

        private void ErrorIcon_MouseLeave(object sender, MouseEventArgs e)
        {
            TT.IsOpen = false;
        }

    }


}
