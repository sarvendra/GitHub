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
            this.repoListUserControl.repoLongListSelector.ItemsSource = repoList;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            repoUrl = NavigationContext.QueryString["repo"];
        }
    }
}