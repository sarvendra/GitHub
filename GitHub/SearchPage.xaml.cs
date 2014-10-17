using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Input;
using Newtonsoft.Json;

namespace GitHub
{
    public partial class SearchPage : PhoneApplicationPage
    {
        public SearchPage()
        {
            InitializeComponent();
        }

        private async void RepoSearchTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // call repo search api
                string repo = RepoSearchTextBox.Text;
                GitHubManager manager = GitHubManager.Instance;
                string response = await manager.Search("repositories",repo);
                if (response != null)
                {
                    Repos repos = JsonConvert.DeserializeObject<Repos>(response);
                    RepoList.ItemsSource = repos.Items;
                }
            }
        }

        private async void UserSearchTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // call user search api
                string user = UserSearchTextBox.Text;
                GitHubManager manager = GitHubManager.Instance;
                string response = await manager.Search("users", user);
                if (response != null)
                {
                    Users users = JsonConvert.DeserializeObject<Users>(response);
                    UserList.ItemsSource = users.Items;
                }
            }
        }

        private void UserList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LongListSelector selector = sender as LongListSelector;
            if (selector == null)
                return;
            User user = selector.SelectedItem as User; 
            if (user == null)
                return;
            string uri = "/UserPage.xaml?name=" + user.login;
            NavigationService.Navigate(new Uri(uri, UriKind.Relative));

            // clear list and text box
            UserSearchTextBox.Text = string.Empty;
            UserList.ItemsSource = null;
        }

        private void RepoList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LongListSelector selector = sender as LongListSelector;
            if (selector == null)
                return;
            Repo repo = selector.SelectedItem as Repo;
            if (repo == null)
                return;
            string uri = "/RepoPage.xaml?reponame=" + repo.name + "&ownername=" + repo.owner.login;
            NavigationService.Navigate(new Uri(uri, UriKind.Relative));

            // clear list and text box
            RepoSearchTextBox.Text = string.Empty;
            RepoList.ItemsSource = null;
        }
    }
}