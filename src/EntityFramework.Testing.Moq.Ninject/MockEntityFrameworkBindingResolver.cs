//-----------------------------------------------------------------------------------------------------
// <copyright file="MockEntityFrameworkBindingResolver.cs" company="Scott Xu">
//   Copyright (c) 2014 Scott Xu.
// </copyright>
//-----------------------------------------------------------------------------------------------------

namespace EntityFramework.Testing.Moq.Ninject
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using global::Ninject.Activation;
    using global::Ninject.Components;
    using global::Ninject.Infrastructure;
    using global::Ninject.MockingKernel;
    using global::Ninject.Planning.Bindings;
    using global::Ninject.Planning.Bindings.Resolvers;

    /// <summary>
    /// EntityFramework binding resolver that creates a mock for <see cref="DbContext"/> and <see cref="DbSet{T}"/>.
    /// </summary>
    public class MockEntityFrameworkBindingResolver : NinjectComponent, IMissingBindingResolver
    {
        /// <summary>
        /// The call back provider for creating the mock provider.
        /// </summary>
        private readonly IMockProviderCallbackProvider mockProviderCallbackProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="MockEntityFrameworkBindingResolver"/> class.
        /// </summary>
        /// <param name="mockProviderCallbackProvider">The mock provider callback provider.</param>
        public MockEntityFrameworkBindingResolver(IMockProviderCallbackProvider mockProviderCallbackProvider)
        {
            this.mockProviderCallbackProvider = mockProviderCallbackProvider;
        }

        /// <summary>
        /// Returns any bindings from the specified collection that match the specified request.
        /// </summary>
        /// <param name="bindings">The <see cref="Multimap{T1,T2}"/> of all registered bindings.</param>
        /// <param name="request">The request in question.</param>
        /// <returns>The series of matching bindings.</returns>
        public IEnumerable<IBinding> Resolve(Multimap<Type, IBinding> bindings, IRequest request)
        {
            if (typeof(DbContext).IsAssignableFrom(request.Service) || (request.Service.IsGenericType() && request.Service.GetGenericTypeDefinition() == typeof(DbSet<>)))
            {
                return new[]
                {
                    new Binding(request.Service)
                    {
                        ProviderCallback = this.mockProviderCallbackProvider.GetCreationCallback(),
                        ScopeCallback = ctx => StandardScopeCallbacks.Singleton,
                        IsImplicit = true
                    }
                };
            }

            return Enumerable.Empty<IBinding>();
        }

        /// <summary>
        /// Dispose the object.
        /// </summary>
        /// <param name="disposing">Whether it's disposing.</param>
        public override void Dispose(bool disposing)
        {
            this.mockProviderCallbackProvider.Dispose();
            base.Dispose(disposing);
        }
    }
}
