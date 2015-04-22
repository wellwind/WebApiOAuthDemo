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
    /// <summary>
    /// 使用Facebook Api的基本設定存成FacebookBaseController
    /// </summary>
    public abstract class GoogleBaseController : Controller
    {
        protected string RedirectUrl { get; set; }

        protected ApiSettings ApiSettings = new GoogleApi();

        protected string GoogleAccessToken
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
            RedirectUrl = ConfigHelper.GetAuthReturnUrl("AuthReturn", filterContext.ActionDescriptor.ControllerDescriptor.ControllerName);
        }

        public virtual ActionResult AuthReturn()
        {
            throw new NotImplementedException();
        }

        public virtual ActionResult CancelAuth()
        {
            string deletePermissionUrl = @"https://accounts.google.com/o/oauth2/revoke?token={0}";
            var request = WebRequest.Create(String.Format(deletePermissionUrl, GoogleAccessToken));
            request.Method = "GET";
            var response = (HttpWebResponse)request.GetResponse();

            GoogleAccessToken = "";

            return RedirectToAction("Index", "Home");
        }
    }
}