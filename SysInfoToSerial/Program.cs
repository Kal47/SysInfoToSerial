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
            UI uI = new UI();
            var timer = new PeriodicTimer(TimeSpan.FromSeconds(10));
            
            while (true)
            {
                program.Run(uI.running);
                Thread.Sleep(2000);
            }
        }
    }
}
