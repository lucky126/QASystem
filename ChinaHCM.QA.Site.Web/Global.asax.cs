using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.ComponentModel.Composition.Hosting;

using ChinaHCM.QA.Site.Helper.Ioc;
using ChinaHCM.QA.Site.Web.Controllers;

namespace ChinaHCM.QA.Site.Web
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //设置MEF依赖注入容器
            DirectoryCatalog catalog = new DirectoryCatalog(AppDomain.CurrentDomain.SetupInformation.PrivateBinPath);
            MefDependencySolver solver = new MefDependencySolver(catalog);
            DependencyResolver.SetResolver(solver);
        }
        
        protected void Application_Error(object sender, EventArgs e)
        {

            Response.Clear();
            Exception exception = Server.GetLastError();
            HttpException httpException = exception as HttpException;
            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "Prompt");
            if (exception == null)
            {
                routeData.Values.Add("action", "Index");
            }
            else if (httpException == null)
            {
                routeData.Values.Add("action", "Index");
            }
            else
            {
                switch (httpException.GetHttpCode())
                {
                    case 404:
                        routeData.Values.Add("action", "HttpError404");
                        break;
                    case 500:
                        routeData.Values.Add("action", "HttpError500");
                        break;
                    default:
                        routeData.Values.Add("action", "General");
                        break;
                }
            }
            // Pass exception details to the target error View. 
            routeData.Values.Add("errorDetail", exception.Message);
            // Clear the error on server. 
            Server.ClearError();
            // Call target Controller and pass the routeData. 
            IController promptController = new PromptController();
            promptController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
        }
    }
}