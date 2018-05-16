using AutoMapper;
using PhotoGO.BLL.Infrastructure;
using Microsoft.Owin;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Binding;

namespace Web.App_Start
{
    public class Resolver
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                PhotoGO.BLL.Infrastructure.AutoMapperConfig.Configure(cfg);
                Web.Binding.AutoMapperConfig.Configure(cfg);
            });

            NinjectModule serviceModule = new ServiceModule();
            NinjectModule connectionModule = new ConnectionModule("DefaultConnection");

            var kernel = new StandardKernel(connectionModule, serviceModule);
            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
        }
    }
}