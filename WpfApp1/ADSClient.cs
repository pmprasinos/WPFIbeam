using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Microsoft.VisualBasic;
using System.Diagnostics;
using TwinCAT.Ads;
using System.Threading;

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
    public int CurX;
    public int CurY;
    public int CurSpeed;
    public int DeadMan;
    public int DeadMan2;
    public int EStop;
    public int GantryTargetWindow;
    public int Gantry1BeaconNumber;
    public int Gantry2BeaconNumber;
    public bool IsBeckhoff;
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

    public enum Parameter : int
    {
        GantryBeaconNumber,
        GantryTargetWindow,
        /// <summary> Target X position in user units </summary>
        TargetX,
        /// <summary> Target Y position in user units </summary>
        TargetY,
        /// <summary> Maximum acceleration for move </summary>
        PosAccel,
        /// <summary> Maximum decceleration for move </summary>
        //PosDecel,
        /// <summary> Maximum velocity for move </summary>
        PosVel,
        /// <summary> Motion command (motion type) </summary>
        //MotCmd,
        /// <summary> Start bit to initiate move </summary>
        //StartBit,
        /// <summary> AMC Slave position </summary>
        CurPos,
        /// <summary> Current speed </summary>
        CurSpeed,

        DeadMan,

        DeadMan2,

        Estop,

        TimeOutSwitch,

        FaultReset,
        ScreenPower,
    };


    public ADSClient(String AMSAddress, Boolean IsBeckhoff, int NumberOfKids)
    {
        Client.Timeout = 1000;
        AmsAddress g = new AmsAddress("10.99.1.1.0.0", 851);
        TwinCAT.Ads.DeviceInfo di = new TwinCAT.Ads.DeviceInfo();
        
    try
        {
            Client.Synchronize = true;
            Client.Disconnect();
          
            Client.Connect(AMSAddress, 851);
            di = Client.ReadDeviceInfo();
       
            TcAdsSymbolInfoLoader ld = Client.CreateSymbolInfoLoader();
            Thread.Sleep(200);
            Connected = false;
            if (Client.IsConnected)
            {
                ITcAdsSymbol t = ld.FindSymbol("MAIN.TimeOutSwitch");
               
                TimeOutSwitch = Client.CreateVariableHandle("MAIN.TimeOutSwitch");
                DeadMan = Client.CreateVariableHandle("MAIN.Joystick1DeadMan");
                FaultReset = Client.CreateVariableHandle("Main.GlobalReset");
                GlobalEstop = Client.CreateVariableHandle("Main.GlobalEstopActive");
                MomControl = Client.CreateVariableHandle("MAIN.Execute");
                Connected = true;
            }
          
            // TcADSSI = Client.CreateSymbolInfoLoader().GetSymbols(true);
            //    ROS = Client.CreateSymbolLoader(TwinCAT.SymbolsLoadMode.Flat).Symbols;
        }
        catch
        {
            Connected =false;
            throw;
        }

        Kids = new AdsKid[NumberOfKids];

        for (int x = 1; x<=NumberOfKids; x++)
        {
            Kids[x-1] = new AdsKid(Client, x);
        }
     
     //   TcAdsSymbolInfoCollection ADSSI = Client.CreateSymbolInfoLoader().GetSymbols(false);
       // DeviceInfo DI = Client.ReadDeviceInfo();
        //StateInfo SI = Client.ReadState();
  


      

        this.IsBeckhoff = IsBeckhoff;


        ParamHandles = new int[]
        {
               TargetX,
               TargetY,
               PosAccel,
			   //PosDecel,
			   PosVel,
			   //MotCmd,
			   //StartBit,
			   CurX,
               CurY,
               CurSpeed
        };

    }



    public object ReadValue(int ParamAddress)
    {

        object g;
        TcAdsClient Kid = (TcAdsClient)this.Client;

        //hard coded types, will have better array solution moving forward:
        if (ParamAddress == this.CurX || ParamAddress == this.CurY || ParamAddress == this.PosAccel)
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
            if (IsBeckhoff)
            {

                TcAdsClient Kid = (TcAdsClient)this.Client;
               
                if (bool.Parse(newValue.ToString()) == true) newValue = 1;
                if (bool.TryParse(newValue.ToString(), out bool b) && b ==false) newValue = 0;
               
                if (int.Parse(newValue.ToString()) == 1 || int.Parse(newValue.ToString()) == 0)
                {
                   
                   // Kid.TryWrite(paramAddress, new AdsStream(new byte[]  { 1 }), 0, byte.Parse(newValue.ToString()));
                    Kid.WriteAny(paramAddress, byte.Parse(newValue.ToString()));
                }
                else
                {
                    if (paramAddress == Gantry1BeaconNumber || paramAddress == Gantry2BeaconNumber || paramAddress == Joystick1SelectedScreenber || paramAddress == Joystick2SelectedScreenber) { Kid.WriteAny(paramAddress, short.Parse(newValue.ToString())); }
                    else { Kid.WriteAny(paramAddress, int.Parse(newValue.ToString())); }
                }
            }
         
            return true;
        }
        catch
        {
            return false;
        }
    }


    /// <summary>
    /// Asyncronus write adds the write operation to PendingOps list 
    /// </summary>
    /// <param name="ID"></param>
    /// <param name="ParamAddress"></param>
    //public void WriteValue(int ID, int ParamAddress, Object newValue)
    //{
    //	Task<bool> t = Task.Run(() =>
    //	{
    //		return WriteValue(ParamAddress, newValue);
    //	}
    //	);

    //	object[] d = new object[] { ID, ParamAddress, t };
    //	PendingOps.Add(d);
    //}

    public void Move()
    {
        if (IsBeckhoff)
        {
            TcAdsClient Kid = (TcAdsClient)this.Client;
        }
    }

    private static byte[] ValueToBytes(int value, bool Is32Bit = true)
    {
        byte[] g = BitConverter.GetBytes(value);
        Array.Reverse(g);
        if (Is32Bit == false) { g = new byte[2] { g[2], g[3] }; }
        return g;
    }

    private static void OnResponseData(ushort id, byte unit, byte function, byte[] data)
    {
        //Console.WriteLine(data[1]);

        // do stuff, store response data to list?
    }

}

