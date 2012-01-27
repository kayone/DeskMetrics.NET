using System;

namespace DeskMetrics
{
    public interface IDeskMetricsClient
    {
        bool Started { get; }
        string SessionId { get; }
        string ApplicationId { get; }
        Version ApplicationVersion { get; }
        bool Enabled { get; set; }
        string UserId { get; }

        /// <summary>
        /// Starts the application tracking.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the application tracking and send the collected data to DeskMetrics
        /// </summary>
        void Stop();

        /// <summary>
        /// Register an event occurrence
        /// </summary>
        /// <param name="eventCategory">EventCategory Category</param>
        /// <param name="eventName">EventCategory eventName</param>
        void RegisterEvent(string eventCategory, string eventName);

        /// <summary>
        /// Tracks an event related to time and intervals
        /// </summary>
        /// <param name="eventCategory">
        /// The event category
        /// </param>
        /// <param name="eventName">
        /// The event name
        /// </param>
        /// <param name="eventTime">
        /// The event duration 
        /// </param>
        /// <param name="completed">
        /// True if the event was completed.
        /// </param>
        void RegisterEventPeriod(string eventCategory, string eventName, TimeSpan eventTime, bool completed);

        /// <summary>
        /// Tracks an installation
        /// </summary>
        void RegisterInstall();

        /// <summary>
        /// Tracks an uninstall
        /// </summary>
        void RegisterUninstall();

        /// <summary>
        /// Tracks an exception
        /// </summary>
        /// <param name="exception">
        /// The exception object to be tracked
        /// </param>
        void RegisterException(Exception exception);

        /// <summary>
        /// Tracks an event with custom value
        /// </summary>
        /// <param name="eventCategory">
        /// The event category
        /// </param>
        /// <param name="eventName">
        /// The event name
        /// </param>
        /// <param name="eventValue">
        /// The custom value
        /// </param>
        void RegisterEventValue(string eventCategory, string eventName, string eventValue);

        /// <summary>
        /// Tracks custom data
        /// </summary>
        /// <param name="key">
        /// The custom data name
        /// </param>
        /// <param name="value">
        /// The custom data value
        /// </param>
        void RegisterCustomData(string key, string value);

        /// <summary>
        /// Tracks a log
        /// </summary>
        /// <param name="message">
        /// The log message
        /// </param>
        void RegisterLog(string message);
    }
}