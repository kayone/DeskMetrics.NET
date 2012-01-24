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

        internal string ApplicationId { get; set; }

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
        /// <param name="appId">
        /// Your app ID. You can get it at http://analytics.deskmetrics.com/
        /// </param>
        /// <param name="appVersion">
        /// Your app version.
        /// </param>
        public void Start()
        {
            Started = true;
            var json = new StartAppJson(this);
            PostToServer(json);
        }

        /// <summary>
        /// Stops the application tracking and send the collected data to DeskMetrics
        /// </summary>
        public void Stop()
        {
            PostToServer(new StopAppJson());
            Started = false;
        }

        /// <summary>
        /// Register an event occurrence
        /// </summary>
        /// <param name="EventCategory">EventCategory Category</param>
        /// <param name="eventName">EventCategory Name</param>
        public void TrackEvent(string EventCategory, string EventName)
        {
            var json = new EventJson(EventCategory, EventName, GetFlowNumber());
            PostToServer(json);
        }

        /// <summary>
        /// Tracks an event related to time and intervals
        /// </summary>
        /// <param name="EventCategory">
        /// The event category
        /// </param>
        /// <param name="EventName">
        /// The event name
        /// </param>
        /// <param name="EventTime">
        /// The event duration 
        /// </param>
        /// <param name="Completed">
        /// True if the event was completed.
        /// </param>
        public void TrackEventPeriod(string EventCategory, string EventName, int EventTime, bool Completed)
        {
            if (Started)
            {
                var json = new EventPeriodJson(EventCategory, EventName, GetFlowNumber(), EventTime, Completed);
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
        public void TrackInstall(string version, string appid)
        {
            var json = new InstallJson(version);
            PostToServer(json);
        }
        /// <summary>
        /// Tracks an uninstall
        /// </summary>
        /// <param name="version">
        /// Your app version
        /// </param>
        /// <param name="appid">
        /// Your app ID. You can get it at http://analytics.deskmetrics.com/
        /// </param>
        public void TrackUninstall(string version, string appid)
        {
            var json = new UninstallJson(version);
            PostToServer(json);
        }

        /// <summary>
        /// Tracks an exception
        /// </summary>
        /// <param name="ApplicationException">
        /// The exception object to be tracked
        /// </param>
        public void TrackException(Exception ApplicationException)
        {
            var json = new ExceptionJson(ApplicationException, GetFlowNumber());
            PostToServer(json);
        }



        /// <summary>
        /// Tracks an event with custom value
        /// </summary>
        /// <param name="EventCategory">
        /// The event category
        /// </param>
        /// <param name="EventName">
        /// The event name
        /// </param>
        /// <param name="EventValue">
        /// The custom value
        /// </param>
        public void TrackEventValue(string EventCategory, string EventName, string EventValue)
        {
            var json = new EventValueJson(EventCategory, EventName, EventValue, GetFlowNumber());
            PostToServer(json);
        }

        /// <summary>
        /// Tracks custom data
        /// </summary>
        /// <param name="CustomDataName">
        /// The custom data name
        /// </param>
        /// <param name="CustomDataValue">
        /// The custom data value
        /// </param>
        public void TrackCustomData(string CustomDataName, string CustomDataValue)
        {
            var json = new CustomDataJson(CustomDataName, CustomDataValue, GetFlowNumber());
            PostToServer(json);
        }

        /// <summary>
        /// Tracks a log
        /// </summary>
        /// <param name="Message">
        /// The log message
        /// </param>
        public void TrackLog(string Message)
        {
            var json = new LogJson(Message, GetFlowNumber());
            PostToServer(json);
        }

        private void PostToServer(BaseJson json)
        {
            lock (_objectLock)
            {
                if (!Started)
                    throw new InvalidOperationException("The application is not started");

                _services.PostData(Settings.ApiEndpoint, json.GetJson(SessionId,UserId));
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
