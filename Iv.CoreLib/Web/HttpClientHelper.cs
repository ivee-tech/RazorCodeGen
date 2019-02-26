using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Iv.Web
{
    public class HttpClientHelper 
    {
        private static HttpClient _client;

        public HttpClientHelper(string baseAddress)
        {
            _client = WebHelper.CreateClient(baseAddress);
        }

        public HttpClientHelper(HttpClient client)
        {
            _client = client;
        }

        public HttpClient Client
        {
            get
            {
                return _client;
            }
        }

        public async Task<T> GetAsync<T>(string path)
            where T : class
        {
            T obj = default(T);
            HttpResponseMessage response = await _client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                obj = await response.Content.ReadAsAsync<T>();
            }
            return obj;
        }

        public async Task<HttpResponseMessage> PostAsync<T>(T obj, string path)
            where T : class
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync(
                path, obj);
            response.EnsureSuccessStatusCode();

            return response;
        }

        public async Task<HttpResponseMessage> PutAsync<T>(T obj, string path)
            where T : class
        {
            HttpResponseMessage response = await _client.PutAsJsonAsync(
                path, obj);
            response.EnsureSuccessStatusCode();

            return response;
        }

        public async Task<HttpResponseMessage> DeleteAsync(string path)
        {
            HttpResponseMessage response = await _client.DeleteAsync(
                path);
            response.EnsureSuccessStatusCode();

            return response;
        }

    }
}
