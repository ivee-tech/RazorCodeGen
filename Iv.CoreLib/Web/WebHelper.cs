using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Security;
using System.Security.Principal;
using System.Web.Routing;
using System.Configuration;
using System.Web.Configuration;
using System.Collections.Specialized;
using System.Net;
using System.IO;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;

// using HtmlAgilityPack;

namespace Iv.Web
{

	public class WebHelper
	{

		public static string GetIdentityName(RequestContext requestContext)
		{
			string sUserName = requestContext.HttpContext.User.Identity.Name;
			bool isBasicAuth = requestContext.HttpContext.Request.Headers["Authorization"] != null && requestContext.HttpContext.Request.Headers["Authorization"].StartsWith("Basic");

			if ((isBasicAuth)) {
				string encodedHeader = requestContext.HttpContext.Request.Headers["Authorization"].Substring(6);
				string decodedHeader = new System.Text.ASCIIEncoding().GetString(Convert.FromBase64String(encodedHeader));
				string[] detail = decodedHeader.Split(Convert.ToChar(":"));
				sUserName = detail[0];
			} else {
				sUserName = requestContext.HttpContext.User.Identity.Name;
			}
			return sUserName;
		}

		public static AuthenticationMode GetAuthMode()
		{
			Configuration cfg = WebConfigurationManager.OpenWebConfiguration("~/");
			AuthenticationSection authenticationSection = (AuthenticationSection)cfg.GetSection("system.web/authentication");
			return authenticationSection.Mode;
		}

		public static ProviderSettings GetMembershipProvider(string providerName)
		{
			Configuration config = WebConfigurationManager.OpenWebConfiguration("~/");
			MembershipSection section = (MembershipSection)config.GetSection("system.web/membership");
			ProviderSettingsCollection settingsCol = section.Providers;
			return settingsCol[providerName];
		}

		public static NameValueCollection GetMembershipProviderConfiguration(string providerName)
		{
			ProviderSettings providerSettings = GetMembershipProvider(providerName);
			if (providerSettings == null) {
				return new NameValueCollection();
			}
			NameValueCollection membershipParams = providerSettings.Parameters;
			return membershipParams;
		}

		public static ProviderSettings GetRoleProvider(string providerName)
		{
			Configuration config = WebConfigurationManager.OpenWebConfiguration("~/");
			RoleManagerSection section = (RoleManagerSection)config.GetSection("system.web/roleManager");
			ProviderSettingsCollection settingsCol = section.Providers;
			return settingsCol[providerName];
		}

		public static NameValueCollection GetRoleProviderConfiguration(string providerName)
		{
			ProviderSettings providerSettings = GetRoleProvider(providerName);
			if (providerSettings == null) {
				return new NameValueCollection();
			}
			NameValueCollection roleParams = providerSettings.Parameters;
			return roleParams;
		}

        public static GlobalizationSection GetGlobalization()
		{
			Configuration cfg = WebConfigurationManager.OpenWebConfiguration("~/");
            GlobalizationSection section = (GlobalizationSection)cfg.GetSection("system.web/globalization");
			return section;
		}

        /*
        public static void ExecuteRequest(string address, string referrer, RequestSettings settings, string postData, CookieContainer cc, out HtmlDocument doc, out CookieContainer outCC)
        {
            doc = null;
            outCC = null;

            HttpWebResponse response = GetWebResponse(address, referrer, settings, postData, cc);
            if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
            {
                outCC = new CookieContainer();
                foreach (Cookie c in response.Cookies)
                {
                    //Console.Out.WriteLine("{0}={1}", c.Name, c.Value)
                    outCC.Add(c);
                }

                Stream dataStream = response.GetResponseStream();
                doc = new HtmlDocument();
                doc.Load(dataStream);
                dataStream.Close();
                response.Close();
            }
        }
        */

        public static HttpWebResponse GetWebResponse(string address, string referrer, RequestSettings settings, string postData, CookieContainer cc)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(address);
            if (settings.UseProxy && !(string.IsNullOrEmpty(settings.ProxyHost)))
            {
                var proxy = new WebProxy(string.Format("{0}:{1}", settings.ProxyHost, settings.ProxyPort), true);
                if (settings.ProxyUseDefaultCredentials)
                {
                    proxy.UseDefaultCredentials = true;
                }
                else
                {
                    proxy.Credentials = new NetworkCredential(settings.ProxyUserName, settings.ProxyPassword, settings.ProxyDomain);
                }
                request.Proxy = proxy;
            }
            request.UserAgent = settings.UserAgent;
            request.Referer = referrer;
            if (settings.UseDefaultCredentials)
            {
                request.UseDefaultCredentials = true;
            }
            else
            {
                request.Credentials = new NetworkCredential(settings.UserName, settings.Password, settings.Domain);
            }
            if (cc != null)
            {
                request.CookieContainer = cc;
            }

            foreach (var kvp in settings.Headers)
            {
                request.Headers.Add(kvp.Key, kvp.Value);
            }

            Stream dataStream = default(Stream);
            if (!(string.IsNullOrEmpty(postData)))
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;
                request.Method = "POST";

                dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
            }

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            return response;
        }

        public static HttpClient CreateClient(string baseAddress)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseAddress);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Accept-Language", "en-GB,en-US;q=0.8,en;q=0.6,ru;q=0.4");
            return client;
        }

    }

}