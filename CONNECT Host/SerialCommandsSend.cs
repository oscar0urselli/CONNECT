using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;

public class SerialCommandsSend
{
    //public static SerialCommandsSend instance;
    public static SerialPort serialPort;
    public static bool portInitialized = false;

    public static void InitializeSerialCommunication()
    {
        serialPort = new SerialPort();
        serialPort.PortName = "COM3"; // COM4 for Arduino Nano (only on my PC)
        serialPort.BaudRate = 9600;
        serialPort.Open();
        portInitialized = true;
    }

    public static void LedCommand(string _ledCommand)
    {
        if (!portInitialized)
        {
            InitializeSerialCommunication();
        } 
        serialPort.Write(_ledCommand);
    }
}
