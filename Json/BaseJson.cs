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
        private readonly int _timeStamp;

        protected BaseJson(string type)
        {
            Type = type;
            _timeStamp = GetTimeStamp();
        }

        protected abstract Hashtable GetJsonHashTable();

        public string GetJson(string sessionId, string userId)
        {
            var hashTable = GetJsonHashTable();
            hashTable.Add("tp", Type);
            hashTable.Add("ss", sessionId);
            hashTable.Add("ts", _timeStamp);
            hashTable.Add("ID", userId);
            return JsonBuilder.GetJsonFromHashTable(hashTable);
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

