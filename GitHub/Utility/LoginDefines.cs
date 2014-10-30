using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHub.Utility
{
    public class LoginDefines
    {
        public const string CLIENT_ID = "92c35bdc6dd8104a4986";
        public const string CLIENT_SECRET = "ad45424fb64bed02b26b70e2d55b6e47d05d395b";
        public const string REDIRECT_URI = "http://localhost/callback";
        public const string ACCESS_TOKEN_URI = "https://github.com/login/oauth/access_token";
        public const string STATE = "abcd";
        public const string BASE_LOGIN_URI = "https://github.com/login/oauth/authorize?client_id={0}&scope=user,public_repo&redirect_uri={1}" +
                "&state={2}";
    }
}
