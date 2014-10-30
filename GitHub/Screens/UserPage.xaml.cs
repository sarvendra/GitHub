using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using GitHub.Model;
using GitHub.Utility;
using GitHub.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;
using System.Windows.Media.Imaging;

namespace GitHub
{
    public partial class UserPage : PhoneApplicationPage
    {
        private UserViewModel userViewModel = new UserViewModel();
        public UserViewModel UserViewModel
        {
            get { return this.userViewModel; }
        }

        private string loginName = null;
        public UserPage()
        {
            InitializeComponent();
            Loaded += UserPage_Loaded;
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
            logout.Click += logout_Click;
            ApplicationBar.MenuItems.Add(about);
            ApplicationBar.MenuItems.Add(logout);
        }

        private void about_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri(PageLocator.ABOUT_PAGE, UriKind.RelativeOrAbsolute));
        }

        private void logout_Click(object sender, EventArgs e)
        {
            try
            {
                // perform logout operation
                GitHubManager manager = GitHubManager.Instance;
                manager.Logout();
                NavigationService.Navigate(new Uri(PageLocator.START_PAGE, UriKind.RelativeOrAbsolute));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        async void UserPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                SystemTray.ProgressIndicator = new ProgressIndicator();
                SystemTray.ProgressIndicator.Text = "loading";
                setProgressIndicator(true);
                await this.userViewModel.GetUserProfile(loginName);
                setProgressIndicator(false);
                // followers
                followersHyperLink.Content = this.userViewModel.NoOfFollowers + " followers";
                // following
                followingHyperLink.Content = this.userViewModel.NoOfFollowing + " following";
                // repos
                reposHyperLink.Content = this.userViewModel.NoOfRepos + " repos";

                ApplicationBarMenuItem logoutMenuItem = (ApplicationBarMenuItem)ApplicationBar.MenuItems[1];
                if (!userViewModel.IsLoggedIn())
                {
                    logoutMenuItem.IsEnabled = false;
                }
                else
                {
                    logoutMenuItem.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void setProgressIndicator(bool isVisible)
        {
            SystemTray.ProgressIndicator.IsIndeterminate = isVisible;
            SystemTray.ProgressIndicator.IsVisible = isVisible;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            loginName = NavigationContext.QueryString["name"];
        }

        private void followersHyperLink_Click(object sender, RoutedEventArgs e)
        {
            string uri = PageLocator.FOLLOWERSFOLLOWING_PAGE+"?type=Followers&";
            uri += "url=" + this.userViewModel.Followers_URL;
            this.NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }

        private void followingHyperLink_Click(object sender, RoutedEventArgs e)
        {
            string uri = PageLocator.FOLLOWERSFOLLOWING_PAGE+"?type=Following&";
            uri += "url=" + this.userViewModel.Following_URL;
            this.NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }

        private void reposHyperLink_Click(object sender, RoutedEventArgs e)
        {
            string navigationUri = PageLocator.REPOLIST_PAGE+"?repo=" + this.userViewModel.Repos_URL;
            this.NavigationService.Navigate(new Uri(navigationUri, UriKind.Relative));
        }
    }
}