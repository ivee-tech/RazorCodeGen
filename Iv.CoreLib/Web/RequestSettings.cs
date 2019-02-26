using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Iv.Web
{
    public class RequestSettings
    {

        public RequestSettings()
        {
            Headers = new Dictionary<string, string>();
        }

        public string Address { get; set; }

        public string Domain { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public bool UseDefaultCredentials { get; set; }

        public string Referer { get; set; }

        public bool UseProxy { get; set; }

        public string ProxyHost { get; set; }

        public Int32 ProxyPort { get; set; }

        public string ProxyDomain { get; set; }

        public string ProxyUserName { get; set; }

        public string ProxyPassword { get; set; }

        public bool ProxyUseDefaultCredentials { get; set; }

        public string UserAgent { get; set; }

        public IDictionary<string, string> Headers { get; private set; }

        public RequestSettings Clone()
        {
            var s = new RequestSettings();
            s.Address = Address;
            s.Domain = Domain;
            s.UserName = UserName;
            s.Password = Password;
            s.UseDefaultCredentials = UseDefaultCredentials;
            s.Referer = Referer;
            s.UseProxy = UseProxy;
            s.ProxyHost = ProxyHost;
            s.ProxyPort = ProxyPort;
            s.ProxyDomain = ProxyDomain;
            s.ProxyUserName = ProxyUserName;
            s.ProxyPassword = ProxyPassword;
            s.ProxyUseDefaultCredentials = ProxyUseDefaultCredentials;
            s.UserAgent = UserAgent;
            foreach (var kvp in Headers)
                s.Headers.Add(kvp.Key, kvp.Value);
            return s;
        }

    }
}
