// **********************************************************************//
//                                                                       //
//     DeskMetrics NET - Json/ExceptionJson.cs                           //
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
	public class ExceptionJson : BaseJson
    {
        protected Exception Exception;
        protected int Flow;
        public ExceptionJson(Exception e,int flow)
            : base(EventType.Exception)
        {
            Exception = e;
            Flow = flow;
        }

	    protected override Hashtable GetJsonHashTable()
        {
            var json = new Hashtable();
            json.Add("msg", Exception.Message.Trim().Replace("\r\n", "").Replace("  ", " ").Replace("\n", "").Replace(@"\n", "").Replace("\r", "").Replace("&", "").Replace("|", "").Replace(">", "").Replace("<", "").Replace("\t", "").Replace(@"\", @"/"));
            if (Exception.StackTrace != null)
				json.Add("stk", Exception.StackTrace.Trim().Replace("\r\n", "").Replace("  ", " ").Replace("\n", "").Replace(@"\n", "").Replace("\r", "").Replace("&", "").Replace("|", "").Replace(">", "").Replace("<", "").Replace("\t", "").Replace(@"\", @"/"));
			else
				json.Add("stk","");
			if (Exception.Source != null)
            	json.Add("src", Exception.Source.Trim().Replace("\r\n", "").Replace("  ", " ").Replace("\n", "").Replace(@"\n", "").Replace("\r", "").Replace("&", "").Replace("|", "").Replace(">", "").Replace("<", "").Replace("\t", "").Replace(@"\", @"/"));
			else
				json.Add("src","");
            if (Exception.TargetSite!=null)
				json.Add("tgs", Exception.TargetSite.ToString());
			else
				json.Add("tgs","");
            json.Add("fl", Flow);
            return json;
        }
        

    }
}

