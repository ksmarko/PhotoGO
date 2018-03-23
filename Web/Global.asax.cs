using BLL.Infrastructure;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Web.Util;

namespace Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //BLL.Infrastructure.AutoMapperConfig.Initialize();

            ////for quest and reserv services binding
            //NinjectModule orderModule = new ServiceModule();

            ////for binding unit of work
            //NinjectModule serviceModule = new ConnectionModule("DefaultConnection");

            //var kernel = new StandardKernel(orderModule, serviceModule);
            //DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
        }
    }
}
