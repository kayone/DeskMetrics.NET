using System;
namespace DeskMetrics.OperatingSystem
{
    internal static class OperatingSystemFactory
    {
        public static IOperatingSystem GetOperatingSystem()
        {
            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                if (IOperatingSystem.GetCommandExecutionOutput("uname", "") == "Darwin\n")
                {
                    return new MacOSXOperatingSystem();
                }
                return new UnixOperatingSystem();
            }
            return new WindowsOperatingSystem();
        }
    }
}

