using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ifrit.AzureBridge
{
    class IoTData
    {
        public double Data { get; set; }
        public string Device { get; set; }

    }

    class Program
    {
        static SerialPort sp;
        static DeviceClient deviceClient;

        // ConnectionString dla IotHuba, zmień na swój
        private const string DeviceConnectionString = "";

        static void Main(string[] args)
        {
            deviceClient = DeviceClient.CreateFromConnectionString(DeviceConnectionString, TransportType.Mqtt);

            deviceClient.OpenAsync().Wait();

            // połącz się z Bluetoothem (zmień port na odpowiedni)
            sp = new SerialPort("COM7", 9600);

            sp.Open();
            Console.WriteLine("Działam.");

            ReceiveCommands(deviceClient).Wait();

            Console.ReadLine();
            sp.Close();            
        }

        // odbieranie komend z IoT Hub
        static async Task ReceiveCommands(DeviceClient deviceClient)
        {
            Console.WriteLine("\nDevice waiting for commands from IoTHub...\n");
            Message receivedMessage;
            string obj;

            while (true)
            {
                receivedMessage = await deviceClient.ReceiveAsync();

                if (receivedMessage != null)
                {
                    // odbierz komendę, zrób z niej tekst, wyślij przez Bluetooth
                    obj = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                    sp.Write(obj);
                    await deviceClient.CompleteAsync(receivedMessage);
                }

                Thread.Sleep(10000);
            }
        }
    }
}
