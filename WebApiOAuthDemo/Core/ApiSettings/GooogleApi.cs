using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiOAuthDemo.Core.ApiSettings
{
    public class GoogleApi : ApiSettings
    {
        public string ClientId { get { return "585508823640-55jeeu3tfah8qsikf46c22mgkcnm4kuc.apps.googleusercontent.com"; } set { ;} }

        public string Secret { get { return "Lh-n4AFP4BhqgrwtxDDr4qyx"; } set { ;} }

        public string AuthorizationEndpoint { get { return "https://accounts.google.com/o/oauth2/auth"; } set { ;} }

        public string TokenEndpoint { get { return "https://accounts.google.com/o/oauth2/token"; } set { ;} }
    }
}