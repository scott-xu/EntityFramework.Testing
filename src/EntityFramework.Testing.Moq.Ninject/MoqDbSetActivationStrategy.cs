//-----------------------------------------------------------------------------------------------------
// <copyright file="MoqDbSetActivationStrategy.cs" company="Scott Xu">
//   Copyright (c) 2014 Scott Xu.
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
    /// <see cref="DbSet{T}"/> property injection strategy.
    /// </summary>
    public class MoqDbSetActivationStrategy : DbSetActivationStrategy
    {
        /// <summary>
        /// <see cref="Mock"/>'s Get generic method.
        /// </summary>
        private readonly MethodInfo getMethod = typeof(Mock).GetMethod("Get");

        /// <summary>
        /// Seed data for the <see cref="DbSet{T}"/>.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="reference">The reference to the <see cref="DbSet{T}"/>.</param>
        protected override void ActivateDbSet(IContext context, InstanceReference reference)
        {
            dynamic mock = this.getMethod.MakeGenericMethod(new[] { context.Request.Service }).Invoke(null, new[] { reference.Instance });
            MoqDbSetExtensions.SetupData(mock);
        }
    }
}
