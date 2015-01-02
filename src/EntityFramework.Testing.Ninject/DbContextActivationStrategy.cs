//-----------------------------------------------------------------------------------------------------
// <copyright file="DbContextActivationStrategy.cs" company="Scott Xu">
//   Copyright (c) 2014 Scott Xu.
// </copyright>
//-----------------------------------------------------------------------------------------------------

namespace EntityFramework.Testing.Ninject
{
    using System;
    using System.Data.Entity;
    using System.Reflection;
    using global::Ninject.Activation;
    using global::Ninject.Activation.Strategies;

    /// <summary>
    /// <see cref="DbContext"/> activation strategy.
    /// </summary>
    public abstract class DbContextActivationStrategy : ActivationStrategy
    {
        /// <summary>
        /// Activates the <see cref="DbContext"/>.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="reference">The reference to the instance being activated.</param>
        public sealed override void Activate(IContext context, InstanceReference reference)
        {
            if (reference == null)
            {
                throw new ArgumentNullException("reference");
            }

            if (typeof(DbContext).IsAssignableFrom(context.Request.Service))
            {
                this.ActivateDbContext(context, reference);
            }
        }

        /// <summary>
        /// Activates the <see cref="DbContext"/>.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="reference">The reference to the <see cref="DbContext"/>.</param>
        protected abstract void ActivateDbContext(IContext context, InstanceReference reference);
    }
}