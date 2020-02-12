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
        #region Properties

        /// <summary>
        /// Check the container build status
        /// </summary>
        public bool IsBuilt { get; set; }

        /// <summary>
        /// The Autofac container
        /// </summary>
        public IContainer Container { get; private set; }

        /// <summary>
        /// The Autofac builder
        /// </summary>
        public ContainerBuilder Builder { get; }

        #endregion

        #region Ctor

        public AutofacManager()
        {
            Builder = new ContainerBuilder();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Builds the Autofac container if it's not already built, otherwise update the container
        /// </summary>
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

        /// <summary>
        /// Register all assembly types and life time managers for Autofac builder
        /// </summary>
        /// <param name="builder">Container to configure</param>
        /// <param name="assembly">The assembly which contains the types</param>
        /// <param name="whereType">The filter clause</param>
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

        #endregion
    }
}
