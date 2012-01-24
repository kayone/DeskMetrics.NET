// **********************************************************************//
//                                                                       //
//     DeskMetrics NET - EventType.cs                                    //
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

namespace DeskMetrics.Watcher
{
    public class EventType
    {
        public const string StartApplication = "strApp";
        public const string StopApplication = "stApp";
        public const string Event = "ev";
        public const string CustomData = "ctDR";
        public const string Log = "lg";
        public const string Exception = "exC";
        public const string EventValue = "evV";
        public const string EventPeriod = "evP";
    }
}
