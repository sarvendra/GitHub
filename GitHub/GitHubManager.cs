using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GitHub.ClientServices;
using GitHub.Utility;

namespace GitHub
{
    public sealed class GitHubManager
    {
        private const string accessTokenFilePath = "accessTokenFile.txt";
        private HTTPClientService clientService = null;
        private string access_token = null;
        private static readonly GitHubManager instance = new GitHubManager();

        private GitHubManager() { }

        public static GitHubManager Instance
        {
            get
            {
                return instance;
            }
        }

        public void Login(string accesstoken)
        {
            this.access_token = accesstoken;
            // saving access token in file
            using (IsolatedStorageFile storageFile = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (storageFile.FileExists(accessTokenFilePath))
                {
                    storageFile.DeleteFile(accessTokenFilePath);
                }

                using (var stream = storageFile.CreateFile(accessTokenFilePath))
                {
                    if (stream == null)
                    {
                        return;
                    }
                    StreamWriter writer = new StreamWriter(stream);
                    writer.Write(access_token);
                    writer.Close();
                }
            }
        }

        public bool IsLoggedIn()
        {
            using (IsolatedStorageFile storageFile = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (storageFile.FileExists(accessTokenFilePath))
                {
                    using (var stream = new IsolatedStorageFileStream(accessTokenFilePath, System.IO.FileMode.Open, storageFile))
                    {
                        StreamReader reader = new StreamReader(stream);
                        access_token = reader.ReadToEnd();
                        reader.Close();
                        if (access_token == null)
                        {
                            return false;
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void Logout()
        {
            using (IsolatedStorageFile storageFile = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (storageFile.FileExists(accessTokenFilePath))
                {
                    storageFile.DeleteFile(accessTokenFilePath);
                    access_token = null;
                }
            }
        }

        public async Task<string> getStringAsync(string uri)
        {
            clientService = new HTTPClientService();
            string response = await clientService.GetStringAsync(uri);
            return response;
            
        }

        public async Task<string> Search(string searchType, string arg)
        {
            string searchUri = GitHubAPIPathDefines.BASE_SEARCH_URI + "?q={1}";
            searchUri = string.Format(searchUri, searchType, arg);
            return await getStringAsync(searchUri);
        }

        public async Task<string> GetUserProfile(string loginName)
        {
            string userUri = GitHubAPIPathDefines.BASE_USER_URI + loginName;
            return await getStringAsync(userUri);
        }

        public async Task<string> GetAsyncStringResponse(string uri)
        {
            return await getStringAsync(uri);
        }

        public async Task<string> GetRepo(string owner, string repo)
        {
            string repoUri = string.Format(GitHubAPIPathDefines.BASE_REPO_URI, owner, repo);
            return await getStringAsync(repoUri);
        }

        public async Task<string> GetListofBranches(string owner, string repo)
        {
            string branchesUri = string.Format(GitHubAPIPathDefines.BASE_BRANCHES_URI, owner, repo);
            return await getStringAsync(branchesUri);
        }

        public async Task<string> GetBranchContent(string owner, string repo, string branchname)
        {
            string branchUri = string.Format(GitHubAPIPathDefines.BASE_BRANCH_CONTENT_URI, owner, repo, branchname);
            return await getStringAsync(branchUri);
        }

        public async Task<string> GetAuthenticatedUserProfile()
        {
            string uri = "https://api.github.com/user";
            uri += "?access_token=" + access_token;
            return await getStringAsync(uri);
        }

        public async Task<string> GetAuthenticatedUserFollowing()
        {
            string uri = "https://api.github.com/user/following";
            uri += "?access_token=" + access_token;
            return await getStringAsync(uri);
        }

        public async Task<string> GetAuthenticatedUserFollowers()
        {
            string uri = "https://api.github.com/user/followers";
            uri += "?access_token=" + access_token;
            return await getStringAsync(uri);
        }

        public async Task<string> GetAuthenticatedUserRepos()
        {
            string uri = "https://api.github.com/user/repos";
            uri += "?access_token=" + access_token;
            return await getStringAsync(uri);
        }
    }
}
