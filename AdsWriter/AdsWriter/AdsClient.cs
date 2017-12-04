using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinCAT.Ads;
using System.Timers;

namespace AdsWriter
{
    using System.Runtime;

    public class ADSClient
    {
        //Default to Modbus adressing, will change to named Beckhoff addresses during init
        public int TargetX, HomeGantry1, HomeGantry2;
        public int TargetY;
        public int[] BeaconX = new int[4];
        public int[] BeaconY = new int[4];
        public int PosAccel;
        //public int PosDecel;
        public int PosVel;
        //public int MotCmd;
        //public int StartBit;
   
        public int TimeOutSwitch;
        public int MomControl;
        public int FaultReset;
        public int GantryBeaconNumber;
        public TcAdsClient Client { get; } = new TcAdsClient();
        public int Joystick1SelectedScreenber, Joystick1SelectedNum, Joystick2SelectedNum;
        public int Joystick2SelectedScreenber;
        private int[] ParamHandles;
        private List<object[]> PendingOps = new List<object[]>();
        public int GlobalEstop;
        public int ScreenPower;
        public AdsKid[] Kids;
        public TcAdsSymbolInfoCollection TcADSSI;
        //TwinCAT.TypeSystem.ReadOnlySymbolCollection ROS;
        public bool Connected = false;

        public int OpsPending()
        {
            return PendingOps.Count;
        }

        public Int64 GetResult(int ID)
        {
            for (int y = 0; y < OpsPending(); y++)
            {
                if ((int)PendingOps[y][0] == ID)
                {
                    Task<Int64> t = (Task<Int64>)PendingOps[y][2];
                    Int64 r = t.Result;
                    PendingOps.Remove(PendingOps[y]);
                    return r;
                }
            }
            throw new KeyNotFoundException("No pending operations with the ID: " + ID.ToString() + "were found");
        }

        public ADSClient(String AMSAddress,  int NumberOfKids)
        {
            Client.Timeout = 1000;
            AmsAddress g = new AmsAddress(AMSAddress, 851);
            try
            {
                Client.Connect(AMSAddress, 851);
                TimeOutSwitch = Client.CreateVariableHandle("MAIN.TimeOutSwitch");
                //DeadMan = Client.CreateVariableHandle("MAIN.Joystick1DeadMan");
                FaultReset = Client.CreateVariableHandle("Main.GlobalReset");
                GlobalEstop = Client.CreateVariableHandle("Main.GlobalEstopActive");
                MomControl = Client.CreateVariableHandle("MAIN.Execute");
                Connected = true;              
            }
            catch
            {
                Connected = false;
                throw;
            }
            Client.Timeout = 500;
            Kids = new AdsKid[NumberOfKids];

            for (int x = 1; x <= NumberOfKids; x++)
            {
                Kids[x - 1] = new AdsKid(Client, x);
            }
        }
        

        public object ReadValue(int ParamAddress)
        {
            object g;
            TcAdsClient Kid = (TcAdsClient)this.Client;
            //hard coded types, will have better array solution moving forward:
            if ( ParamAddress == this.PosAccel)
            {
                g = Double.Parse(Kid.ReadAny(ParamAddress, typeof(int)).ToString());
            }
            else if (ParamAddress == this.TargetX || ParamAddress == this.TargetY)
            {
                g = long.Parse(Kid.ReadAny(ParamAddress, typeof(Double)).ToString());
            }
            else
            {
                g = int.Parse(Kid.ReadAny(ParamAddress, typeof(int)).ToString());
                //g = int.Parse(Kid.ReadAny(ParamAddress, typeof(Single)).ToString());
            }
            return g;
        }


        /// <summary>
        /// Asyncronus read adds the read operation to PendingOps list 
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="ParamAddress"></param>
        public void ReadValue(int ID, int ParamAddress)
        {
            //long g = this.ReadValue(ParamAddress);
            Task<object> t = Task<object>.Run(() => this.ReadValue(ParamAddress));

            object[] d = new object[] { ID, ParamAddress, t };
            PendingOps.Add(d);
        }



