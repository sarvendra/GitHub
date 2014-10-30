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
    public class RepoListViewModel:ViewModelBase
    {
        private GitHubManager manager = null;
        private JSONDeserializer jsonDeserializer = null;

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

        public RepoListViewModel()
        {
            repoItems = new ObservableCollection<Repo>();
        }

        public async Task GetRepos(string repoUrl)
        {
            manager = GitHubManager.Instance;
            string response = await manager.getStringAsync(repoUrl);
            if (response != null)
            {
                if (jsonDeserializer == null)
                {
                    jsonDeserializer = new JSONDeserializer();
                }
                List<Repo> repoList = jsonDeserializer.DeserializeRepoList(response);
                if (repoList == null)
                    return;
                repoItems.Clear();
                foreach (var repoItem in repoList)
                {
                    repoItems.Add(repoItem);
                }
            }
        }

        public bool IsLoggedIn()
        {
            manager = GitHubManager.Instance;
            return manager.IsLoggedIn();
        }
    }
}
