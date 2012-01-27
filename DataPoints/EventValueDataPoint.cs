using Newtonsoft.Json;

namespace DeskMetrics.DataPoints
{
    internal class EventValueDataPoint : EventDataPoint
    {
        public override string JsonType
        {
            get
            {
                return "evV";
            }
        }

        [JsonProperty("vl")]
        public string EventValue { get; set; }
    }

}

