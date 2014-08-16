//-----------------------------------------------------------------------------------------------------
// <copyright file="BindingSyntaxExtensions.cs" company="Scott Xu">
//   Copyright (c) 2014 Scott Xu.
// </copyright>
//-----------------------------------------------------------------------------------------------------

namespace Ninject.MockingKernel
{
    using System.Data.Entity;
    using System.Linq;
    using System.Reflection;
    using global::Moq;
    using global::Ninject;
    using global::Ninject.MockingKernel.Moq;
    using global::Ninject.Syntax;

    /// <summary>
    /// Extension methods for <see cref="IBindingToSyntax{T}"/>.
    /// </summary>
    public static class BindingSyntaxExtensions
    {
        /// <summary>
        /// SetReturnsDefault method.
        /// </summary>
        private static readonly MethodInfo SetReturnsDefaultMethod
            = typeof(Mock<>).GetMethod("SetReturnsDefault");

        /// <summary>
        /// Bind the derived <see cref="DbContext"/> to mock and auto setup its <see cref="DbSet{T}"/> properties. 
        /// </summary>
        /// <typeparam name="T">The derived <see cref="DbContext"/> type.</typeparam>
        /// <param name="builder">The binding builder.</param>
        /// <returns>The binding syntax.</returns>
        public static IBindingNamedWithOrOnSyntax<T> ToMockDbContext<T>(this IBindingToSyntax<T> builder) where T : DbContext
        {
            var kernel = builder.Kernel as MoqMockingKernel;
            var result = builder.ToMock().InSingletonScope();

            foreach (var dbsetType in typeof(T).GetProperties()
                .Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>) && p.CanWrite)
                .Select(pi => pi.PropertyType))
            {
                kernel.Bind(dbsetType)
                    .ToMethod(ctx =>
                    {
                        dynamic mDbSet = kernel.Get(typeof(Mock<>).MakeGenericType(new[] { dbsetType }));
                        return mDbSet.Object;
                    })
                    .InSingletonScope();

                SetReturnsDefaultMethod.MakeGenericMethod(dbsetType).Invoke(kernel.GetMock<T>(), new[] { kernel.Get(dbsetType) });
            }

            return result;
        }
    }
}
