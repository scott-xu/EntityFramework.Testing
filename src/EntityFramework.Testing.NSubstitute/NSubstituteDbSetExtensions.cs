//-----------------------------------------------------------------------------------------------------
// <copyright file="NSubstituteDbSetExtensions.cs" company="Justin Yoo">
//   Copyright (c) 2014 Justin Yoo.
// </copyright>
//-----------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using EntityFramework.Testing;
using NSubstitute.Core;

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1200:UsingDirectivesMustBePlacedWithinNamespace", Justification = "Allowed to place using directives outside namespace.")]

namespace NSubstitute
{
    /// <summary>
    /// Extension methods for <see cref="NSubstitute"/>.
    /// </summary>
    public static class NSubstituteDbSetExtensions
    {
        /// <summary>
        /// Setup data to <see cref="DbSet{T}"/>.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="dbSet">The <see cref="DbSet{T}"/>.</param>
        /// <param name="data">The seed data.</param>
        /// <param name="find">The find action.</param>
        /// <returns>The updated <see cref="DbSet{T}"/>.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Allowed parameters starting with two lowercases followed by one uppercase.")]
        public static DbSet<TEntity> SetupData<TEntity>(this DbSet<TEntity> dbSet, ICollection<TEntity> data = null, Func<object[], TEntity> find = null) where TEntity : class
        {
            data = data ?? new List<TEntity>();
            find = find ?? (o => null);

            // In order to avoid enumerator modification exception, use the lambda expression to send query to the captured list.
            // http://stackoverflow.com/questions/27308190/manipulating-objects-with-dbsett-and-iqueryablet-with-nsubstitute-returns-er#27474056
            Func<IQueryable<TEntity>> getQuery = () => new InMemoryAsyncQueryable<TEntity>(data.AsQueryable());

            ((IQueryable<TEntity>)dbSet).Provider.Returns(info => getQuery().Provider);
            ((IQueryable<TEntity>)dbSet).Expression.Returns(info => getQuery().Expression);
            ((IQueryable<TEntity>)dbSet).ElementType.Returns(info => getQuery().ElementType);
            ((IQueryable<TEntity>)dbSet).GetEnumerator().Returns(info => getQuery().GetEnumerator());

#if !NET40
            ((IDbAsyncEnumerable<TEntity>)dbSet).GetAsyncEnumerator()
                                                .Returns(info => new InMemoryDbAsyncEnumerator<TEntity>(getQuery().GetEnumerator()));
            ((IQueryable<TEntity>)dbSet).Provider.Returns(info => getQuery().Provider);
#endif

            dbSet.Include(Arg.Any<string>()).Returns(dbSet);
            dbSet.Find(Arg.Any<object[]>()).Returns(find as Func<CallInfo, TEntity>);

            dbSet.Remove(Arg.Do<TEntity>(entity => data.Remove(entity)));
            dbSet.RemoveRange(Arg.Do<IEnumerable<TEntity>>(entities =>
                                                           {
                                                               foreach (var entity in entities)
                                                               {
                                                                   data.Remove(entity);
                                                               }
                                                           }));

            dbSet.Add(Arg.Do<TEntity>(entity => data.Add(entity)));
            dbSet.AddRange(Arg.Do<IEnumerable<TEntity>>(entities =>
                                                        {
                                                            foreach (var entity in entities)
                                                            {
                                                                data.Add(entity);
                                                            }
                                                        }));

            return dbSet;
        }
    }
}