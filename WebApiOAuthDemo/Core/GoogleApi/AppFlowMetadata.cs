using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Calendar.v3;
using Google.Apis.Util.Store;
using System;
using System.Web.Mvc;

namespace WebApiOAuthDemo.Core.GoogleApi
{
    /// <summary>
    /// 設定Google OAuth驗證用FloeMetadata
    /// </summary>
    public class AppFlowMetadata : FlowMetadata
    {
        private ApiSettings.ApiSettings apiSettings = new ApiSettings.GoogleApi();

        private IAuthorizationCodeFlow flow { get; set; }

        public AppFlowMetadata()
            : base()
        {
            flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = apiSettings.ClientId,
                    ClientSecret = apiSettings.Secret
                },
                Scopes = new[] { CalendarService.Scope.Calendar },
                DataStore = new FileDataStore("Calendar.Api.Auth.Store")
            });
        }

        public override string GetUserId(Controller controller)
        {
            // In this sample we use the session to store the user identifiers.
            // That's not the best practice, because you should have a logic to identify
            // a user. You might want to use "OpenID Connect".
            // You can read more about the protocol in the following link:
            // https://developers.google.com/accounts/docs/OAuth2Login.
            var user = controller.Session["user"];
            if (user == null)
            {
                user = Guid.NewGuid();
                controller.Session["user"] = user;
            }
            return user.ToString();
        }

        public override IAuthorizationCodeFlow Flow
        {
            get { return flow; }
        }

        public override string AuthCallback
        {
            get
            {
                // 不設定AuthCallback的話, 預設是/AuthCallback/IndexAsync
                return "/WebApiOAuthDemo/GoogleAuthCallback/IndexAsync";
            }
        }
    }
}