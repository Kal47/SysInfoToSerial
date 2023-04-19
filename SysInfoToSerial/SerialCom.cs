using System;
using System.IO.Ports;


namespace SysInfoToSerial
{
    public class SerialCom
    {
        // Create the serial port with basic settings
        private readonly SerialPort port;
        
        public SerialCom (string portName)
        {
             port = new SerialPort("COM1", 9600, Parity.None, 8, StopBits.One);
        }

        public void Write(string msg)
        {
            port.Write(msg);
        }

        public void Write(byte[] byteArr, int offset, int count)
        {
            port.Write(byteArr, offset, count);
        }
    }
}