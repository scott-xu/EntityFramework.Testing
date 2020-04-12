//-----------------------------------------------------------------------------------------------------
// <copyright file="DefaultIfEmptyRewriter.cs" company="Microsoft Open Technologies, Inc">
// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// </copyright>
// <author>Rafal Furman</author>
//-----------------------------------------------------------------------------------------------------

namespace EntityFramework.Testing
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Replaces call to <see cref="Enumerable.DefaultIfEmpty{TSource}(System.Collections.Generic.IEnumerable{TSource})" />
    /// with calls to <see cref="Enumerable.DefaultIfEmpty{TSource}(System.Collections.Generic.IEnumerable{TSource}, TSource)"/>
    /// This rewriter dose its work only if public parametless constructor is available for TSource type.
    /// </summary>
    public class DefaultIfEmptyRewriter : ExpressionVisitor
    {
        private static readonly MethodInfo EnumerableDefaultIfEmpty
            = typeof(Enumerable).GetMethods()
                .Single(x => x.Name == "DefaultIfEmpty" && x.GetParameters().Length == 2);

        private static readonly MethodInfo QueryableDefaultIfEmpty
            = typeof(Queryable).GetMethods()
                .Single(x => x.Name == "DefaultIfEmpty" && x.GetParameters().Length == 2);

        /// <summary>
        /// Visits the children of the System.Linq.Expressions.MethodCallExpression.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>
        /// The modified expression, if it or any subexpression was modified; otherwise,
        /// returns the original expression.
        /// </returns>
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.Name == "DefaultIfEmpty"
                && node.Method.GetParameters().Length == 1)
            {
                var sourceType = node.Method.GetGenericArguments().Single();
                if (sourceType.GetConstructor(new Type[0]) == null)
                {
                    return base.VisitMethodCall(node);
                }

                MethodInfo overload;
                if (node.Method.DeclaringType.Name == "Enumerable")
                {
                    overload = EnumerableDefaultIfEmpty
                        .MakeGenericMethod(node.Method.GetGenericArguments());
                }
                else if (node.Method.DeclaringType.Name == "Queryable")
                {
                    overload = QueryableDefaultIfEmpty
                        .MakeGenericMethod(node.Method.GetGenericArguments());
                }
                else
                {
                    return base.VisitMethodCall(node);
                }

                var defaultValue = Activator.CreateInstance(sourceType);
                return Expression.Call(overload, node.Arguments.Single(), Expression.Constant(defaultValue));
            }

            return base.VisitMethodCall(node);
        }
    }
}