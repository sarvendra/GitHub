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
    public partial class FollowersFollowingPage : PhoneApplicationPage
    {
        private string type = null;
        private string url = null;
        public FollowersFollowingPage()
        {
            InitializeComponent();
            Loaded += FollowersFollowingPage_Loaded;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            type = NavigationContext.QueryString["type"];
            FollowersFollowingTextBlock.Text = type;
            url = NavigationContext.QueryString["url"];
        }

        async void FollowersFollowingPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (url.Contains('{'))
            {
                url = url.Split('{')[0];
            }
            GitHubManager manager = GitHubManager.Instance;
            string response = await manager.GetAsyncStringResponse(url);
            if (response == null)
                return;
            List<User> userlist = JsonConvert.DeserializeObject<List<User>>(response);
            if (userlist == null)
                return;
            longListSelector.ItemsSource = userlist;
        }

        private void longListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LongListSelector selector = sender as LongListSelector;
            if (selector == null)
                return;
            User user = selector.SelectedItem as User;
            if (user == null)
                return;
            string navigationUri = "/UserPage.xaml?name=" + user.login;
            NavigationService.Navigate(new Uri(navigationUri, UriKind.Relative));
        }
    }
}