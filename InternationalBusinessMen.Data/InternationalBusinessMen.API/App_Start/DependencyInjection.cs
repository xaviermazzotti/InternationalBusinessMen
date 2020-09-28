using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using InternationalBusinessMen.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;

namespace InternationalBusinessMen.API.App_Start
{
    public class DependencyInjection
    {
        public static void Injection()
        {
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(typeof(WebApiApplication).Assembly);
            builder.RegisterControllers(typeof(WebApiApplication).Assembly);

            builder.RegisterAssemblyTypes(
                AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => a.GetName().FullName.StartsWith("InternationalBusinessMen")).ToArray())
                .AsImplementedInterfaces();

            var dbContext = new DataModel();
            builder.RegisterInstance(dbContext);
            builder.Register(c => dbContext).As<DataModel>().SingleInstance();

            

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver((IContainer)container);
        }
    }
}