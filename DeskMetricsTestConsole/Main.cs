using System;
using DeskMetrics;

namespace DeskMetricsConsole
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var watcher = new DeskMetricsClient(Guid.NewGuid().ToString(), "4ea8d347a14ad71442000002",new Version(0, 1) );
            Console.Write("Starting...");
            
            watcher.RegisterInstall();
            watcher.Start();
            Console.WriteLine("[ok]");

            Console.Write("Adding event...");
            watcher.RegisterEvent("RegisterEvent", "Sample event name");
            Console.WriteLine("[ok]");

            Console.Write("Adding event value...");
            watcher.RegisterEventValue("RegisterEventValue", "KEventName", "KEventValue");
            Console.WriteLine("[ok]");

            Console.Write("Adding event timed...");
            watcher.RegisterEventPeriod("RegisterEventPeriod", "KEventName", TimeSpan.FromSeconds(10), true);
            watcher.RegisterEventPeriod("RegisterEventPeriod", "KEventName", TimeSpan.FromSeconds(1), false);
            Console.WriteLine("[ok]");

            Console.Write("Adding custom data...");
            watcher.RegisterCustomData("RegisterCustomData", "KCustomData");
            Console.WriteLine("[ok]");

            Console.Write("Adding log");
            watcher.RegisterLog("This is my log, babe");
            Console.Write("[ok]");

            Console.Write("Sending stop");
            watcher.Stop();
            Console.ReadLine();
        }
    }
}

