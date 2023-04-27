using LibreHardwareMonitor.Hardware;
using System;
using System.Windows.Forms;
using System.Drawing;

namespace SysInfoToSerial
{
    public class Program
    {        
        static void Main(string[] args)
        {
            Console.WriteLine("Start");
            SysInfoToSerial program = new SysInfoToSerial();            
            var timer = new PeriodicTimer(TimeSpan.FromSeconds(10));
            UI uI = new UI();

                program.Run(uI.Config);
                Thread.Sleep(2000);
        }
    }
}
