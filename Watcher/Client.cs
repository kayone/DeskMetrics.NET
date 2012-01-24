// **********************************************************************//
//                                                                       //
//     DeskMetrics NET - Client.cs                                      //
//     Copyright (c) 2010-2011 DeskMetrics Limited                       //
//                                                                       //
//     http://deskmetrics.com                                            //
//     http://support.deskmetrics.com                                    //
//                                                                       //
//     support@deskmetrics.com                                           //
//                                                                       //
//     This code is provided under the DeskMetrics Modified BSD License  //
//     A copy of this license has been distributed in a file called      //
//     LICENSE with this source code.                                    //
//                                                                       //
// **********************************************************************//

using System;
using System.Collections.Generic;
using DeskMetrics.DataPoints;
using DeskMetrics.Json;

namespace DeskMetrics.Watcher
{
    public class Client : IDisposable
    {
        private readonly Object _objectLock = new Object();
        private string SessionId { get; set; }
        private int _flowglobalnumber;
        private readonly Services _services;

        internal bool Started { get; private set; }

        internal string ApplicationId { get; private set; }

        internal Version ApplicationVersion { get; set; }

        internal string Error { get; set; }

        public bool Enabled { get; set; }

        public string UserId { get; private set; }

        public Client(string userId, string appId, Version appVersion)
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
            var json = new StartAppDataPoint();
            PostToServer(json);
        }

        /// <summary>
        /// Stops the application tracking and send the collected data to DeskMetrics
        /// </summary>
        public void Stop()
        {
            PostToServer(new StopAppDataPoint());
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
            PostToServer(json);
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

                PostToServer(json);
            }
        }

        /// <summary>
        /// Tracks an installation
        /// </summary>
        /// <param name="version">
        /// Your app version
        /// </param>
        /// <param name="appid">
        /// Your app ID. You can get it at http://analytics.deskmetrics.com/
        /// </param>
        public void TrackInstall()
        {
            var json = new InstallDataPoint();
            PostToServer(json);
        }
        /// <summary>
        /// Tracks an uninstall
        /// </summary>
        public void TrackUninstall()
        {
            var json = new UninstallDataPoint();
            PostToServer(json);
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
            PostToServer(json);
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
            PostToServer(json);
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
            PostToServer(json);
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
            PostToServer(json);
        }

        private void PostToServer(BaseDataPoint dataPoint)
        {
            lock (_objectLock)
            {
                //if (!Started)
                //throw new InvalidOperationException("The application is not started");

                //dataPoint.Flow = GetFlowNumber();
                dataPoint.SessionId = SessionId;
                dataPoint.UserId = UserId;
                dataPoint.Version = ApplicationVersion.ToString();

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(dataPoint);
                _services.PostData(Settings.ApiEndpoint, json);
            }
        }

        private int GetFlowNumber()
        {
            lock (_objectLock)
            {
                _flowglobalnumber = _flowglobalnumber + 1;
                return _flowglobalnumber;
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
