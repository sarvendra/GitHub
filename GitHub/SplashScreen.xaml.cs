using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Threading;

namespace GitHub
{
    public partial class Page1 : PhoneApplicationPage
    {
        public Page1()
        {
            InitializeComponent();
        }

        private void SplashScreenPage_Loaded(object sender, RoutedEventArgs e)
        {
            Thread.Sleep(2000);

            GitHubManager manager = GitHubManager.Instance;
            if (manager.IsLoggedIn())
            {
                this.NavigationService.Navigate(new Uri("/ProfilePage.xaml", UriKind.Relative));

                // enable logout menu item
                //((ApplicationBarMenuItem)ApplicationBar.MenuItems[0]).IsEnabled = true;
            }
            else
            {
                this.NavigationService.Navigate(new Uri("/StartPage.xaml", UriKind.Relative));

                // disable logout menu item
                //((ApplicationBarMenuItem)(ApplicationBar as ApplicationBar).MenuItems[1]).IsEnabled = false;
            }
        }
    }
}