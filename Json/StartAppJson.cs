// **********************************************************************//
//                                                                       //
//     DeskMetrics NET - Json/StartAppJson.cs                            //
//     Copyright (c) 2010-2011 DeskMetrics Limited                       //
//                                                                       //
//     http://deskmetrics.com                                            //
//     http://support.deskmetrics.com                                    //
//                                                                       //
//     support@deskmetrics.com                                           //
//                                                                       //
//     This code is provided under the DeskMetrics Modified BSD License  //
//     A copy of this license has been distributed in a file called      //
//     LICENSE with this source code.                                    //
//                                                                       //
// **********************************************************************//

using System;
using System.Collections;
using DeskMetrics.OperatingSystem;
using DeskMetrics.OperatingSystem.Hardware;
using DeskMetrics.Watcher;

namespace DeskMetrics.Json
{
    public class StartAppJson : BaseJson
    {
        private Client _client;
        public StartAppJson(Client client)
            : base(EventType.StartApplication)
        {
            _client = client;
        }

        protected override Hashtable GetJsonHashTable()
        {
            IOperatingSystem GetOsInfo = OperatingSystemFactory.GetOperatingSystem();
            IHardware GetHardwareInfo = GetOsInfo.Hardware;
            var json = new Hashtable();

            json.Add("aver", _client.ApplicationVersion.ToString());
            json.Add("osv", GetOsInfo.Version);
            json.Add("ossp", GetOsInfo.ServicePack);
            json.Add("osar", GetOsInfo.Architecture);
            json.Add("osjv", GetOsInfo.JavaVersion);
            json.Add("osnet", GetOsInfo.FrameworkVersion);
            json.Add("osnsp", GetOsInfo.FrameworkServicePack);
            json.Add("oslng", GetOsInfo.Lcid);
            json.Add("osscn", GetHardwareInfo.ScreenResolution);
            json.Add("cnm", GetHardwareInfo.ProcessorName);
            json.Add("car", GetHardwareInfo.ProcessorArchicteture);
            json.Add("cbr", GetHardwareInfo.ProcessorBrand);
            json.Add("cfr", GetHardwareInfo.ProcessorFrequency);
            json.Add("ccr", GetHardwareInfo.ProcessorCores);
            json.Add("mtt", GetHardwareInfo.MemoryTotal);
            json.Add("mfr", GetHardwareInfo.MemoryFree);
            json.Add("dtt", "null");
            json.Add("dfr", "null");
            return json;
        }
    }
}

