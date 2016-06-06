using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
using System.IO.Ports;
using System.Text;

namespace Ifrit
{
    // struktura która identyfikuje kolor
    public struct Color
    {
        public byte R;
        public byte G;
        public byte B;
    }

    public class Program
    {
        // port szeregowy do komunikacji Bluetooth
        static SerialPort sp;

        // porty wyjściowe podpięte do odpowiednich pinów Netduino
        // można sterować - true stan wysoki (3,3V), false stan niski (0V)
        static OutputPort r = new OutputPort(Pins.GPIO_PIN_D7, false);
        static OutputPort g = new OutputPort(Pins.GPIO_PIN_D6, false);
        static OutputPort b = new OutputPort(Pins.GPIO_PIN_D5, false);
        // aktualny kolor
        static Color _color = new Color() { R = 0, G = 0, B = 0 };

        // zaświecanie odpowiednich diodek w zależności od żądanego koloru
        static void ShowColor(Color c)
        {
            r.Write(false);
            g.Write(false);
            b.Write(false);

            if (c.R == 255) r.Write(true);
            if (c.G == 255) g.Write(true);
            if (c.B == 255) b.Write(true);
        }

        public static void Main()
        {                       
            // SerialPort połączony jest na COM2 (piny 2 i 3) do modułu Bluetooth na parametrach 38400, 8, N, 1
            sp = new SerialPort(SerialPorts.COM2, 38400, Parity.None, 8, StopBits.One);
            sp.DataReceived += Sp_DataReceived;
            sp.Open();

            // na wieczność
            while (true)
            {                
                Thread.Sleep(1000);
            }

            sp.Close();

        }

        private static void Sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // odbierz dane i ustal jaki kolor powinnismy teraz zapalic
            // na podstawie tego co przyszlo i tego co jest
            // tutaj kod jest identyczny do kodu z UWP
            byte r = _color.R, g = _color.G, b = _color.B;
            
            char obj = (char)sp.ReadByte();

            if (obj == 'r') r = (byte)((_color.R == 0) ? 255 : 0);
            if (obj == 'g') g = (byte)((_color.G == 0) ? 255 : 0);
            if (obj == 'b') b = (byte)((_color.B == 0) ? 255 : 0);

            // tutaj pokazywanie jest inne niz w UWP
            _color = new Color() { R = r, B = b, G = g };
            ShowColor(_color);
        }
    }
}
