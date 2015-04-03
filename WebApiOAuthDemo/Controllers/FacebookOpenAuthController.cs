using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth2;
using WebApiOAuthDemo.Core.Helpers;
using WebApiOAuthDemo.Models.ViewModels;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebApiOAuthDemo.Controllers
{
    public class FacebookOpenAuthController : Controller
    {
        private string clientId = ConfigHelper.Facebook.AppId;
        private string clientSecret = ConfigHelper.Facebook.AppSecret;

        private string redirectUrl = ConfigHelper.GetAuthReturnUrl("Facebook", "AuthReturn");

        private WebServerClient oauthRequestClient;

        private string fbAccessToken
        {
            get
            {
                return Session["FbAccessToken"] == null || String.IsNullOrEmpty(Session["FbAccessToken"].ToString()) ? "" : Session["FbAccessToken"].ToString();
            }
            set
            {
                ;
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            AuthorizationServerDescription authorizationServer = new AuthorizationServerDescription
            {
                AuthorizationEndpoint = new Uri("https://graph.facebook.com/oauth/authorize"),
                TokenEndpoint = new Uri("https://graph.facebook.com/oauth/access_token")
            };
            oauthRequestClient = new WebServerClient(authorizationServer);
            oauthRequestClient.ClientIdentifier = clientId;
            oauthRequestClient.ClientCredentialApplicator = ClientCredentialApplicator.PostParameter(clientSecret);
        }

        public ActionResult Index()
        {
            if (String.IsNullOrEmpty(fbAccessToken))
            {
                // 向Facebook取得code
                oauthRequestClient.RequestUserAuthorization(
                    new[] { "user_photos" }, 
                    new Uri(ConfigHelper.GetAuthReturnUrl("FacebookOpenAuth", "AuthReturn")));
                return new EmptyResult();
            }
            else
            {
                return RedirectToAction("MyPhotos", "Facebook");
            }
        }

        public ActionResult AuthReturn()
        {
            var authorizationState = oauthRequestClient.ProcessUserAuthorization();
            if (authorizationState != null)
            {
                Session.Add("FbAccessToken", authorizationState.AccessToken);
            }

            return RedirectToAction("MyPhotos", "Facebook");
        }
    }
}
