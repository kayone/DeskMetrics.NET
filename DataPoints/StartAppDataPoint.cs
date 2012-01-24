using DeskMetrics.OperatingSystem;
using DeskMetrics.OperatingSystem.Hardware;
using Newtonsoft.Json;

namespace DeskMetrics.DataPoints
{
    public class StartAppDataPoint : BaseDataPoint
    {
        private readonly IOperatingSystem _osInfo;
        private readonly IHardware _hardwareInfo;

        public StartAppDataPoint()
        {
            _osInfo = OperatingSystemFactory.GetOperatingSystem();
            _hardwareInfo = _osInfo.Hardware;
        }

        public override string JsonType
        {
            get { return "strApp"; }
        }

        [JsonProperty("osv")]
        public string OsVersion { get { return _osInfo.Version; } }

        [JsonProperty("ossp")]
        public string OsServicePack { get { return _osInfo.ServicePack; } }

        [JsonProperty("osar")]
        public int OsArchitecture { get { return _osInfo.Architecture; } }

        [JsonProperty("osnet")]
        public string FrameworkVersion { get { return _osInfo.FrameworkVersion; } }

        [JsonProperty("osnsp")]
        public string FrameworkSP { get { return _osInfo.ServicePack; } }

        [JsonProperty("oslng")]
        public int OsLanguage { get { return _osInfo.Lcid; } }

        [JsonProperty("osscn")]
        public string ScreenResolution { get { return _hardwareInfo.ScreenResolution; } }

        [JsonProperty("cnm")]
        public string ProcessorName { get { return _hardwareInfo.ProcessorName; } }

        [JsonProperty("car")]
        public int ProcessorArchicteture { get { return _hardwareInfo.ProcessorArchicteture; } }

        [JsonProperty("cbr")]
        public string ProcessorBrand { get { return _hardwareInfo.ProcessorBrand; } }

        [JsonProperty("cfr")]
        public double ProcessorFrequency { get { return _hardwareInfo.ProcessorFrequency; } }

        [JsonProperty("ccr")]
        public double ProcessorCores { get { return _hardwareInfo.ProcessorCores; } }

        [JsonProperty("osjv")]
        public string JavaVersion { get { return _osInfo.JavaVersion; } }

        [JsonProperty("mtt")]
        public double MemoryTotal { get { return _hardwareInfo.MemoryTotal; } }

        [JsonProperty("mfr")]
        public double MemoryFree { get { return _hardwareInfo.MemoryFree; } }

        [JsonProperty("dtt")]
        public string DiskTotal { get { return null; } }

        [JsonProperty("dfr")]
        public string DiskFree { get { return null; } }
    }
}