// **********************************************************************//
//                                                                       //
//     DeskMetrics NET - Json/UninstallJson.cs                           //
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

namespace DeskMetrics.Json
{
    public class UninstallJson : BaseJson
    {
        public string ID;
        public string Version;

        public UninstallJson(string version)
            : base("ust")
        {
            ID = Guid.NewGuid().ToString().Replace("-", "").ToUpper();
            Version = version;
        }

        protected override Hashtable GetJsonHashTable()
        {
            var json = new Hashtable();
            json.Add("ID", ID);
            json.Add("aver", Version);
            return json;
        }
    }
}

