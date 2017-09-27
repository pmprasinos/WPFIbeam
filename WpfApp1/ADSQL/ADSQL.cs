using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;


    public partial  class ADSQL
    {
        public static SqlConnection MomCon = new SqlConnection("data source = MOM0\\MOMSQL; initial catalog = MomSQL; user id = pprasinos; password = Wyman123-; MultipleActiveResultSets = True; App = EntityFramework");
        public static ADSClient Mom = new ADSClient("10.99.1.1.1.1", true, 20);


        public static bool SqlWriteAxis(int axisNumber, string columnName, object newValue, bool ignoreException = false)
        {
        Stopwatch st = Stopwatch.StartNew();

        try
        {
            SqlCommand cmd = new SqlCommand("Update MomSQL..Axis set targetPositon = @newValue Where AxisNumber = @axisNumber", MomCon);

            switch (columnName.ToUpper())
            {
                case ("TARGETPOSITION"):
                    cmd = new SqlCommand("Update MomSQL..Axis set targetPosition = @newValue Where AxisNumber = @axisNumber", MomCon);
                    break;
                case ("ACCELERATION"):
                    cmd = new SqlCommand("Update MomSQL..Axis set ACCELERATION = @newValue Where AxisNumber = @axisNumber", MomCon);
                    break;
                case ("VELOCITY"):
                    cmd = new SqlCommand("Update MomSQL..Axis set VELOCITY = @newValue Where AxisNumber = @axisNumber", MomCon);
                    break;
                case ("LIVEVALUES"):
                    cmd = new SqlCommand("Update MomSQL..Axis set LiveValues = @newValue", MomCon);
                    break;
                case ("CURRENTPOSITION"):
                    cmd = new SqlCommand("Update MomSQL..Axis set currentPosition = @newValue Where AxisNumber = @axisNumber", MomCon);
                    break;
                default:
                    return false;
                    //throw new Exception("The column Name: " + columnName + " Was not defined");
            }

            cmd.Parameters.AddWithValue("@columnName", columnName);
            cmd.Parameters.AddWithValue("@newValue", newValue);
            cmd.Parameters.AddWithValue("@axisNumber", axisNumber);
         
                if (MomCon.State == ConnectionState.Closed) MomCon.Open();
                return cmd.ExecuteNonQuery() == 1;
            }
            catch (SqlException ex)
            {
            if (!ignoreException) throw ex; else return false;
            }
            finally
            {
          //  Debug.Print(columnName + "   " + newValue.ToString() + "   " + axisNumber.ToString() + "    " + st.ElapsedMilliseconds.ToString());
                MomCon.Close();
            }
        }

        public static object SqlReadAxis(int axisNumber, string columnName, bool ignoreException = false)
        {
 Stopwatch st = Stopwatch.StartNew();
        try
        {
           
            SqlCommand cmd = new SqlCommand("Select * from MomSQL..Axis Where AxisNumber = @axisNumber", MomCon);
            cmd.Parameters.AddWithValue("@axisNumber", axisNumber);
        
                if (MomCon.State == ConnectionState.Closed) MomCon.Open();
                SqlDataReader s = cmd.ExecuteReader();
                s.Read();
                return s[columnName];
            }
            catch (SqlException)
            {
            if (!ignoreException) throw; else return false;
            }
            finally
            {
           // Debug.Print(columnName + "   " + axisNumber.ToString() + "    " + st.ElapsedMilliseconds.ToString());
            MomCon.Close();
            }

        }

  
}

