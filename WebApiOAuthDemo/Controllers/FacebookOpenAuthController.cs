using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth2;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApiOAuthDemo.Core.ApiSettings;
using WebApiOAuthDemo.Core.Helpers;
using WebApiOAuthDemo.Models.ViewModels;

namespace WebApiOAuthDemo.Controllers
{
    public class FacebookOpenAuthController : FacebookBaseController
    {
        private WebServerClient oauthRequestClient;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            AuthorizationServerDescription authorizationServer = new AuthorizationServerDescription
            {
                AuthorizationEndpoint = new Uri(apiSettings.AuthorizationEndpoint),
                TokenEndpoint = new Uri(apiSettings.TokenEndpoint)
            };
            oauthRequestClient = new WebServerClient(authorizationServer);
            oauthRequestClient.ClientIdentifier = apiSettings.ClientId;
            oauthRequestClient.ClientCredentialApplicator = ClientCredentialApplicator.PostParameter(apiSettings.Secret);
        }

        public ActionResult Index()
        {
            if (String.IsNullOrEmpty(fbAccessToken))
            {
                // 向Facebook取得code
                oauthRequestClient.RequestUserAuthorization(
                    new[] { "user_photos" },
                    new Uri(redirectUrl));

                return new EmptyResult();
            }
            else
            {
                return RedirectToAction("MyPhotos", "Facebook");
            }
        }

        public override ActionResult AuthReturn()
        {
            var authorizationState = oauthRequestClient.ProcessUserAuthorization();
            if (authorizationState != null)
            {
                fbAccessToken = authorizationState.AccessToken;
            }

            return RedirectToAction("MyPhotos", "Facebook");
        }
    }
}