// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Newtonsoft.Json;
using Microsoft.Azure.Devices.Client;
using Windows.UI.Core;

namespace Ifrit.UWP
{
    class IoTClient
    {        
        

        public static event Action<string> MessageReceived;

        public async static void StartReceiving(CoreDispatcher d)
        {            
            
        }        
    }
}