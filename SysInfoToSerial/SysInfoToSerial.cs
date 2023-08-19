using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO.Ports;
using System.Timers;

namespace SysInfoToSerial
{
    // This code example demonstrates how to handle the Opening event.
    // It also demonstrates dynamic item addition and dynamic 
    // SourceControl determination with reuse.
    class SysInfoToSerial
    {
        private SerialCom serial;    
        private byte[] byteArr = new byte[9];
        private HardwareMonitor Monitor = new HardwareMonitor();
        private IDictionary<string, float> sensors;
        private WebSocketServer webserv;
        private ViewModel Config;
        private System.Timers.Timer t = new System.Timers.Timer();

        public SysInfoToSerial()
        {
            Monitor.Print();
            serial = new SerialCom();
            webserv = new WebSocketServer();
            webserv.RunServerAsync("http://localhost:8080/").Wait();
        }
        public void Run(ViewModel _config)
        {

            System.Timers.Timer t = new System.Timers.Timer();
            t.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            t.Interval = 2000;
            t.Enabled = true;
            Config = _config;

            while (true)
            {
                
            }
        }
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {                       
            if (Config.RunSerialPort || Config.RunWebSocketServer)
            {
                sensors = Monitor.GetData();

                //Console.WriteLine($"CPU Used {sensors["Cpu%"]}");
                byteArr[0] = (byte)sensors["Cpu%"];

                byteArr[1] = (byte)(sensors["CpuFeq"] / 100);

                //Console.WriteLine($"CUP Temp {sensors["CpuTemp"]}");
                byteArr[2] = (byte)sensors["CpuTemp"];

                //Console.WriteLine($"Mem Used {sensors["Mem%"]}");
                byteArr[3] = (byte)(sensors["Mem%"]);

                //Console.WriteLine($"GPU Used {sensors["Gpu%"]}");
                byteArr[4] = (byte)sensors["Gpu%"];

                byteArr[5] = (byte)(sensors["GpuFeq"] / 100);

                //Console.WriteLine($"GUP Temp {sensors["GpuTemp"]}");
                byteArr[6] = (byte)sensors["GpuTemp"];

                //Console.WriteLine($"GPU Mem {sensors["GpuMem"]}");
                byteArr[7] = (byte)(sensors["GpuMem"]);

                byteArr[8] = -1; //set to -1 so it changes to 0 in the for loop below. Probably not the best way to do that

                for (int i = 0; i < 8; i++)
                {
                    byteArr[i] = (byte)(byteArr[i] + 1); //adds one to the byte so 0 values dont end the read loop on arduino
                }                    
                    
                if(Config.RunWebSocketServer)
                    webserv.BroadcastMessageAsync(Encoding.UTF8.GetString(byteArr, 0, byteArr.Length));

                Console.WriteLine(BitConverter.ToString(byteArr));

                serial.Pause(Config.RunSerialPort);
                serial.ChangePort(Config.ActiveSerialPort);
                if (Config.RunSerialPort)
                    serial.Write(byteArr, 0, byteArr.Length);
            }

            Config.AvalableSerialPorts = serial.GetPorts();
            Config.SerialPortOpen = serial.isOpen();            
        }
    }
}