public class AdsKid
{


    public int MinAge = 50; //MinAge is the minimum time elapsed between reading positions (the maximum refresh rate time constant)
    private int TargetPosAddr { get; set; }
    private int targetPosition;

    public int TargetPosition
    {
        get
        { targetPosition = (int)Mom.ReadAny(TargetPosAddr, typeof(int)); return targetPosition; }
        set
        { targetPosition = value; if (Connected) Mom.WriteAny(TargetPosAddr, int.Parse(value.ToString())); }
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
        { return modeAccel; }
        set
        { modeAccel = value; if (Connected) Mom.WriteAny(ModeAccelAddr, int.Parse(value.ToString())); }
    }

    private int IsLiveAddr { get; set; }
    private int isLive;
    public int IsLive
    {
        get
        { return isLive; }
        set
        { isLive = value; if (Connected) Mom.WriteAny(IsLiveAddr, int.Parse(value.ToString())); }
    }

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
                TargetPosAddr = MOM.CreateVariableHandle("MAIN.Kid[" + kidIndex + "].targetpos_FromMom_Raw");
                CurrentPosAddr = MOM.CreateVariableHandle("MAIN.Kid[" + kidIndex + "].HomedPosition_toMom");
                ModeVelAddr = MOM.CreateVariableHandle("MAIN.Kid[" + kidIndex + "].ModeVel_FromMom_Raw");
                ModeAccelAddr = MOM.CreateVariableHandle("MAIN.Kid[" + kidIndex + "].ModeAccel_FromMom_Raw");

                string checkStr = MOM.ReadAny(TargetPosAddr, typeof(int)).ToString();

                currentPosition = int.Parse(MOM.ReadAny(CurrentPosAddr, typeof(int)).ToString());
                targetPosition = currentPosition;
                Connected = true;
            }
            catch
            {

                Connected = false;
                throw;
            }

    }
}


