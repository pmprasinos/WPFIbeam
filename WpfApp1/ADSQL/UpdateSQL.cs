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
       public static String queryStatus= "Querying Axis...";
        public static MomSQLDataSet.AxisDataTable ADT;
       // static string CmdString;
        public static void Refresh()
        {
            long t = DateTime.Now.Ticks;
           
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
                WriteLog(Environment.GetFolderPath(Environment.SpecialFolder.Desktop ) + "\\LOGFILE.txt", e.ToString());
                queryStatus = "Querying Axis...ERROR";
                if (e.ErrorCode == -2146232060) return;
            }
            queryStatus = "Querying Axis...DONE";
           if((DateTime.Now.Ticks - t)/TimeSpan.TicksPerMillisecond > 30) WriteLog(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\LOGFILE.txt", ((DateTime.Now.Ticks - t)/TimeSpan.TicksPerMillisecond).ToString());

        }
        static void WriteLog(String Path, String LogText)
        {


            string path = Path;
            // This text is added only once to the file.

            if (!System.IO.File.Exists(path))
            {
                // Create a file to write to.
                using (System.IO.StreamWriter sw = System.IO.File.CreateText(path))
                {
                    sw.Write(DateTime.Now.ToLongTimeString());
                    sw.Write("  ||  ");
                    sw.WriteLine(LogText);
                    sw.Close();
                }
            }

            // This text is always added, making the file longer over time
            // if it is not deleted.
            using (System.IO.StreamWriter sw = System.IO.File.AppendText(path))
            {
                sw.Write(DateTime.Now.ToLongTimeString());
                sw.Write("  ||  ");
                sw.WriteLine(LogText);
                sw.Close();
            }
           

        }
    }

   
}
