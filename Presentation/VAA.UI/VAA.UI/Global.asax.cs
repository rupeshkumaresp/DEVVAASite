using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using Telerik.Reporting.Services.WebApi;

namespace VAA.UI
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            ReportsControllerConfiguration.RegisterRoutes(GlobalConfiguration.Configuration);
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}