using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.Storage.Streams;

namespace SerialUART
{
    class SerialController
    {
        private SerialDevice SerialPort;
        private DataWriter dataWriter;
        private DataReader dataReader;
        public bool serialConnection = false;

        public SerialController()
        {
            Debug.WriteLine("Constructor");
            InitSerial();
        }
    

        private async void InitSerial()
        {
            Debug.WriteLine("InitSerial");
            string aqs = SerialDevice.GetDeviceSelector("UART0");                   /* Find the selector string for the serial device   */
            Debug.WriteLine("InitSerial 1");
            var dis = await DeviceInformation.FindAllAsync(aqs);                    /* Find the serial device with our selector string  */
            Debug.WriteLine("InitSerial 2");
          
            SerialPort = await SerialDevice.FromIdAsync(dis[0].Id);    /* Create an serial device with our selected device */
            Debug.WriteLine("InitSerial 3");
            /* Configure serial settings */
            SerialPort.WriteTimeout = TimeSpan.FromMilliseconds(1000);
            SerialPort.ReadTimeout = TimeSpan.FromMilliseconds(1000);
            Debug.WriteLine("InitSerial 4");
            SerialPort.BaudRate = 9600;
            SerialPort.Parity = SerialParity.None;
            SerialPort.StopBits = SerialStopBitCount.One;
            SerialPort.DataBits = 8;
            Debug.WriteLine("SerialPort Initialized");
            dataWriter = new DataWriter();
            dataReader = new DataReader(SerialPort.InputStream);
            Debug.WriteLine("Reader and Writer initialized");
            serialConnection = true;
        }

        public async void Read()
        {
            /* Read data in from the serial port */
            const uint maxReadLength = 1024;
        //    DataReader dataReader = new DataReader(SerialPort.InputStream);
            uint bytesToRead = await dataReader.LoadAsync(maxReadLength);
            string rxBuffer = dataReader.ReadString(bytesToRead);
            Debug.WriteLine(rxBuffer);
        }

        public async void Write()
        {
            /* Write a string out over serial */
            string txBuffer = "Hello Serial";
         //   DataWriter dataWriter = new DataWriter();
            dataWriter.WriteString(txBuffer);
            uint bytesWritten = await SerialPort.OutputStream.WriteAsync(dataWriter.DetachBuffer());
        }
    }
}
