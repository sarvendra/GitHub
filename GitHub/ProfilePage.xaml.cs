using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.ComponentModel;
using Newtonsoft.Json;

namespace GitHub
{
    public partial class ProfilePage : PhoneApplicationPage
    {
        private User user = null;
        public ProfilePage()
        {
            InitializeComponent();
            Loaded += ProfilePage_Loaded;
        }

        async void ProfilePage_Loaded(object sender, RoutedEventArgs e)
        {
            GitHubManager manager = GitHubManager.Instance;
            string response = await manager.GetAuthenticatedUserProfile();
            if (response == null)
                return;
            user = JsonConvert.DeserializeObject<User>(response);
            if (user == null)
                return;
            DataContext = user;

            string company = user.company;
            if (company != null)
            {
                this.company.Text = company;
                this.companyStackPanel.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                this.companyStackPanel.Visibility = System.Windows.Visibility.Collapsed;
            }

            string location = user.location;
            if (location != null)
            {
                this.location.Text = location;
                this.locationStackPanel.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                this.locationStackPanel.Visibility = System.Windows.Visibility.Collapsed;
            }

            string email = user.email;
            if (email != null)
            {
                this.email.Text = email;
                this.emailStackPanel.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                this.emailStackPanel.Visibility = System.Windows.Visibility.Collapsed;
            }

            string date = user.created_at;
            if (date != null)
            {
                // Joining date
                this.dateStackPanel.Visibility = System.Windows.Visibility.Visible;
                this.date.Text = user.created_at.Split('T')[0];
            }
            else
            {
                this.dateStackPanel.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit?", "Confirm Exit?",
                MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                Application.Current.Terminate();
            }
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Pivot pivot = sender as Pivot;
            if (pivot == null)
                return;
            PivotItem item = pivot.SelectedItem as PivotItem;
            if (item == null)
                return;
            if (item.Header.ToString() == "following")
            {
                DisplayFollowing();
            }
            else if (item.Header.ToString() == "followers")
            {
                DisplayFollowers();
            }
            else if (item.Header.ToString() == "repos")
            {
                DisplayRepos();
            }
        }

        async private void DisplayRepos()
        {
            GitHubManager manager = GitHubManager.Instance;
            string response =  await manager.GetAuthenticatedUserRepos();
            if (response == null)
                return;
            List<Repo> repoList = JsonConvert.DeserializeObject<List<Repo>>(response);
            if (repoList == null)
                return;
            // show it in the list
            repoLongListSelector.ItemsSource = repoList;
        }

        async private void DisplayFollowing()
        {
            GitHubManager manager = GitHubManager.Instance;
            string response = await manager.GetAuthenticatedUserFollowing();
            if (response == null)
                return;
            List<User> followingList = JsonConvert.DeserializeObject<List<User>>(response);
            if (followingList == null)
                return;
            // show it in the list
            followingLongListSelector.ItemsSource = followingList;
        }

        async private void DisplayFollowers()
        {
            GitHubManager manager = GitHubManager.Instance;
            string response = await manager.GetAuthenticatedUserFollowers();
            if (response == null)
                return;
            List<User> followersList = JsonConvert.DeserializeObject<List<User>>(response);
            if (followersList == null)
                return;
            // show it in the list
            followingLongListSelector.ItemsSource = followersList;
        }

        private void followersFollowing_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LongListSelector selector = sender as LongListSelector;
            if (selector == null)
                return;
            User selectedUser = selector.SelectedItem as User;
            if (selectedUser == null)
                return;
            string uri = "/UserPage.xaml?name=" + selectedUser.login;
            NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }

        private void repoLongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LongListSelector selector = sender as LongListSelector;
            if (selector == null)
                return;
            Repo repo = selector.SelectedItem as Repo;
            if (repo == null)
                return;
            string uri = "/RepoPage.xaml?reponame=" + repo.name + "&ownername=" + repo.owner.login;
            NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }
    }
}