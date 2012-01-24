using Newtonsoft.Json;

namespace DeskMetrics.DataPoints
{
    public class LogDataPoint : BaseDataPoint
    {
        [JsonProperty("msg")]
        public string LogMessage { get; set; }

        public override string JsonType
        {
            get { return "lg"; }
        }

    }
}

