using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiOAuthDemo.Core.ApiSettings
{
    public interface ApiSettings
    {
        string ClientId { get; set; }

        string Secret { get; set; }

        string AuthorizationEndpoint { get; set; }

        string TokenEndpoint { get; set; }
    }
}