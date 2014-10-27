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

            this.profileUserControl.SetUserInfo(user);
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
            this.repoListUserControl.repoLongListSelector.ItemsSource = repoList;
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
            this.followersGridUserControl.userLongListSelector.ItemsSource = followingList;
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
            this.followersGridUserControl.userLongListSelector.ItemsSource = followersList;
        }

        private void repoLongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LongListSelector selector = sender as LongListSelector;
            if (selector == null)
                return;
            Repo repo = selector.SelectedItem as Repo;
            if (repo == null)
                return;
            string uri = PageLocator.REPO_PAGE+"?reponame=" + repo.name + "&ownername=" + repo.owner.login;
            NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }
    }
}