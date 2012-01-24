// **********************************************************************//
//                                                                       //
//     DeskMetrics NET - Services.cs                                     //
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
using System.Text;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace DeskMetrics.Watcher
{
    public class Services
    {
        public string ProxyHost { get; set; }

        public string ProxyUserName { get; set; }

        public string ProxyPassword { get; set; }

        public int ProxyPort { get; set; }

        internal string PostServer { get; set; }

        public int PostPort { get; set; }

        public int PostTimeOut { get; set; }

        private readonly Client _client;
        
        internal Services(Client client)
        {
            PostTimeOut = Settings.Timeout;
            PostServer = Settings.DefaultServer;
            PostPort = Settings.DefaultPort;
            _client = client;
        }

        private object ObjectLock = new Object();

        internal string PostData(string PostMode, string json)
        {
            lock (ObjectLock)
            {
                string url;

                if (PostPort == 443)
                {
                    ServicePointManager.ServerCertificateValidationCallback +=
                        delegate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors sslError)
                        {
                            bool validationResult = true;
                            return validationResult;
                        };

                    url = "https://" + _client.ApplicationId + "." + Settings.DefaultServer + PostMode;
                }
                else
                {
                    url = "http://" + _client.ApplicationId + "." + Settings.DefaultServer + PostMode;
                }

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = Settings.Timeout;

                if (!string.IsNullOrEmpty(ProxyHost))
                {
                    string uri;

                    WebProxy myProxy = new WebProxy();

                    if (ProxyPort != 0)
                    {
                        uri = ProxyHost + ":" + ProxyPort;
                    }
                    else
                    {
                        uri = ProxyHost;
                    }

                    Uri newUri = new Uri(uri);
                    myProxy.Address = newUri;
                    myProxy.Credentials = new NetworkCredential(ProxyUserName, ProxyPassword);
                    request.Proxy = myProxy;
                }
                else
                {
                    request.Proxy = WebRequest.DefaultWebProxy;
                }

                request.UserAgent = Settings.UserAgent;
                request.KeepAlive = false;
                request.ProtocolVersion = HttpVersion.Version10;
                request.Method = "POST";

                byte[] postBytes = null;

                postBytes = Encoding.UTF8.GetBytes("data=[" + json + "]");

                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = postBytes.Length;

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(postBytes, 0, postBytes.Length);
                requestStream.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader streamreader = new StreamReader(response.GetResponseStream());
                Console.WriteLine(streamreader.ReadToEnd());
                streamreader.Close();
                return "";
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="Log">json message</param>
        internal void SendData(string json)
        {
            lock (ObjectLock)
            {
                if (_client.Started)
                    if (!string.IsNullOrEmpty(_client.ApplicationId) && (_client.Enabled == true))
                        PostData(Settings.ApiEndpoint, json);
            }
        }
    }
}
