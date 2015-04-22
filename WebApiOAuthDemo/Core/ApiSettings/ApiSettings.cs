using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiOAuthDemo.Core.ApiSettings
{
    /// <summary>
    /// 以OAuth存取Api的Client相關設定
    /// </summary>
    public interface ApiSettings
    {
        string ClientId { get; set; }

        string Secret { get; set; }

        /// <summary>
        /// 驗證用的Server
        /// </summary>
        string AuthorizationEndpoint { get; set; }

        /// <summary>
        /// 取得Access Token的Server
        /// </summary>
        string TokenEndpoint { get; set; }
    }
}