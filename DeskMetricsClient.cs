using System;
using DeskMetrics.DataPoints;
using DeskMetrics.Json;

namespace DeskMetrics
{
    public class DeskMetricsClient : IDisposable
    {
        private readonly Object _objectLock = new Object();
        
        private int _flowglobalnumber;
        private readonly Services _services;

        public  bool Started { get; private set; }
        public string SessionId { get; set; }
        public string ApplicationId { get; private set; }
        public Version ApplicationVersion { get; set; }
        public string Error { get; set; }
        public bool Enabled { get; set; }
        public string UserId { get; private set; }

        public DeskMetricsClient(string userId, string appId, Version appVersion)
        {
            ApplicationId = appId;
            ApplicationVersion = appVersion;
            UserId = userId;
            Enabled = true;
            Started = false;
            SessionId = Guid.NewGuid().ToString().Replace("-", "").ToUpper();

            _services = new Services(this);
        }

        /// <summary>
        /// Starts the application tracking.
        /// </summary>
        public void Start()
        {
            Started = true;
            Post<StartAppDataPoint>();
        }

        /// <summary>
        /// Stops the application tracking and send the collected data to DeskMetrics
        /// </summary>
        public void Stop()
        {
            Post<StopAppDataPoint>();
            Started = false;
        }

        /// <summary>
        /// Register an event occurrence
        /// </summary>
        /// <param name="eventCategory">EventCategory Category</param>
        /// <param name="eventName">EventCategory eventName</param>
        public void TrackEvent(string eventCategory, string eventName)
        {
            var json = new EventDataPoint { EventCategory = eventCategory, EventName = eventName };
            Post(json);
        }

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
        public void TrackEventPeriod(string eventCategory, string eventName, TimeSpan eventTime, bool completed)
        {
            if (Started)
            {
                var json = new EventPeriodDataPoint
                               {
                                   EventCategory = eventCategory,
                                   EventName = eventName,
                                   EventDuration = eventTime.TotalSeconds,
                                   EventCompleted = completed
                               };

                Post(json);
            }
        }

        /// <summary>
        /// Tracks an installation
        /// </summary>
        public void TrackInstall()
        {
            Post<InstallDataPoint>();
        }

        /// <summary>
        /// Tracks an uninstall
        /// </summary>
        public void TrackUninstall()
        {
            Post<UninstallDataPoint>();
        }

        /// <summary>
        /// Tracks an exception
        /// </summary>
        /// <param name="exception">
        /// The exception object to be tracked
        /// </param>
        public void TrackException(Exception exception)
        {
            var json = new ExceptionDataPoint { Exception = exception };
            Post(json);
        }

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
        public void TrackEventValue(string eventCategory, string eventName, string eventValue)
        {
            var json = new EventValueDataPoint { EventCategory = eventCategory, EventName = eventName, EventValue = eventValue };
            Post(json);
        }

        /// <summary>
        /// Tracks custom data
        /// </summary>
        /// <param name="key">
        /// The custom data name
        /// </param>
        /// <param name="value">
        /// The custom data value
        /// </param>
        public void TrackCustomData(string key, string value)
        {
            var json = new CustomDataDataPoint { CustomDataKey = key, CustomDataValue = value };
            Post(json);
        }

        /// <summary>
        /// Tracks a log
        /// </summary>
        /// <param name="message">
        /// The log message
        /// </param>
        public void TrackLog(string message)
        {
            var json = new LogDataPoint { LogMessage = message };
            Post(json);
        }

        private void Post<T>() where T : BaseDataPoint, new()
        {
            Post(new T());
        }

        private void Post(BaseDataPoint dataPoint)
        {
            lock (_objectLock)
            {
                if (!Started)
                    throw new InvalidOperationException("The application is not started");
                
                dataPoint.Flow = _flowglobalnumber++;
                dataPoint.SessionId = SessionId;
                dataPoint.UserId = UserId;
                dataPoint.Version = ApplicationVersion.ToString();

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(dataPoint);
                _services.PostData(json);
            }
        }

        void IDisposable.Dispose()
        {
            try
            {
                if (Started)
                {
                    Stop();
                }
            }
            catch (Exception e)
            {
                Error = e.Message;
            }
        }
    }
}
