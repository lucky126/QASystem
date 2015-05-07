using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using ChinaHCM.QA.Site.Web.Models;

namespace ChinaHCM.QA.Site.Web.Areas.Manager.Controllers
{
    public class PromptController : Controller
    {
        public ActionResult Notice(Notice notice)
        {
            return View(notice);
        }

        public ActionResult Error(Error error)
        {
            error.Details = Server.UrlDecode(error.Details);
            error.Cause = Server.UrlDecode(error.Cause);
            error.Solution = Server.UrlDecode(error.Solution);
            return View(error);
        }
        public ActionResult Index(string errorDetail)
        {
            Error error = new Models.Error() { Title = "WebSite 网站内部错误", Details = errorDetail };

            return View("Error", error);    //全部路由到Error下的Error视图
        }
        public ActionResult HttpError404(string errorDetail)
        {
            Error error = new Models.Error() { Title = "HTTP 404- 无法找到文件", Details = errorDetail };

            return View("Error", error);
        }
        public ActionResult HttpError500(string errorDetail)
        {
            Error error = new Models.Error() { Title = "HTTP 500 - 内部服务器错误", Details = errorDetail };

            return View("Error", error);
        }
        public ActionResult General(string errorDetail)
        {
            Error error = new Models.Error() { Title = "HTTP 发生错误", Details = errorDetail };

            return View("Error", error);
        }  
    }
}
