using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;


namespace WpfApp1
{ 
    public static class UpdateSQL
    {

        static MomSQLDataSetTableAdapters.AxisTableAdapter ta = new MomSQLDataSetTableAdapters.AxisTableAdapter();
        public static MomSQLDataSet.AxisDataTable ADT = ta.GetData();
       // static string CmdString;
        public static void Refresh()
        {
            try
            {
                if (ta.Connection.State != ConnectionState.Open) ta.Connection.Open();

                ta.ClearBeforeFill = true;
                //ADT.Clear();
                ta.Fill(ADT);

            }
            finally { ta.Connection.Close(); }
           
                
        }

    }
}
