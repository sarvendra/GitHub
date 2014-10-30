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
using System.Windows.Input;
using Newtonsoft.Json;

namespace GitHub
{
    public partial class SearchPage : PhoneApplicationPage
    {
        private SearchViewModel searchViewModel = new SearchViewModel();
        public SearchViewModel SearchViewModel
        {
            get { return this.searchViewModel; }
        }

        public SearchPage()
        {
            InitializeComponent();
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

        private async void RepoSearchTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                try
                {
                    SystemTray.ProgressIndicator = new ProgressIndicator();
                    SystemTray.ProgressIndicator.Text = "loading";
                    setProgressIndicator(true);

                    // call repo search api
                    string repo = RepoSearchTextBox.Text;
                    await this.searchViewModel.GetRepos(repo);
                    setProgressIndicator(false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);                    
                }
            }
        }

        private void setProgressIndicator(bool isVisible)
        {
            SystemTray.ProgressIndicator.IsIndeterminate = isVisible;
            SystemTray.ProgressIndicator.IsVisible = isVisible;
        }

        private async void UserSearchTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                try
                {
                    SystemTray.ProgressIndicator = new ProgressIndicator();
                    SystemTray.ProgressIndicator.Text = "loading";
                    setProgressIndicator(true);
                    // call user search api
                    string user = UserSearchTextBox.Text;
                    await this.searchViewModel.GetUsers(user);
                    setProgressIndicator(false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}