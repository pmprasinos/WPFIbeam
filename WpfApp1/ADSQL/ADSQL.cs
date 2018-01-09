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
    static string MomConStr = "data source = 10.99.1.1\\MOMSQL;  Connection Timeout=10; initial catalog = MomSQL; MultipleActiveResultSets = True; user id = pprasinos; password = Wyman123-;";

    public static bool SqlWriteAxis(int axisNumber, string columnName, object newValue, bool ignoreException = false,  string queName = "")
    {
        Stopwatch st = Stopwatch.StartNew();

        try
        {
            using (SqlConnection MomCon = new SqlConnection("data source = 10.99.1.1\\MOMSQL;  Connection Timeout=10; initial catalog = MomSQL; user id = pprasinos; password = Wyman123-; MultipleActiveResultSets = True; App = EntityFramework"))
            {
                if (newValue == null)
                    Debug.Print("STOP");
                SqlCommand cmd = new SqlCommand("Update MomSQL..Axis set targetPositon = @newValue Where AxisNumber = @axisNumber", MomCon);
                cmd.CommandTimeout = 500;
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
                        if (int.Parse(newValue.ToString()) > 0) cmd = new SqlCommand("Update MomSQL..Axis set IsActive = @newValue Where AxisNumber = @axisNumber", MomCon);
                        if (int.Parse(newValue.ToString()) == 0) cmd = new SqlCommand("Update MomSQL..Axis set IsActive = 0, TargetPosition = CurrentPosition", MomCon);
                        break;
                    case ("FAULTED"):
                        cmd = new SqlCommand("Update MomSQL..Axis set Faulted = @newValue Where AxisNumber = @axisNumber", MomCon);
                        break;
                    case ("FAULTCODE"):
                        cmd = new SqlCommand("Update MomSQL..Axis set FaultCode = @newValue Where AxisNumber = @axisNumber", MomCon);
                        break;
                   

                    default:
                        return false;
                        //throw new Exception("The column Name: " + columnName + " Was not defined");
                }

                cmd.Parameters.AddWithValue("@columnName", columnName);
                cmd.Parameters.AddWithValue("@newValue", newValue);
                cmd.Parameters.AddWithValue("@axisNumber", axisNumber);
                cmd.CommandType = CommandType.Text;

                if (MomCon.State == ConnectionState.Closed) MomCon.Open();


                if (queName != null && queName != "")
                {
                    
                    cmd.ExecuteNonQuery();
                    cmd = new SqlCommand("Update momsql..ques set " + columnName + " = @newValue Where QueName = @QueName and AxisNum = @AxisNumber", MomCon);
                    cmd.Parameters.AddWithValue("@newValue", newValue);
                    cmd.Parameters.AddWithValue("@AxisNumber", axisNumber);
                    cmd.Parameters.AddWithValue("@QueName", queName);
                   
                    return cmd.ExecuteNonQuery() == 1;
                }
                else return cmd.ExecuteNonQuery() == 1;
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
                acmd.CommandTimeout = 500;
                acmd.CommandType = CommandType.StoredProcedure;
                acmd.Parameters.AddWithValue("@ShowSpace", WpfApp1.Properties.Settings.Default.ShowSpace);
                acmd.Parameters.AddWithValue("@QueName", QueName);
                acmd.Parameters.AddWithValue("@TrimFactor", TrimFactor);
                MomCon.Open();
                acmd.ExecuteNonQuery();
                MomCon.Close();
            }
        }
    }

    public static bool ExecuteJog(string AxisName, int TrimFactor, int JogSpeed, float JogAccel, int JogPosition)
    {
        using (SqlConnection MomCon = new SqlConnection(MomConStr))
        {
            using (SqlCommand acmd = new SqlCommand("momsql.dbo.ExeCuteJog", MomCon))
            {
                acmd.CommandTimeout = 500;
                acmd.CommandType = CommandType.StoredProcedure;
                acmd.Parameters.AddWithValue("@AxisName", AxisName);
                acmd.Parameters.AddWithValue("@TrimFactor", TrimFactor);
                acmd.Parameters.AddWithValue("@JogAccel", JogAccel);
                acmd.Parameters.AddWithValue("@JogSpeed", JogSpeed);
                acmd.Parameters.AddWithValue("@RelativeMoveDist", JogPosition);
                MomCon.Open();
                int j = acmd.ExecuteNonQuery();
                return j == 1;
            }
        }
    }

    public static int QueWatchDog(string queName, int TrimFactor)
    {
        using (SqlConnection MomCon = new SqlConnection(MomConStr))
        {
            using (SqlCommand acmd = new SqlCommand("momsql.dbo.QueWatchDog", MomCon))
            {
                acmd.CommandTimeout = 500;
                acmd.CommandType = CommandType.StoredProcedure;
                acmd.Parameters.AddWithValue("@QueName", queName);
                acmd.Parameters.AddWithValue("@ShowSpace", WpfApp1.Properties.Settings.Default.ShowSpace);
                acmd.Parameters.AddWithValue("@TrimFactor", TrimFactor);
                MomCon.Open();
                int j = acmd.ExecuteNonQuery();
                MomCon.Close();
                return j;
            }
        }
    }

    public static object SqlReadAxis(int axisNumber, string columnName, bool ignoreException = false)
    {
        Stopwatch st = Stopwatch.StartNew();
        using (SqlConnection MomCon = new SqlConnection("data source = 10.99.1.1\\MOMSQL;   Connection Timeout=10; initial catalog = MomSQL; user id = pprasinos; password = Wyman123-; MultipleActiveResultSets = True; App = EntityFramework"))
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Select " + columnName + " from MomSQL..Axis Where AxisNumber = @axisNumber", MomCon);
                cmd.Parameters.AddWithValue("@axisNumber", axisNumber);
                cmd.CommandTimeout = 500;
                if (MomCon.State == ConnectionState.Closed) MomCon.Open();
                using (SqlDataReader s = cmd.ExecuteReader())
                {
                    s.Read();
                    string g = s[columnName].ToString();
                    return s[columnName];
                }
            }
            catch (SqlException e)
            {
                if (e.ErrorCode == -2146232060) return null;
                return false;
            }
        }
    }

    public static object SqlReadQue(String QueName, string columnName, bool ignoreException = false)
    {
        Stopwatch st = Stopwatch.StartNew();
        using (SqlConnection MomCon = new SqlConnection("data source = 10.99.1.1\\MOMSQL;  Connection Timeout=10; initial catalog = MomSQL; user id = pprasinos; password = Wyman123-; MultipleActiveResultSets = True; App = EntityFramework"))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("Select * from MomSQL..Ques Where QueName = @queName and ShowSpace = 'SECONDSHOW'", MomCon))
                {
                    cmd.CommandTimeout = 500;
                    cmd.Parameters.AddWithValue("@QueName", QueName);
                    MomCon.Open();
                    using (SqlDataReader s = cmd.ExecuteReader())
                    {
                        if(s.HasRows)
                        { 
                        s.Read();
                        if (s[columnName].ToString() == "")
                        {
                            return "";
                        }
                       
                        return s[columnName];
                        }else
                        {
                            return false;
                        }
                    }
                    
                }
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                if (e.ErrorCode == -2146232060) return null;
                 else return false;
            }

        }
    }
}

