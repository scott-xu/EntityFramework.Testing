//-----------------------------------------------------------------------------------------------------
// <copyright file="TypeExtensions.cs" company="Microsoft Open Technologies, Inc">
// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------------------------------------

namespace EntityFramework.Testing
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Extension methods for <see cref="Type"/>.
    /// </summary>
    internal static class TypeExtensions
    {
        /// <summary>
        /// Get declared methods.
        /// </summary>
        /// <param name="type">The <see cref="Type"/>.</param>
        /// <returns>The methods.</returns>
        public static IEnumerable<MethodInfo> GetDeclaredMethods(this Type type)
        {
            DebugCheck.NotNull(type);
#if NET40
            const BindingFlags BindingFlags
                = BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
            return type.GetMethods(BindingFlags);
#else
            return type.GetTypeInfo().DeclaredMethods;
#endif
        }
    }
}