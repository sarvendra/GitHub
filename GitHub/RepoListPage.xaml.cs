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

namespace GitHub
{
    public partial class RepoList : PhoneApplicationPage
    {
        private string repoUrl = null;

        public RepoList()
        {
            InitializeComponent();
            Loaded += RepoList_Loaded;
        }

        async void RepoList_Loaded(object sender, RoutedEventArgs e)
        {
            GitHubManager manager = GitHubManager.Instance;
            string response = await manager.getStringAsync(repoUrl);
            if (response == null)
                return;
            List<Repo> repoList = JsonConvert.DeserializeObject<List<Repo>>(response);
            if (repoList == null)
                return;
            repoLongListSelector.ItemsSource = repoList;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            repoUrl = NavigationContext.QueryString["repo"];
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