//-----------------------------------------------------------------------------------------------------
// <copyright file="EntityFrameworkTestingMoqModule.cs" company="Scott Xu">
// Copyright (c) Scott Xu. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------------------------------------

namespace EntityFramework.Testing.Moq.Ninject
{
    using EntityFramework.Testing.Ninject;
    using global::Ninject.Activation.Strategies;

    /// <summary>
    /// EntityFramework Testing Module.
    /// </summary>
    public class EntityFrameworkTestingMoqModule : EntityFrameworkTestingModule
    {
        /// <summary>
        /// Load the components.
        /// </summary>
        public override void Load()
        {
            this.Kernel.Components.Add<IActivationStrategy, MoqDbContextActivationStrategy>();
            this.Kernel.Components.Add<IActivationStrategy, MoqDbSetActivationStrategy>();

            base.Load();
        }
    }
}