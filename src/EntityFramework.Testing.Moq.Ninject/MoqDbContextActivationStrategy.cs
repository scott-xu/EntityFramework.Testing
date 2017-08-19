//-----------------------------------------------------------------------------------------------------
// <copyright file="MoqDbContextActivationStrategy.cs" company="Scott Xu">
// Copyright (c) Scott Xu. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------------------------------------

namespace EntityFramework.Testing.Moq.Ninject
{
    using System.Data.Entity;
    using System.Reflection;
    using EntityFramework.Testing.Ninject;
    using global::Moq;
    using global::Ninject.Activation;

    /// <summary>
    /// <see cref="DbContext"/> activation strategy.
    /// </summary>
    public class MoqDbContextActivationStrategy : DbContextActivationStrategy
    {
        /// <summary>
        /// <see cref="Mock"/>'s Get generic method.
        /// </summary>
        private readonly MethodInfo getMethod = typeof(Mock).GetMethod("Get");

        /// <summary>
        /// Setup properties for the <see cref="DbContext"/>.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="reference">The reference to the <see cref="DbContext"/>.</param>
        protected override void ActivateDbContext(IContext context, InstanceReference reference)
        {
            dynamic mock = this.getMethod.MakeGenericMethod(new[] { context.Request.Service }).Invoke(null, new[] { reference.Instance });
            mock.SetupAllProperties();
        }
    }
}