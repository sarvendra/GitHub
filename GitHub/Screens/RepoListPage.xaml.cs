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

        async void RepoList_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                SystemTray.ProgressIndicator = new ProgressIndicator();
                SystemTray.ProgressIndicator.Text = "loading";
                setProgressIndicator(true);
                await this.repoListViewModel.GetRepos(repoUrl);
                setProgressIndicator(false);

                ApplicationBarMenuItem logoutMenuItem = (ApplicationBarMenuItem)ApplicationBar.MenuItems[1];
                if (!repoListViewModel.IsLoggedIn())
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
            repoUrl = NavigationContext.QueryString["repo"];
        }
    }
}