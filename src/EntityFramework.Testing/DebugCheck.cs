//-----------------------------------------------------------------------------------------------------
// <copyright file="DebugCheck.cs" company="Microsoft Open Technologies, Inc">
// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------------------------------------

namespace EntityFramework.Testing
{
    using System.Diagnostics;

    /// <summary>
    /// Asserts value in debug mode.
    /// </summary>
    internal class DebugCheck
    {
        /// <summary>
        /// Asserts the value being not null.
        /// </summary>
        /// <typeparam name="T">The reference type. </typeparam>
        /// <param name="value">The value. </param>
        [Conditional("DEBUG")]
        public static void NotNull<T>(T value)
            where T : class
        {
            Debug.Assert(value != null, "The value should not be null.");
        }
    }
}