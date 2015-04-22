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
    /// <summary>
    /// 使用Google Sdk存取Google Api的範例
    /// Google Sdk進行OAuth的方法可以參考 https://developers.google.com/api-client-library/dotnet/guide/aaa_oauth
    /// 重要：使用Google Sdk時在偵錯模式下可能會造成記憶體存取錯誤問題，目前已知是bug且無解，要測試Google Sdk請使用Ctrl+F5(啟動但不偵錯)執行
    /// </summary>
    public class GoogleSdkController : GoogleBaseController
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }

        public async Task<ActionResult> IndexAsync(CancellationToken cancellationToken)
        {
            // 設定好FlowMetadata及回傳的Url後Controller後，只需一行即可完成OAuth驗證授權
            var result = await new AuthorizationCodeMvcApp(this, new AppFlowMetadata()).AuthorizeAsync(cancellationToken);

            if (result.Credential != null)
            {
                GoogleAccessToken = result.Credential.Token.AccessToken;

                // 以下範例使用CalendarServer存取Google Calendar
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