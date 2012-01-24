using System;
using DeskMetrics.Watcher;
using Newtonsoft.Json;

namespace DeskMetrics.DataPoints
{
    public class ExceptionDataPoint : BaseDataPoint
    {


        [JsonIgnore]
        public Exception Exception { get; set; }

        [JsonProperty("msg")]
        public string Message
        {
            get { return CleanUpString(Exception.Message); }
        }


        [JsonProperty("stk")]
        public string Stack
        {
            get { return CleanUpString(Exception.StackTrace); }
        }

        
        [JsonProperty("src")]
        public string Source
        {
            get { return CleanUpString(Exception.Source); }
        }

        [JsonProperty("tgs")]
        public string TargetSite
        {
            get { return CleanUpString(Exception.TargetSite); }
        }

        private static string CleanUpString(object @object)
        {
            if (@object == null) return string.Empty;
            return @object.ToString().Trim()
                    .Replace("\r\n", "").Replace("  ", " ").Replace("\n", "").Replace(@"\n", "")
                    .Replace("\r", "").Replace("&", "").Replace("|", "").Replace(">", "").Replace("<", "")
                    .Replace("\t", "").Replace(@"\", @"/");
        }


        public override string JsonType
        {
            get { return "exC"; }
        }
    }
}

