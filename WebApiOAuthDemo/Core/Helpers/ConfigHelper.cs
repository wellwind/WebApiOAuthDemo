using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace WebApiOAuthDemo.Core.Helpers
{
    public static class ConfigHelper
    {
        public static string GetAuthReturnUrl(string controller, string action)
        {
            return String.Format("http://localhost:30888/WebApiDemo/{0}/{1}", controller, action);
        }

        public static class Facebook
        {
            public static string AppId { get { return WebConfigurationManager.AppSettings["FacebookAppId"]; } set { ;} }
            public static string AppSecret { get { return WebConfigurationManager.AppSettings["FacebookAppSecret"]; } set { ;} }
        }
    }
}