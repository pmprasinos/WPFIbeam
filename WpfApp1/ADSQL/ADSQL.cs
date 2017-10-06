using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;


public partial class ADSQL
{

  //  public static ADSClient Mom = new ADSClient("10.99.1.1.1.1", true, 20);
   static string MomConStr = "data source = MOM0\\MOMSQL; initial catalog = MomSQL; MultipleActiveResultSets = True; user id = pprasinos; password = Wyman123-;";

    public static bool SqlWriteAxis(int axisNumber, string columnName, object newValue, bool ignoreException = false)
    {
        Stopwatch st = Stopwatch.StartNew();

        try
        {
            using (SqlConnection MomCon = new SqlConnection("data source = MOM0\\MOMSQL; initial catalog = MomSQL; user id = pprasinos; password = Wyman123-; MultipleActiveResultSets = True; App = EntityFramework"))
            {

                SqlCommand cmd = new SqlCommand("Update MomSQL..Axis set targetPositon = @newValue Where AxisNumber = @axisNumber", MomCon);

                switch (columnName.ToUpper())
                {
                    case ("TARGETPOSITION"):
                        cmd = new SqlCommand("Update MomSQL..Axis set targetPosition = @newValue, TrimFactor = CASE when ABS(targetposition-currentposition)/5 < 100 then ABS(targetposition-currentposition)/5 else 100 end, IsActive = 1 Where AxisNumber = @axisNumber", MomCon);
                        break;
                    case ("ACCELERATION"):
                        cmd = new SqlCommand("Update MomSQL..Axis set ACCELERATION = @newValue Where AxisNumber = @axisNumber", MomCon);
                        break;
                    case ("DECELERATION"):
                        cmd = new SqlCommand("Update MomSQL..Axis set DECELERATION = @newValue Where AxisNumber = @axisNumber", MomCon);
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
                    case ("ISACTIVE"):
                        if(int.Parse(newValue.ToString())>0) cmd = new SqlCommand("Update MomSQL..Axis set IsActive = @newValue Where AxisNumber = @axisNumber", MomCon);
                        if (int.Parse(newValue.ToString()) == 0) cmd = new SqlCommand("Update MomSQL..Axis set IsActive = 0, TargetPosition = CurrentPosition", MomCon);
                        break;
                    case ("AXISSTATUS"):
                        cmd = new SqlCommand("Update MomSQL..Axis set AxisStatus = @newValue Where AxisNumber = @axisNumber", MomCon);
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
        }
        catch (SqlException ex)
        {
            if (!ignoreException) throw ex; else return false;
        }

    }

    public static void ExecuteQue(string QueName, int TrimFactor)
    {
        using (SqlConnection MomCon = new SqlConnection(MomConStr))
        {
            using (SqlCommand acmd = new SqlCommand("momsql.dbo.ExeCuteQue", MomCon))
            {
                acmd.CommandType = CommandType.StoredProcedure;
                acmd.Parameters.AddWithValue("@QueName", QueName);
                acmd.Parameters.AddWithValue("@TrimFactor", TrimFactor);
                MomCon.Open();
                acmd.ExecuteNonQuery();
                Debug.Print("TargetsWritten");
            }
        }
    }

    public static object SqlReadAxis(int axisNumber, string columnName, bool ignoreException = false)
    {
        Stopwatch st = Stopwatch.StartNew();
        using (SqlConnection MomCon = new SqlConnection("data source = MOM0\\MOMSQL; initial catalog = MomSQL; user id = pprasinos; password = Wyman123-; MultipleActiveResultSets = True; App = EntityFramework"))
        {
            try
        {
           
                SqlCommand cmd = new SqlCommand("Select * from MomSQL..Axis Where AxisNumber = @axisNumber", MomCon);
                cmd.Parameters.AddWithValue("@axisNumber", axisNumber);

                if (MomCon.State == ConnectionState.Closed) MomCon.Open();
                using (SqlDataReader s = cmd.ExecuteReader())
                {
                    s.Read();
                    return s[columnName];
                }
                    
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

  
}

