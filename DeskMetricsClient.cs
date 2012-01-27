using System;
using DeskMetrics.DataPoints;

namespace DeskMetrics
{
    public class DeskMetricsClient : IDisposable, IDeskMetricsClient
    {
        private readonly Object _objectLock = new Object();

        private int _flowglobalnumber;
        private readonly Services _services;

        /// <summary>
        /// Indicates if the Start() has been called and a session is active.
        /// </summary>
        public bool Started { get; private set; }

        /// <summary>
        /// Currently active session. will be null if no sessions are active.
        /// </summary>
        public string SessionId { get; private set; }

        /// <summary>
        /// DeskmMtrics Application ID
        /// </summary>
        public string ApplicationId { get; private set; }

        /// <summary>
        /// Version of application being tracked.
        /// </summary>
        public Version ApplicationVersion { get; private set; }

        /// <summary>
        /// Checks if application events are tracked.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Anonymous identifier of the user being tracked.
        /// </summary>
        public string UserId { get; private set; }

        public DeskMetricsClient(string userId, string appId, Version appVersion)
        {
            ApplicationId = appId;
            ApplicationVersion = appVersion;
            UserId = userId;
            Enabled = true;
            Started = false;

            _services = new Services(this);
        }

        /// <summary>
        /// Starts the application tracking.
        /// </summary>
        public void Start()
        {
            Started = true;
            SessionId = Guid.NewGuid().ToString().Replace("-", "").ToUpper();
            Register<StartAppDataPoint>();
        }

        /// <summary>
        /// Stops the application tracking and send the collected data to DeskMetrics
        /// </summary>
        public void Stop()
        {
            Register<StopAppDataPoint>();
            Started = false;
            SessionId = null;
        }

        /// <summary>
        /// Register an event occurrence
        /// </summary>
        /// <param name="eventCategory">EventCategory Category</param>
        /// <param name="eventName">EventCategory eventName</param>
        public void RegisterEvent(string eventCategory, string eventName)
        {
            var json = new EventDataPoint { EventCategory = eventCategory, EventName = eventName };
            Register(json);
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
        public void RegisterEventPeriod(string eventCategory, string eventName, TimeSpan eventTime, bool completed)
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

                Register(json);
            }
        }

        /// <summary>
        /// Tracks an installation
        /// </summary>
        public void RegisterInstall()
        {
            RegisterStandAlone<InstallDataPoint>();
        }

        /// <summary>
        /// Tracks an uninstall
        /// </summary>
        public void RegisterUninstall()
        {
            RegisterStandAlone<UninstallDataPoint>();
        }

        /// <summary>
        /// Tracks an exception
        /// </summary>
        /// <param name="exception">
        /// The exception object to be tracked
        /// </param>
        public void RegisterException(Exception exception)
        {
            var json = new ExceptionDataPoint { Exception = exception };
            Register(json);
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
        public void RegisterEventValue(string eventCategory, string eventName, string eventValue)
        {
            var json = new EventValueDataPoint { EventCategory = eventCategory, EventName = eventName, EventValue = eventValue };
            Register(json);
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
        public void RegisterCustomData(string key, string value)
        {
            var json = new CustomDataDataPoint { CustomDataKey = key, CustomDataValue = value };
            RegisterStandAlone(json);
        }

        /// <summary>
        /// Tracks a log
        /// </summary>
        /// <param name="message">
        /// The log message
        /// </param>
        public void RegisterLog(string message)
        {
            var json = new LogDataPoint { LogMessage = message };
            Register(json);
        }

        private void Register<T>() where T : BaseDataPoint, new()
        {
            Register(new T());
        }


        private void Register(BaseDataPoint dataPoint)
        {
            if (!Started)
                throw new InvalidOperationException("The application is not started");

            RegisterStandAlone(dataPoint);
        }

        private void RegisterStandAlone<T>() where T : BaseDataPoint, new()
        {
            RegisterStandAlone(new T());
        }

        private void RegisterStandAlone(BaseDataPoint dataPoint)
        {
            lock (_objectLock)
            {
                var session = SessionId;

                if (String.IsNullOrWhiteSpace(SessionId))
                {
                    session = SessionId = Guid.NewGuid().ToString().Replace("-", "").ToUpper();
                }

                dataPoint.Flow = _flowglobalnumber++;
                dataPoint.SessionId = session;
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
                //Error = e.Message;
            }
        }
    }
}
