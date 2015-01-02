//-----------------------------------------------------------------------------------------------------
// <copyright file="DbSetActivationStrategy.cs" company="Scott Xu">
//   Copyright (c) 2014 Scott Xu.
// </copyright>
//-----------------------------------------------------------------------------------------------------

namespace EntityFramework.Testing.Ninject
{
    using System;
    using System.Data.Entity;
    using global::Ninject.Activation;
    using global::Ninject.Activation.Strategies;
    using global::Ninject.Planning.Directives;

    /// <summary>
    /// <see cref="DbSet{T}"/> property injection strategy.
    /// </summary>
    public abstract class DbSetActivationStrategy : ActivationStrategy
    {
        /// <summary>
        /// Activates the <see cref="DbSet{T}"/>.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="reference">The reference to the instance being activated.</param>
        public sealed override void Activate(IContext context, InstanceReference reference)
        {
            if (reference == null)
            {
                throw new ArgumentNullException("reference");
            }

            if (context.Request.Service.IsGenericType() && context.Request.Service.GetGenericTypeDefinition() == typeof(DbSet<>))
            {
                this.ActivateDbSet(context, reference);
            }
        }

        /// <summary>
        /// Activates the <see cref="DbSet{T}"/>.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="reference">The reference to the <see cref="DbSet{T}"/>.</param>
        protected abstract void ActivateDbSet(IContext context, InstanceReference reference);
    }
}
