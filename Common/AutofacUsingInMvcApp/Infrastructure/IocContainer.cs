using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using AutofacUsingInMvcApp.Services;
using Common.Autofac;
using Common.Caching;

namespace AutofacUsingInMvcApp.Infrastructure
{
	public class IocContainer
	{
		/// <summary>
		/// Registers the dependencies.
		/// </summary>
		/// <param name="assembly">The assembly.</param>
		public static void RegisterDependencies(Assembly assembly)
		{
			var autofacManager = new AutofacManager();
			autofacManager.Builder.RegisterControllers(assembly);
			autofacManager.Builder.RegisterFilterProvider();

			autofacManager.Builder.RegisterType<DebugLogger>().As<ILogger>().SingleInstance();
			autofacManager.Builder.RegisterType<AppInsightsLogger>()
				.As<ITelemetryLogger>()
				.WithParameter("appInsightsKey", "TestKey")
				.WithParameter("appInsightsName", "TestName")
				.SingleInstance();

			autofacManager.Builder.RegisterType<TestService>()
				.As<ITestService>()
				.InstancePerDependency();

			autofacManager.Builder.RegisterType<DistributedCacheManager>()
				.As<ICacheManager>()
				.WithParameter("connectionString", "TestConnection")
				.SingleInstance();

			autofacManager.BuildContainer();
			DependencyResolver.SetResolver(new AutofacDependencyResolver(autofacManager.Container));
		}
	}
}