using Processors;
using System.IO.Ports;

public class SerialPortBase : IConnectable
{
    readonly SerialPort Port = new SerialPort();

    public SerialPortBase()
    {
    }

    public void Connect()
    {
        if (Port.IsOpen) Port.Close();
        Port.Open();
    }

    public void Disconnect()
    {
        if (Port.IsOpen) Port.Close();
    }
}
