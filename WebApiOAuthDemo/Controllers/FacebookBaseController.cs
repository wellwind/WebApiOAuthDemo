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
    public abstract class FacebookBaseController : Controller
    {
        protected string RedirectUrl { get; set; }

        protected ApiSettings ApiSettings = new FacebookApi();

        protected string FbAccessToken
        {
            get
            {
                return Session["FbAccessToken"] == null || String.IsNullOrEmpty(Session["FbAccessToken"].ToString()) ? "" : Session["FbAccessToken"].ToString();
            }
            set
            {
                if (Session["FbAccessToken"] == null)
                {
                    Session.Add("FbAccessToken", value);
                }
                else
                {
                    Session["FbAccessToken"] = value;
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
            string deletePermissionUrl = @"https://graph.facebook.com/me/permissions?access_token={0}";
            var request = WebRequest.Create(String.Format(deletePermissionUrl, FbAccessToken));
            request.Method = "DELETE";
            var response = (HttpWebResponse)request.GetResponse();

            FbAccessToken = "";

            return RedirectToAction("Index", "Home");
        }
    }
}