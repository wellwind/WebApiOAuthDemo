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
    public class GoogleOpenAuthController : GoogleBaseController
    {
        /// <summary>
        /// DotNetOpenAuth提供的Client物件
        /// </summary>
        private WebServerClient oauthRequestClient;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            // // 設定Authorization Server資訊
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
            if (String.IsNullOrEmpty(GoogleAccessToken))
            {
                // 向Google Authorizatoin Server取得code
                oauthRequestClient.RequestUserAuthorization(
                    new[] { "https://www.googleapis.com/auth/calendar" },
                    new Uri(RedirectUrl));
                return new EmptyResult();
            }
            else
            {
                return RedirectToAction("Holdays");
            }
        }

        public override ActionResult AuthReturn()
        {
            // 回傳code後, 再向Authorization Server驗證取得Access Token
            var authorizationState = oauthRequestClient.ProcessUserAuthorization();
            if (authorizationState != null)
            {
                GoogleAccessToken = authorizationState.AccessToken;
            }

            return RedirectToAction("Holdays");
        }

        public ActionResult Holdays()
        {
            // 台灣假日行事曆ID
            string calendarId = "zh_tw.taiwan#holiday@group.v.calendar.google.com";

            // Facebook Api的相關說明可以參考 https://developers.facebook.com/docs/graph-api/reference/

            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            string calendatEventsListApiUrl = @"https://www.googleapis.com/calendar/v3/calendars/{0}/events?key={1}&access_token={2}";
            string calendarEventsJsonString = client.DownloadString(String.Format(calendatEventsListApiUrl, HttpUtility.UrlEncode(calendarId), ApiSettings.ClientId, GoogleAccessToken));
            JObject jsonCalendarEvents = (JsonConvert.DeserializeObject(calendarEventsJsonString) as JObject);

            List<GoogleCalendar> calendarEvents = new List<GoogleCalendar>();
            foreach (JObject calendarEvent in jsonCalendarEvents["items"])
            {
                calendarEvents.Add(new GoogleCalendar()
                {
                    Name = calendarEvent["summary"].ToString(),
                    Link = calendarEvent["htmlLink"].ToString(),
                    StartDate = Convert.ToDateTime(calendarEvent["start"]["date"].ToString()),
                    EndDate = Convert.ToDateTime(calendarEvent["end"]["date"].ToString()),
                });
            }

            return View("../Google/Holdays", calendarEvents.OrderBy(x => x.StartDate));
        }
    }
}