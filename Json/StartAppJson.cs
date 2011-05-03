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
using System.Collections.Generic;
using DeskMetrics.OperatingSystem;
using DeskMetrics.OperatingSystem.Hardware;

namespace DeskMetrics.Json
{
	public class StartAppJson:BaseJson
    {
        private Watcher Watcher;
        public StartAppJson(Watcher w):base(EventType.StartApplication,w.SessionGUID.ToString())
        {
            Watcher = w;
        }

        public override Dictionary<string, string> GetJsonHashTable()
        {
            IOperatingSystem GetOsInfo = OperatingSystemFactory.GetOperatingSystem();
            IHardware GetHardwareInfo = GetOsInfo.Hardware;
            var json = base.GetJsonHashTable();
			
            json.Add("aver",Watcher.ApplicationVersion);
            json.Add("ID", Watcher.UserGUID.ToString());
            json.Add("osv", GetOsInfo.Version);
            json.Add("ossp", GetOsInfo.ServicePack);
            json.Add("osar", GetOsInfo.Architecture.ToString());
            json.Add("osjv", GetOsInfo.JavaVersion);
            json.Add("osnet", GetOsInfo.FrameworkVersion);
            json.Add("osnsp", GetOsInfo.FrameworkServicePack);
            json.Add("oslng", GetOsInfo.Lcid.ToString());
            json.Add("osscn", GetHardwareInfo.ScreenResolution);
            json.Add("cnm", GetHardwareInfo.ProcessorName);
			json.Add("car", GetHardwareInfo.ProcessorArchicteture.ToString());
            json.Add("cbr", GetHardwareInfo.ProcessorBrand);
            json.Add("cfr", GetHardwareInfo.ProcessorFrequency.ToString());
            json.Add("ccr", GetHardwareInfo.ProcessorCores.ToString());
            json.Add("mtt", GetHardwareInfo.MemoryTotal.ToString());
            json.Add("mfr", GetHardwareInfo.MemoryFree.ToString());
            json.Add("dtt", "null");
            json.Add("dfr", "null");
            return json;
        }
    }	
}

