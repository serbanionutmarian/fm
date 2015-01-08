using ConsoleTest.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using System.Web.Mvc;
using Autofac.Integration.Mvc;
using DataService;
using System.ComponentModel;
using DataService.Services;

namespace ConsoleTest
{
    class Program
    {
        private static T GetService<T>()
        {
            return DependencyResolver.Current.GetService<T>();
        }

        static void Main(string[] args)
        {
            InitIoc();
            var repository = GetService<CountryService>();
            repository.GetAll();
        }

        private static void InitIoc()
        {
            //Autofac Configuration
            var builder = new Autofac.ContainerBuilder();

            builder.RegisterModule(new RepositoryModule());
            builder.RegisterModule(new ServiceModule());
            builder.RegisterModule(new EFModule());

            var container = builder.Build();
            new AutofacDependencyResolver(container);
            //    DependencyResolver.SetResolver(new AutofacDependencyResolver(container));


        }
    }
}
