using Facebook;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApiOAuthDemo.Models.ViewModels;

namespace WebApiOAuthDemo.Controllers
{
    /// <summary>
    /// 使用Facebook Sdk存取Facebook Api範例
    /// 關於Facebook Sdk可以參考: http://facebooksdk.net/
    /// </summary>
    public class FacebookSdkController : FacebookBaseController
    {
        /// <summary>
        /// FacebookSdk提供的FacebookClient物件
        /// </summary>
        private FacebookClient fbClient = new FacebookClient();

        public ActionResult Index()
        {
            if (String.IsNullOrEmpty(FbAccessToken))
            {
                // 利用FacebookClient物件取得Authorization Server位址
                fbClient.AppId = ApiSettings.ClientId;
                fbClient.AppSecret = ApiSettings.Secret;

                Uri fbOAuthLoginUrl = fbClient.GetLoginUrl(new { redirect_uri = RedirectUrl, scope = "user_photos" });
                return Redirect(fbOAuthLoginUrl.ToString());
            }
            return RedirectToAction("MyPhotos");
        }

        public override ActionResult AuthReturn()
        {
            // 利用FacebookClient物件將code給Token Server換取Access Token
            dynamic result = fbClient.Get("oauth/access_token", new
            {
                client_id = ApiSettings.ClientId,
                client_secret = ApiSettings.Secret,
                code = Request.QueryString["code"],
                redirect_uri = RedirectUrl
            });

            FbAccessToken = result.access_token;

            return RedirectToAction("MyPhotos");
        }

        public override ActionResult CancelAuth()
        {
            FacebookClient client = new FacebookClient(FbAccessToken);
            client.Delete("me/permissions");

            return RedirectToAction("Index", "Home");
        }

        public ActionResult MyPhotos()
        {
            // 利用FacebookClient(帶入AccessToken參數)來存取Api範例
            // Facebook Sdk有自己的Json物件, 但其實不是很好用
            FacebookClient client = new FacebookClient(FbAccessToken);
            JsonObject myPhotos = client.Get("me?fields=photos") as JsonObject;

            List<FacebookPhoto> photos = new List<FacebookPhoto>();
            foreach (JsonObject photo in (myPhotos["photos"] as JsonObject)["data"] as JsonArray)
            {
                FacebookPhoto singlePhoto = new FacebookPhoto();
                if (photo.ContainsKey("name"))
                {
                    singlePhoto.Name = photo["name"].ToString();
                }
                singlePhoto.Link = photo["link"].ToString();
                singlePhoto.Picture = photo["picture"].ToString();

                photos.Add(singlePhoto);
            }

            return View("../Facebook/MyPhotos", photos);
        }
    }
}