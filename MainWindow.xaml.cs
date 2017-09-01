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
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
//using devDept.Eyeshot.Entities;
using devDept.Eyeshot.Triangulation;
using devDept.Eyeshot.Translators;
using System.Threading;
using System.IO;
using System.Windows.Forms;
using devDept.Eyeshot.Labels;
using DockingLibrary;

namespace WPFiBeam
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
   
    public partial class MainWindow : Window
    {
        private Mesh[] iBeams;
        private long[] KidPositions = new long[6];
        private const string EYESHOT_SERIAL = "ULTWPF-94GF-N1277-FNLR3-1PPHF";
        private int LayerInd = 0;
        private ReadFileAsynch iBeamObj;
        private EventOptions ev = new WPFiBeam.EventOptions();
        public MainWindow()
        {
            InitializeComponent();
        //    viewPortLayout1.Unlock(EYESHOT_SERIAL);
        //    viewPortLayout1.WorkCompleted += new devDept.Eyeshot.ViewportLayout.WorkCompletedEventHandler(viewPortLayout1_WorkCompleted);
        //   // viewPortLayout1.WorkCancelled += new devDept.Eyeshot.ViewportLayout.WorkCancelledEventHandler(viewPortLayout1_WorkCancelled);
        //    viewPortLayout1.WorkFailed += new devDept.Eyeshot.ViewportLayout.WorkFailedEventHandler(viewPortLayout1_WorkFailed);
        //   // viewPortLayout1.BoundingBox.LabelColor = RenderContextUtility.ConvertColor(Draw.Color);
        //   // viewPortLayout1.BoundingBox.LineColor = RenderContextUtility.ConvertColor(Draw.Color);
        //    viewPortLayout1.MagnifyingGlass.Factor = 3;
        //    viewPortLayout1.MagnifyingGlass.Size = new System.Drawing.Size(200, 200);

        // //   tabControl1.SelectedIndex = 1;
        }



        private void loadSTL()
        {
            iBeams = new Mesh[2];

            string fileName = "C:\\Users\\pmpra\\Documents\\inventor\\iBeam.stl";
            //ReadSTL stl = new ReadSTL(fileName);
            ReadAutodesk readDwg = new ReadAutodesk(Environment.SpecialFolder.MyDocuments + "\\inventor\\iBeam.dwg");
            readDwg.Unlock(EYESHOT_SERIAL);
            readDwg.Simplify = true;
            readDwg.SkipLayouts = false;
            viewPortLayout1.DoWork(readDwg);
            readDwg.AddToScene(viewPortLayout1);

            //stl.Unlock(EYESHOT_SERIAL);

            // stl.AddToScene(viewPortLayout1, 0, System.Drawing.Color.Black);
            //  viewPortLayout1.Units = linearUnitsType.Millimeters;

            viewPortLayout1.Layers[0].Visible = true;
            viewPortLayout1.Layers[0].Color = System.Drawing.Color.Blue;

            viewPortLayout1.Clear();
            viewPortLayout1.Invalidate();
        }


        //private void OpenCADFile()
        //{
        //    System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
        //    string theFilter = "Points|*.asc|" + "Stereolithography|*.stl|" + "WaveFront OBJ|*.obj|" + "Laser LAS|*.las" + "|IGES|*.igs; *.iges|" + "STEP|*.stp; *.step";
        //    openFileDialog1.Filter = theFilter;
        //    openFileDialog1.Multiselect = false;
        //    openFileDialog1.AddExtension = true;
        //    openFileDialog1.CheckFileExists = true;
        //    openFileDialog1.CheckPathExists = true;
        //    //Nullable<bool> result = openFileDialog1.ShowDialog() != System.Windows.Forms.DialogResult.Yes;
        //    //if (result == true)
        //    //{
        //        viewPortLayout1.Entities.Clear();
        //        ReadFileAsynch rfa = null;
        //    //switch (openFileDialog1.FilterIndex)
        //    //{
        //    //    case 1:
        //    //        rfa = new ReadASC(openFileDialog1.FileName);                        break;
        //    //    case 2:
        //    string FileName = "C:\\users\\Phil\\Documents\\inventor\\iBeam.stl";
        //                rfa = new ReadSTL(FileName);                    //    break;
        //        //    case 3:
        //        //        rfa = new ReadOBJ(openFileDialog1.FileName);                        break;
        //        //    case 4:
        //        //        rfa = new ReadLAS(openFileDialog1.FileName);                        break;
        //        //    case 5:
        //        //        rfa = new ReadIGES(openFileDialog1.FileName);                        break;
        //        //    case 6:
        //        //        rfa = new ReadSTEP(openFileDialog1.FileName);                        break;
        //        //}

        //        rfa.Unlock(EYESHOT_SERIAL);
        //    rfa.LoadingText = "iBeam";
        //        viewPortLayout1.StartWork(rfa);

        //        viewPortLayout1.SetView(viewType.Trimetric, true, viewPortLayout1.AnimateCamera);
        //    //}
        //}



        //private void MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (e.ClickCount == 3) OpenCADFile();

        //}

        //private void viewPortLayout1_WorkCompleted(object sender, devDept.Eyeshot.WorkCompletedEventArgs e)
        //{


        //        ReadFileAsynch rfa = (ReadFileAsynch)e.WorkUnit;
        //    if (rfa.LoadingText == "iBeam") iBeamObj = rfa;
        //            viewPortLayout1.Layers.Add(new Layer("0"));
        //        rfa.AddToScene(viewPortLayout1, 0);

        //   // rfa.AddToScene(viewPortLayout1, LayerInd);
        //    //rfa.AddToScene(viewPortLayout1, LayerInd);
        //    viewPortLayout1.ZoomFit();
        //    viewPortLayout1.Layers.Add(new Layer("1"));
        //   // }

        //}

        //private void viewPortLayout1_WorkFailed(object sender, WorkFailedEventArgs e)
        //{
        //  //  tabControl1.IsEnabled = true;
        //   // importButton.IsEnabled = true;
        //}

        //private void TranslateBeam(object sender, RoutedEventArgs e)
        //{
        //    updateBeams(0);

        //   // viewPortLayout1.Entities[1].Translate(d);
        //}

        //private void updateBeams(int BeamIndex)
        //{


        //    Vector3D d = new Vector3D(0, 0, 1);

        //    KidPositions[0] = long.Parse(TextBox1.Text);
        //    KidPositions[1] = long.Parse(TextBox2.Text);
        //    KidPositions[2] = long.Parse(TextBox3.Text);
        //    KidPositions[3] = long.Parse(TextBox4.Text);
        //    KidPositions[4] = long.Parse(TextBox5.Text);
        //    KidPositions[5] = long.Parse(TextBox6.Text);
        //   // viewPortLayout1.Clear();
        //   foreach(Entity j in viewPortLayout1.Entities)
        //    {
        //        if (j.LayerIndex != 0) viewPortLayout1.Entities.Remove(j);
        //    }
        //    // Thread.Sleep(1000);
        //    viewPortLayout1.Entities[0].Visible = false;
        //    // viewPortLayout1.Clear();
        //    // viewPortLayout1.Layers.Add(new Layer("0"));
        //    // viewPortLayout1.Layers[0].Visible = false;
        //    Entity u = (Entity)viewPortLayout1.Entities[0].Clone();
        //    viewPortLayout1.Clear();
        //    viewPortLayout1.Entities.Add(u);
        //    for (int i = 0; i<3; i++)
        //    {
        //        Entity e =  (Entity)viewPortLayout1.Entities[0].Clone();
        //        e.Rotate(1.5708, new Vector3D(1, 0, 0));
        //        //viewPortLayout1.Layers.Add(new Layer((i+1).ToString()));

        //        e.Translate(0, 1000*i, 0);
        //        e.Rotate(System.Math.Tan((KidPositions[i * 2] - KidPositions[(i * 2) + 1]) / (120 * 25.3)), new Vector3D(0,1), boxCenter(e));

        //        e.Color = System.Drawing.Color.Black;
        //        e.Visible = true;
        //        viewPortLayout1.Entities.Add(e);

        //        viewPortLayout1.Entities[i + 1].Color = System.Drawing.Color.Black;
        //        viewPortLayout1.Entities[i + 1].Visible = true;
        //        viewPortLayout1.Invalidate();
        //        viewPortLayout1.ZoomFit();
        //    }


        //    //  viewPortLayout1.Entities.Add(e);
        //    // viewPortLayout1.Entities[0].Color = System.Drawing.Color.Black;
        //    //  viewPortLayout1.Entities[0].Visible = true;


        //}


        //private Point3D boxCenter(Entity en)
        //{
        //    Point3D result = new Point3D();
        //    Point3D.Origin.X = (en.BoxMax.X - en.BoxMin.X)/ 2;
        //    Point3D.Origin.Y = (en.BoxMax.X - en.BoxMin.X) / 2;
        //    Point3D.Origin.Z = (en.BoxMax.X - en.BoxMin.X) / 2;
        //    return result;
        //}

        //private void EventOptions(object sender, RoutedEventArgs e)
        //{
        //    ev.Show();
        //}


    }
        
   }

