using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiOAuthDemo.Core.ApiSettings
{
    public class FacebookApi : ApiSettings
    {
        public string ClientId { get { return "1612267449003491"; } set { ;} }

        public string Secret { get { return "8edbcd8968fa511c51e95db51ea8603f"; } set { ;} }

        public string AuthorizationEndpoint { get { return "https://graph.facebook.com/oauth/authorize"; } set { ;} }

        public string TokenEndpoint { get { return "https://graph.facebook.com/oauth/access_token"; } set { ;} }
    }
}