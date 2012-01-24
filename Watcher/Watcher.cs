// **********************************************************************//
//                                                                       //
//     DeskMetrics NET - Watcher.cs                                      //
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
using System.Reflection;
using System.Net;
using DeskMetrics.Json;

namespace DeskMetrics
{
    public class Watcher : IDisposable
    {
        private readonly Object _objectLock = new System.Object();
        private string SessionGUID { get; set; }
        private string _componentName;
        private string _componentVersion;
        private int _flowglobalnumber = 0;
        private readonly Services _services;
        private readonly CurrentUser _user;


        internal bool Started { get; private set; }

        internal string ApplicationId { get; set; }

        internal Version ApplicationVersion { get; set; }

        internal string Error { get; set; }

        public bool Enabled { get; set; }
        
        public string UserId { get; private set; }

        public string ComponentName
        {
            get
            {
                Assembly thisAsm = this.GetType().Assembly;
                object[] attrs = thisAsm.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                _componentName = ((AssemblyTitleAttribute)attrs[0]).Title;
                return _componentName;
            }
        }

        public string ComponentVersion
        {
            get
            {
                _componentVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                return _componentVersion;
            }

        }

        public Watcher(string userId)
        {
            UserId = userId;
            Enabled = true;
            Started = false;
            _services = new Services(this);
            _user = new CurrentUser();
        }
        
        internal void CheckApplicationCorrectness()
        {
            if (string.IsNullOrEmpty(ApplicationId.Trim()))
                throw new Exception("You must specify an non-empty application ID");
            
            if (!Enabled)
                throw new InvalidOperationException("The application is stopped or not enabled");
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
        public void Start(string appId, Version appVersion)
        {
            SessionGUID = _user.GetSessionID();
            this.ApplicationId = appId;
            this.ApplicationVersion = appVersion;

            CheckApplicationCorrectness();

            lock (_objectLock)
                if (Enabled)
                    StartAppJson();
            Started = true;
            //SendDataAsync();
        }

        private void StartAppJson()
        {
            var startjson = new StartAppJson(this);
            PostToServer(startjson.GetJson());
        }

        /// <summary>
        /// Stops the application tracking and send the collected data to DeskMetrics
        /// </summary>
        public void Stop()
        {
            CheckApplicationCorrectness();
            lock (_objectLock)
            {
                PostToServer(new StopAppJson().GetJson());
            }
        }

        private void PostToServer(string message)
        {
            lock (_objectLock)
            {
                CheckApplicationCorrectness();
                _services.PostData(Settings.ApiEndpoint, message);
            }
        }

        /// <summary>
        /// Register an event occurence
        /// </summary>
        /// <param name="EventCategory">EventCategory Category</param>
        /// <param name="eventName">EventCategory Name</param>
        public void TrackEvent(string EventCategory, string EventName)
        {
            lock (_objectLock)
            {
                if (Started)
                {
                    CheckApplicationCorrectness();
                    var json = new EventJson(EventCategory, EventName, GetFlowNumber());
                    PostToServer(json.GetJson());
                }
            }
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
            lock (_objectLock)
            {
                if (Started)
                {
                    CheckApplicationCorrectness();
                    var json = new EventPeriodJson(EventCategory, EventName, GetFlowNumber(), EventTime, Completed);
                    PostToServer(json.GetJson());
                }
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
            lock (_objectLock)
            {
                var json = new InstallJson(version);
                ApplicationId = appid;
                Started = true;
                try
                {
                    _services.SendData(JsonBuilder.GetJsonFromHashTable(json.GetJsonHashTable()));
                }
                catch (WebException)
                {
                    // only hide unhandled exception due no internet connection
                }
            }
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
            lock (_objectLock)
            {
                var json = new UninstallJson(version);
                ApplicationId = appid;
                Started = true;
                try
                {
                    _services.SendData(JsonBuilder.GetJsonFromHashTable(json.GetJsonHashTable()));
                }
                catch (WebException)
                {
                    // only hide unhandled exception due no internet connection
                }
            }
        }

        /// <summary>
        /// Tracks an exception
        /// </summary>
        /// <param name="ApplicationException">
        /// The exception object to be tracked
        /// </param>
        public void TrackException(Exception ApplicationException)
        {
            lock (_objectLock)
            {
                if (Started && ApplicationException != null)
                {
                    CheckApplicationCorrectness();
                    var json = new ExceptionJson(ApplicationException, GetFlowNumber());
                    PostToServer(json.GetJson());
                }
            }
        }

        /// <summary>
        /// </summary>
        void IDisposable.Dispose()
        {
            try
            {
                this.Stop();
            }
            catch (Exception e)
            {
                Error = e.Message;
            }
        }

        protected int GetFlowNumber()
        {
            lock (_objectLock)
            {
                try
                {
                    _flowglobalnumber = _flowglobalnumber + 1;
                    return _flowglobalnumber;
                }
                catch
                {
                    return 0;
                }
            }
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
            lock (_objectLock)
            {
                if (Started)
                {
                    CheckApplicationCorrectness();
                    var json = new EventValueJson(EventCategory, EventName, EventValue, GetFlowNumber());
                    PostToServer(json.GetJson());
                }
            }
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
            lock (_objectLock)
            {
                if (Started)
                {
                    CheckApplicationCorrectness();
                    var json = new CustomDataJson(CustomDataName, CustomDataValue, GetFlowNumber());
                    PostToServer(json.GetJson());
                }
            }
        }

        /// <summary>
        /// Tracks a log
        /// </summary>
        /// <param name="Message">
        /// The log message
        /// </param>
        public void TrackLog(string Message)
        {
            lock (_objectLock)
            {
                if (Started)
                {
                    CheckApplicationCorrectness();
                    var json = new LogJson(Message, GetFlowNumber());
                    PostToServer(json.GetJson());
                }
            }
        }


        /// <summary>
        /// Try to track real time customized data and caches it to send later if any network error occurs.
        /// </summary>
        /// <param name="CustomDataName">
        /// A <see cref="System.String"/>
        /// </param>
        /// <param name="CustomDataValue">
        /// A <see cref="System.String"/>
        /// </param>
        /// <returns>
        /// True if it was sended in real time, false otherwise
        /// </returns>
        public bool TrackCachedCustomDataR(string CustomDataName, string CustomDataValue)
        {
            try
            {
                TrackCustomDataR(CustomDataName, CustomDataValue);
            }
            catch (Exception)
            {
                lock (_objectLock)
                {
                    var json = new CustomDataRJson(CustomDataName, CustomDataValue, GetFlowNumber(), ApplicationId, ApplicationVersion);
                    PostToServer(json.GetJson());
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Tracks a custom data without cache support
        /// </summary>
        /// <param name="CustomDataName">
        /// Self-explanatory ;)
        /// </param>
        /// <param name="CustomDataValue">
        /// Self-explanatory ;)
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public void TrackCustomDataR(string CustomDataName, string CustomDataValue)
        {
            lock (_objectLock)
            {
                if (Started)
                {
                    CheckApplicationCorrectness();
                    try
                    {
                        var json = new CustomDataRJson(CustomDataName, CustomDataValue, GetFlowNumber(), ApplicationId, ApplicationVersion);
                        _services.PostData(Settings.ApiEndpoint, JsonBuilder.GetJsonFromHashTable(json.GetJsonHashTable()));
                    }
                    catch (WebException)
                    {
                        // only hide unhandled exception due no internet connection
                    }
                }
            }
        }
    }
}
