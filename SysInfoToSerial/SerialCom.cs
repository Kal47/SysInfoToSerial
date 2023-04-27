using System;
using System.IO.Ports;


namespace SysInfoToSerial
{
    public class SerialCom : IDisposable
    {
        // Create the serial port with basic settings
        private SerialPort port;
        private bool disposedValue;

        public SerialCom (string portName)
        {
            port = new SerialPort(portName, 9600, Parity.None, 8, StopBits.One);
            port.Open();
        }

        public bool ChangePort(string portName)
        {
            if (port.PortName != portName)
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

        public void Write(byte[] byteArr, int offset, int count)
        {

            port.Write(byteArr, offset, count);
        }

        public void Pause(bool state)
        {
            if (state)
            {
                if (!port.IsOpen)
                    port.Open();
            }
            else
            {
                if (port.IsOpen)
                    port.Close();
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
                
                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~SerialCom()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}