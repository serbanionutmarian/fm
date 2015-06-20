using DataModel.Tables;
using DataService.Interfaces;
using Ioc;
using LightInject;
using ServiceStack.CacheAccess;
using ServiceStack.CacheAccess.Providers;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.WebHost.Endpoints;
using System;
using System.Globalization;
using DataService;
using DataModel.Extensions;

namespace SSService.Config
{
    public class AppHost : AppHostBase
    {
        //Tell ServiceStack the name of your application and where to find your services
        public AppHost()
            : base("Web Service", typeof(AppHost).Assembly)
        {

        }
        public override void Configure(Funq.Container container)
        {
            var serviceContainer = RegisterIoc(container);

            Plugins.Add(new AuthFeature(() => new CustomAuthUserSession(),
                new IAuthProvider[] { 
                    new EFCredentialsAuthProvider()
                }));

            Plugins.Add(new RegistrationFeature());
            container.Register<ICacheClient>(new MemoryCacheClient());
            container.Register<IUserAuthRepository>(new InMemoryAuthRepository());

            Config.DebugMode = true;
        }

        public override IServiceRunner<TRequest> CreateServiceRunner<TRequest>(ActionContext actionContext)
        {
            return new CustomServiceRunner<TRequest>(this, actionContext);
        }

        private ServiceContainer RegisterIoc(Funq.Container container)
        {
            container.Adapter = LightInjectContainer.Adapter;

            var lightInjectContainer = LightInjectContainer.Container;
            lightInjectContainer.Register<IAuthProvider, EFCredentialsAuthProvider>(new PerScopeLifetime());
            lightInjectContainer.EnablePerWebRequestScope();

            return lightInjectContainer;
        }
    }

    public class CustomServiceRunner<T> : ServiceRunner<T>
    {
        public CustomServiceRunner(IAppHost appHost, ActionContext actionContext) :
            base(appHost, actionContext)
        {
        }

        public override object HandleException(IRequestContext requestContext, T request, Exception ex)
        {
            var logService = LightInjectContainer.Adapter.Resolve<ILogService>();
            var log = ex.ToLog();
            log.Request = ServiceStack.Text.JsonSerializer.SerializeToString<T>(request);
            log.AbsoluteUri = requestContext.AbsoluteUri;
            logService.Create(log);

            return base.HandleException(requestContext, request, ex);
        }
    }
}