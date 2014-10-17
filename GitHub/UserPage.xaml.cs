using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
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

                string company = user.company;
                if (company != null)
                {
                    this.company.Text = company;
                }
                else
                {
                    this.companyStackPanel.Visibility = System.Windows.Visibility.Collapsed;
                }

                string location = user.location;
                if (location != null)
                {
                    this.location.Text = location;
                }
                else
                {
                    this.locationStackPanel.Visibility = System.Windows.Visibility.Collapsed;
                }

                string email = user.email;
                if (email != null)
                {
                    this.email.Text = email;
                }
                else
                {
                    this.emailStackPanel.Visibility = System.Windows.Visibility.Collapsed;
                }

                string date = user.created_at;
                if (date != null)
                {
                    // Joining date
                    this.date.Text = user.created_at.Split('T')[0];
                }
                else
                {
                    this.dateStackPanel.Visibility = System.Windows.Visibility.Collapsed;
                }

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
            string uri = "/FollowersFollowingPage.xaml?type=Followers&";
            uri += "url=" + user.followers_url;
            this.NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }

        private void followingHyperLink_Click(object sender, RoutedEventArgs e)
        {
            string uri = "/FollowersFollowingPage.xaml?type=Following&";
            uri += "url=" + user.following_url;
            this.NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }

        private void reposHyperLink_Click(object sender, RoutedEventArgs e)
        {
            string navigationUri = "/RepoListPage.xaml?repo=" + user.repos_url;
            this.NavigationService.Navigate(new Uri(navigationUri, UriKind.Relative));
        }
    }
}