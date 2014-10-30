﻿using System;
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
    public partial class FollowersFollowingPage : PhoneApplicationPage
    {
        private string url = null;

        private FollowersFollowingViewModel followersFollowingViewModel = new FollowersFollowingViewModel();
        public FollowersFollowingViewModel FollowersFollowingViewModel
        {
            get { return this.followersFollowingViewModel; }
        }

        public FollowersFollowingPage()
        {
            InitializeComponent();
            Loaded += FollowersFollowingPage_Loaded;
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            FollowersFollowingTextBlock.Text = NavigationContext.QueryString["type"];
            url = NavigationContext.QueryString["url"];
        }

        async void FollowersFollowingPage_Loaded(object sender, RoutedEventArgs e)
        {
            SystemTray.ProgressIndicator = new ProgressIndicator();
            SystemTray.ProgressIndicator.Text = "loading";
            setProgressIndicator(true);
            await this.followersFollowingViewModel.GetUsersAsync(url);
            setProgressIndicator(false);
        }

        private void setProgressIndicator(bool isVisible)
        {
            SystemTray.ProgressIndicator.IsIndeterminate = isVisible;
            SystemTray.ProgressIndicator.IsVisible = isVisible;
        }
    }
}