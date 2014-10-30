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
    public class FollowersFollowingViewModel:ViewModelBase
    {
        private GitHubManager manager = null;
        private JSONDeserializer jsonDeserializer = null;

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
                    // DO your logic
                    string navigationUri = PageLocator.USER_PAGE + "?name=" + selectedUser.login;
                    (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri(navigationUri, UriKind.Relative));
                    selectedUser = null;
                }
                RaisePropertyChanged("SelectedUser");
            }
        }

        public FollowersFollowingViewModel()
        {
            userItems = new ObservableCollection<User>();
        }

        public async Task GetUsersAsync(string url)
        {
            if (url.Contains('{'))
            {
                url = url.Split('{')[0];
            }
            manager = GitHubManager.Instance;
            string response = await manager.GetAsyncStringResponse(url);
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
