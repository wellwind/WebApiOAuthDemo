using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace WebApiOAuthDemo.Core.Helpers
{
    public static class ConfigHelper
    {
        public static string GetAuthReturnUrl(string actionName, string controllerName)
        {
            return String.Format("http://localhost:30888/WebApiOAuthDemo/{0}/{1}", controllerName, actionName);
        }
    }
}