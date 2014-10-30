using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GitHub.ClientServices;
using GitHub.Utility;

namespace GitHub.ViewModels
{
    public class LoginViewModel:ViewModelBase
    {
        private GitHubManager manager = null;
        private HTTPClientService clientService = null;

        public async Task GetAccessToken(string code)
        {
            clientService = new HTTPClientService();
            var values = new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>("client_id", LoginDefines.CLIENT_ID),
                            new KeyValuePair<string, string>("client_secret", LoginDefines.CLIENT_SECRET),
                            new KeyValuePair<string, string>("code", code)                     
                        };

            string responseString = await clientService.PostStringAsync(values);

            string accessToken = responseString.Split('&')[0].Split('=')[1];

            manager = GitHubManager.Instance;
            manager.Login(accessToken);
        }
    }
}
