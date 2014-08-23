//-----------------------------------------------------------------------------------------------------
// <copyright file="DbSetInjectionHeuristic.cs" company="Scott Xu">
//   Copyright (c) 2014 Scott Xu.
// </copyright>
//-----------------------------------------------------------------------------------------------------

namespace EntityFramework.Testing.Moq.Ninject
{
    using System.Data.Entity;
    using System.Reflection;
    using global::Ninject.Selection.Heuristics;

    /// <summary>
    /// Should inject <see cref="DbSet{T}"/> properties.
    /// </summary>
    public class DbSetInjectionHeuristic : StandardInjectionHeuristic
    {
        /// <summary>
        /// Returns a value indicating whether the specified member should be injected
        /// </summary>
        /// <param name="member">The member in question.</param>
        /// <returns>True if the member should be injected; otherwise false.</returns>
        public override bool ShouldInject(MemberInfo member)
        {
            return base.ShouldInject(member) ||
                (member is PropertyInfo &&
                ((PropertyInfo)member).CanWrite &&
                ((PropertyInfo)member).PropertyType.IsGenericType() &&
                ((PropertyInfo)member).PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>) &&
                ((PropertyInfo)member).GetAccessors()[0].IsVirtual &&
                !((PropertyInfo)member).GetAccessors()[0].IsFinal);
        }
    }
}
