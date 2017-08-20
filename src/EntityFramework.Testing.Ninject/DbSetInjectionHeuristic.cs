//-----------------------------------------------------------------------------------------------------
// <copyright file="DbSetInjectionHeuristic.cs" company="Scott Xu">
// Copyright (c) Scott Xu. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------------------------------------

namespace EntityFramework.Testing.Ninject
{
    using System.Data.Entity;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using global::Ninject.Components;
    using global::Ninject.Selection.Heuristics;

    /// <summary>
    /// Should inject <see cref="DbSet{T}"/> properties.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly", Justification = "Dispose has been implemented by base DisposableObject")]
    public class DbSetInjectionHeuristic : NinjectComponent, IInjectionHeuristic
    {
        /// <summary>
        /// Returns a value indicating whether the specified member should be injected
        /// </summary>
        /// <param name="member">The member in question.</param>
        /// <returns>True if the member should be injected; otherwise false.</returns>
        public bool ShouldInject(MemberInfo member)
        {
            return member is PropertyInfo &&
                ((PropertyInfo)member).CanWrite &&
                ((PropertyInfo)member).PropertyType.IsGenericType &&
                ((PropertyInfo)member).PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>) &&
                ((PropertyInfo)member).GetAccessors()[0].IsVirtual &&
                !((PropertyInfo)member).GetAccessors()[0].IsFinal;
        }
    }
}