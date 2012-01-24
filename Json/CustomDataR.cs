// **********************************************************************//
//                                                                       //
//     DeskMetrics NET - Json/CustomDataR.cs                             //
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
	public class CustomDataRJson : CustomDataJson
    {
	    private readonly string _id;
        private readonly Version _appVersion;
        public CustomDataRJson(string name, string value, int flow, string id, Version appVersion):base(name,value,flow)
        {
            _id = id;
            _appVersion = appVersion;
        }

	    protected override Hashtable GetJsonHashTable()
        {
            var json = new Hashtable();
            json.Add("aver", _appVersion.ToString());
            json.Add("ID", _id);
            return json;
        }
    }
}

