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
            var result = await new AuthorizationCodeMvcApp(this, new AppFlowMetadata()).
                AuthorizeAsync(cancellationToken);

            if (result.Credential != null)
            {
                googleAccessToken = result.Credential.Token.AccessToken;

                var service = new CalendarService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = result.Credential,
                    ApplicationName = "MyApiTest"
                });
                var holidayCalendar = service.Calendars.Get("zh_tw.taiwan#holiday@group.v.calendar.google.com");

                //var service = new DriveService(new BaseClientService.Initializer
                //{
                //    HttpClientInitializer = result.Credential,
                //    ApplicationName = "ASP.NET MVC Sample"
                //});

                //// YOUR CODE SHOULD BE HERE..
                //// SAMPLE CODE:
                //var list = await service.Files.List().ExecuteAsync();
                //ViewBag.Message = "FILE COUNT IS: " + list.Items.Count();
                return View();
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
            return View("../Facebook/Holdays");
        }
    }
}