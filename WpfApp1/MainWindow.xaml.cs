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
        SqlConnection MomCon = new SqlConnection("data source = CONSOLE1; initial catalog = MomSQL; user id = pprasinos; password = Wyman123-; MultipleActiveResultSets = True; App = EntityFramework");
        private Vector3D[][] IBeamTrans = new Vector3D[3][];
        private double[] KidPositions = new double[6];
        private const string EYESHOT_SERIAL = "ULTWPF-94GF-N1277-FNLR3-1PPHF";
        private int LayerInd = 0;
        static System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
        bool disabled = false; bool MomConnected = true; bool SerialConnected = true;
        double[] AxisSpeed = { 0, 0, 0, 0, 0, 0 };
        ADSClient Mom;
        List<AxisGroup> AxisData = new List<AxisGroup>();
        List<AxisGroup> SelectedGroup;
        /// <summary>
        /// 1=Live mode, 2 = Virtual Mode, 3 = Fault Active, 4 = Estop Active
        /// </summary>
        int LiveDisplay = 1; //1=Live mode, 2 = Virtual Mode, 3 = Fault Active, 4 = Estop Active
        Point4D[] JoySticks = new Point4D[4];
        Dictionary<string, long[]> States = new Dictionary<string, long[]>();
        static System.IO.Ports.SerialPort sp = new System.IO.Ports.SerialPort();
        SerialRemote sr;
        DataTable tbl = new DataTable("Customers");
        List<_treeListView.TreeListViewItem> TviList = new List<_treeListView.TreeListViewItem>();
        ToolTip TT = new System.Windows.Controls.ToolTip();
        int loopCount = 0;
        private System.Drawing.Point _mouseLocation;
        private int SelectedLayer = -1;
        bool ModeSelDisabled;

        public MainWindow() : base()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandeledEx);

            InitializeComponent();
            ErrorIcon.Visibility = Visibility.Hidden;
            viewPortLayout1.Unlock(EYESHOT_SERIAL);

            sr = null;
           

            for (int i = 0; i < 100; i++)
            {
                if (i < JoySticks.Length) JoySticks[i] = new Point4D(0, 0, 0, 0);
                if (i < 6)
                {
                    AxisData.Add(new AxisGroup("Axis_" + i, "Hoist", "10.0.0." + i + 1, 0, "3000", "0", "MM"));
                    AxisData[i].Foreground = System.Windows.Media.Brushes.White;
                }
            }
            TLV.Tag = "";
       
            viewPortLayout1.ToolBar.Visible = false;
            //JoySticks[0].X = 20;

        }

     

        private void Joystick_Setup(object sender, RoutedEventArgs e)
        {
            JoyStickSetup js = new JoyStickSetup();

            js.ShowDialog();
        }

        private void T_tick(object myObject, EventArgs e)
        {
            try
            {
                t.Stop();
            ConnectMom();
            ConnectSerial();

         
                if (disabled) return;
               // if(sp.IsOpen && System.Math.Abs(JoySticks[0].X +JoySticks[0].Y + JoySticks[0].Z)  > 1 || loopCount % 10 ==8) UpdateCad(1);
                UpdateCad(1);
                t.Interval =10;
                Label1.Content = "";
                for (int i = 0; i < JoySticks.Length; i++)
                {
                    Label1.Content += i.ToString();
                    if (sp.IsOpen) { JoySticks[i].X = sr.x[i]; JoySticks[i].Y = sr.y[i]; JoySticks[i].Z = sr.z[i]; JoySticks[i].W = sr.w[i]; }
                    Label1.Content += " X: " + JoySticks[i].X + " Y: " + JoySticks[i].Y + " Z: " + JoySticks[i].Z + " Z: " + JoySticks[i].W + "\r\n";
                }
                AxisSpeed[Math.Abs(SelectedLayer - 1) * 2] = JoySticks[0].X / 2;
                AxisSpeed[1 + Math.Abs(SelectedLayer - 1) * 2] = JoySticks[0].Z / 2;

                //TLV.Items.Refresh();
               // TVI2.Items.Refresh();
          
            loopCount++;
            loopCount = loopCount % 100;
            if (loopCount % 10==9) ModeSelect();
            if (Mom != null && sr!= null) if(sr.DeadmanRightPressed || sr.DeadmanRightPressed) Mom.WriteValue(Mom.DeadMan, true, "DEADMAN"); else Mom.WriteValue(Mom.DeadMan, false, "DEADMAN");
            if (Mom != null && sr != null) if (loopCount % 10 == 2 && Mom.Connected) Mom.WriteValue(Mom.MomControl, true, "MOMCONTROL");

            }
            catch { }
            finally { t.Start(); }

        }

        void ConnectMom()
        {
            string s = (string)ErrorIcon.ToolTip;
            if (MomConnected) try
                {

                    if (Mom is null) { Mom = new ADSClient("192.168.10.96.1.1", true, 2); }
                    Mom.WriteValue(Mom.TimeOutSwitch, false);
                }
                catch
                {
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
                        sp = new System.IO.Ports.SerialPort(ComPort, 38400);
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
            MaximizeToSecondaryMonitor(this);
            viewPortLayout1.Layers.Add(new Layer("0")); viewPortLayout1.Layers.Add(new Layer("1")); viewPortLayout1.Layers.Add(new Layer("2")); viewPortLayout1.Layers.Add(new Layer("3"));
            OpenCADFile();
            t = new System.Windows.Forms.Timer();
            t.Interval = 100;
            t.Enabled = true;
            t.Start();
            t.Tick += new EventHandler(T_tick);
            JoyStick_Activate();
        }


        private void OpenCADFile()
        {
            if (disabled) return;
            string AppDir = System.AppDomain.CurrentDomain.BaseDirectory;
            ReadFileAsynch rfa = null;
            string FileName = AppDir + "CAD\\iBeam.stl";
            rfa = new ReadSTL(FileName);                    //    break;
            rfa.Unlock(EYESHOT_SERIAL);
            rfa.LoadingText = "iBeam";

            viewPortLayout1.DoWork(rfa);
            rfa.AddToSceneAsSingleObject(viewPortLayout1, "iBeamParent", 0);
            FileName = AppDir + "CAD\\Pulley.stl";
            rfa = new ReadSTL(FileName);
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
                if (LiveDisplay == 1 && i ==0 && Mom != null)
                {
                    
                    KidPositions[i*2] = Mom.Kids[(i*2)].CurrentPosition;
                    if (loopCount % 3 == 2) Mom.Kids[(i* 2)].TargetPosition = Mom.Kids[(i*2)].CurrentPosition + int.Parse(Math.Round(3 * AxisSpeed[(i*2)], 0).ToString());
                   // if (sr.DeadmanRightPressed || sr.DeadmanRightPressed) Mom.WriteValue(Mom.DeadMan, true, "DEADMAN"); else Mom.WriteValue(Mom.DeadMan, false, "DEADMAN");
                    KidPositions[i] = Mom.Kids[(i * 2) + 1].CurrentPosition;
                    if (loopCount % 3 == 2) Mom.Kids[(i * 2) + 1].TargetPosition = Mom.Kids[(i * 2) + 1].CurrentPosition + int.Parse(Math.Round(3 * AxisSpeed[(i * 2) + 1], 0).ToString());
                    
                }
                Entity e = (Entity)e0.Clone();
                Entity P = (Entity)P0.Clone();
                e.Rotate(1.5708, new Vector3D(1, 0, 0));
                e.Rotate(System.Math.Tan((KidPositions[i * 2] - KidPositions[(i * 2) + 1]) / (120 * 25.3)), new Vector3D(0, 1), boxCenter(e));
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
                    P.Translate(e.BoxMax.X + (System.Math.Sin((KidPositions[i * 2] - KidPositions[(i * 2) + 1]) / (120 * 25.3)) * e.BoxSize.Y), boxCenter(e).Y, e.BoxMax.Z - P.BoxMin.Z);
                }
                else
                {
                    P.Translate(e.BoxMax.X - (System.Math.Sin((KidPositions[i * 2] - KidPositions[(i * 2) + 1]) / (120 * 25.3)) * e.BoxSize.Y), boxCenter(e).Y, e.BoxMin.Z + 300);
                }

                viewPortLayout1.Entities.Add(P, i + 1, System.Drawing.Color.AliceBlue);
                P = (Entity)viewPortLayout1.Blocks["PulleyParent"].Entities[0].Clone();
                P.Rotate(Math.PI / 2, d);
                P.Rotate(Math.PI / 2, new Vector3D(0, -1, 0));
                if (KidPositions[i * 2] - KidPositions[(i * 2) + 1] < 0)
                {
                    P.Translate(e.BoxMin.X - (System.Math.Sin((KidPositions[i * 2] - KidPositions[(i * 2) + 1]) / (120 * 25.3)) * e.BoxSize.Y), boxCenter(e).Y, e.BoxMin.Z + 300);
                }
                else
                {
                    P.Translate(e.BoxMin.X + (System.Math.Sin((KidPositions[i * 2] - KidPositions[(i * 2) + 1]) / (120 * 25.3)) * e.BoxSize.Y), boxCenter(e).Y, e.BoxMax.Z - P.BoxMin.Z);
                }
                viewPortLayout1.Entities.Add(P, i + 1);

            }
            viewPortLayout1.Invalidate();
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
                    PopulateAxisInfo(SelectedLayer);
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
                Selection();
                viewPortLayout1.Invalidate();
            }
            TLV.Tag = "";
        }

        private void Selection()
        {
            BlockReference currBlockref = viewPortLayout1.Entities.CurrentBlockReference;
            if (currBlockref != null)
            {
                devDept.Eyeshot.Block bl = viewPortLayout1.Blocks[currBlockref.BlockName];
                int currentEntity = viewPortLayout1.GetEntityUnderMouseCursor(_mouseLocation);
                if (currentEntity != -1)
                {
                    BlockReference br = bl.Entities[currentEntity] as BlockReference;
                    if (br != null)
                    {
                        //if (chkRecursiveSearch.IsChecked.Value)
                        //{
                        // If the block contains another blockreference then we want to search inside it.
                        currBlockref.Selected = false;
                        viewPortLayout1.Entities.SetCurrent(br);
                        LayerInd = br.LayerIndex;
                        // }
                    }
                    else
                    {
                        currBlockref.Selected = true;
                        // Uncomment the line below if you want to select the entity inside this block reference                        
                        //if (!bl.Entities[currentEntity].Selected) bl.Entities[currentEntity].Selected = true;                        
                    }
                }
            }
        }



        private void JoyStick_Activate()
        {
            JS1.Source = ImageFilter.SetJoyStick(true, true, false, true); JS1.Opacity = 100;
            JS2.Source = ImageFilter.SetJoyStick(false, true, true, true); JS2.Opacity = 20;
            JS3.Source = ImageFilter.SetJoyStick(false, true, true, true); JS3.Opacity = 20;
            JS4.Source = ImageFilter.SetJoyStick(false, true, true, true); JS4.Opacity = 20;
        }

        private void PopulateAxisInfo(int AxisNum)
        {
          

            bool added = false;



               
              
                if (!this.TLV.Tag.ToString().Contains(AxisNum.ToString()))
                {
                    _treeListView.TreeListViewItem TVI2 = new _treeListView.TreeListViewItem();
                 
                    TviList.Add(TVI2);
                    // int GroupNum = 0;
                    if (!added)
                    {
                        TLV.Items.Clear();
                        TLV.Items.Refresh();
                        SelectedGroup = new List<AxisGroup>();
                        
                        
                        string CmdString = string.Empty;
                     
                            CmdString = "exec getaxisdata @AxisNumber = " + AxisNum ;
                            SqlCommand cmd = new SqlCommand(CmdString, MomCon);
                            SqlDataAdapter sda = new SqlDataAdapter(cmd);
                            DataTable dt = new DataTable("Employee");
                            sda.Fill(dt);
                            TVI2.ItemsSource = dt.DefaultView;
                            TLVI ax = new TLVI();
                        string s = dt.Rows[0].Field<string>("AxisGroupName");
                        ax.Name = s;
                        ax.Property = "Property";
                        ax.Value = "Value";
                        TVI2.Header = ax;
                        TLV.Items.Add(TVI2);
                        TVI2.IsExpanded = true;
                        added = true;
                    }
                   // SelectedGroup.Add(AxisData[((AxisGroup - 1) * 2) + x]);
                    // AxisData[((AxisGroup - 1) * 2) + x].GroupNumber = GroupNum;
                    this.TLV.Tag = this.TLV.Tag.ToString() + AxisNum.ToString();


                }
                else
                {
                    // _treeListView.TreeListViewItem g = (_treeListView.TreeListViewItem)TLV.Items[AxisData[((AxisGroup - 1) * 2) + x].GroupNumber];

                    // Axis p = (Axis)g.Items[AxisData[((AxisGroup - 1) * 2) + x].Address];
                    // p.Position = p.Position + 1;
                    // AxisData[((AxisGroup - 1) * 2) + x].Visibility = Visibility.Hidden;
                   foreach(_treeListView.TreeListViewItem t in TLV.Items)
                {
                    t.Items.Refresh();
                }

                }

       

        }



        private void StoreAxisGrouState_Clicked(object sender, RoutedEventArgs e)
        {
            NameStateDialog nsd = new NameStateDialog();
            nsd.ShowDialog();
            double[] g = new double[] { LayerInd, KidPositions[LayerInd] };
            // if (nsd.SaveClicked) States.Add(nsd.NameResult, g);
            string[] row = new string[] { "THIS", "IS", "TEST" };
            //ListViewItem l = new ListViewItem();
            //LV.Items.Add(new string[] { "0", nsd.NameResult, "KidAxis_" + (LayerInd + 1).ToString(), KidPositions[LayerInd].ToString() });


           //StateData y = new StateData();// { id = 0, Name = nsd.NameResult, AxisGroup = "AxisGroup" + (LayerInd + 1).ToString() };
          //  StatesGrid.Items.Add(y);
        }

        private void UnhandeledEx(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            MessageBox.Show(e.ToString(), "Unhandled Exception");
        }

        private void TreeListView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
           string s = sender.ToString();
           // _treeListView.TreeListView t = (_treeListView.TreeListView)sender;
           // TLVI f = (TLVI)t.SelectedItem;
            // _treeListView.TreeListViewItem[] t = (_treeListView.TreeListViewItem[])sender;
         
        }

        private void TLV_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
           // string s = sender.ToString();
        }

        private void ModeButton_Click(object sender, RoutedEventArgs e)
        {
          //  if(!(Mom is null))LiveDisplay = !LiveDisplay;
        }

        private void closed(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ModeSelect()
        {
            if (Mom != null) if(!Mom.Connected)
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

                    if ((int)Mom.ReadValue(Mom.GlobalEstop) > 0) LiveDisplay = 3;
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
                Mom.WriteValue(Mom.GlobalEstop, true);
            }

            for (int axs = 0; axs < 2; axs++)
            {

                if (LiveDisplay == 2)
                {
                    ShadeRectangle1.Stroke = new SolidColorBrush(System.Windows.Media.Colors.Yellow);
                    ModeLabel.Foreground = new SolidColorBrush(System.Windows.Media.Colors.Yellow);
                    ModeLabel.Content = "Simulation Mode Active\r";
                    KidPositions[axs] = KidPositions[axs] + AxisSpeed[axs];
                    AxisData[axs].Position = AxisData[axs].Position + AxisSpeed[axs];
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
                    Mom.WriteValue(Mom.MomControl, false, "MOMCONTROL");
                } else if(LiveDisplay == 1)
                 {

                    ShadeRectangle1.Stroke = new SolidColorBrush(System.Windows.Media.Colors.Gray);
                    ModeLabel.Foreground = new SolidColorBrush(System.Windows.Media.Colors.Gray);
                    ModeLabel.Content = "Live mode Active";
                    VirtualModeButton.Content = "Enable Simulation";
               }
               

            }
            ModeSelDisabled = false;
        }

        private void ResetSaftey(object sender, RoutedEventArgs e)
        {
           if(!(Mom is null)) Mom.WriteValue(Mom.FaultReset, true, "FAULTRESET");
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
            }else
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

        private void TLVTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            object s = sender;
        }

        private void TLVTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var s = sender;
           

        }

        private void TLVTextBox_GotKeyBoardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            _treeListView.SpecialTextBox tb = (sender as _treeListView.SpecialTextBox);
            if (tb != null)
            {
                tb.SelectAll();
                e.Handled = true;

            }
        }

        private void SelectivelyIgnoreMouseButton(object sender, MouseButtonEventArgs e)
        {
            _treeListView.SpecialTextBox tb = (sender as _treeListView.SpecialTextBox);
            if (tb != null)
            {
                if (!tb.IsKeyboardFocusWithin)
                {
                    e.Handled = true;
                    tb.Focus();
                }

            }
        }
    }

    public struct StateData
    {
        public int id { set; get; }
        public string Name { set; get; }
        public string AxisGroup { set; get; }
        // public DateTime lastrun { set; get; }
        //public DateTime nextrun { set; get; }
    }
     
}
