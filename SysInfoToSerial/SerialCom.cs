using System;
using System.IO.Ports;


namespace SysInfoToSerial
{
    public class SerialCom : IDisposable
    {
        // Create the serial port with basic settings
        private SerialPort port;
        private bool disposedValue;

        public SerialCom ()
        {
            port = new SerialPort("COM1", 9600, Parity.None, 8, StopBits.One);
            try
            {
                port.PortName = GetPorts()[0];
                port.Open();

            }
            catch { }
        }

        public bool ChangePort(string portName)
        {         
            if (port.PortName != portName && portName.Contains("COM"))
            { 
                port.Close();
                port.PortName = portName;
                try {
                    port.Open();
                } catch { }
            }
            return port.IsOpen;
        }

        public void Write(string msg)
        {
            port.Write(msg);
        }

        public bool Write(byte[] byteArr, int offset, int count)
        {
            bool sucsess = true;
            try
            {
                port.Write(byteArr, offset, count);
            }
            catch { sucsess = false; }
            return sucsess;
        }

        public void Pause(bool state)
        {
            if (state)
            {
                if (!port.IsOpen)
                    try { port.Open(); } catch { }               
            }
            else
            {
                if (port.IsOpen)
                    try { port.Close(); } catch { }
            }
        }

        public bool isOpen()
        {
            return port.IsOpen;
        }

        public List<String> GetPorts()
        {
            return SerialPort.GetPortNames().ToList();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    port.Close();
                }
                
                disposedValue = true;
            }
        }

        ~SerialCom()
        {            
             // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}