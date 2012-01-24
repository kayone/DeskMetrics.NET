using System;
using DeskMetrics;

namespace DeskMetricsConsole
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var watcher = new Client(Guid.NewGuid().ToString(), "4ea8d347a14ad71442000002",new Version(0, 1) );
            Console.Write("Starting...");
            watcher.Start();
            watcher.TrackInstall();
            Console.WriteLine("[ok]");

            Console.Write("Adding event...");
            watcher.TrackEvent("TrackEvent", "Sample event name");
            Console.WriteLine("[ok]");

            Console.Write("Adding event value...");
            watcher.TrackEventValue("TrackEventValue", "KEventName", "KEventValue");
            Console.WriteLine("[ok]");

            Console.Write("Adding event timed...");
            watcher.TrackEventPeriod("TrackEventPeriod", "KEventName", TimeSpan.FromSeconds(10), true);
            watcher.TrackEventPeriod("TrackEventPeriod", "KEventName", TimeSpan.FromSeconds(1), false);
            Console.WriteLine("[ok]");

            Console.Write("Adding custom data...");
            watcher.TrackCustomData("TrackCustomData", "KCustomData");
            Console.WriteLine("[ok]");

            Console.Write("Adding log");
            watcher.TrackLog("This is my log, babe");
            Console.Write("[ok]");

            Console.Write("Sending stop");
            watcher.Stop();
            Console.ReadLine();
        }
    }
}

