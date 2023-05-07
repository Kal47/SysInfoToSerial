using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibreHardwareMonitor.Hardware;

public class UpdateVisitor : IVisitor
{
    public void VisitComputer(IComputer computer)
    {
        computer.Traverse(this);
    }
    public void VisitHardware(IHardware hardware)
    {
        hardware.Update();
        foreach (IHardware subHardware in hardware.SubHardware) subHardware.Accept(this);
    }
    public void VisitSensor(ISensor sensor) { }
    public void VisitParameter(IParameter parameter) { }
}


public class HardwareMonitor : IDisposable
{
    private readonly Computer computer;
    private bool disposedValue;

    public HardwareMonitor(bool isCpuEnabled = true,
        bool isGpuEnabled = true,
        bool isMemoryEnabled = true,
        bool isMotherboardEnabled = true,
        bool isControllerEnabled = true,
        bool isNetworkEnabled = false,
        bool isStorageEnabled = false)
    {
        computer = new Computer
        {
            IsCpuEnabled = isCpuEnabled,
            IsGpuEnabled = isGpuEnabled,
            IsMemoryEnabled = isMemoryEnabled,
            IsMotherboardEnabled = isMotherboardEnabled,
            IsControllerEnabled = isControllerEnabled,
            IsNetworkEnabled = isNetworkEnabled,
            IsStorageEnabled = isStorageEnabled
        };
        computer.Open();
    }
    public void Print()
    {        
        computer.Accept(new UpdateVisitor());

        foreach (IHardware hardware in computer.Hardware)
        {
            Console.WriteLine("Hardware: {0}", hardware.Name);

            foreach (IHardware subhardware in hardware.SubHardware)
            {
                Console.WriteLine("\tSubhardware: {0}", subhardware.Name);

                foreach (ISensor sensor in subhardware.Sensors)
                {
                    Console.WriteLine("\t\tSensor: {0}, {1}, value: {2}", sensor.Name,sensor.SensorType, sensor.Value);
                }
            }
            foreach (ISensor sensor in hardware.Sensors)
            {
                Console.WriteLine("\t\tSensor: {0}, {1}, value: {2}", sensor.Name, sensor.SensorType, sensor.Value);
            }
        }        
    }
    public IDictionary<string, float> GetData()
    {
        computer.Accept(new UpdateVisitor());

        Dictionary<string, float> sensor = new Dictionary<string, float>();

        sensor["Cpu%"] = computer.Hardware.First(item => item.HardwareType == HardwareType.Cpu).Sensors.First(item => item.Name == "CPU Total").Value ?? -1;
        sensor["CpuFeq"] = computer.Hardware.First(item => item.HardwareType == HardwareType.Cpu).Sensors.First(item => item.SensorType == SensorType.Clock).Value ?? -1;
        sensor["CpuTemp"] = computer.Hardware.First(item => item.HardwareType == HardwareType.Cpu).Sensors.First(item => item.SensorType == SensorType.Temperature && item.Name == "CPU Package").Value ?? -1;
        sensor["Mem%"] = computer.Hardware.First(item => item.HardwareType == HardwareType.Memory).Sensors.First(item => item.Name == "Memory").Value ?? -1 /
        computer.Hardware.First(item => item.HardwareType == HardwareType.Memory).Sensors.First(item => item.Name == "Memory Used").Value ?? -1;
        sensor["Gpu%"] = computer.Hardware.First(item => item.HardwareType == HardwareType.GpuNvidia).Sensors.First(item => item.Name == "GPU Core" && item.SensorType == SensorType.Load).Value ?? -1;
        sensor["GpuFeq"] = computer.Hardware.First(item => item.HardwareType == HardwareType.GpuNvidia).Sensors.First(item => item.SensorType == SensorType.Clock).Value ?? -1;
        sensor["GpuTemp"] = computer.Hardware.First(item => item.HardwareType == HardwareType.GpuNvidia).Sensors.First(item => item.Name == "GPU Core" && item.SensorType == SensorType.Temperature).Value ?? -1;
        float gpumemtotal = (computer.Hardware.First(item => item.HardwareType == HardwareType.GpuNvidia).Sensors.First(item => item.Name == "GPU Memory Total").Value ?? -1);
        float gpumemused =  (computer.Hardware.First(item => item.HardwareType == HardwareType.GpuNvidia).Sensors.First(item => item.Name == "GPU Memory Used").Value ?? -1);
        //Console.WriteLine(gpumemtotal);
        //Console.WriteLine(gpumemused);
        //Console.WriteLine(gpumemused / gpumemtotal);
        sensor["GpuMem"] = (gpumemused / gpumemtotal) * 100;
        
        return sensor;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                computer.Close();
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            disposedValue = true;
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~HardwareMonitor()
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