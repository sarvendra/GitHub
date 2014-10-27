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
using System.Windows.Media.Imaging;

namespace GitHub
{
    public partial class RepoPage : PhoneApplicationPage
    {
        private string _owner = null;
        private string _reponame = null;
        private string _selectedbranchname = null;
        private bool _isListPickerSelected = false;

        private Repo _repo;

        public RepoPage()
        {
            InitializeComponent();
            Loaded += RepoPage_Loaded;
            this.repoDetailsUserControl.branchListPickerSelectionChangedEvent += 
                new EventHandler(branchListPicker_SelectionChanged);
            this.repoDetailsUserControl.branchListPickerMouseEnterEvent +=
                new EventHandler(branchListPicker_MouseEnter);
            this.repoDetailsUserControl.OwnerButtonClickEvent +=
                new EventHandler(ownerButton_Click);
            this.repoDetailsUserControl.DownloadButtonClickEvent +=
                new EventHandler(downloadButton_Click);
        }

        async void RepoPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (_isListPickerSelected)
            {
                _isListPickerSelected = false;
                return;
            }

            GitHubManager manager = GitHubManager.Instance;
            string response = await manager.GetRepo(_owner, _reponame);
            if (response == null)
                return;
            _repo = JsonConvert.DeserializeObject<Repo>(response);

            if (_repo == null)
                return;

            DataContext = _repo;

            // display owner's name and image
            this.repoDetailsUserControl.OwnerTextBlock.Text = _repo.owner.login;
            this.repoDetailsUserControl.OwnerImage.Height = this.repoDetailsUserControl.OwnerGrid.Height;
            this.repoDetailsUserControl.OwnerImage.Width = 50;
            this.repoDetailsUserControl.OwnerImage.Source = new BitmapImage(new Uri(_repo.owner.avatar_url, UriKind.Absolute));

            // get branch list
            response = await manager.GetListofBranches(_owner, _reponame);
            if (response == null)
                return;
            List<Branch> branches = JsonConvert.DeserializeObject<List<Branch>>(response);
            if (branches == null)
                return;
            this.repoDetailsUserControl.branchListPicker.ItemsSource = branches;
            _selectedbranchname = branches[0].name;
        }

        void branchListPicker_SelectionChanged(object sender, EventArgs e)
        {
            ListPicker listPicker = sender as ListPicker;
            if (listPicker == null)
                return;
            Branch branch = listPicker.SelectedItem as Branch;
            if (branch == null)
                return;
            _selectedbranchname = branch.name;
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
            GitHubManager manager = GitHubManager.Instance;
            string collaboratorsUri = _repo.url + "/collaborators";
            string response = await manager.getStringAsync(collaboratorsUri);
            if (response == null)
                return;
            List<User> collaborators = JsonConvert.DeserializeObject<List<User>>(response);
            if (collaborators == null)
                return;
            // show it in the list
            this.collaboratorsUserGridUserControl.userLongListSelector.ItemsSource = collaborators;
        }

        async private void DisplayCommits()
        {
            GitHubManager manager = GitHubManager.Instance;
            string commitsUri = _repo.url + "/commits?";
            // Get current date and time
            string currDateTime = DateTime.Now.ToString("yyyy-MM-ddThh:mm:ssZ");
            string branch = "&sha=" + _selectedbranchname;
            currDateTime = string.Format("until={0}", currDateTime);
            commitsUri += currDateTime + branch;
            string response =  await manager.getStringAsync(commitsUri);
            if (response == null)
                return;
            List<CommitRoot> commitRoot = JsonConvert.DeserializeObject<List<CommitRoot>>(response);
            if (commitRoot == null)
                return;
            // show it in the list
            this.commitUserControl.commitLongListSelector.ItemsSource = commitRoot;
        }
    
        private async void DisplayTree()
        {
            GitHubManager manager = GitHubManager.Instance;
            string response = await manager.GetBranchContent(_repo.owner.login, _repo.name, _selectedbranchname);
            if (response == null)
                return;
            List<BranchContent> branchContents = JsonConvert.DeserializeObject<List<BranchContent>>(response);
            if (branchContents == null)
                return;
            List<BranchContent> SortedList = branchContents.OrderBy(o => o.type).ToList();
            this.TreeUserControl.treeLongListSelector.ItemsSource = SortedList;
        }

        private void ownerButton_Click(object sender, EventArgs e)
        {
            string uri = PageLocator.USER_PAGE+"?name=" + _repo.owner.login;
            this.NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }

        private void downloadButton_Click(object sender, EventArgs e)
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
            string uri = PageLocator.USER_PAGE+"?name=" + user.login;
            NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }
    }
}