using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChinaHCM.QA.Component.Tools;
using System.Configuration;
using System.IO;

using ChinaHCM.QA.Site.Web.Models;

namespace ChinaHCM.QA.Site.Web.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            Error _e = new Error
            {
                Title = "系统错误",
                Details = filterContext.Exception.Message,
                Cause = Server.UrlEncode("<li>参数错误</li><li>系统故障</li>"),
                Solution = Server.UrlEncode("请通过正确方式访问系统")
            };
            Response.Redirect(Url.Action("Error", "Prompt", _e));
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (int.Parse(ConfigurationManager.AppSettings["EnabledLogFile"]) == 1)
            {
                #region 记录日志
                string strPath = filterContext.RequestContext.HttpContext.Server.MapPath("~");	//获得应用程序根目录
                if (strPath.Substring(strPath.Length - 1, 1) != @"\")
                {
                    strPath += @"\";
                }

                strPath += string.Format(@"Acc_Log_Files\{0}\", DateTime.Today.ToString("yyyyMM"));

                if (!Directory.Exists(strPath))
                {
                    Directory.CreateDirectory(strPath);
                }

                PageLoger.DebugFileName = string.Format("{0}IIS_Page_Acc_Log{1}.Log",
                    strPath, DateTime.Today.ToString("yyyyMMdd"));

                //每一行标记信息：Time【自带】，（IP，SessionID，URL，HTTP_REFERER）,

                bool bIsUri = filterContext.RequestContext.HttpContext.Request.Url is Uri;   //防止出现传过来非法Url的情况
                //真实IP，代理IP，正在访问你的URL，来源URL
                string strRealIp, strNormalIp, strUrl, strRefer;
                try
                {
                    //获得真实IP和代理IP串信息[注意out参数有可能是伪造的，无法避免]
                    strRealIp = Functions.GetRealIP(out strNormalIp);

                    strUrl = bIsUri ? filterContext.RequestContext.HttpContext.Request.Url.AbsoluteUri : "Erratic Url";
                }
                catch
                {
                    strRealIp = "Erratic IP";
                    strNormalIp = string.Empty;

                    strUrl = "Erratic Url";
                }

                try
                {
                    strRefer = (filterContext.RequestContext.HttpContext.Request.UrlReferrer is Uri) ? filterContext.RequestContext.HttpContext.Request.UrlReferrer.AbsoluteUri : "No REFERER";
                }
                catch (Exception err)
                {
                    strRefer = err.Message;
                }

                if (!string.IsNullOrEmpty(strNormalIp))
                {
                    strRealIp += "|" + strNormalIp;
                }

                string strRecord = string.Format("{0}\t{1}\t{2}\t{3}",
                    strRealIp, filterContext.RequestContext.HttpContext.Session.SessionID, strUrl, strRefer);

                PageLoger.WriteLog(strRecord);
                #endregion
            }

            base.OnActionExecuting(filterContext);            
        }
    }
}
