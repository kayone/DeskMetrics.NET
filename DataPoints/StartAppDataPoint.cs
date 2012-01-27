using Newtonsoft.Json;

namespace DeskMetrics.DataPoints
{
    internal class StartAppDataPoint : BaseDataPoint
    {
        private static readonly EnviromentInformation EnviromentInformation = new EnviromentInformation();


        public override string JsonType
        {
            get { return "strApp"; }
        }

        [JsonProperty("osv")]
        public string OsVersion { get { return EnviromentInformation.OsVersion; } }

        [JsonProperty("ossp")]
        public string OsServicePack { get { return EnviromentInformation.OsServicePack; } }

        [JsonProperty("osar")]
        public int OsArchitecture { get { return EnviromentInformation.OsArchitecture; } }

        [JsonProperty("osnet")]
        public string FrameworkVersion { get { return EnviromentInformation.FrameworkVersion; } }

        [JsonProperty("osnsp")]
        public string FrameworkSP { get { return EnviromentInformation.FrameworkServicePack; } }

        [JsonProperty("oslng")]
        public int OsLanguage { get { return EnviromentInformation.Language; } }

        [JsonProperty("osscn")]
        public string ScreenResolution { get { return EnviromentInformation.ScreenResolution; } }

        [JsonProperty("cnm")]
        public string ProcessorName { get { return EnviromentInformation.ProcessorName; } }

        [JsonProperty("car")]
        public int ProcessorArchicteture { get { return EnviromentInformation.ProcessorArchicteture; } }

        [JsonProperty("cbr")]
        public string ProcessorBrand { get { return EnviromentInformation.ProcessorBrand; } }

        [JsonProperty("cfr")]
        public double ProcessorFrequency { get { return EnviromentInformation.ProcessorFrequency; } }

        [JsonProperty("ccr")]
        public double ProcessorCores { get { return EnviromentInformation.ProcessorCores; } }

        [JsonProperty("osjv")]
        public string JavaVersion { get { return EnviromentInformation.JavaVersion; } }

        [JsonProperty("mtt")]
        public double MemoryTotal { get { return EnviromentInformation.MemoryTotal; } }

        [JsonProperty("mfr")]
        public double MemoryFree { get { return EnviromentInformation.MemoryFree; } }

        //[JsonProperty("dtt")]
        //public string DiskTotal { get { return null; } }

        //[JsonProperty("dfr")]
        //public string DiskFree { get { return null; } }
    }
}