//-----------------------------------------------------------------------------------------------------
// <copyright file="EntityFrameworkTestingNSubstituteModule.cs" company="Scott Xu">
// Copyright (c) Scott Xu. All rights reserved.
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
