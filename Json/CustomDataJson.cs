// **********************************************************************//
//                                                                       //
//     DeskMetrics NET - Json/CustomDataJson.cs                          //
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

using System.Collections;
using DeskMetrics.Watcher;

namespace DeskMetrics.Json
{
    public class CustomDataJson : BaseJson
    {
        private readonly string _name;
        private readonly string _value;
        private readonly int _flow;

        public CustomDataJson(string name,string value, int flow)
            : base(EventType.CustomData)
        {
            _name = name;
            _value = value;
            _flow = flow;
        }

        protected override Hashtable GetJsonHashTable()
        {
            var json = new Hashtable();
            json.Add("nm", _name);
            json.Add("vl", _value);
            json.Add("fl", _flow);
            return json;
        }

    }
}

