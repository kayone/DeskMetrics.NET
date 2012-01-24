// **********************************************************************//
//                                                                       //
//     DeskMetrics NET - Json/EventJson.cs                               //
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

namespace DeskMetrics.Json
{
	public class EventJson : BaseJson
    {
        private readonly string _category;
        private readonly string _name;
        private readonly int _flow;

        public EventJson(string category, string name, int flow)
            : base(EventType.Event, BaseJson.Session)
        {
            _category = category;
            _name = name;
            _flow = flow;
        }

        public override Hashtable GetJsonHashTable()
        {
            var json = base.GetJsonHashTable();
            json.Add("ca", _category);
            json.Add("nm", _name);
            json.Add("fl", _flow);

            return json;
        }
    }
}

