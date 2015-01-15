using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using System.Web.Mvc;
using DataService;
using System.ComponentModel;
using DataService.Services;
using DataService.Interfaces;
using AutofacModules.Modules;
using System.Data.Entity;
using DataModel;
using DataModel.Auth;

namespace ConsoleTest
{
    class MyClass
    {
        private readonly float _intervalBetweenMatches;
        private readonly int _totalDays;
        private float _currentDay = 1;

        public MyClass(int totalDays, int nrOfMatches)
        {
            if (totalDays < nrOfMatches - 1)
            {
                throw new ArgumentException("totalDays must be greater or equal to nrOfMatches -1");
            }
            _totalDays = totalDays;
            _intervalBetweenMatches = (float)(totalDays - 2) / nrOfMatches;  // ignore first and last date
        }
        public int Current
        {
            get { return (int)Math.Round(_currentDay); }
        }
        public bool MoveNext()
        {
            return (_currentDay += _intervalBetweenMatches) < (float)_totalDays;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            MyClass c = new MyClass(29, 20);

            while (c.MoveNext())
            {
                var g = c.Current;
            }

            Database.SetInitializer<DbManagerContext>(null);
            RunUsingIoc(RunMethod);
        }

        private static void RunMethod(ILifetimeScope scope)
        {
            //var service = scope.Resolve<IAuthService>();
            //service.SignUp(new SignupDto() { 
            //    DisplayName="Ionut S.",
            //    Email="serban.ionut.marian@gmail.com",
            //    Password="test..."
            //});

            //var service = scope.Resolve<IDataGeneratorService>();
            //service.AddLeagesToAllCountries();

            var service = scope.Resolve<ISchedulerService>();
            service.AllTableMatches();
        }

        private static void RunUsingIoc(Action<ILifetimeScope> action)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new RepositoryModule());
            builder.RegisterModule(new ServiceModule());
            builder.RegisterModule(new EFModule());

            using (var container = builder.Build())
            using (var scope = container.BeginLifetimeScope())
            {
                action(scope);
            }
        }
    }
}
