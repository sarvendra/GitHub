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
    public partial class DirectoryPage : PhoneApplicationPage
    {
        private string branchUri = null;
        public DirectoryPage()
        {
            InitializeComponent();
            Loaded += DirectoryPage_Loaded;
        }

        async void DirectoryPage_Loaded(object sender, RoutedEventArgs e)
        {
            GitHubManager manager = GitHubManager.Instance;
            string response = await manager.GetAsyncStringResponse(branchUri);
            if (response == null)
                return;
            List<BranchContent> branches = JsonConvert.DeserializeObject<List<BranchContent>>(response);
            if (branches == null)
                return;
            List<BranchContent> SortedList = branches.OrderBy(o => o.type).ToList();
            longListSelector.ItemsSource = SortedList;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            branchUri = NavigationContext.QueryString["uri"];
        }

        private void longListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LongListSelector selector = sender as LongListSelector;
            if (selector == null)
                return;
            BranchContent branchcontent = selector.SelectedItem as BranchContent;
            if (branchcontent == null)
                return;
            if (branchcontent.type == "dir")
            {
                string uri = branchcontent.url;
                // navigate to direcotry explorer page
                this.NavigationService.Navigate(new Uri("/DirectoryPage.xaml?uri=" + uri, UriKind.Relative));
            }

            selector.SelectedItem = null;
        }
    }
}