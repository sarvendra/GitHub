using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GitHub.Deserializer;
using GitHub.Model;

namespace GitHub.ViewModels
{
    public class UserViewModel:ViewModelBase
    {
        private GitHubManager manager = null;
        private JSONDeserializer jsonDeserializer = null;

        public string NoOfFollowing = "";
        public string NoOfFollowers = "";
        public string NoOfRepos = "";
        public string Following_URL = "";
        public string Followers_URL = "";
        public string Repos_URL = "";

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

        public async Task GetUserProfile(string loginName)
        {
            manager = GitHubManager.Instance;
            string response = await manager.GetUserProfile(loginName);
            if (response == null)
                return;
            if (jsonDeserializer == null)
            {
                jsonDeserializer = new JSONDeserializer();
            }
            User user = jsonDeserializer.DeserializeUser(response);
            if (user == null)
                return;

            NoOfFollowing = user.following.ToString();
            NoOfFollowers = user.followers.ToString();
            NoOfRepos = user.public_repos.ToString();
            Following_URL = user.following_url;
            Followers_URL = user.followers_url;
            Repos_URL = user.repos_url;
            Name = user.name;
            Login = user.login;
            AvatarURL = user.avatar_url;
            Company = user.company;
            Location = user.location;
            Email = user.email;
            Date = user.created_at;
        }
    }
}
