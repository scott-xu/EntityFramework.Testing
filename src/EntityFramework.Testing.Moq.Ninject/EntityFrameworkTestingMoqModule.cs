//-----------------------------------------------------------------------------------------------------
// <copyright file="EntityFrameworkTestingMoqModule.cs" company="Scott Xu">
//   Copyright (c) 2014 Scott Xu.
// </copyright>
//-----------------------------------------------------------------------------------------------------

namespace EntityFramework.Testing.Moq.Ninject
{
    using EntityFramework.Testing.Ninject;
    using global::Ninject.Activation.Strategies;

    /// <summary>
    /// EntityFramework Testing Module
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
