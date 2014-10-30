using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using GitHub.Model;

namespace GitHub.Interfaces
{
    public interface IDeserializer
    {
        Users DeserializeUsers(string users);
        Repos DeserializeRepos(string repos);
        List<BranchContent> DeserializeBranchContents(string branchContents);
        List<User> DeserializeUserList(string userList);
        List<Repo> DeserializeRepoList(string repoList);
        User DeserializeUser(string user);
        Repo DeserializeRepo(string repo);
        List<Branch> DeserializeBranchList(string branchList);
        List<CommitRoot> DeserializeCommitList(string commitList);
    }
}
