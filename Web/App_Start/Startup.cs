using BLL.Infrastructure;
using BLL.Interfaces;
using BLL.Services;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Mvc;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using Web.Util;

[assembly: OwinStartup(typeof(Web.App_Start.Startup))]

namespace Web.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {                        
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
            });
        }
    }
}