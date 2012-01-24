using DeskMetrics.Watcher;
using Newtonsoft.Json;

namespace DeskMetrics.DataPoints
{
    public class CustomDataDataPoint : BaseDataPoint
    {
        public override string JsonType
        {
            get { return "ctDR"; }  
        }

        [JsonProperty("nm")]
        public string CustomDataKey { get; set; }

        [JsonProperty("vl")]
        public string CustomDataValue { get; set; }
    }
}

