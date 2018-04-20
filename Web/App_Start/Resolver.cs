using BLL.Infrastructure;
using Microsoft.Owin;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Util;

namespace Web.App_Start
{
    public class Resolver
    {
        public static void Configure()
        {
            AutoMapperConfig.Initialize();
            NinjectModule orderModule = new ServiceModule();
            NinjectModule serviceModule = new ConnectionModule("DefaultConnection");

            var kernel = new StandardKernel(orderModule, serviceModule);
            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
        }
    }
}