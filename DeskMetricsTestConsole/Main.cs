using System;
using DeskMetrics;
using DeskMetrics.Watcher;

namespace DeskMetricsConsole
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var watcher = new Client(new Guid().ToString(), "4ea8d347a14ad71442000002",new Version(0, 1) );
            Console.Write("Starting...");
            watcher.Start();
            Console.WriteLine("[ok]");

            Console.Write("Adding event...");
            watcher.TrackEvent("DeskMetricsConsole", "InitialEvent");
            Console.WriteLine("[ok]");

            Console.Write("Adding event value...");
            watcher.TrackEventValue("DeskMetricsConsole", "EventValue", "Aha!");
            Console.WriteLine("[ok]");

            Console.Write("Adding event timed...");
            watcher.TrackEventPeriod("DeskMetricsConsole", "EventValue", 100, true);
            watcher.TrackEventPeriod("DeskMetricsConsole", "EventValue", 30, false);
            Console.WriteLine("[ok]");

            Console.Write("Adding custom data...");
            watcher.TrackCustomData("DeskMetricsConsole", "CustomData");
            Console.WriteLine("[ok]");

            Console.Write("Adding log");
            watcher.TrackLog("This is my log, babe");
            Console.Write("[ok]");

            Console.Write("Finishing app and sending data to DeskMetrics");
            watcher.Stop();
            Console.ReadLine();
        }
    }
}

