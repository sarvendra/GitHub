using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GitHub.Interfaces;
using GitHub.Utility;

namespace GitHub.ClientServices
{
    public class HTTPClientService:IClientService
    {
        private HttpClient client = null;

        public async Task<string> GetStringAsync(string uri)
        {
            client = new HttpClient();
            string response = await client.GetStringAsync(uri);
            return response;
        }

        public async Task<string> PostStringAsync(List<KeyValuePair<string, string>> values)
        {
            var httpClient = new HttpClient(new HttpClientHandler());
            HttpResponseMessage response = await httpClient.PostAsync(LoginDefines.ACCESS_TOKEN_URI, new FormUrlEncodedContent(values));
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }
    }
}
