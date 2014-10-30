using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using GitHub.Utility;
using GitHub.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Net.Http;
using System.ComponentModel;

namespace GitHub
{
    public partial class LoginPage : PhoneApplicationPage
    {
        private LoginViewModel loginViewModel = new LoginViewModel();
        public LoginViewModel LoginViewModel
        {
            get { return this.loginViewModel; }
        }

        public LoginPage()
        {
            InitializeComponent();
            loginBrowser.Loaded += loginBrowser_Loaded;
            loginBrowser.Navigating += loginBrowser_Navigating;
            BuildLocalizedApplicationBar();
        }

        private void BuildLocalizedApplicationBar()
        {
            ApplicationBar = new ApplicationBar();
            ApplicationBarMenuItem about = new ApplicationBarMenuItem();
            about.Text = "about";
            about.Click += about_Click;
            ApplicationBarMenuItem logout = new ApplicationBarMenuItem();
            logout.Text = "logout";
            logout.IsEnabled = false;
            ApplicationBar.MenuItems.Add(about);
            ApplicationBar.MenuItems.Add(logout);
        }

        private void about_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri(PageLocator.ABOUT_PAGE, UriKind.RelativeOrAbsolute));
        }
        async void loginBrowser_Navigating(object sender, NavigatingEventArgs e)
        {
            if (e.Uri.AbsoluteUri.StartsWith(LoginDefines.REDIRECT_URI))
            {
                try
                {
                    string uri = e.Uri.AbsoluteUri;
                    string code = uri.Split('?')[1].Split('&')[0].Split('=')[1];

                    // CHECK STATE
                    string currstate = uri.Split('?')[1].Split('&')[1].Split('=')[1];
                    if (currstate != LoginDefines.STATE)
                    {
                        throw new System.ArgumentException("State do not match."); 
                    }

                    await this.loginViewModel.GetAccessToken(code);

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

            string loginUri = string.Format(LoginDefines.BASE_LOGIN_URI, LoginDefines.CLIENT_ID, LoginDefines.REDIRECT_URI, LoginDefines.STATE);
            this.loginBrowser.Navigate(new Uri(loginUri, UriKind.Absolute));
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            this.NavigationService.Navigate(new Uri(PageLocator.START_PAGE, UriKind.Relative));
        }
    }
}