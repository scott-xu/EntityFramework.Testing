//-----------------------------------------------------------------------------------------------------
// <copyright file="NSubstituteDbSetExtensions.cs" company="Justin Yoo">
//   Copyright (c) 2014 Justin Yoo.
// </copyright>
//-----------------------------------------------------------------------------------------------------

namespace NSubstitute
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using EntityFramework.Testing;
    using NSubstitute.Core;

    /// <summary>
    /// Extension methods for <see cref="NSubstitute"/>.
    /// </summary>
    public static class NSubstituteDbSetExtensions
    {
        /// <summary>
        /// Setup data to <see cref="DbSet{T}"/>.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="dbset">The <see cref="DbSet{T}"/>.</param>
        /// <param name="data">The seed data.</param>
        /// <param name="find">The find action.</param>
        /// <returns>The updated <see cref="DbSet{T}"/>.</returns>
        public static IDbSet<TEntity> SetupData<TEntity>(this IDbSet<TEntity> dbset, ICollection<TEntity> data = null, Func<object[], TEntity> find = null) where TEntity : class
        {
            data = data ?? new List<TEntity>();
            find = find ?? (o => null);

            var query = new InMemoryAsyncQueryable<TEntity>(data.AsQueryable());

            dbset.Provider.Returns(query.Provider);
            dbset.Expression.Returns(query.Expression);
            dbset.ElementType.Returns(query.ElementType);
            dbset.GetEnumerator().Returns(query.GetEnumerator());

#if !NET40
            if (dbset is IDbAsyncEnumerable)
            {
                ((IDbAsyncEnumerable<TEntity>)dbset).GetAsyncEnumerator().Returns(new InMemoryDbAsyncEnumerator<TEntity>(query.GetEnumerator()));
                dbset.Provider.Returns(new InMemoryAsyncQueryProvider(query.Provider));
            }
#endif

            dbset.Include(Arg.Any<string>()).Returns(dbset);
            dbset.Find(Arg.Any<object[]>()).Returns(find as Func<CallInfo, TEntity>);

            dbset.When(ds => ds.Remove(Arg.Any<TEntity>()))
                 .Do(ci =>
                 {
                     var entity = ci.Arg<TEntity>();
                     data.Remove(entity);

                     dbset.SetupData(data, find);
                 });

            ((DbSet<TEntity>)dbset).When(ds => ds.RemoveRange(Arg.Any<IEnumerable<TEntity>>()))
                                    .Do(ci =>
                                    {
                                        var entities = ci.Arg<IEnumerable<TEntity>>();
                                        foreach (var entity in entities)
                                        {
                                            data.Remove(entity);
                                        }

                                        dbset.SetupData(data, find);
                                    });

            dbset.When(ds => ds.Add(Arg.Any<TEntity>()))
                 .Do(ci =>
                 {
                     var entity = ci.Arg<TEntity>();
                     data.Add(entity);

                     dbset.SetupData(data, find);
                 });

            ((DbSet<TEntity>)dbset).When(ds => ds.AddRange(Arg.Any<IEnumerable<TEntity>>()))
                                    .Do(ci =>
                                    {
                                        var entities = ci.Arg<IEnumerable<TEntity>>();
                                        foreach (var entity in entities)
                                        {
                                            data.Add(entity);
                                        }

                                        dbset.SetupData(data, find);
                                    });

            return dbset;
        }
    }
}