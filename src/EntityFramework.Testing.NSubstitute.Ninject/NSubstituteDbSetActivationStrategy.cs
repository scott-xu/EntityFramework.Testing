//-----------------------------------------------------------------------------------------------------
// <copyright file="NSubstituteDbSetActivationStrategy.cs" company="Scott Xu">
//   Copyright (c) 2014 Scott Xu.
// </copyright>
//-----------------------------------------------------------------------------------------------------

namespace EntityFramework.Testing.NSubstitute.Ninject
{
    using System.Data.Entity;
    using System.Reflection;
    using EntityFramework.Testing.Ninject;
    using global::Ninject.Activation;
    using global::NSubstitute;

    /// <summary>
    /// <see cref="DbSet{T}"/> property injection strategy.
    /// </summary>
    public class NSubstituteDbSetActivationStrategy : DbSetActivationStrategy
    {
        /// <summary>
        /// Seed data for the <see cref="DbSet{T}"/>.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="reference">The reference to the <see cref="DbSet{T}"/>.</param>
        protected override void ActivateDbSet(IContext context, InstanceReference reference)
        {
            dynamic substitute = reference.Instance;
            NSubstituteDbSetExtensions.SetupData(substitute);
        }
    }
}
