using Microsoft.Azure.Devices.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Ifrit.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private string[] connectionStrings;

        public MainPage()
        {
            this.InitializeComponent();

            // 3 rozne connectionstring (identyfikatory) do wyboru przez uzytkownika
            connectionStrings = new string[3];
            connectionStrings[0] = "";
            connectionStrings[1] = "";
            connectionStrings[2] = "";
        }

        // aktualinie wyswietlany kolor
        private Color _color;

        // obsluga tapniecia w prostokat z kolorem
        private void color_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // podlacz się z odpowiednim ID
            var deviceClient = DeviceClient.CreateFromConnectionString(connectionStrings[device.SelectedIndex], TransportType.Amqp);

            active.IsActive = true;

            // uruchom watek odbierajacy w tle
            Task.Run(async () =>
            {
                await deviceClient.OpenAsync();

                while (true)
                {
                    var receivedMessage = await deviceClient.ReceiveAsync();
                    if (receivedMessage != null)
                    {
                        // odbierz dane i zdekoduj
                        var obj = Encoding.ASCII.GetString(receivedMessage.GetBytes());

                        // nakazuje Dispatcherowi zaktualizowac wątek UI, bo nie mamy z tego
                        // wątku bezpośredniego dostępu
                        await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            // ustalenie jaki nowy kolor się teraz pojawi na podstawie danych z serwera
                            // i aktualnego koloru - wyłącza i włącza jedną ze składowych R, G, B
                            byte r = _color.R, g = _color.G, b = _color.B;

                            if (obj == "r") r = (byte)((_color.R == 0) ? 255 : 0);
                            if (obj == "g") g = (byte)((_color.G == 0) ? 255 : 0);
                            if (obj == "b") b = (byte)((_color.B == 0) ? 255 : 0);

                            // ustalenie wypełnienia
                            _color = Color.FromArgb(255, r, g, b);
                            color.Fill = new SolidColorBrush(_color);
                        });

                        // potwierdzenie odebrania informacji
                        await deviceClient.CompleteAsync(receivedMessage);
                    }

                    //  Note: In this sample, the polling interval is set to 
                    //  10 seconds to enable you to see messages as they are sent.
                    //  To enable an IoT solution to scale, you should extend this 
                    //  interval. For example, to scale to 1 million devices, set 
                    //  the polling interval to 25 minutes.
                    //  For further information, see
                    //  https://azure.microsoft.com/documentation/articles/iot-hub-devguide/#messaging
                    await Task.Delay(TimeSpan.FromSeconds(10));
                }
            }
            );
        }
    }
}
