using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GitHub.Deserializer;
using GitHub.Model;
using GitHub.Utility;
using Microsoft.Phone.Controls;

namespace GitHub.ViewModels
{
    public class RepoViewModel:ViewModelBase
    {
        private GitHubManager manager = null;
        private JSONDeserializer jsonDeserializer = null;

        private string repoURL;

        public string RepoURL
        {
            get
            {
                return this.repoURL; 
            }
            set
            {
                repoURL = value;
            }
        }

        private string repoName;

        public string RepoName
        {
            get
            {
                return this.repoName;
            }
            set
            {
                repoName = value;
            }
        }


        private string login;

        public string Login
        {
            get
            {
                return login;
            }
            set
            {
                login = value;
                RaisePropertyChanged("Login");
            }
        }

        private string avatarURL;

        public string AvatarURL
        {
            get
            {
                return avatarURL;
            }
            set
            {
                avatarURL = value;
                RaisePropertyChanged("AvatarURL");
            }
        }

        private ObservableCollection<string> branchItems;

        public ObservableCollection<string> BranchItems
        {
            get
            {
                return branchItems;
            }

            set
            {
                branchItems = value;
                RaisePropertyChanged("BranchItems");
            }
        }

        private string selectedBranch;

        public string SelectedBranch
        {
            get
            {
                return selectedBranch;
            }

            set
            {
                if (selectedBranch != null)
                    selectedBranch = value;
                RaisePropertyChanged("SelectedBranch");
            }
        }

        private int watchers = 0;

        public int Watchers
        {
            get
            {
                return watchers;
            }

            set
            {
                watchers = value;
                RaisePropertyChanged("Watchers");
            }
        }

        private int forks = 0;

        public int Forks
        {
            get
            {
                return forks;
            }

            set
            {
                forks = value;
                RaisePropertyChanged("Forks");
            }
        }

        private int issues = 0;

        public int Issues
        {
            get
            {
                return issues;
            }

            set
            {
                issues = value;
                RaisePropertyChanged("Issues");
            }
        }

        private ObservableCollection<User> userItems;

        public ObservableCollection<User> UserItems
        {
            get
            {
                return userItems;
            }

            set
            {
                userItems = value;
                RaisePropertyChanged("UserItems");
            }
        }

        private User selectedUser;

        public User SelectedUser
        {
            get
            {
                return selectedUser;
            }

            set
            {
                selectedUser = value;
                if (selectedUser != null)
                {
                    // Do your logic
                    string uri = PageLocator.USER_PAGE + "?name=" + selectedUser.login;
                    (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri(uri, UriKind.Relative));
                    selectedUser = null;
                }
                RaisePropertyChanged("SelectedUser");
            }
        }

        private ObservableCollection<BranchContent> branchContentItems;

        public ObservableCollection<BranchContent> BranchContentItems
        {
            get
            {
                return branchContentItems;
            }

            set
            {
                branchContentItems = value;
                RaisePropertyChanged("BranchContentItems");
            }
        }

        private BranchContent selectedBranchContent;

        public BranchContent SelectedBranchContent
        {
            get
            {
                return selectedBranchContent;
            }

            set
            {
                selectedBranchContent = value;
                if (selectedBranchContent != null)
                {
                    // Do your logic
                    string uri = selectedBranchContent.url;
                    // navigate to direcotry explorer page
                    (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri(PageLocator.DIRECTORY_PAGE + "?uri=" + uri, UriKind.Relative));
                    selectedBranchContent = null;
                }
                RaisePropertyChanged("SelectedBranchContent");
            }
        }

        private ObservableCollection<CommitRoot> commitItems;

        public ObservableCollection<CommitRoot> CommitItems
        {
            get
            {
                return commitItems;
            }

            set
            {
                commitItems = value;
                RaisePropertyChanged("CommitItems");
            }
        }

        public RepoViewModel()
        {
            userItems = new ObservableCollection<User>();
            branchContentItems = new ObservableCollection<BranchContent>();
            commitItems = new ObservableCollection<CommitRoot>();
            branchItems = new ObservableCollection<string>();
        }

        public async Task GetCollaborators()
        {
            manager = GitHubManager.Instance;
            string collaboratorsUri = repoURL + "/collaborators";
            string response = await manager.getStringAsync(collaboratorsUri);
            if (response != null)
            {
                if (jsonDeserializer == null)
                {
                    jsonDeserializer = new JSONDeserializer();                    
                }
                List<User> users = jsonDeserializer.DeserializeUserList(response);
                userItems.Clear();
                foreach (var userItem in users)
                {
                    userItems.Add(userItem);
                }
            }
        }

        public async Task GetBranchContents()
        {
            manager = GitHubManager.Instance;
            string response = await manager.GetBranchContent(login, repoName, selectedBranch);
            if (response != null)
            {
                if (jsonDeserializer == null)
                {
                    jsonDeserializer = new JSONDeserializer();
                }
                List<BranchContent> branches = jsonDeserializer.DeserializeBranchContents(response);
                if (branches == null)
                    return;
                List<BranchContent> SortedList = branches.OrderBy(o => o.type).ToList();
                branchContentItems.Clear();
                foreach (var branch in SortedList)
                {
                    branchContentItems.Add(branch);
                }
            }
        }

        public async Task GetRepoDetails(string ownername, string reponame)
        {
            manager = GitHubManager.Instance;
            string response = await manager.GetRepo(ownername, reponame);
            if (response != null)
            {
                if (jsonDeserializer == null)
                {
                    jsonDeserializer = new JSONDeserializer();
                }
                Repo repo = jsonDeserializer.DeserializeRepo(response);
                if (repo == null)
                    return;

                Login = repo.owner.login;
                RepoName = reponame;
                AvatarURL = repo.owner.avatar_url;
                RepoURL = repo.url;
                Watchers = repo.watchers;
                Forks = repo.forks;
                Issues = repo.open_issues;
                await GetBranchList();
            }
        }

        public async Task GetBranchList()
        {
            manager = GitHubManager.Instance;
            string response = await manager.GetListofBranches(login, repoName);
            if (response == null)
                return;
            if (jsonDeserializer == null)
            {
                jsonDeserializer = new JSONDeserializer();
            }
            var branchList = jsonDeserializer.DeserializeBranchList(response);
            branchItems.Clear();
            foreach (var branch in branchList)
            {
                branchItems.Add(branch.name);
            }
            if (branchItems.Count > 0)
            {
                selectedBranch = branchItems[0];   
            }
        }

        public async Task GetCommits()
        {
            manager = GitHubManager.Instance;
            string commitsUri = repoURL + "/commits?";
            // Get current date and time
            string currDateTime = DateTime.Now.ToString("yyyy-MM-ddThh:mm:ssZ");
            string branch = "&sha=" + selectedBranch;
            currDateTime = string.Format("until={0}", currDateTime);
            commitsUri += currDateTime + branch;
            string response = await manager.getStringAsync(commitsUri);
            if (response == null)
                return;
            if (jsonDeserializer == null)
            {
                jsonDeserializer = new JSONDeserializer();
            }
            var commits = jsonDeserializer.DeserializeCommitList(response);
            commitItems.Clear();
            foreach (var commit in commits)
            {
                commitItems.Add(commit);
            }
        }

        public bool IsLoggedIn()
        {
            manager = GitHubManager.Instance;
            return manager.IsLoggedIn();
        }
    }
}
