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
    public class GoogleOpenAuthController : GoogleBaseController
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
            if (String.IsNullOrEmpty(googleAccessToken))
            {
                oauthRequestClient.RequestUserAuthorization(
                    new[] { "https://www.googleapis.com/auth/calendar" },
                    new Uri(redirectUrl));
                return new EmptyResult();
            }
            else
            {
                return RedirectToAction("Holdays");
            }
        }

        public override ActionResult AuthReturn()
        {
            var authorizationState = oauthRequestClient.ProcessUserAuthorization();
            if (authorizationState != null)
            {
                googleAccessToken = authorizationState.AccessToken;
            }

            return RedirectToAction("Holdays");
        }

        public ActionResult Holdays()
        {
            // 台灣假日行事曆ID
            string calendarId = "zh_tw.taiwan#holiday@group.v.calendar.google.com";

            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            string calendatEventsListApiUrl = @"https://www.googleapis.com/calendar/v3/calendars/{0}/events?key={1}&access_token={2}";
            string calendarEventsJsonString = client.DownloadString(String.Format(calendatEventsListApiUrl, HttpUtility.UrlEncode(calendarId), apiSettings.ClientId, googleAccessToken));
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