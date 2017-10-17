using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Diagnostics;


namespace AdsWriter
{

    public partial class Program
    {

        // public static ADSClient Mom; //= new ADSClient("10.99.1.1.1.1", true, 20);
        // public static AdSqlAxis[] KidAxis = new AdSqlAxis[6];
        static DataRowCollection AxisRows;

        public static void Main(string[] args)
        {
           

                bool[] isActive = new bool[6];
                int[] DeadCounter = new int[6];
               
                    try
                    {
                        int InstanceCount = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length;
                        if (InstanceCount > 2) return;
                    if (InstanceCount > 1) do {  InstanceCount = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length; Thread.Sleep(200); } while (InstanceCount > 1);
                            if (InstanceCount > 1) Thread.Sleep(5000);
                        Console.Write("STARTING CONNECTION TO MOM...");

                        ADSClient Mom = new ADSClient(new TwinCAT.Ads.AmsAddress("10.99.1.1.1.1", 851).NetId.ToString(), true, 6);
                        Console.Write("DONE");
                        Stopwatch j = Stopwatch.StartNew();
                        int loopCount = 0;
                    

                        do
                        {

          
                            loopCount++; Debug.Print(loopCount.ToString());
                            if (loopCount > 0) { Console.WriteLine(loopCount.ToString() + "   " + j.ElapsedMilliseconds.ToString()); j = Stopwatch.StartNew(); }
                            Mom.WriteValue(Mom.TimeOutSwitch, false);
                            AxisRows = (DataRowCollection)ADSQL.SqlPullAxis();
                            Thread.Sleep(50);
                            
                            bool GlobalReset = (string)ADSQL.SqlReadAxis(1, "AxisStatus")=="RESET";
                        
                        if ((string)ADSQL.SqlReadAxis(1, "AxisStatus") == "FAULT") Mom.WriteValue(Mom.GlobalEstop, true);
                       
                        

                        if (GlobalReset)
                        {
                            Mom.WriteValue(Mom.FaultReset, true);
                        }
 bool GlobalEstop = (int)Mom.ReadValue(Mom.GlobalEstop) > 0;

                        for (int x = 1; x <= 6; x++)
                            {
                            if (GlobalReset)
                            {
                                ADSQL.SqlWriteAxis(x, "AxisStatus", "OK");
                            }

                            string FaultStatus = (string)ADSQL.SqlReadAxis(x, "AxisStatus");
                           
                            if (loopCount > 200) loopCount = 0;
                             
                              
                                bool QueStarted = (bool)ADSQL.SqlReadAxis(x, "QueStarted");
                            if (QueStarted) { Mom.Kids[x - 1].MomControl = 0; ADSQL.SqlWriteAxis(x, "QueStarted", false); }
                            bool thisActive = (bool)ADSQL.SqlReadAxis(x, "IsActive");
                            int KidCurrent = Mom.Kids[x - 1].CurrentPosition;
                            ADSQL.SqlWriteAxis(x, "CurrentPosition", KidCurrent);
                            int KidTarget;

                            if (thisActive)
                                {
                                    ADSQL.SqlWriteAxis(x, "IsActive", 0);
                                    Mom.Kids[x - 1].DeadManPressed = 1;
                                KidTarget = Mom.Kids[x - 1].TargetPosition;
                                int KidSqlTarget = (int)ADSQL.SqlReadAxis(x, "TargetPosition");

                                int scaler = Math.Abs(KidCurrent - KidSqlTarget) / 5;
                                Debug.Print(scaler.ToString());
                                Mom.Kids[x - 1].ScalingInt = (int)ADSQL.SqlReadAxis(x, "TrimFactor"); ;


                                if (KidSqlTarget != KidTarget) Mom.Kids[x - 1].TargetPosition = KidSqlTarget;
                            }

                                if (isActive[x - 1] == thisActive && !thisActive)
                                {
                                    DeadCounter[x - 1]++;
                                    if (DeadCounter[x - 1] > 3 && DeadCounter[x - 1] < 5) Mom.Kids[x - 1].DeadManPressed = 0;
                                }
                                else DeadCounter[x - 1] = 0;
                               


                           

                                int KidAccel = Mom.Kids[x - 1].ModeAccel;
                                int KidDecel = Mom.Kids[x - 1].ModeDecel;
                                int KidVel = Mom.Kids[x - 1].ModeVel;

                                int KidSqlDecel = (int)ADSQL.SqlReadAxis(x, "Deceleration");
                                int KidSqlAccel = (int)ADSQL.SqlReadAxis(x, "Acceleration");
                                int KidSqlVel = (int)ADSQL.SqlReadAxis(x, "Velocity");
                                bool JogMode = (bool)ADSQL.SqlReadAxis(x, "JogMode");
                                if (JogMode)
                                {

                                    int accelRate = (int)((int)ADSQL.SqlReadAxis(x, "JogSpeed") / (double)ADSQL.SqlReadAxis(x, "JogAccel"));
                                   
                                    KidSqlAccel = accelRate;
                                    if (accelRate < 400) accelRate = 400;
                                    KidSqlDecel = accelRate;
                                    KidSqlVel = (int)ADSQL.SqlReadAxis(x, "JogSpeed");
                                }

                                if (KidSqlAccel != KidAccel) Mom.Kids[x - 1].ModeAccel = KidSqlAccel;
                                if (KidSqlDecel != KidDecel) Mom.Kids[x - 1].ModeDecel = KidSqlDecel;
                                if (KidSqlVel != KidVel) Mom.Kids[x - 1].ModeVel = KidSqlVel;
                            
                        

                            
                                if (isActive[x - 1])
                                {
                                   
                                    
                                    

                                }
                                else 
                                {
                                    KidTarget = KidCurrent;
                                    Mom.Kids[x - 1].TargetPosition = Mom.Kids[x - 1].CurrentPosition;
                                }

                            if (Mom.Kids[x - 1].MomControl == 0) ADSQL.SqlWriteAxis(x, "AxisStatus", "OFFLINE"); else if (GlobalEstop && !GlobalReset) ADSQL.SqlWriteAxis(x, "AxisStatus", "FAULT");



                               
                                if (QueStarted) Mom.Kids[x - 1].MomControl = 1;
                            isActive[x - 1] = thisActive;
                        }
                           

                        } while (true);
                    }
                    catch
                    {
                        Thread.Sleep(20000); Console.WriteLine("Error");
                        Environment.Exit(0);

                    }
              

          
           
        }

