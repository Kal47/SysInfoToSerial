using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysInfoToSerial
{
    internal class ViewModel
    {
        public string ActiveSerialPort = "";
        public bool RunWebSocketServer;
        public bool RunSerialPort;
        public bool SerialPortOpen;
        public List<string> AvalableSerialPorts = new List<string>();
    }
}
