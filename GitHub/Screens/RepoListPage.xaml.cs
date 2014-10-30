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

namespace GitHub
{
    public partial class RepoList : PhoneApplicationPage
    {
        private RepoListViewModel repoListViewModel = new RepoListViewModel();
        public RepoListViewModel RepoListViewModel
        {
            get { return this.repoListViewModel; }
        }

        private string repoUrl = null;

        public RepoList()
        {
            InitializeComponent();
            Loaded += RepoList_Loaded;
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

        async void RepoList_Loaded(object sender, RoutedEventArgs e)
        {
            SystemTray.ProgressIndicator = new ProgressIndicator();
            SystemTray.ProgressIndicator.Text = "loading";
            setProgressIndicator(true);
            await this.repoListViewModel.GetRepos(repoUrl);
            setProgressIndicator(false);
        }

        private void setProgressIndicator(bool isVisible)
        {
            SystemTray.ProgressIndicator.IsIndeterminate = isVisible;
            SystemTray.ProgressIndicator.IsVisible = isVisible;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            repoUrl = NavigationContext.QueryString["repo"];
        }
    }
}