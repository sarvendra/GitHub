using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using GitHub.Utility;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.ComponentModel;

namespace GitHub
{
    public partial class StartPage : PhoneApplicationPage
    {
        public StartPage()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string uri = PageLocator.LOGIN_PAGE;
            NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string uri = PageLocator.SEARCH_PAGE;
            NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            Application.Current.Terminate();
        }
    }
}