//-----------------------------------------------------------------------------------------------------
// <copyright file="EntityFrameworkTestingMoqModule.cs" company="Scott Xu">
//   Copyright (c) 2014 Scott Xu.
// </copyright>
//-----------------------------------------------------------------------------------------------------

namespace EntityFramework.Testing.Moq.Ninject
{
    using global::Ninject.Activation.Strategies;
    using global::Ninject.MockingKernel;
    using global::Ninject.Modules;
    using global::Ninject.Planning.Bindings.Resolvers;
    using global::Ninject.Selection.Heuristics;

    /// <summary>
    /// EntityFramework Testing Module
    /// </summary>
    public class EntityFrameworkTestingMoqModule : NinjectModule
    {
        /// <summary>
        /// Load the components.
        /// </summary>
        public override void Load()
        {
            this.Kernel.Components.Add<IInjectionHeuristic, DbSetInjectionHeuristic>();

            this.Kernel.Components.Add<IActivationStrategy, DbContextActivationStrategy>();
            this.Kernel.Components.Add<IActivationStrategy, DbSetActivationStrategy>();

            this.Kernel.Components.Remove<IActivationStrategy, PropertyInjectionStrategy>();
            this.Kernel.Components.Add<IActivationStrategy, PropertyInjectionStrategy>();

            this.Kernel.Components.RemoveAll<IMissingBindingResolver>();
            this.Kernel.Components.Add<IMissingBindingResolver, MockEntityFrameworkBindingResolver>();
            this.Kernel.Components.Add<IMissingBindingResolver, SingletonSelfBindingResolver>();
            this.Kernel.Components.Add<IMissingBindingResolver, MockMissingBindingResolver>();
        }
    }
}
