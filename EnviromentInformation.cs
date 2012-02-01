using System;
using System.Linq;
using System.Management;
using System.Threading;
using Microsoft.Win32;

namespace DeskMetrics
{
    internal class EnviromentInformation
    {
        readonly PropertyDataCollection _win32Processor = new ManagementObjectSearcher("select * from Win32_Processor").Get().OfType<ManagementObject>().First().Properties;
        readonly PropertyDataCollection _win32OperatingSystem = new ManagementObjectSearcher("select * from Win32_OperatingSystem").Get().OfType<ManagementObject>().First().Properties;

        public EnviromentInformation()
        {
            GetFrameworkVersion();
        }

        public string ProcessorName
        {
            get
            {
                return _win32Processor["Name"].Value.ToString();
            }
        }

        public int ProcessorArchicteture
        {
            get
            {
                return Convert.ToInt32(_win32Processor["AddressWidth"].Value);
            }
        }

        public int ProcessorCores
        {
            get
            {
                return Convert.ToInt32(_win32Processor["NumberOfCores"].Value);
            }
        }

        public string ProcessorBrand
        {
            get { return CleanUpBrand(_win32Processor["Manufacturer"].Value.ToString()); }
        }

        public double ProcessorFrequency
        {
            get { return Convert.ToInt32(_win32Processor["MaxClockSpeed"].Value); }
        }

        public double MemoryTotal
        {
            get
            {
                return Convert.ToDouble(_win32OperatingSystem["TotalVisibleMemorySize"].Value) * 1024;
            }
        }

        public double MemoryFree
        {
            get { return Convert.ToDouble(_win32OperatingSystem["FreePhysicalMemory"].Value) * 1024; }
        }

        public string ScreenResolution
        {
            get { return string.Empty; }
        }


        public string FrameworkVersion { get; private set; }

        public string FrameworkServicePack { get; private set; }

        public string OsVersion
        {
            get { return RemoveEdition(_win32OperatingSystem["Caption"].Value.ToString()); }
        }

        public string OsServicePack
        {
            get
            {
                return _win32OperatingSystem["CSDVersion"].Value.ToString();
            }
        }

        public int OsArchitecture
        {
            get
            {
                if (Environment.Is64BitOperatingSystem)
                    return 64;

                return 32;
            }
        }

        public string JavaVersion { get; private set; }

        public int Language
        {
            get { return Thread.CurrentThread.CurrentCulture.LCID; }
        }




        private string CleanUpBrand(string brand)
        {
            if (brand.ToLower().Contains("intel")) return "Intel";
            if (brand.ToLower().Contains("amd")) return "AMD";

            return brand;
        }

        private static string RemoveEdition(string caption)
        {
            var lowerCaption = caption.ToLower();
            if (lowerCaption.Contains("windows 7")) return "Windows 7";
            if (lowerCaption.Contains("xp")) return "Windows XP";
            if (lowerCaption.Contains("server 2008 r")) return "Server 2008 R2";
            if (lowerCaption.Contains("server 2008")) return "Server 2008";
            if (lowerCaption.Contains("server 2003 r")) return "Server 2003 R2";
            if (lowerCaption.Contains("server 2003")) return "Server 2003";
            if (lowerCaption.Contains("home server 2003")) return "Server 2003";

            return caption.Replace("Microsoft", "");
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
                    if (key.OpenSubKey("v4") != null)
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

