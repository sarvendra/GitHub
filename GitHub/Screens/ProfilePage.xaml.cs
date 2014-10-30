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
using System.ComponentModel;
using Newtonsoft.Json;

namespace GitHub
{
    public partial class ProfilePage : PhoneApplicationPage
    {
        private ProfileViewModel profileViewModel = new ProfileViewModel();
        public ProfileViewModel ProfileViewModel
        {
            get { return this.profileViewModel; }
        }

        public ProfilePage()
        {
            InitializeComponent();
            Loaded += ProfilePage_Loaded;

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
            // perform logout operation
            GitHubManager manager = GitHubManager.Instance;
            manager.Logout();
            NavigationService.Navigate(new Uri(PageLocator.START_PAGE, UriKind.RelativeOrAbsolute));
        }

        async void ProfilePage_Loaded(object sender, RoutedEventArgs e)
        {
            SystemTray.ProgressIndicator = new ProgressIndicator();
            SystemTray.ProgressIndicator.Text = "loading";
            setProgressIndicator(true);
            await this.profileViewModel.GetAuthenticatedUserProfile();
            setProgressIndicator(false);
        }

        private void setProgressIndicator(bool isVisible)
        {
            SystemTray.ProgressIndicator.IsIndeterminate = isVisible;
            SystemTray.ProgressIndicator.IsVisible = isVisible;
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
            SystemTray.ProgressIndicator = new ProgressIndicator();
            SystemTray.ProgressIndicator.Text = "loading";
            setProgressIndicator(true);
            await this.profileViewModel.GetRepos();
            setProgressIndicator(false);
        }

        async private void DisplayFollowing()
        {
            SystemTray.ProgressIndicator = new ProgressIndicator();
            SystemTray.ProgressIndicator.Text = "loading";
            setProgressIndicator(true);
            await this.profileViewModel.GetFollowing();
            setProgressIndicator(false);
        }

        async private void DisplayFollowers()
        {
            SystemTray.ProgressIndicator = new ProgressIndicator();
            SystemTray.ProgressIndicator.Text = "loading";
            setProgressIndicator(true);
            await this.profileViewModel.GetFollowers();
            setProgressIndicator(false);
        }
    }
}