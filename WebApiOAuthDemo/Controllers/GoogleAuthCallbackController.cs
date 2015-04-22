using Google.Apis.Auth.OAuth2.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApiOAuthDemo.Core.GoogleApi;

namespace WebApiOAuthDemo.Controllers
{
    /// <summary>
    /// 使用Google提供的Api元件時, 需產生一個繼承自Google.Apis.Auth.OAuth2.Mvc.Controllers.AuthCallbackController的Controller類別, 並指定FloaData
    /// </summary>
    public class GoogleAuthCallbackController : Google.Apis.Auth.OAuth2.Mvc.Controllers.AuthCallbackController
    {
        protected override Google.Apis.Auth.OAuth2.Mvc.FlowMetadata FlowData
        {
            get { return new AppFlowMetadata(); }
        }
    }
}