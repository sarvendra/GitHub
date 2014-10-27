using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using GitHub.Utility;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Net.Http;
using System.ComponentModel;

namespace GitHub
{
    public partial class LoginPage : PhoneApplicationPage
    {
        private const string client_id = "92c35bdc6dd8104a4986";
        private const string client_secret = "ad45424fb64bed02b26b70e2d55b6e47d05d395b";
        private const string redirect_uri = "http://localhost/callback";
        private const string accesstokenuri = "https://github.com/login/oauth/access_token";
        private const string state = "abcd";

        public LoginPage()
        {
            InitializeComponent();
            loginBrowser.Loaded += loginBrowser_Loaded;
            loginBrowser.Navigating += loginBrowser_Navigating;
        }

        async void loginBrowser_Navigating(object sender, NavigatingEventArgs e)
        {
            if (e.Uri.AbsoluteUri.StartsWith(redirect_uri))
            {
                try
                {
                    string uri = e.Uri.AbsoluteUri;
                    string code = uri.Split('?')[1].Split('&')[0].Split('=')[1];

                    // CHECK STATE
                    string currstate = uri.Split('?')[1].Split('&')[1].Split('=')[1];
                    if (currstate != state)
                    {
                        throw new System.ArgumentException("State do not match."); 
                    }

                    var httpClient = new HttpClient(new HttpClientHandler());
                    var values = new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>("client_id", client_id),
                            new KeyValuePair<string, string>("client_secret", client_secret),
                            new KeyValuePair<string, string>("code", code)                     
                        };
                    HttpResponseMessage response = await httpClient.PostAsync(accesstokenuri, new FormUrlEncodedContent(values));
                    response.EnsureSuccessStatusCode();
                    var responseString = await response.Content.ReadAsStringAsync();

                    string accessToken = responseString.Split('&')[0].Split('=')[1];
                    GitHubManager manager = GitHubManager.Instance;
                    manager.Login(accessToken);
                    this.NavigationService.Navigate(new Uri(PageLocator.PROFILE_PAGE, UriKind.Relative));

                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    this.NavigationService.GoBack();
                }
            }
        }

        void loginBrowser_Loaded(object sender, RoutedEventArgs e)
        {
            // clear cache 
            WebBrowserExtensions.ClearInternetCacheAsync(loginBrowser);

            string baseloginUri = "https://github.com/login/oauth/authorize?client_id={0}&scope=user,public_repo&redirect_uri={1}" +
                "&state={2}";
            string loginUri = string.Format(baseloginUri, client_id, redirect_uri, state);
            this.loginBrowser.Navigate(new Uri(loginUri, UriKind.Absolute));
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            this.NavigationService.Navigate(new Uri(PageLocator.START_PAGE, UriKind.Relative));
        }
    }
}