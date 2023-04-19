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

        public SysInfoToSerial()
        {
            Monitor.Print();
            serial = new SerialCom("COM1");
        }
        public void Run()
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

            Console.WriteLine(BitConverter.ToString(byteArr));
            //serial.Write(byteArr, 0, byteArr.Length);
                      
        }
    }
}
