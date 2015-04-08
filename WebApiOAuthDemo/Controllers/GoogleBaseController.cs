using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApiOAuthDemo.Core.ApiSettings;
using WebApiOAuthDemo.Core.Helpers;

namespace WebApiOAuthDemo.Controllers
{
    public abstract class GoogleBaseController : Controller
    {
        protected string redirectUrl { get; set; }

        protected ApiSettings apiSettings = new GoogleApi();

        protected string googleAccessToken
        {
            get
            {
                return Session["GoogleAccessToken"] == null || String.IsNullOrEmpty(Session["GoogleAccessToken"].ToString()) ? "" : Session["GoogleAccessToken"].ToString();
            }
            set
            {
                if (Session["GoogleAccessToken"] == null)
                {
                    Session.Add("GoogleAccessToken", value);
                }
                else
                {
                    Session["GoogleAccessToken"] = value;
                }
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            redirectUrl = ConfigHelper.GetAuthReturnUrl("AuthReturn", filterContext.ActionDescriptor.ControllerDescriptor.ControllerName);
        }

        public virtual ActionResult AuthReturn()
        {
            throw new NotImplementedException();
        }

        public virtual ActionResult CancelAuth()
        {
            string deletePermissionUrl = @"https://accounts.google.com/o/oauth2/revoke?token={0}";
            var request = WebRequest.Create(String.Format(deletePermissionUrl, googleAccessToken));
            request.Method = "GET";
            var response = (HttpWebResponse)request.GetResponse();

            googleAccessToken = "";

            return RedirectToAction("Index", "Home");
        }
    }
}