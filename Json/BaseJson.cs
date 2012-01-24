// **********************************************************************//
//                                                                       //
//     DeskMetrics NET - Json/BaseJson.cs                                //
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
    public abstract class BaseJson
    {
        protected string Type;
        protected static string Session { get; set; }

        private readonly int _timeStamp;
        private readonly Hashtable _jsonHashTable;

        protected BaseJson(string type, string session)
        {
            Type = type;

            if (Type == "strApp") //StartApp
                Session = System.Guid.NewGuid().ToString().Replace("-", "").ToUpper();
            else
                Session = session;

            _timeStamp = GetTimeStamp();
            _jsonHashTable = new Hashtable();
        }

        public virtual Hashtable GetJsonHashTable()
        {
            _jsonHashTable.Add("tp", Type);
            _jsonHashTable.Add("ss", Session);
            _jsonHashTable.Add("ts", _timeStamp);
            return _jsonHashTable;
        }

        public string GetJson()
        {
            return JsonBuilder.GetJsonFromHashTable(GetJsonHashTable());
        }

        private static int GetTimeStamp()
        {
            double timeStamp = 0;
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            var diff = DateTime.UtcNow - origin;
            timeStamp = Math.Floor(diff.TotalSeconds);
            return Convert.ToInt32(timeStamp);
        }
    }
}

