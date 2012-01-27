using System;
using Newtonsoft.Json;

namespace DeskMetrics.DataPoints
{
    internal abstract class BaseDataPoint
    {
        [JsonProperty("tp")]
        public abstract string JsonType { get; }

        [JsonProperty("aver")]
        public string Version { get; set; }

        [JsonProperty("ID")]
        public string UserId { get; set; }

        [JsonProperty("ss")]
        public string SessionId { get; set; }

        [JsonProperty("ts")]
        public int TimeStamp { get { return GetTimeStamp(); } }

        [JsonProperty("fl")]
        public int Flow { get; set; }

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