        public bool WriteValue(int paramAddress, Object newValue, string DebugTag = "")
        {
            try
            {
                    TcAdsClient Kid = (TcAdsClient)this.Client;
                    if (bool.Parse(newValue.ToString()) == true) newValue = 1;
                    if (bool.TryParse(newValue.ToString(), out bool b) && b == false) newValue = 0;

                    if (int.Parse(newValue.ToString()) == 1 || int.Parse(newValue.ToString()) == 0)
                    {
                        Kid.WriteAny(paramAddress, byte.Parse(newValue.ToString())); 
                    }
                    else
                    {
                         Kid.WriteAny(paramAddress, int.Parse(newValue.ToString())); 
                    }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    public class AdsKid
    {
        public bool Connected = false;
        public TcAdsClient Mom;
        private System.Diagnostics.Stopwatch LastPolled = new System.Diagnostics.Stopwatch();
        private int kidIndex;

        public AdsKid(TcAdsClient MOM, int KidIndex)
        {
           
            LastPolled.Start();
            if (!Connected) try
                {
                    this.Mom = MOM;
                    this.kidIndex = KidIndex;
                  //  TargetPosAddr = MOM.CreateVariableHandle("MAIN.Kid[" + kidIndex + "].targetpos_FromMom_Raw");
                   // CurrentPosAddr = MOM.CreateVariableHandle("MAIN.Kid[" + kidIndex + "].HomedPosition_toMom");
                    //CurrentVelocityAddr = Mom.CreateVariableHandle("MAIN.Kid[" + kidIndex + "].Speed");
                    //ModeVelAddr = MOM.CreateVariableHandle("MAIN.Kid[" + kidIndex + "].ModeVel_FromMom");
                    //ModeAccelAddr = MOM.CreateVariableHandle("MAIN.Kid[" + kidIndex + "].ModeAccel_FromMom");
                    //ModeDecelAddr = MOM.CreateVariableHandle("MAIN.Kid[" + kidIndex + "].ModeDecel_FromMom");
                    //DeadManPressedAddr = Mom.CreateVariableHandle("MAIN.Kid[" + KidIndex + "].RMT_DeadManPressed");
                    //ScalingIntAddr = Mom.CreateVariableHandle("MAIN.Kid[" + KidIndex + "].ScalingNumber");
                    //MomControlAddr = Mom.CreateVariableHandle("MAIN.Kid[" + KidIndex + "].CheckIn.MomControl");
                    //StatusWordAddr = Mom.CreateVariableHandle("MAIN.Kid[" + KidIndex + "].UpdateWord_ToMom");
                    
                    //string checkStr = MOM.ReadAny(TargetPosAddr, typeof(int)).ToString();
                    //  TcAdsSymbolInfoLoader tcl = MOM.CreateSymbolInfoLoader();
                    // TcAdsSymbolInfo tci = tcl.FindSymbol("MAIN.Kid[" + KidIndex + "].SCALINGINT");
                    //currentPosition = int.Parse(MOM.ReadAny(CurrentPosAddr, typeof(int)).ToString());
                    //targetPosition = currentPosition;
                    Connected = true;
                }
                catch
                {

                    Connected = false;
                    throw;
                }
        }

       // public int MinAge = 50; //MinAge is the minimum time elapsed between reading positions (the maximum refresh rate time constant)

        private int TargetPosAddr { get; set; }
        private int targetPosition;
        public int TargetPosition
        {
            get
            { targetPosition = (int)Mom.ReadAny(TargetPosAddr, typeof(int)); return targetPosition; }
            set
            { targetPosition = value; if (Connected) Mom.WriteAny(TargetPosAddr, int.Parse(value.ToString())); }
        }

        private int CurrentVelocityAddr { get; set; }
        private int currentVelocity;
        public int CurrentVelocity
        {
            get
            { currentVelocity = (int)Mom.ReadAny(CurrentVelocityAddr, typeof(int)); return currentVelocity; }
            set
            { currentVelocity = value;  }
        }

        private int CurrentPosAddr { get; set; }
        private int currentPosition;
        public int CurrentPosition
        {
            get
            {
                    currentPosition = int.Parse(Mom.ReadAny(CurrentPosAddr, typeof(int)).ToString());     
                return currentPosition;
            }
        }

        private int ModeVelAddr { get; set; }
        private int modeVel;
        public int ModeVel
        {
            get
            { if (Connected) { modeVel = (int)Mom.ReadAny(ModeVelAddr, typeof(int)); return modeVel; } throw new Exception("READ OPERATION FUCKED UP"); }
            set
            { modeVel = value; if (Connected) Mom.WriteAny(ModeVelAddr, int.Parse(value.ToString())); }
        }

        private int ModeAccelAddr { get; set; }
        private int modeAccel;
        public int ModeAccel
        {
            get
            { modeAccel= (int)Mom.ReadAny(ModeAccelAddr, typeof(int));  return modeAccel; }
            set
            { modeAccel = value; if (Connected) Mom.WriteAny(ModeAccelAddr, int.Parse(value.ToString())); }
        }

        private int ModeDecelAddr { get; set; }
        private int modeDecel;
        public int ModeDecel
        {
            get
            { modeDecel = (int)Mom.ReadAny(ModeDecelAddr, typeof(int)); return modeDecel; }
            set
            { modeDecel = value; if (Connected) Mom.WriteAny(ModeDecelAddr, int.Parse(value.ToString())); }
        }

  
        private int isLive;
        public int IsLive
        {
            get
            { return isLive; }
            set
            { isLive = value;  }
        }


        private int DeadManPressedAddr { get; set; }
        private int deadManPressed;
        public int DeadManPressed
        {
            get
            { return deadManPressed ; }
            set
            { deadManPressed = Convert.ToInt32(value); if (Connected) { Mom.WriteAny(DeadManPressedAddr, Byte.Parse(value.ToString())); Console.WriteLine(kidIndex.ToString() + "  WROTE: " + value.ToString()); } }
        }


        private int MomControlAddr { get; set; }
        private int momControl;
        public int MomControl
        {
            get
            {momControl = (int)Mom.ReadAny(MomControlAddr, typeof(int)); return momControl; }
            set
            { momControl = Convert.ToInt32(value); if (Connected) { Mom.WriteAny(MomControlAddr, Byte.Parse(value.ToString())); } }
        }

        private int ScalingIntAddr { get; set; }
        private int scalingInt;
        public int ScalingInt
        {
            get
            { return scalingInt; }
            set
            { scalingInt = int.Parse(value.ToString()); if (Connected) { Mom.WriteAny(ScalingIntAddr, short.Parse(value.ToString())); Console.WriteLine(kidIndex.ToString() + " Scaling WROTE: " + value.ToString()); } }
        }

        private int StatusWordAddr { get; set; }
        private long statusWord;
        public long StatusWord
        {
            get
            { statusWord = (long)Mom.ReadAny(StatusWordAddr, typeof(long)); Console.WriteLine("STATUS: " + statusWord.ToString()); return statusWord;  }
            set
            { statusWord = long.Parse(value.ToString()); if (Connected) { Mom.WriteAny(StatusWordAddr, long.Parse(value.ToString()));  } }
        }

       
    }
}
