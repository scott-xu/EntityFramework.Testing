//-----------------------------------------------------------------------------------------------------
// <copyright file="EntityFrameworkTestingNSubstituteModule.cs" company="Scott Xu">
//   Copyright (c) 2014 Scott Xu.
// </copyright>
//-----------------------------------------------------------------------------------------------------

namespace EntityFramework.Testing.NSubstitute.Ninject
{
    using EntityFramework.Testing.Ninject;
    using global::Ninject.Activation.Strategies;

    /// <summary>
    /// EntityFramework Testing Module
    /// </summary>
    public class EntityFrameworkTestingNSubstituteModule : EntityFrameworkTestingModule
    {
        /// <summary>
        /// Load the components.
        /// </summary>
        public override void Load()
        {
            this.Kernel.Components.Add<IActivationStrategy, NSubstituteDbSetActivationStrategy>();

            base.Load();
        }
    }
}
