using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO.Ports;

namespace SysInfoToSerial
{
    // This code example demonstrates how to handle the Opening event.
    // It also demonstrates dynamic item addition and dynamic 
    // SourceControl determination with reuse.
    class SysInfoToSerial
    {
        private SerialCom serial;    
        private byte[] byteArr = new byte[7];
        private HardwareMonitor Monitor = new HardwareMonitor();
        private IDictionary<string, float> sensors;
        private WebSocketServer webserv;
        private string WsMessage;

        public SysInfoToSerial()
        {
            Monitor.Print();
            serial = new SerialCom("COM4");         
            webserv = new WebSocketServer();
            webserv.RunServerAsync("http://localhost:8080/").Wait();
        }
        public void Run(bool state)
        {
            serial.Pause(state);
            if (state)
            {                
                sensors = Monitor.GetData();

                Console.WriteLine($"CPU Used {sensors["Cpu%"]}");
                byteArr[0] = (byte)sensors["Cpu%"];

                Console.WriteLine($"CUP Temp {sensors["CpuTemp"]}");
                byteArr[1] = (byte)sensors["CpuTemp"];

                Console.WriteLine($"Mem Used {sensors["Mem%"]}");
                byteArr[2] = (byte)(sensors["Mem%"]);

                Console.WriteLine($"GPU Used {sensors["Gpu%"]}");
                byteArr[3] = (byte)sensors["Gpu%"];

                Console.WriteLine($"GUP Temp {sensors["GpuTemp"]}");
                byteArr[4] = (byte)sensors["GpuTemp"];

                Console.WriteLine($"GPU Mem {sensors["GpuMem"]}");
                byteArr[5] = (byte)(sensors["GpuMem"]);

                byteArr[6] = (byte)('\n');

                for (int i = 0; i < 7; i++)
                {
                    byteArr[i] = (byte)(byteArr[i] + 32);
                }

                WsMessage = "";
                webserv.BroadcastMessageAsync(Encoding.UTF8.GetString(byteArr, 0, byteArr.Length));

                Console.WriteLine(BitConverter.ToString(byteArr));
                serial.Write(byteArr, 0, byteArr.Length);
            }
        }
    }
}
