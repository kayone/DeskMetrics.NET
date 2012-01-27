using System;
using System.Linq;
using System.Management;
using System.Threading;
using Microsoft.Win32;

namespace DeskMetrics
{
    internal class EnviromentInformation
    {

        private static readonly PropertyDataCollection Win32Processor;
        private static readonly PropertyDataCollection Win32OperatingSystem;

        public string ProcessorName { get; private set; }

        public int ProcessorArchicteture { get; private set; }

        public int ProcessorCores { get; private set; }

        public string ProcessorBrand { get; private set; }

        public double ProcessorFrequency { get; private set; }

        public double MemoryTotal { get; private set; }

        public double MemoryFree { get; private set; }

        public string ScreenResolution { get; private set; }


        public string FrameworkVersion { get; private set; }

        public string FrameworkServicePack { get; private set; }

        public string OsVersion { get; private set; }

        public string OsServicePack { get; private set; }

        public int OsArchitecture { get; private set; }

        public string JavaVersion { get; private set; }

        public int Language { get; private set; }

        static EnviromentInformation()
        {
            Win32Processor = new ManagementObjectSearcher("select * from Win32_Processor").Get().OfType<ManagementObject>().First().Properties;
            Win32OperatingSystem = new ManagementObjectSearcher("select * from Win32_OperatingSystem").Get().OfType<ManagementObject>().First().Properties;
        }


        public EnviromentInformation()
        {
            ProcessorName = Win32Processor["Name"].Value.ToString();
            ProcessorArchicteture = Convert.ToInt32(Win32Processor["AddressWidth"].Value);
            ProcessorCores = Convert.ToInt32(Win32Processor["NumberOfCores"].Value);
            ProcessorBrand = Win32Processor["Manufacturer"].Value.ToString();
            ProcessorFrequency = Convert.ToInt32(Win32Processor["MaxClockSpeed"].Value);

            MemoryTotal = Convert.ToDouble(Win32OperatingSystem["TotalVisibleMemorySize"].Value);
            MemoryFree = Convert.ToDouble(Win32OperatingSystem["FreePhysicalMemory"].Value);

            OsVersion = Win32OperatingSystem["Caption"].Value.ToString();
            OsServicePack = Win32OperatingSystem["CSDVersion"].Value.ToString();

            Language = Thread.CurrentThread.CurrentCulture.LCID;

            if (Win32OperatingSystem["OSArchitecture"].Value.ToString().Contains("32"))
            {
                OsArchitecture = 32;
            }
            else
            {
                OsArchitecture = 64;
            }

            GetFrameworkVersion();

            ScreenResolution = string.Empty;
        }

        private void GetFrameworkVersion()
        {
            try
            {
                FrameworkVersion = Environment.Version.ToString();
                FrameworkServicePack = "";

                var key = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\NET Framework Setup\NDP");
                if (key != null)
                {
                    if ((key.OpenSubKey("v4")) != null)
                    {
                        FrameworkVersion = "4.0";
                        FrameworkServicePack = "0";
                        return;
                    }

                    if ((key.OpenSubKey("v3.5")) != null)
                    {
                        FrameworkVersion = "3.5";
                        FrameworkServicePack = key.GetValue("SP", 0).ToString();
                        return;
                    }

                    if ((key.OpenSubKey("v3.0")) != null)
                    {
                        FrameworkVersion = "3.0";
                        FrameworkServicePack = key.GetValue("SP", 0).ToString();
                        return;
                    }

                    if ((key.OpenSubKey("v2.0.50727")) != null)
                    {
                        FrameworkVersion = "2.0.50727";
                        FrameworkServicePack = key.GetValue("SP", 0).ToString();
                        return;
                    }

                    if ((key.OpenSubKey("v1.1.4322")) != null)
                    {
                        FrameworkVersion = "1.1.4322";
                        FrameworkServicePack = key.GetValue("SP", 0).ToString();
                        return;
                    }


                    if ((key.OpenSubKey("v1.0")) != null)
                    {
                        FrameworkVersion = "1.0";
                        FrameworkServicePack = key.GetValue("SP", 0).ToString();
                    }
                }
            }
            catch
            {

            }
        }
    }
}

