using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using GitHub.Deserializer;
using GitHub.Model;
using GitHub.Utility;
using Microsoft.Phone.Controls;
using Newtonsoft.Json;

namespace GitHub.ViewModels
{
    public class SearchViewModel:ViewModelBase
    {
        private GitHubManager manager = null;
        private JSONDeserializer jsonDeserializer = null;

        public string loginName = "";

        private ObservableCollection<Repo> repoItems;

        public ObservableCollection<Repo> RepoItems
        {
            get
            {
                return repoItems;
            }

            set
            {
                repoItems = value;
                RaisePropertyChanged("RepoItems");
            }
        }

        private Repo selectedRepo;
        public Repo SelectedRepo
        {
            get
            {
                return selectedRepo;
            }

            set
            {
                selectedRepo = value;
                if (selectedRepo != null)
                {
                    // Do your logic
                    string uri = PageLocator.REPO_PAGE + "?reponame=" + selectedRepo.name + "&ownername=" + selectedRepo.owner.login;
                    (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri(uri, UriKind.Relative));
                    selectedRepo = null;
                }
                RaisePropertyChanged("SelectedRepo");
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
                    string uri = PageLocator.USER_PAGE + "?name=" + selectedUser.login;
                    (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri(uri, UriKind.Relative));
                    selectedUser = null;
                }
                RaisePropertyChanged("SelectedUser");
            }
        }

        public SearchViewModel()
        {
            repoItems = new ObservableCollection<Repo>();
            userItems = new ObservableCollection<User>();
        }

        public async Task GetUsers(string user)
        {
            manager = GitHubManager.Instance;
            string response = await manager.Search("users", user);
            if (response != null)
            {
                if (jsonDeserializer == null)
                {
                    jsonDeserializer = new JSONDeserializer();                    
                }
                Users users = jsonDeserializer.DeserializeUsers(response);
                userItems.Clear();
                foreach (var userItem in users.Items)
                {
                    userItems.Add(userItem);
                }
            }
        }

        public async Task GetRepos(string repo)
        {
            manager = GitHubManager.Instance;
            string response = await manager.Search("repositories", repo);
            if (response != null)
            {
                if (jsonDeserializer == null)
                {
                    jsonDeserializer = new JSONDeserializer();
                }

                Repos repos = jsonDeserializer.DeserializeRepos(response);
                repoItems.Clear();
                foreach (var repoItem in repos.Items)
                {
                    repoItems.Add(repoItem);
                }
            }
        }
    }
}
