using System;
using System.Text;
using System.IO;
using System.Net;

namespace DeskMetrics
{
    internal class Services
    {
        public const string UserAgent = "DeskMetricsNET";
        public const string DefaultServer = ".api.deskmetrics.com/sendData";

        public const int DefaultPort = 443;
        public const int Timeout = 25000; // 20 seconds


        public string ProxyHost { get; set; }

        public string ProxyUserName { get; set; }

        public string ProxyPassword { get; set; }

        public int ProxyPort { get; set; }

        internal string PostServer { get; set; }

        public int PostPort { get; set; }

        public int PostTimeOut { get; set; }

        private readonly DeskMetricsClient _deskMetricsClient;

        internal Services(DeskMetricsClient deskMetricsClient)
        {
            PostTimeOut = Timeout;
            PostServer = DefaultServer;
            PostPort = DefaultPort;
            _deskMetricsClient = deskMetricsClient;
        }

        private readonly object _objectLock = new Object();

        internal void PostData(string json)
        {
            lock (_objectLock)
            {
                string url;

                if (PostPort == 443)
                {
                    ServicePointManager.ServerCertificateValidationCallback +=
                        delegate
                        {
                            return true;
                        };

                    url = "https://" + _deskMetricsClient.ApplicationId + DefaultServer;
                }
                else
                {
                    url = "http://" + _deskMetricsClient.ApplicationId + DefaultServer;
                }

                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = Timeout;

                if (!String.IsNullOrEmpty(ProxyHost))
                {
                    string uri;

                    var myProxy = new WebProxy();

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

                request.UserAgent = UserAgent;
                request.KeepAlive = false;
                request.ProtocolVersion = HttpVersion.Version10;
                request.Method = "POST";

                byte[] postBytes = Encoding.UTF8.GetBytes("data=[" + json + "]");
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = postBytes.Length;

                var requestStream = request.GetRequestStream();
                requestStream.Write(postBytes, 0, postBytes.Length);
                requestStream.Close();

                var response = (HttpWebResponse)request.GetResponse();
                var streamreader = new StreamReader(response.GetResponseStream());
                streamreader.Close();
            }
        }
    }
}