        public static class ADSQL
        {
            public static void WatchDog()
            {
                //using (SqlCommand CMD = new SqlCommand("Exec MOMSQL..QueWatchDog", MomCon))
                //{
                //    if (MomCon.State == ConnectionState.Closed) MomCon.Open();
                //    CMD.ExecuteNonQuery();
                    
                //}
            }
            public static bool SqlWriteAxis(int axisNumber, string columnName, object newValue)
            {
                if (SqlReadAxis(axisNumber, columnName) == newValue) return true;
                using (SqlConnection MomCon = new SqlConnection("data source = MOM0\\MOMSQL; initial catalog = MomSQL; user id = pprasinos; password = Wyman123-; MultipleActiveResultSets = True; App = EntityFramework"))
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
                        case ("CURRENTPOSITION"):
                            cmd = new SqlCommand("Update MomSQL..Axis set currentPosition = @newValue Where AxisNumber = @axisNumber", MomCon);
                            break;
                        case ("ISACTIVE"):
                            cmd = new SqlCommand("Update MomSQL..Axis set IsActive = @newValue Where AxisNumber = @axisNumber", MomCon);
                            break;
                        case ("AXISSTATUS"):
                         cmd = new SqlCommand("Update MomSQL..Axis set AxisStatus = @newValue Where AxisNumber = @axisNumber", MomCon);
                            break;
                        case ("QUESTARTED"):
                            cmd = new SqlCommand("Update MomSQL..Axis set QUESTARTED = @newValue Where AxisNumber = @axisNumber", MomCon);
                            break;
                        default:
                            return false;
                            //throw new Exception("The column Name: " + columnName + " Was not defined");
                    }
               
