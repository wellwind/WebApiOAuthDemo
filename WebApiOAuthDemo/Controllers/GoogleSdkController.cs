using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApiOAuthDemo.Core.GoogleApi;
using WebApiOAuthDemo.Models.ViewModels;

namespace WebApiOAuthDemo.Controllers
{
    public class GoogleSdkController : GoogleBaseController
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }

        public async Task<ActionResult> IndexAsync(CancellationToken cancellationToken)
        {
            var result = await new AuthorizationCodeMvcApp(this, new AppFlowMetadata()).AuthorizeAsync(cancellationToken);

            if (result.Credential != null)
            {
                googleAccessToken = result.Credential.Token.AccessToken;

                var service = new CalendarService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = result.Credential,
                    ApplicationName = "MyApiTest"
                });

                var events = service.Events.List("zh_tw.taiwan#holiday@group.v.calendar.google.com").Execute();
                List<GoogleCalendar> holidays = new List<GoogleCalendar>();
                foreach (var holiday in events.Items)
                {
                    holidays.Add(new GoogleCalendar()
                    {
                        Name = holiday.Summary,
                        Link = holiday.HtmlLink,
                        StartDate = Convert.ToDateTime(holiday.Start.Date),
                        EndDate = Convert.ToDateTime(holiday.End.Date)
                    });
                }
                return View("../Google/Holdays", holidays.OrderBy(x => x.StartDate));
            }
            else
            {
                return new RedirectResult(result.RedirectUri);
            }
        }

        public override ActionResult AuthReturn()
        {
            return RedirectToAction("Holdays");
        }

        public override ActionResult CancelAuth()
        {
            return base.CancelAuth();
        }

        public ActionResult Hoildays()
        {
            return View("../Google/Holdays");
        }
    }
}