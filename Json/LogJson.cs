// **********************************************************************//
//                                                                       //
//     DeskMetrics NET - Json/LogJson.cs                                 //
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
using DeskMetrics.Watcher;

namespace DeskMetrics.Json
{
	public class LogJson : BaseJson
    {
        protected string Message;
        protected int Flow;
        public LogJson(string msg,int flow)
            : base(EventType.Log)
        {
            Message = msg;
            Flow = flow;
        }

	    protected override Hashtable GetJsonHashTable()
        {
            var json = new Hashtable();
            json.Add("ms", Message);
            json.Add("fl", Flow);
            return json;
        }
    }
}