                cmd.Parameters.AddWithValue("@columnName", columnName);
                cmd.Parameters.AddWithValue("@newValue", newValue);
                cmd.Parameters.AddWithValue("@axisNumber", axisNumber);
            MomCon.Open();
                    bool res;
                    try { res =cmd.ExecuteNonQuery() == 1; }
                    catch { Thread.Sleep(1); res = cmd.ExecuteNonQuery() == 1; }



                    MomCon.Close();
                    return res;
                
                }
            }

          

            public static object SqlPullAxis()
            {
                SqlDataReader s;
                using (SqlConnection MomCon = new SqlConnection("data source = MOM0\\MOMSQL; initial catalog = MomSQL; user id = pprasinos; password = Wyman123-; MultipleActiveResultSets = True; App = EntityFramework"))
                {
                    SqlCommand cmd = new SqlCommand("Select * from MomSQL..Axis", MomCon);
                    try
                    {
                        if (MomCon.State == ConnectionState.Closed) MomCon.Open();
                        s = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                        //s.Read();
                        DataTable dt = new DataTable();
                        dt.Load(s);
                        return dt.Rows;
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }
                   
                }
                return s;
            }

    
            public static object SqlReadAxis(int AxisNumber, string columnName)
            {
                        return AxisRows[AxisNumber-1][columnName];
            }

        }

        public class AdSqlAxis
        {
            string TableName;
            string ColumnName;


            AxisParam ID;
            AxisParam AxisNumber;
            AxisParam AxisName;
            AxisParam AxisType;
            AxisParam AdminMax;
            AxisParam AdminMin;
            AxisParam UserMax;
            AxisParam UserMin;
           AxisParam CurrentPosition;
           public AxisParam TargetPosition;
            AxisParam Acceleration;
            AxisParam Velocity;
            AxisParam AxisGroup;
            AxisParam AxisGroupName;
            AxisParam IpAddress;
            AxisParam NetID;
            AxisParam AxisStatus;
            AxisParam Faulted;
            AxisParam FaultCode;
            AxisParam UserCanEdit;

            public AdSqlAxis(int AxisNum, ADSClient Mom)
            {
                //AxisParam ID = new AxisParam(AxisNum, "ID", 1000, 0);
                //AxisParam AxisNumber = new AxisParam(AxisNum, "AxisNumber", 1000, 0);
                //AxisParam AxisName = new AxisParam(AxisNum, "AxisName", 1000, 0);
               // AxisParam AxisType = new AxisParam(AxisNum, "AxisType", 1000, 0);
               // AxisParam AdminMax = new AxisParam(AxisNum, "AdminMax", 1000, 0);
                //AxisParam AdminMin = new AxisParam(AxisNum, "AdminMin", 1000, 0);
                //AxisParam UserMax = new AxisParam(AxisNum, "UserMax", 1000, 0);
                //AxisParam UserMin = new AxisParam(AxisNum, "UserMin", 1000, 0);
                AxisParam CurrentPosition = new AxisParam(AxisNum, "CurrentPosition", 1000, 0, Mom);
                AxisParam TargetPosition = new AxisParam(AxisNum, "TargetPosition", 1000, 0, Mom);
               // AxisParam Acceleration = new AxisParam(AxisNum, "Acceleration", 1000, 0);
                //AxisParam Velocity = new AxisParam(AxisNum, "Velocity", 1000, 0);
                //AxisParam AxisGroup = new AxisParam(AxisNum, "AxisGroup", 1000, 0);
                //AxisParam AxisGroupName = new AxisParam(AxisNum, "AxisGroupName", 1000, 0);
               // AxisParam IpAddress = new AxisParam(AxisNum, "IpAddress", 1000, 0);
               // AxisParam NetID = new AxisParam(AxisNum, "NetID", 1000, 0);
               // AxisParam AxisStatus = new AxisParam(AxisNum, "AxisStatus", 1000, 0);
              //  AxisParam Faulted = new AxisParam(AxisNum, "Faulted", 1000, 0);
             //   AxisParam FaultCode = new AxisParam(AxisNum, "FaultCode", 1000, 0);
                //AxisParam UserCanEdit = new AxisParam(AxisNum, "UserCanEdit", 1000, 0);
            }

        }

        public class AxisParam
        {
            public int ReadOnly;
            public int WriteOnly;
            int AxisNum;
            int ReadInterval;
            int WriteInterval;
            private bool ReadPending;
            private bool WritePending;
            String ColumnName;
            public object WriteValue;
            Object currentValue;
            Object Value
            {
                get
                {
                    currentValue =ADSQL.SqlReadAxis(AxisNum, ColumnName);
                    return currentValue;
                }
                set
                {
                    ADSQL.SqlWriteAxis(AxisNum, ColumnName, value);
                }
            }
            public Stopwatch ReadTimer = new Stopwatch();
            public Stopwatch WriteTimer = new Stopwatch();
            Task ScanTask;
            bool Running = true;

            public AxisParam(int axisNum, string columnName, int readInterval, int writeInterval, ADSClient Mom, Object InitValue = null )
            {
                AxisNum = axisNum;
                ReadInterval = readInterval;
                ColumnName = columnName;
                ReadTimer = Stopwatch.StartNew();
                if (writeInterval != 0) WriteTimer = Stopwatch.StartNew();
              //  ScanTask = new Task(() => ADSQLScan(Mom));
                if (InitValue == null)
                {
                    ADSQL.SqlWriteAxis(AxisNum, ColumnName, ReadKidValue(AxisNum, ColumnName, Mom, false));
                    currentValue = ReadKidValue(axisNum, columnName, Mom);
                }

                //ADSQLScan();
               // ScanTask.Start();
            }

            public object ReadKidValue(int axisNum, string ColumnName, ADSClient Mom, bool trysql = true )
            {
                object currentValue;
                if (ColumnName.ToUpper() == "TARGETPOSITION") currentValue = Mom.Kids[axisNum].TargetPosition;
                else if (ColumnName.ToUpper() == "CURRENTPOSITION") currentValue = Mom.Kids[AxisNum].CurrentPosition;
                else if (ColumnName.ToUpper() == "FAULTED") currentValue = Mom.ReadValue(Mom.GlobalEstop);
                else if (ColumnName.ToUpper() == "ACCELERATION") currentValue = Mom.Kids[AxisNum].ModeAccel;
                else if (ColumnName.ToUpper() == "VELOCITY") currentValue = Mom.Kids[AxisNum].ModeVel;
                else currentValue = ADSQL.SqlReadAxis(axisNum, ColumnName);
                return currentValue;
            }

            public object WriteKidValue(int axisNum, string ColumnName, object newValue, ADSClient Mom)
            {
                if (ColumnName.ToUpper() == "TARGETPOSITION")  Mom.Kids[axisNum].TargetPosition = (int)newValue;
                else if (ColumnName.ToUpper() == "ACCELERATION")  Mom.Kids[AxisNum].ModeAccel = (int)newValue;
                else if (ColumnName.ToUpper() == "VELOCITY")  Mom.Kids[AxisNum].ModeVel = (int)newValue;

                else currentValue = ADSQL.SqlReadAxis(axisNum, ColumnName);
                return currentValue;
            }

            void ADSQLScan(ADSClient Mom)
            {
                do
                {
                    Thread.Sleep(10);
                    if (ReadTimer.ElapsedMilliseconds > ReadInterval)
                    {
                        ReadTimer = Stopwatch.StartNew();
                        ReadTimer.Stop();
                        currentValue = ADSQL.SqlReadAxis(AxisNum, ColumnName);
                        if (currentValue != WriteValue && WriteValue != null)
                        {
                            WriteKidValue(AxisNum, ColumnName, WriteValue, Mom);
                        }
                         ADSQL.SqlWriteAxis(AxisNum, ColumnName, ReadKidValue(AxisNum, ColumnName, Mom, false));
                        ReadTimer = Stopwatch.StartNew();
                    }
                 


                        } while (Running);
            }
        }

    }
}



