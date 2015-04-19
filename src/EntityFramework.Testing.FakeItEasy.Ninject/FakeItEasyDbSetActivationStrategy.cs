//-----------------------------------------------------------------------------------------------------
// <copyright file="FakeItEasyDbSetActivationStrategy.cs" company="Scott Xu">
//   Copyright (c) 2015 Scott Xu.
// </copyright>
//-----------------------------------------------------------------------------------------------------

namespace EntityFramework.Testing.FakeItEasy.Ninject
{
    using System.Data.Entity;
    using System.Reflection;
    using EntityFramework.Testing.Ninject;
    using global::FakeItEasy;
    using global::Ninject.Activation;

    /// <summary>
    /// <see cref="DbSet{T}"/> property injection strategy.
    /// </summary>
    public class FakeItEasyDbSetActivationStrategy : DbSetActivationStrategy
    {
        /// <summary>
        /// Seed data for the <see cref="DbSet{T}"/>.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="reference">The reference to the <see cref="DbSet{T}"/>.</param>
        protected override void ActivateDbSet(IContext context, InstanceReference reference)
        {
            dynamic substitute = reference.Instance;
            FakeItEasyDbSetExtensions.SetupData(substitute);
        }
    }
}
