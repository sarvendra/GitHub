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
    public class ProfileViewModel:ViewModelBase
    {
        private GitHubManager manager = null;
        private JSONDeserializer jsonDeserializer = null;

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

        private string name;

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                RaisePropertyChanged("Name");
            }
        }

        private string company;

        public string Company
        {
            get
            {
                return company;
            }
            set
            {
                company = value;
                RaisePropertyChanged("Company");
            }
        }

        private string location;

        public string Location
        {
            get
            {
                return location;
            }
            set
            {
                location = value;
                RaisePropertyChanged("Location");
            }
        }

        private string email;

        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
                RaisePropertyChanged("Email");
            }
        }

        private string date;

        public string Date
        {
            get
            {
                return date;
            }
            set
            {
                date = value;
                RaisePropertyChanged("Date");
            }
        }


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

        public ProfileViewModel()
        {
            repoItems = new ObservableCollection<Repo>();
            userItems = new ObservableCollection<User>();
        }

        public async Task GetAuthenticatedUserProfile()
        {
            manager = GitHubManager.Instance;
            string response = await manager.GetAuthenticatedUserProfile();
            if (response == null)
            {
                return;
            }
            if (jsonDeserializer == null)
            {
                jsonDeserializer = new JSONDeserializer();
            }
            User user = jsonDeserializer.DeserializeUser(response);
            Name = user.name;
            Login = user.login;
            AvatarURL = user.avatar_url;
            Company = user.company;
            Location = user.location;
            Email = user.email;
            Date = user.created_at;
        }

        public async Task GetRepos()
        {
            manager = GitHubManager.Instance;
            string response =  await manager.GetAuthenticatedUserRepos();
            if (response != null)
            {
                if (jsonDeserializer == null)
                {
                    jsonDeserializer = new JSONDeserializer();
                }

                List<Repo> repos = jsonDeserializer.DeserializeRepoList(response);
                foreach (var repo in repos)
                {
                    repoItems.Add(repo);
                }
            }
        }

        public async Task GetFollowing()
        {
            manager = GitHubManager.Instance;
            string response = await manager.GetAuthenticatedUserFollowing();
            if (response != null)
            {
                if (jsonDeserializer == null)
                {
                    jsonDeserializer = new JSONDeserializer();
                }

                List<User> users = jsonDeserializer.DeserializeUserList(response);
                userItems.Clear();
                foreach (var user in users)
                {
                    userItems.Add(user);
                }
            }
        }

        public async Task GetFollowers()
        {
            manager = GitHubManager.Instance;
            string response = await manager.GetAuthenticatedUserFollowers();
            if (response != null)
            {
                if (jsonDeserializer == null)
                {
                    jsonDeserializer = new JSONDeserializer();
                }

                List<User> users = jsonDeserializer.DeserializeUserList(response);
                userItems.Clear();
                foreach (var user in users)
                {
                    userItems.Add(user);
                }
            }
        }
    }
}
