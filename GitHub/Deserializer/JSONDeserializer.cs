using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GitHub.Interfaces;
using GitHub.Model;
using Newtonsoft.Json;

namespace GitHub.Deserializer
{
    public class JSONDeserializer:IDeserializer
    {
        public Users DeserializeUsers(string usersText)
        {
            Users users = JsonConvert.DeserializeObject<Users>(usersText);
            return users;
        }

        public Repos DeserializeRepos(string reposText)
        {
            Repos repos = JsonConvert.DeserializeObject<Repos>(reposText);
            return repos;
        }

        public List<BranchContent> DeserializeBranchContents(string branchContentsText)
        {
            List<BranchContent> branchContents = JsonConvert.DeserializeObject<List<BranchContent>>(branchContentsText);
            return branchContents;
        }

        public List<User> DeserializeUserList(string userList)
        {
            List<User> users = JsonConvert.DeserializeObject<List<User>>(userList);
            return users;
        }

        public List<Repo> DeserializeRepoList(string repoList)
        {
            List<Repo> repos = JsonConvert.DeserializeObject<List<Repo>>(repoList);
            return repos;
        }

        public User DeserializeUser(string userText)
        {
            User user = JsonConvert.DeserializeObject<User>(userText);
            return user;
        }

        public Repo DeserializeRepo(string repoText)
        {
            Repo repo = JsonConvert.DeserializeObject<Repo>(repoText);
            return repo;
        }

        public List<Branch> DeserializeBranchList(string branchListText)
        {
            var branchList = JsonConvert.DeserializeObject<List<Branch>>(branchListText);
            return branchList;
        }

        public List<CommitRoot> DeserializeCommitList(string commitListText)
        {
            var commitList = JsonConvert.DeserializeObject<List<CommitRoot>>(commitListText);
            return commitList;
        }
    }
}
