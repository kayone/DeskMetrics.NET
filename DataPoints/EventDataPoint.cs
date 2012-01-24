using DeskMetrics.DataPoints;
using Newtonsoft.Json;

namespace DeskMetrics.Json
{
	public class EventDataPoint : BaseDataPoint
    {
        [JsonProperty("ca")]
        public string EventCategory { get; set; }

        [JsonProperty("nm")]
        public string EventName { get; set; }

	    public override string JsonType
	    {
            get { return "ev"; }
	    }
    }
}

