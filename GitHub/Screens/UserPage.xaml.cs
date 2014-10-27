using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using GitHub.Model;
using GitHub.Utility;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;
using System.Windows.Media.Imaging;

namespace GitHub
{
    public partial class UserPage : PhoneApplicationPage
    {
        private string loginName = null;
        private User user = null;
        public UserPage()
        {
            InitializeComponent();
            Loaded += UserPage_Loaded;
        }

        async void UserPage_Loaded(object sender, RoutedEventArgs e)
        {
            GitHubManager manager = GitHubManager.Instance;
            string response = await manager.GetUserProfile(loginName);
            user = JsonConvert.DeserializeObject<User>(response);
            if (user != null)
            {
                DataContext = user;

                this.profileUserControl.SetUserInfo(user);

                // followers
                followersHyperLink.Content = user.followers.ToString() + " followers";
                // following
                followingHyperLink.Content = user.following.ToString() + " following";
                // repos
                reposHyperLink.Content = user.public_repos.ToString() + " repos";
            }

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            loginName = NavigationContext.QueryString["name"];
        }

        private void followersHyperLink_Click(object sender, RoutedEventArgs e)
        {
            string uri = PageLocator.FOLLOWERSFOLLOWING_PAGE+"?type=Followers&";
            uri += "url=" + user.followers_url;
            this.NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }

        private void followingHyperLink_Click(object sender, RoutedEventArgs e)
        {
            string uri = PageLocator.FOLLOWERSFOLLOWING_PAGE+"?type=Following&";
            uri += "url=" + user.following_url;
            this.NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }

        private void reposHyperLink_Click(object sender, RoutedEventArgs e)
        {
            string navigationUri = PageLocator.REPOLIST_PAGE+"?repo=" + user.repos_url;
            this.NavigationService.Navigate(new Uri(navigationUri, UriKind.Relative));
        }
    }
}