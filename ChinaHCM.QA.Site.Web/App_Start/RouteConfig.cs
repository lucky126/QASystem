using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ChinaHCM.QA.Site.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //主页：/
            routes.MapRoute(
                name: "Home",
                url: "",
                defaults: new
                {
                    controller = "Board",
                    action = "Index"
                },
                namespaces: new[] { "ChinaHCM.QA.Site.Web.Controllers" });

            //版区列表（第N页）：/Board-1-1、/Board-1-2、...
            routes.MapRoute(
                name: "BoardPage",
                url: "Board-{boardId}-{qatype}-{pageIndex}.shtml",
                defaults: new
                {
                    controller = "Board",
                    action = "GetBoard",
                    pageIndex = 1,
                    qatype = 2
                },
                constraints: new
                {
                    boardId = @"\d+",
                    qatype = @"[0-3]{1}",
                    pageIndex = @"\d+"
                },
                namespaces: new[] { "ChinaHCM.QA.Site.Web.Controllers" }
            );

            //帖子列表（第N页）：/Topic-1-1、/Topic-1-2、...
            routes.MapRoute(
                name: "TopicPage",
                url: "Topic-{boardId}-{topicId}-{pageIndex}.shtml",
                defaults: new
                {
                    controller = "Topic",
                    action = "GetTopic",
                    pageIndex = 1
                },
                constraints: new
                {
                    boardId = @"\d+",
                    topicId = @"\d+",
                    pageIndex = @"\d+"
                },
                namespaces: new[] { "ChinaHCM.QA.Site.Web.Controllers" }
            );

            //用户信息：/user/id
            routes.MapRoute(
               name: "UserInfo",
               url: "user/{userId}",
               defaults: new
               {
                   controller = "Account",
                   action = "GetUser"
               },
               constraints: new
               {
                   userId = @"\d+"
               },
               namespaces: new[] { "ChinaHCM.QA.Site.Web.Controllers" }
           );

            //用户登录：/Login
            routes.MapRoute(
                name: "Login",
                url: "Login",
                defaults: new { controller = "Account", action = "Login" },
                 namespaces: new[] { "ChinaHCM.QA.Site.Web.Controllers" });

            //登出注销：/Logout
            routes.MapRoute(
                name: "Logout",
                url: "Logout",
                defaults: new { controller = "Account", action = "Logout" },
                 namespaces: new[] { "ChinaHCM.QA.Site.Web.Controllers" });

            //用户注册：/Register
            routes.MapRoute(
                name: "Register",
                url: "Register",
                defaults: new { controller = "Account", action = "Register" },
                 namespaces: new[] { "ChinaHCM.QA.Site.Web.Controllers" });

            //默认
            routes.MapRoute(
               "Default", // Route name
               "{controller}/{action}",
               new { controller = "Home", action = "Index" },
               null,
               new[] { "ChinaHCM.QA.Site.Web.Controllers" }
           );
        }
    }
}