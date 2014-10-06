//-----------------------------------------------------------------------------------------------------
// <copyright file="DbSetActivationStrategy.cs" company="Scott Xu">
//   Copyright (c) 2014 Scott Xu.
// </copyright>
//-----------------------------------------------------------------------------------------------------

namespace EntityFramework.Testing.Moq.Ninject
{
    using System;
    using System.Data.Entity;
    using System.Reflection;
    using global::Moq;
    using global::Ninject.Activation;
    using global::Ninject.Activation.Strategies;
    using global::Ninject.Planning.Directives;

    /// <summary>
    /// <see cref="DbSet{T}"/> property injection strategy.
    /// </summary>
    public class DbSetActivationStrategy : ActivationStrategy
    {
        /// <summary>
        /// <see cref="Mock"/>'s Get generic method.
        /// </summary>
        private readonly MethodInfo getMethod = typeof(Mock).GetMethod("Get");

        /// <summary>
        /// Injects values into the properties as described by <see cref="PropertyInjectionDirective"/>s
        /// contained in the plan.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="reference">The reference to the instance being activated.</param>
        public override void Activate(IContext context, InstanceReference reference)
        {
            if (reference == null)
            {
                throw new ArgumentNullException("reference");
            }

            if (context.Request.Service.IsGenericType() && context.Request.Service.GetGenericTypeDefinition() == typeof(DbSet<>))
            {
                dynamic mock = this.getMethod.MakeGenericMethod(new[] { context.Request.Service }).Invoke(null, new[] { reference.Instance });
                MockDbSetExtenstions.SetupData(mock);
            }
        }
    }
}
