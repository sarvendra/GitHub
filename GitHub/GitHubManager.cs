using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHub
{
    public sealed class GitHubManager
    {
        private string access_token = null;
        private const string accessTokenFilePath = "accessTokenFile.txt";

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
                }
            }
        }
    }
}
