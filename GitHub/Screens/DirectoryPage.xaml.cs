using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using GitHub.Model;
using GitHub.Utility;
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
            this.treeUserControl.treeLongListSelector.ItemsSource = SortedList;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            branchUri = NavigationContext.QueryString["uri"];
        }
    }
}