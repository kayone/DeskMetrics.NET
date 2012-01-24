using System.Text.RegularExpressions;
namespace DeskMetrics.OperatingSystem.Hardware
{
    public class MacOSXHadware : UnixHardware
    {


        #region IHardware implementation
        public override string ProcessorName
        {
            get
            {
                return GetProcessorName();
            }
        }

        public override int ProcessorArchicteture
        {
            get
            {
                return GetProcessorArchitecture();
            }

        }

        public override int ProcessorCores
        {
            get
            {
                return GetProcessorCores();
            }
        }

        public override string ProcessorBrand
        {
            get
            {
                return "GenuineIntel";
            }
        }

        public override double ProcessorFrequency
        {
            get
            {
                return GetProcessorFrequency();
            }
        }

        public override double MemoryTotal
        {
            get
            {
                return GetTotalMemory();
            }
        }

        #endregion
        string _systemProfiler;
        string _sysctl;

        private string Sysctl
        {
            get
            {
                if (string.IsNullOrEmpty(_sysctl))
                    _sysctl = IOperatingSystem.GetCommandExecutionOutput("sysctl", "-a hw");
                return _sysctl;
            }
        }

        private string SystemProfiler
        {
            get
            {
                if (string.IsNullOrEmpty(_systemProfiler))
                    _systemProfiler = IOperatingSystem.GetCommandExecutionOutput("system_profiler", "");
                return this._systemProfiler;
            }
        }

        string GetProcessorName()
        {
            try
            {
                Regex regex = new Regex(@"Processor EventName\s*:\\s*(?<processor>[\w\s\d\.]+)");
                MatchCollection matches = regex.Matches(SystemProfiler);
                return matches[0].Groups["processor"].Value;
            }
            catch { }

            return "Generic";
        }

        double GetTotalMemory()
        {
            Regex regex = new Regex(@"hw\.memsize\s*(:|=)\s*(?<memory>\d+)");
            MatchCollection matches = regex.Matches(Sysctl);
            return double.Parse(matches[0].Groups["memory"].Value);
        }

        int GetProcessorCores()
        {
            Regex regex = new Regex(@"hw\.availcpu\s*(:|=)\s*(?<cpus>\d+)");
            MatchCollection matches = regex.Matches(Sysctl);
            return int.Parse(matches[0].Groups["cpus"].Value);
        }

        int GetProcessorArchitecture()
        {
            Regex regex = new Regex(@"hw\.cpu64bit_capable\s*(:|=)\s*(?<capable>\d+)");
            MatchCollection matches = regex.Matches(Sysctl);
            if (matches[0].Groups["cpus"].Value == "1")
                return 64;
            return 32;
        }

        double GetProcessorFrequency()
        {
            Regex regex = new Regex(@"hw\.cpufrequency\s*(:|=)\s*(?<cpu_frequency>\d+)");
            MatchCollection matches = regex.Matches(Sysctl);
            return double.Parse(matches[0].Groups["cpu_frequency"].Value);
        }
    }
}