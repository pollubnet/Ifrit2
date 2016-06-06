Ifrit2
======

**Program demonstracyjny ze spotkania grupy z 31 maja 2016.**

Aplikacja składa się z dwóch solucji - Ifrit2 oraz Ifrit2.Web. Pierwsza
z nich zawiera aplikację dla .NET Micro Framework dla Netduino2 Plus,
która odbiera dane z Bluetooth i świeci diodami podłączonymi do płytki.

Drugi projekt w tej solucji, Ifrit2.AzureBridge odbiera dane z Azure i
przesyła je dalej przez Bluetooth, nic po drodze nie zmieniając.

Ifrit2.UWP to aplikacja, która sama łączy się z Azure i oczekuje na
komunikaty z usługi i zmienia kolor prostokąta w niej.

Aplikacje muszą być poprawnie skonfigurowane poprzez SharedAccessToken,
który można łatwo wygenerować np. w [Device Explorer](https://github.com/Azure/azure-iot-sdks/releases/download/2016-05-20/SetupDeviceExplorer.msi).

Druga solucja, Ifrit2.Web, to aplikacja ASP.NET Core 1.0 RC2, ale dla
"pełnego" .NET Frameworka (nie działa na .NET Core), która wysyła do
Azure komunikaty przeznaczone dla konkretnych urządzeń. Identyfikatory
urządzeń do których wysyła muszą być zgodne z nazwami nadanymi im
przy tworzeniu SharedAccessToken.

Dodatkowo, Ifrit2.Web wymaga, aby w pliku `secrets.json`, czyli nowym
mechaniźmie do zapisu danych wrażliwych, znalazł się klucz dostępu
do Azure IoT Hub.