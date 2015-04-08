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
    public class FacebookSdkController : FacebookBaseController
    {
        private FacebookClient fbClient = new FacebookClient();

        public ActionResult Index()
        {
            if (String.IsNullOrEmpty(fbAccessToken))
            {
                fbClient.AppId = apiSettings.ClientId;
                fbClient.AppSecret = apiSettings.Secret;

                Uri fbOAuthLoginUrl = fbClient.GetLoginUrl(new { redirect_uri = redirectUrl, scope = "user_photos" });
                return Redirect(fbOAuthLoginUrl.ToString());
            }
            return RedirectToAction("MyPhotos");
        }

        public override ActionResult AuthReturn()
        {
            dynamic result = fbClient.Get("oauth/access_token", new
            {
                client_id = apiSettings.ClientId,
                client_secret = apiSettings.Secret,
                code = Request.QueryString["code"],
                redirect_uri = redirectUrl
            });

            fbAccessToken = result.access_token;

            return RedirectToAction("MyPhotos");
        }

        public override ActionResult CancelAuth()
        {
            FacebookClient client = new FacebookClient(fbAccessToken);
            client.Delete("me/permissions");

            return RedirectToAction("Index", "Home");
        }

        public ActionResult MyPhotos()
        {
            FacebookClient client = new FacebookClient(fbAccessToken);
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