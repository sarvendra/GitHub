using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHub.Utility
{
    public class GitHubAPIPathDefines
    {
        public const string GIT_BASE_URL = "https://api.github.com/";
        public const string BASE_SEARCH_URI = GIT_BASE_URL + "search/{0}";
        public const string BASE_USER_URI = GIT_BASE_URL + "users/";
        public const string BASE_REPO_URI = GIT_BASE_URL + "repos/{0}/{1}";
        public const string BASE_BRANCHES_URI = GIT_BASE_URL + "repos/{0}/{1}/branches";
        public const string BASE_BRANCH_CONTENT_URI = GIT_BASE_URL + "repos/{0}/{1}/contents?ref={2}";
    }
}
