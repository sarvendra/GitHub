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
    public partial class RepoPage : PhoneApplicationPage
    {
        private RepoViewModel repoViewModel = new RepoViewModel();
        public RepoViewModel RepoViewModel
        {
            get { return this.repoViewModel; }
        }

        private string _owner = null;
        private string _reponame = null;
        private bool _isListPickerSelected = false;

        public RepoPage()
        {
            InitializeComponent();
            Loaded += RepoPage_Loaded;
            this.repoDetailsUserControl.branchListPickerMouseEnterEvent +=
                new EventHandler(branchListPicker_MouseEnter);
            this.repoDetailsUserControl.OwnerButtonClickEvent +=
                new EventHandler(ownerButton_Click);

            BuildLocalizedApplicationBar();
        }

        async void RepoPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (_isListPickerSelected)
            {
                _isListPickerSelected = false;
                return;
            }
            try
            {
                SystemTray.ProgressIndicator = new ProgressIndicator();
                SystemTray.ProgressIndicator.Text = "loading...";
                setProgressIndicator(true);
                await repoViewModel.GetRepoDetails(_owner, _reponame);
                setProgressIndicator(false);

                ApplicationBarMenuItem logoutMenuItem = (ApplicationBarMenuItem)ApplicationBar.MenuItems[1];
                if (!repoViewModel.IsLoggedIn())
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _owner = NavigationContext.QueryString["ownername"];
            _reponame = NavigationContext.QueryString["reponame"];
        }

        private void branchListPicker_MouseEnter(object sender, EventArgs e)
        {
            _isListPickerSelected = true;
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Pivot pivot = sender as Pivot;
            if (pivot == null)
                return;
            PivotItem item = pivot.SelectedItem as PivotItem;
            if (item == null)
                return;
            if (item.Header.ToString() == "tree")
            {
                DisplayTree();
            }
            else if(item.Header.ToString() == "commits")
            {
                DisplayCommits();
            }
            else if (item.Header.ToString() == "collaborators")
            {
                DisplayCollaborators();
            }
        }

        async private void DisplayCollaborators()
        {
            try
            {
                SystemTray.ProgressIndicator = new ProgressIndicator();
                SystemTray.ProgressIndicator.Text = "loading...";
                setProgressIndicator(true);
                await repoViewModel.GetCollaborators();
                setProgressIndicator(false);
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

        async private void DisplayCommits()
        {
            try
            {
                SystemTray.ProgressIndicator = new ProgressIndicator();
                SystemTray.ProgressIndicator.Text = "loading...";
                setProgressIndicator(true);
                await repoViewModel.GetCommits();
                setProgressIndicator(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    
        private async void DisplayTree()
        {
            try
            {
                SystemTray.ProgressIndicator = new ProgressIndicator();
                SystemTray.ProgressIndicator.Text = "loading...";
                setProgressIndicator(true);
                await this.repoViewModel.GetBranchContents();
                setProgressIndicator(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ownerButton_Click(object sender, EventArgs e)
        {
            string uri = PageLocator.USER_PAGE+"?name=" + repoViewModel.Login;
            this.NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }
    }
}