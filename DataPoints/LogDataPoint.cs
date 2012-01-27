using Newtonsoft.Json;

namespace DeskMetrics.DataPoints
{
    internal class LogDataPoint : BaseDataPoint
    {
        [JsonProperty("msg")]
        public string LogMessage { get; set; }

        public override string JsonType
        {
            get { return "lg"; }
        }

    }
}

