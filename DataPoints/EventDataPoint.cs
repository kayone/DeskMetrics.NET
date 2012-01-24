using Newtonsoft.Json;

namespace DeskMetrics.DataPoints
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

