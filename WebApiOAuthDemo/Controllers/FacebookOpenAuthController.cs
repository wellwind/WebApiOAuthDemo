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
    /// <summary>
    /// 使用DotNetOpenAuth元件進行OAuth驗證的參考範例
    /// </summary>
    public class FacebookOpenAuthController : FacebookBaseController
    {
        /// <summary>
        /// DotNetOpenAuth提供的Client物件
        /// </summary>
        private WebServerClient oauthRequestClient;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            // 設定Authorization Server資訊
            AuthorizationServerDescription authorizationServer = new AuthorizationServerDescription
            {
                AuthorizationEndpoint = new Uri(ApiSettings.AuthorizationEndpoint),
                TokenEndpoint = new Uri(ApiSettings.TokenEndpoint)
            };
            oauthRequestClient = new WebServerClient(authorizationServer);
            oauthRequestClient.ClientIdentifier = ApiSettings.ClientId;
            oauthRequestClient.ClientCredentialApplicator = ClientCredentialApplicator.PostParameter(ApiSettings.Secret);
        }

        public ActionResult Index()
        {
            if (String.IsNullOrEmpty(FbAccessToken))
            {
                // 向Facebook Authorizatoin Server取得code
                oauthRequestClient.RequestUserAuthorization(
                    new[] { "user_photos" },
                    new Uri(RedirectUrl));

                return new EmptyResult();
            }
            else
            {
                return RedirectToAction("MyPhotos", "Facebook");
            }
        }

        public override ActionResult AuthReturn()
        {
            // 回傳code後, 再向Authorization Server驗證取得Access Token
            var authorizationState = oauthRequestClient.ProcessUserAuthorization();
            if (authorizationState != null)
            {
                FbAccessToken = authorizationState.AccessToken;
            }

            return RedirectToAction("MyPhotos", "Facebook");
        }
    }
}