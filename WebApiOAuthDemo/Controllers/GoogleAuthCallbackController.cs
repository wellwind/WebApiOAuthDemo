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
    public class GoogleAuthCallbackController : Google.Apis.Auth.OAuth2.Mvc.Controllers.AuthCallbackController
    {
        protected override Google.Apis.Auth.OAuth2.Mvc.FlowMetadata FlowData
        {
            get { return new AppFlowMetadata(); }
        }

        //public override async Task<ActionResult> IndexAsync(AuthorizationCodeResponseUrl authorizationCode, CancellationToken taskCancellationToken)
        //{
        //    base.IndexAsync(authorizationCode, taskCancellationToken);
        //    return new EmptyResult();
        //}
    }
}