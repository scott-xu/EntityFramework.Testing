//-----------------------------------------------------------------------------------------------------
// <copyright file="EntityFrameworkTestingFakeItEasyModule.cs" company="Scott Xu">
//   Copyright (c) 2015 Scott Xu.
// </copyright>
//-----------------------------------------------------------------------------------------------------

namespace EntityFramework.Testing.FakeItEasy.Ninject
{
    using EntityFramework.Testing.Ninject;
    using global::Ninject.Activation.Strategies;

    /// <summary>
    /// EntityFramework Testing Module
    /// </summary>
    public class EntityFrameworkTestingFakeItEasyModule : EntityFrameworkTestingModule
    {
        /// <summary>
        /// Load the components.
        /// </summary>
        public override void Load()
        {
            this.Kernel.Components.Add<IActivationStrategy, FakeItEasyDbSetActivationStrategy>();

            base.Load();
        }
    }
}
