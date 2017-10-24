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
        static int t_Kid  = 0;

        static bool GlobalEstop;
        static ADSClient Mom;


            public static void Main(string[] args)
        {
            Console.WriteLine("VERSION 6");
           
            bool[] isActive = new bool[6];
                int[] DeadCounter = new int[6];
               
                    try
                    {
                AxisRows = (DataRowCollection)ADSQL.SqlPullAxis();
              
                System.Timers.Timer tSaftey = new System.Timers.Timer();
                tSaftey.Elapsed += new System.Timers.ElapsedEventHandler(tSaftey_Tick);
                tSaftey.Interval = 300; 

                int InstanceCount = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length;
                        if (InstanceCount > 2) return;
                             
                     
                if (InstanceCount > 1) do { InstanceCount = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length; Thread.Sleep(200); } while (InstanceCount > 1);

                if (InstanceCount > 1) Thread.Sleep(5000);
                Console.Write("STARTING CONNECTION TO MOM...");

                Mom = new ADSClient(new TwinCAT.Ads.AmsAddress("10.99.1.1.1.1", 851).NetId.ToString(), 6);
                tSaftey.Start();
                Console.Write("DONE");
                System.Timers.Timer tSlow = new System.Timers.Timer();
                tSlow.Elapsed += new System.Timers.ElapsedEventHandler(T_tick);

                Stopwatch j = Stopwatch.StartNew();
                        int loopCount = 0;
                tSlow.Enabled = true;
                tSlow.Interval = 107;
                Thread.Sleep(100);
                tSlow.AutoReset = true;
                tSlow.Start();

                do
                {
                    AxisRows = (DataRowCollection)ADSQL.SqlPullAxis();
                    loopCount++; Debug.Print(loopCount.ToString());
                           // if (loopCount > 0) { Console.WriteLine(loopCount.ToString() + "   " + j.ElapsedMilliseconds.ToString()); j = Stopwatch.StartNew(); }
                            Mom.WriteValue(Mom.TimeOutSwitch, false);
                    Thread.Sleep(51);
                    for (int x = 1; x <= 6; x++)
                            {
                            if (loopCount > 200) loopCount = 0;
                                bool QueStarted = (bool)ADSQL.SqlReadAxis(x, "QueStarted");
                            if (QueStarted) {
                                                Mom.Kids[x - 1].ModeAccel = (int)ADSQL.SqlReadAxis(x, "Acceleration");
                                                Mom.Kids[x - 1].ModeDecel = (int)ADSQL.SqlReadAxis(x, "Deceleration");
                            Mom.Kids[x - 1].ModeVel = (int)ADSQL.SqlReadAxis(x, "Velocity");
                            ADSQL.SqlWriteAxis(x, "QueStarted", 0);
                        } else
                        {
                            var isthis = ADSQL.SqlReadAxis(x, "IsActive");
                            bool thisActive;
                        if (x % 2 == 0)
                            {
                                thisActive = (bool)ADSQL.SqlReadAxis(x, "IsActive") || (bool)ADSQL.SqlReadAxis(x - 1, "IsActive") ;
                            }else
                            {
                                thisActive = (bool)ADSQL.SqlReadAxis(x, "IsActive") || (bool)ADSQL.SqlReadAxis(x + 1, "IsActive"); 
                            }
                            int KidCurrent = Mom.Kids[x - 1].CurrentPosition;
                            ADSQL.SqlWriteAxis(x, "CurrentPosition", KidCurrent);
                           

                            if (thisActive || isActive[x - 1])
                                {
                            Console.WriteLine("ISACTIVE: " + x);
                            
                                  if(!GlobalEstop)  Mom.Kids[x - 1].DeadManPressed = 1;
                               // KidTarget = Mom.Kids[x - 1].TargetPosition;
                                int KidSqlTarget = (int)ADSQL.SqlReadAxis(x, "TargetPosition");
                                ADSQL.SqlWriteAxis(x, "IsActive", false);
                                //int scaler = Math.Abs(KidCurrent - KidSqlTarget) / 5;
                                //Debug.Print(scaler.ToString());
                                Mom.Kids[x - 1].ScalingInt = (int)ADSQL.SqlReadAxis(x, "TrimFactor"); 
                                 Mom.Kids[x - 1].TargetPosition = KidSqlTarget;
                            }

                                if (isActive[x - 1] == thisActive && !thisActive)
                                {
                                    DeadCounter[x - 1]++;
                                    if (DeadCounter[x - 1] > 3 && DeadCounter[x - 1] < 5)
                                    {
                                        Console.WriteLine("DEADMAN0");
                                        Mom.Kids[x - 1].DeadManPressed = 0;
                                        Mom.Kids[x - 1].TargetPosition = Mom.Kids[x - 1].CurrentPosition;
                                    ADSQL.SqlWriteAxis(x, "TargetPosition", KidCurrent);
                                    }
                                }
                                else DeadCounter[x - 1] = 0;
                          
                            isActive[x - 1] = thisActive;
                        }
                    }   
                } while (true);
                    }
                    catch
                    {
                        Thread.Sleep(20000); Console.WriteLine("Error");
                        Environment.Exit(0);
                    }             
            }

        static void tSaftey_Tick(object myObject, EventArgs e)
            {
            int s = Convert.ToInt16(ADSQL.SqlRead("SELECT Faulted From Axis Where AxisNumber = 1"));
            Thread.Sleep(5);
            if (s == 1)
            {
                if (GlobalEstop)
                {
                    Mom.WriteValue(Mom.GlobalEstop, true);
                    Console.WriteLine("ESTOPACTIVATED");
                    ((System.Timers.Timer)myObject).Interval = 300;
                }
                else
                {
                   // for (int x = 0; x < 6; x++) Mom.Kids[x].DeadManPressed = 0;
                    ((System.Timers.Timer)myObject).Interval = 250;
                }
                GlobalEstop = true;
                
               // return;
            }else
            {
                
                GlobalEstop = (int)Mom.ReadValue(Mom.GlobalEstop) > 0;
                if (GlobalEstop) ADSQL.SqlWrite("UPDATE MOMSQL..AXIS SET FAULTED = 1 where AxisNumber = 1");
            }
            Thread.Sleep(2);
            s = Convert.ToInt16(ADSQL.SqlRead("SELECT FaultCode From Axis Where AxisNumber = 1"));
            if (s ==2)
            {               
                Mom.WriteValue(Mom.FaultReset, true);
                ADSQL.SqlWrite("Update MOMSQL..axis set FaultCode = 0, Faulted = 0"); Console.WriteLine("SAFTEYRESET");
                Thread.Sleep(100);
                return;
            }

            if(AxisRows != null)
            {
                for (int x = 0; x < 6; x++)
                {
                    long CheckInt = 0;// (long)ADSQL.SqlReadAxis(x + 1, "StatusWord");
                    bool SoftDown = (CheckInt % 32768) / 16384 > 0;
                    bool SoftUp = (CheckInt % 16384) / 8192 > 0;
                    bool HardDown = (CheckInt % 8192)/ 4096 > 0;
                    bool HardUp = (CheckInt % 4096) / 2048 > 0;
                    bool Ultimate = (CheckInt % 2048) / 1024 > 0;
                    bool kidEstopPressed = (CheckInt % 1024) / 512 > 0;
                    bool mcByPass = (CheckInt % 256) / 128 > 0;
                    bool VoltageEnabled = (CheckInt % 64) / 32 > 0;
                    bool SafeyTrip = (CheckInt % 16) / 8 > 0;
                    ////                  BOOL_TO_UDINT(KIDAXIS.modeTargetPos = gvl_iNDEX.Admin_MIN) * 16384 +
                    //BOOL_TO_UDINT(KIDAXIS.modeTargetPos = gvl_iNDEX.Admin_Max) * 8192 +
                    //BOOL_TO_UDINT(gvl_sAFE_io.bLimit_Hard_Down) * 4096 +
                    //BOOL_TO_UDINT(gvl_sAFE_io.bLimit_Hard_uP) * 2048 +
                    //BOOL_TO_UDINT(gvl_sAFE_io.bLimiT_uLTIMATE) * 1024 +
                    //                                        BOOL_TO_UDINT(NOT GVL_Safe_IO.bES_Local_Status) * 512 +
                    //                                        BOOL_TO_UDINT(momControl) * 128 +
                    //                                        BOOL_TO_UDINT(GVL_Safe_IO.bSlack_Line) * 64 +
                    //                                        
                    //                                        BOOL_TO_UDINT(GVL_PLC.bComTimeOutError) * 16 +
                    //                                        BOOL_TO_UDINT(SafteyTripped) * 8 +
                    //                                        BOOL_TO_UDINT(Hard_Limit_Tripped) * 4 +
                    //                                        BOOL_TO_UDINT(LeaveEnabled) * 2 +
                    //                                        BOOL_TO_UDINT(NOT HeartBeat);
                }
            }
           
     
        }

         static void T_tick(object myObject, EventArgs e)
        {
            if (GlobalEstop) return;
            ((System.Timers.Timer)myObject).AutoReset = true;
            ((System.Timers.Timer)myObject).Interval = 180;
            if (t_Kid > 5) t_Kid = 0;
            t_Kid++;
            int x = t_Kid;
            //int KidAccel = Mom.Kids[x - 1].ModeAccel;
            //Thread.Sleep(2);
            //int KidDecel = Mom.Kids[x - 1].ModeDecel;
            //Thread.Sleep(2);
            //int KidVel = Mom.Kids[x - 1].ModeVel;
            //Thread.Sleep(2);
            int KidSqlDecel = (int)ADSQL.SqlReadAxis(x, "Deceleration");
            Thread.Sleep(2);
            int KidSqlAccel = (int)ADSQL.SqlReadAxis(x, "Acceleration");
            Thread.Sleep(2);
            int KidSqlVel = (int)ADSQL.SqlReadAxis(x, "Velocity");
            Thread.Sleep(2);


            //if (KidSqlAccel != KidAccel) Mom.Kids[x - 1].ModeAccel = KidSqlAccel;
            Mom.Kids[x - 1].ModeAccel = KidSqlAccel;
            Thread.Sleep(2);
            //  if (KidSqlDecel != KidDecel ) Mom.Kids[x - 1].ModeDecel = KidSqlDecel;
            Mom.Kids[x - 1].ModeDecel = KidSqlDecel;
            Thread.Sleep(2);
            //if (KidSqlVel != KidVel) Mom.Kids[x - 1].ModeVel = KidSqlVel;
            Mom.Kids[x - 1].ModeVel = KidSqlVel;
            Thread.Sleep(2);
            Console.WriteLine(x);
            //ADSQL.SqlWriteAxis(x, "StatusWord", Mom.Kids[x - 1].StatusWord);
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
                        case ("DECELERATION"):
                            cmd = new SqlCommand("Update MomSQL..Axis set DeCELERATION = @newValue Where AxisNumber = @axisNumber", MomCon);
                            break;
                        case ("VELOCITY"):
                            cmd = new SqlCommand("Update MomSQL..Axis set VELOCITY = @newValue Where AxisNumber = @axisNumber", MomCon);
                            break;
                        case ("CURRENTVELOCITY"):
                            cmd = new SqlCommand("Update MomSQL..Axis set VELOCITY = @newValue Where AxisNumber = @axisNumber", MomCon);
                            break;
                        case ("CURRENTPOSITION"):
                            cmd = new SqlCommand("Update MomSQL..Axis set currentPosition = @newValue Where AxisNumber = @axisNumber", MomCon);
                            break;
                        case ("ISACTIVE"):
                            cmd = new SqlCommand("Update MomSQL..Axis set IsActive = @newValue Where AxisNumber = @axisNumber", MomCon);
                            break;
                        case ("Faulted"):
                         cmd = new SqlCommand("Update MomSQL..Axis set Faulted = @newValue Where AxisNumber = @axisNumber", MomCon);
                            break;
                        case ("QUESTARTED"):
                            cmd = new SqlCommand("Update MomSQL..Axis set QUESTARTED = @newValue Where AxisNumber = @axisNumber", MomCon);
                            break;
                        case ("STATUSWORD"):
                            cmd = new SqlCommand("Update MomSQL..Axis set STATUSWORD = @newValue Where AxisNumber = @axisNumber", MomCon);
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
                    catch {
                        Console.WriteLine("Error encountered writing " + newValue.ToString() + " To Column " + columnName.ToString() + " on Axis " + axisNumber.ToString());
                        Thread.Sleep(1); res = cmd.ExecuteNonQuery() == 1; }



                    MomCon.Close();
                    return res;
                
                }
            }

          

            public static object SqlPullAxis()
            {
                SqlDataReader s;
                using (SqlConnection MomCon = new SqlConnection("data source = MOM0\\MOMSQL; initial catalog = MomSQL; user id = pprasinos; password = Wyman123-; MultipleActiveResultSets = True; App = EntityFramework"))
                {
                    using (SqlCommand cmd = new SqlCommand("Select AxisNumber, Targetposition, Acceleration, Deceleration, Velocity, IsActive, StatusWord, QueStarted, CurrentPosition, Faulted, TrimFactor, FaultCode from MomSQL..Axis", MomCon))
                        {
                            if (MomCon.State == ConnectionState.Closed) MomCon.Open();
                            s = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                            //s.Read();
                            DataTable dt = new DataTable();
                            dt.Load(s);
                            return dt.Rows;
                        }
                }
                return s;
            }
            public static object SqlRead(string CmdString)
            {
                using (SqlConnection MomCon = new SqlConnection("data source = MOM0\\MOMSQL; initial catalog = MomSQL; user id = pprasinos; password = Wyman123-; MultipleActiveResultSets = True; App = EntityFramework"))
                {
                    using (SqlCommand cmd = new SqlCommand(CmdString, MomCon))
                    {
                        if (MomCon.State == ConnectionState.Closed) MomCon.Open();
                        return cmd.ExecuteScalar();
                    }
                }
            }

            public static object SqlWrite(string CmdString)
            {
                using (SqlConnection MomCon = new SqlConnection("data source = MOM0\\MOMSQL; initial catalog = MomSQL; user id = pprasinos; password = Wyman123-; MultipleActiveResultSets = True; App = EntityFramework"))
                {
                    using (SqlCommand cmd = new SqlCommand(CmdString, MomCon))
                    {
                        if (MomCon.State == ConnectionState.Closed) MomCon.Open();
                        return cmd.ExecuteNonQuery();
                    }
                }
            }

            public static object SqlReadAxis(int AxisNumber, string columnName)
            {
                        return AxisRows[AxisNumber-1][columnName];
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
                else if (ColumnName.ToUpper() == "DECELERATION") currentValue = Mom.Kids[AxisNum].ModeDecel;
                else if (ColumnName.ToUpper() == "VELOCITY") currentValue = Mom.Kids[AxisNum].ModeVel;
                else if (ColumnName.ToUpper() == "STATUSWORD") currentValue = Mom.Kids[AxisNum].StatusWord;
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



