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

        public static MomSQLDataSet.AxisDataTable ADT;
       // static string CmdString;
        public static void Refresh()
        {
            try
            {
                using (MomSQLDataSetTableAdapters.AxisTableAdapter ta = new MomSQLDataSetTableAdapters.AxisTableAdapter())
                {
                     ta.Connection.Open();
                    ADT = ta.GetData();
                    ta.ClearBeforeFill = true;
                    //ADT.Clear();
                    ta.Fill(ADT);
                    ta.Connection.Close();

                }

            }
            catch(SqlException e)
            {
                if (e.ErrorCode == -2146232060) return;
            }
           
                
        }

    }
}
