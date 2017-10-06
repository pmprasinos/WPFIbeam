using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Diagnostics;
using System.Threading;


class SerialRemote
{
    public SerialPort spin; //from bluetooth module
                            //private  SerialPort spout; //to beckoff

    private DateTime lastRecieved = new DateTime(1900, 1, 1);
    bool debug = false;

    private Stopwatch lw;
    private Stopwatch sw;
    public bool connected;
    public int recCount = 0;
    public bool enabled;
    public DateTime LastRecieved { get { return lastRecieved; } }
    public TimeSpan Age { get { return DateTime.Now - lastRecieved; } }
    public int[] x = { 0, 0, 0, 0 };
    public int[] y = { 0, 0, 0, 0 };
    public int[] z = { 0, 0, 0, 0 };
    public int[] w = { 0, 0, 0, 0 };
    public int[] Lights = { 1, 1, 1, 1, 1, 1, 1, 1 };
    public bool DeadmanLeftPressed;
    public bool DeadmanRightPressed;
    public bool EstopPressed;
    public int ScreenStateCommand = 72;
    public bool ErrorState = false;

    Task RefTask;



    public SerialRemote(ref SerialPort ComPort)
    {
        connected = false;
        this.spin = ComPort;
        this.spin.NewLine = "\r\n";
        this.spin.RtsEnable = true;
        this.spin.DtrEnable = true;
        //this.spin.WriteBufferSize = 20;
        //this.spin.ReadBufferSize = 20;
        //	this.spin.ReadTimeout = 6000;
        //this.spin.WriteTimeout = 6000;
        lw = new Stopwatch();
        sw = new Stopwatch();
        sw.Start();
        lw.Start();


    }
    public void start()
    {
        this.enabled = true;
        RefTask = Task.Run(() => Scan());

    }
    private void Scan()
    {
        do { System.Threading.Thread.Sleep(100); } while (!spin.IsOpen);
        int[] LastLights = Lights;

    Thread.Sleep(2000);
        do
        {
  
            spin.ReadTimeout = 1000;
            if (spin.BytesToRead > 200) { spin.ReadExisting(); Thread.Sleep(5); }
          
            long t = DateTime.Now.Ticks;
            byte[] frame = { 0, 0, 0, 0, 0 };

            int CheckByte = 0;
            //System.Threading.Thread.Sleep(3000);
            do
            {
                do
                {
                    CheckByte = this.spin.ReadByte();
                } while (CheckByte != 13);

                CheckByte = this.spin.ReadByte();
            } while (CheckByte != 10);
            do
            {
                Thread.Sleep(1);
            } while (this.spin.BytesToRead < 5);


            //Console.Write(this.spin.BytesToRead);
            int yRead, xRead, zRead, JoyStickRead;
            this.spin.Read(frame, 0, 4);
            JoyStickRead = frame[0];
            JoyStickRead = JoyStickRead - 48;
            if (JoyStickRead == 5)
            {
                w[0] = frame[1] % 2;
                w[1] = (frame[1] / 2) % 2;
                w[2] = (frame[1] / 4) % 2;
                w[3] = (frame[1] / 8) % 2;
                DeadmanRightPressed = (frame[1] / 16) % 2 > 0;
                 DeadmanLeftPressed = (frame[1] / 32) % 2 > 0;
                EstopPressed = frame[1] <= 128 ; 

            }
            xRead = frame[1];
            yRead = frame[2];
            zRead = frame[3];
            this.lastRecieved = DateTime.Now;

            sw.Stop();
            //Count number of cycles with buttons pressed to prevent incorrect button interpretations


            if (JoyStickRead < 4 && JoyStickRead >= 0)
            {
                y[JoyStickRead] = yRead - 150; if (System.Math.Abs(yRead - 150) < 2) y[JoyStickRead] = 0;
                x[JoyStickRead] = xRead - 150; if (System.Math.Abs(xRead - 150) < 2) x[JoyStickRead] = 0;
                z[JoyStickRead] = zRead - 150; if (System.Math.Abs(zRead - 150) < 2) z[JoyStickRead] = 0;


            }
           if(debug) Debug.Print(spin.BytesToRead + "   X: " + x[0].ToString() + " Y: " + y[0].ToString() + " Z: " + z[0].ToString() +
                            "  2" + "   X: " + x[1].ToString() + " Y: " + y[1].ToString() + " Z: " + z[1].ToString() +
            
                           "  3" + "   X: " + x[2].ToString() + " Y: " + y[2].ToString() + " Z: " + z[2].ToString() +
            
                           "  4" + "   X: " + x[3].ToString() + " Y: " + y[3].ToString() + " Z: " + z[3].ToString() + " LDEAD: " + DeadmanLeftPressed + " RDEAD: " + DeadmanRightPressed);
            sw = Stopwatch.StartNew();

            LastLights= Lights;
        } while (enabled);


    }

}


