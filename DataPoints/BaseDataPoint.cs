using System;
using Newtonsoft.Json;

namespace DeskMetrics.DataPoints
{
    internal abstract class BaseDataPoint
    {
        protected BaseDataPoint()
        {
            var origin = new DateTime(1970, 1, 1);
            var diff = DateTime.Now.AddHours(-8) - origin;
            TimeStamp = Convert.ToInt32(diff.TotalSeconds);
        }

        [JsonProperty("tp")]
        public abstract string JsonType { get; }

        [JsonProperty("aver")]
        public string Version { get; set; }

        [JsonProperty("ID")]
        public string UserId { get; set; }

        [JsonProperty("ss")]
        public string SessionId { get; set; }

        [JsonProperty("ts")]
        public int TimeStamp { get; private set; }

        [JsonProperty("fl")]
        public int Flow { get; set; }
    }
}

