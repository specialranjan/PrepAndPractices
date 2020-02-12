using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Autofac;

namespace Common.Autofac
{

    /// <summary>
    /// Wrapper for direct access to Autofac Container
    /// </summary>
    public sealed class AutofacManager
    {
        public bool IsBuilt { get; set; }
        public IContainer Container { get; private set; }
        public ContainerBuilder Builder { get; }

        public AutofacManager()
        {
            Builder = new ContainerBuilder();
        }

        public void BuildContainer()
        {
            if (!IsBuilt)
            {
                Container = Builder.Build();
                IsBuilt = true;
            }
            else
            {
                Builder.Update(Container);
            }
        }
        public void RegisterAssemblyTypes(ContainerBuilder builder, Assembly assembly, Func<Type, bool> whereType)
        {
            try
            {
                builder.RegisterAssemblyTypes(assembly)
                       .Where(whereType)
                       .AsImplementedInterfaces()
                       .InstancePerDependency();
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error occurred in RegisterAssemblyTypes with WhereType {0} - Exception : {1}", whereType, ex);
            }
        }
    }
}
