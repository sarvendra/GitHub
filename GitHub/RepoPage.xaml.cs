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
using System.Windows.Media.Imaging;

namespace GitHub
{
    public partial class RepoPage : PhoneApplicationPage
    {
        private string owner = null;
        private string reponame = null;
        private string selectedbranchname = null;
        private bool IsListPickerSelected = false;

        private Repo repo = null;

        public RepoPage()
        {
            InitializeComponent();
            Loaded += RepoPage_Loaded;
        }

        async void RepoPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsListPickerSelected)
            {
                IsListPickerSelected = false;
                return;
            }

            GitHubManager manager = GitHubManager.Instance;
            string response = await manager.GetRepo(owner, reponame);
            if (response == null)
                return;
            repo = JsonConvert.DeserializeObject<Repo>(response);

            if (repo == null)
                return;

            DataContext = repo;

            // display owner's name and image
            OwnerTextBlock.Text = repo.owner.login;
            OwnerImage.Height = OwnerGrid.Height;
            OwnerImage.Width = 50;
            OwnerImage.Source = new BitmapImage(new Uri(repo.owner.avatar_url, UriKind.Absolute));

            // get branch list
            response = await manager.GetListofBranches(owner, reponame);
            if (response == null)
                return;
            List<Branch> branches = JsonConvert.DeserializeObject<List<Branch>>(response);
            if (branches == null)
                return;
            branchListPicker.ItemsSource = branches;
            selectedbranchname = branches[0].name;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            owner = NavigationContext.QueryString["ownername"];
            reponame = NavigationContext.QueryString["reponame"];
        }



        private void branchListPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListPicker listPicker = sender as ListPicker;
            if (listPicker == null)
                return;
            Branch branch = listPicker.SelectedItem as Branch;
            if (branch == null)
                return;
            selectedbranchname = branch.name;
        }

        private void branchListPicker_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            IsListPickerSelected = true;
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
            GitHubManager manager = GitHubManager.Instance;
            string collaboratorsUri = repo.url + "/collaborators";
            string response = await manager.getStringAsync(collaboratorsUri);
            if (response == null)
                return;
            List<User> collaborators = JsonConvert.DeserializeObject<List<User>>(response);
            if (collaborators == null)
                return;
            // show it in the list
            collaboratorsLongListSelector.ItemsSource = collaborators;
        }

        async private void DisplayCommits()
        {
            GitHubManager manager = GitHubManager.Instance;
            string commitsUri = repo.url + "/commits?";
            // Get current date and time
            string currDateTime = DateTime.Now.ToString("yyyy-MM-ddThh:mm:ssZ");
            string branch = "&sha=" + selectedbranchname;
            currDateTime = string.Format("until={0}", currDateTime);
            commitsUri += currDateTime + branch;
            string response =  await manager.getStringAsync(commitsUri);
            if (response == null)
                return;
            List<CommitRoot> commitRoot = JsonConvert.DeserializeObject<List<CommitRoot>>(response);
            if (commitRoot == null)
                return;
            // show it in the list
            commitLongListSelector.ItemsSource = commitRoot;
        }
    
        private async void DisplayTree()
        {
            GitHubManager manager = GitHubManager.Instance;
            string response = await manager.GetBranchContent(repo.owner.login, repo.name, selectedbranchname);
            if (response == null)
                return;
            List<BranchContent> branchContents = JsonConvert.DeserializeObject<List<BranchContent>>(response);
            if (branchContents == null)
                return;
            List<BranchContent> SortedList = branchContents.OrderBy(o => o.type).ToList();
            treeLongListSelector.ItemsSource = SortedList;
        }

        private void treeLongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LongListSelector selector = sender as LongListSelector;
            if (selector == null)
                return;
            BranchContent branchcontent= selector.SelectedItem as BranchContent;
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

        private void ownerButton_Click(object sender, RoutedEventArgs e)
        {
            string uri = "/UserPage.xaml?name=" + repo.owner.login;
            this.NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }

        private void downloadButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void collaboratorsLongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LongListSelector selector = sender as LongListSelector;
            if (selector == null)
                return;
            User user = selector.SelectedItem as User;
            if (user == null)
                return;
            string uri = "/UserPage.xaml?name=" + user.login;
            NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }
    }
}