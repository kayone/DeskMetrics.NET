using Newtonsoft.Json;

namespace DeskMetrics.DataPoints
{
    internal class StartAppDataPoint : BaseDataPoint
    {
        private readonly EnviromentInformation _enviromentInformation;

        public StartAppDataPoint()
        {
            _enviromentInformation = new EnviromentInformation();
        }


        public override string JsonType
        {
            get { return "strApp"; }
        }

        [JsonProperty("osv")]
        public string OsVersion { get { return _enviromentInformation.OsVersion; } }

        [JsonProperty("ossp")]
        public string OsServicePack { get { return _enviromentInformation.OsServicePack; } }

        [JsonProperty("osar")]
        public int OsArchitecture { get { return _enviromentInformation.OsArchitecture; } }

        [JsonProperty("osnet")]
        public string FrameworkVersion { get { return _enviromentInformation.FrameworkVersion; } }

        [JsonProperty("osnsp")]
        public string FrameworkSP { get { return _enviromentInformation.FrameworkServicePack; } }

        [JsonProperty("oslng")]
        public int OsLanguage { get { return _enviromentInformation.Language; } }

        [JsonProperty("osscn")]
        public string ScreenResolution { get { return _enviromentInformation.ScreenResolution; } }

        [JsonProperty("cnm")]
        public string ProcessorName { get { return _enviromentInformation.ProcessorName; } }

        [JsonProperty("car")]
        public int ProcessorArchicteture { get { return _enviromentInformation.ProcessorArchicteture; } }

        [JsonProperty("cbr")]
        public string ProcessorBrand { get { return _enviromentInformation.ProcessorBrand; } }

        [JsonProperty("cfr")]
        public double ProcessorFrequency { get { return _enviromentInformation.ProcessorFrequency; } }

        [JsonProperty("ccr")]
        public double ProcessorCores { get { return _enviromentInformation.ProcessorCores; } }

        [JsonProperty("osjv")]
        public string JavaVersion { get { return _enviromentInformation.JavaVersion; } }

        [JsonProperty("mtt")]
        public double MemoryTotal { get { return _enviromentInformation.MemoryTotal; } }

        [JsonProperty("mfr")]
        public double MemoryFree { get { return _enviromentInformation.MemoryFree; } }

        //[JsonProperty("dtt")]
        //public string DiskTotal { get { return null; } }

        //[JsonProperty("dfr")]
        //public string DiskFree { get { return null; } }
    }
}