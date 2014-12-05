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

            var query = new InMemoryAsyncQueryable<TEntity>(data.AsQueryable());

            ((IQueryable<TEntity>)dbSet).Provider.Returns(query.Provider);
            ((IQueryable<TEntity>)dbSet).Expression.Returns(query.Expression);
            ((IQueryable<TEntity>)dbSet).ElementType.Returns(query.ElementType);
            ((IQueryable<TEntity>)dbSet).GetEnumerator().Returns(query.GetEnumerator());

#if !NET40
            ((IDbAsyncEnumerable<TEntity>)dbSet).GetAsyncEnumerator().Returns(new InMemoryDbAsyncEnumerator<TEntity>(query.GetEnumerator()));
            ((IQueryable<TEntity>)dbSet).Provider.Returns(query.Provider);
#endif

            dbSet.Include(Arg.Any<string>()).Returns(dbSet);
            dbSet.Find(Arg.Any<object[]>()).Returns(find as Func<CallInfo, TEntity>);

            dbSet.Remove(Arg.Do<TEntity>(entity =>
                                         {
                                             data.Remove(entity);
                                             dbSet.SetupData(data, find);
                                         }));

            dbSet.RemoveRange(Arg.Do<IEnumerable<TEntity>>(entities =>
                                                           {
                                                               foreach (var entity in entities)
                                                               {
                                                                   data.Remove(entity);
                                                               }

                                                               dbSet.SetupData(data, find);
                                                           }));

            dbSet.Add(Arg.Do<TEntity>(entity =>
                                      {
                                          data.Add(entity);
                                          dbSet.SetupData(data, find);
                                      }));

            dbSet.AddRange(Arg.Do<IEnumerable<TEntity>>(entities =>
                                                        {
                                                            foreach (var entity in entities)
                                                            {
                                                                data.Add(entity);
                                                            }

                                                            dbSet.SetupData(data, find);
                                                        }));

            return dbSet;
        }
    }
}