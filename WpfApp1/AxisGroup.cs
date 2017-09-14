using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using Aga.Controls.Tree;
using System.Windows.Data;
using System.Threading;

namespace WpfApp1
{


    public class TLVIModel : ITreeModel
    {





        public TLVI Root { get; private set; }

        public TLVIModel()
        {
            Root = new TLVI();
        }

        public System.Collections.IEnumerable GetChildren(object parent)
        {
            if (parent == null)
                parent = Root;
            return (parent as TLVI).Children;
        }

        public bool HasChildren(object parent)
        {
            return (parent as TLVI).Children.Count > 0;
        }



    }
    public class TLVI
    {


        private readonly System.Collections.ObjectModel.ObservableCollection<TLVI> _children = new System.Collections.ObjectModel.ObservableCollection<TLVI>();
        public System.Collections.ObjectModel.ObservableCollection<TLVI> Children
        {
            get { return _children; }
        }

        // public TextBox TextBoxValue;

        public object Value { get; set; }
        public string Status { get; set; }
        public string Name { get; set; }
        public string Property { get; set; }
        public string Type { get; set; }
        public int Id { get; set; }
        public TextBlock NewValue { get; set; }

        static int _i;
        public TLVI()
        {
            Id = ++_i;

        }

        public override string ToString()
        {
            return Name;
        }
    }


    public class AxisGroup : _treeListView.TreeListViewItem
    {
        //public _treeListView.TreeListViewItem Values { get; set; }
        public string Status { get { return status; } set { this.tstatus.Value = value; status = value; } }
        private string status = "";
        public string name = "";
        private string type = "";
        private string ip;
        private int id = -1;
        //public string Name { get { return name; } set { this.tname.Name = value; name = value; } } = new string("");
        public string Type { get { return type; } set { this.ttype.Value = value; this.type = value; } }
        public int Id { get { return id; } set { id = value; } }
        public string IP { get { return ip; } set { this.tIP.Value = value; this.ip = value; } }
        public double Position { get { return position; } set { this.tposition.Value = value; position = value; } }
        public double TargetPosition;
        public TextBlock NewValue { get; set; } = new TextBlock();
        private double position;
        public string MaxValue;
        public string MinValue;
        private TLVI tstatus;
        private TLVI tname;
        private TLVI ttype;
        private TLVI tposition;
        private TLVI tMaxValue;
        private TLVI tMinValue;
        private TLVI tIP;
        public int GroupNumber;
        public int Address;

        // _treeListView.TreeListViewItem Axis;
        // public _treeListView.TreeListViewItem Axis();

        public AxisGroup(string Name, string Type, string IP, double InitialPosition, string maxValue, string minValue, string Units)
        {
            //this.Values = new _treeListView.TreeListViewItem();
            name = Name; ip = IP; position = InitialPosition; MaxValue = maxValue; MinValue = minValue;
            tname = new TLVI();
            ttype = new TLVI();
            tposition = new TLVI();
            tMaxValue = new TLVI();
            tMinValue = new TLVI();
            tIP = new TLVI();
            tstatus = new TLVI();
            tname.NewValue = new TextBlock();
            tMinValue.NewValue = new TextBlock();
            tMaxValue.NewValue = new TextBlock();
            tIP.NewValue = new TextBlock();
            tMinValue.Type = "Distance_" + Units;
            tMaxValue.Type = "Distance_" + Units;
            tposition.Type = "Distance_" + Units;
            tMinValue.Name = "Min";
            tMaxValue.Name = "Max";
            tposition.Name = "Position";
            tIP.Name = "Net Address";
            tIP.Type = "IP";
            tIP.Value = IP;
            tname.Name = Name;
            tname.Type = Type;
    
            NewValue.Text = "EAT SHIT";
            tname.NewValue = NewValue;
            tname.Value = "";
            this.Header = tname;
            tMaxValue.Value = MaxValue;
            tMinValue.Value = MinValue;


            tposition.Value = InitialPosition;
            //this.AddChild(tposition);
            //this.AddChild(tMaxValue);

            this.Items.Add(tposition);
            this.Items.Add(tMaxValue);
            this.Items.Add(tMinValue);
            // this.Values.Items.Add(ttype);
            //this.Items.Add(tstatus);
            this.Items.Add(tIP);
            this.Foreground = System.Windows.Media.Brushes.White;
            this.IsExpanded = true;
            // this.IsExpanded = true;
            //Axis.Items.Add(tname);


            //this.Name = Name;
            //this.IP = IP;
            //this.Position = InitialPosition;
            //this.MaxValue = MaxValue;
            //this.MinValue = MinValue;
            //return this;
        }
    }
   
}

 public class Tasks : System.Collections.ObjectModel.ObservableCollection<Task>
    {
        // Creating the Tasks collection in this way enables data binding from XAML.
    }