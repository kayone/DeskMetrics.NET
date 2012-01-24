using DeskMetrics.Json;
using Newtonsoft.Json;

namespace DeskMetrics.DataPoints
{
    public class EventPeriodDataPoint : EventDataPoint
    {
        [JsonProperty("tm")]
        public double EventDuration { get; set; }

        [JsonProperty("ec")]
        public int EventCompletedJson
        {
            get
            {
                if (EventCompleted) return 1;
                return 0;
            }
        }

        [JsonIgnore]
        public bool EventCompleted { get; set; }

        public override string JsonType
        {
            get
            {
                return "evP";
            }
        }
    }
}

