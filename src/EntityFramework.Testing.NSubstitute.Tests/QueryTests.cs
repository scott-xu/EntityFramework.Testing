//-----------------------------------------------------------------------------------------------------
// <copyright file="QueryTests.cs" company="Justin Yoo">
//   Copyright (c) 2014 Justin Yoo.
// </copyright>
//-----------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace EntityFramework.Testing.NSubstitute.Tests
{
    [TestClass]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Allowed test class to be left without being documented.")]
    public class QueryTests
    {
        [TestMethod]
        public void Can_enumerate_set()
        {
            var data = new List<Blog> { new Blog { }, new Blog { } };

            var set = Substitute.For<DbSet<Blog>, IQueryable<Blog>, IDbAsyncEnumerable<Blog>>()
                                .SetupData(data);

            var count = 0;
            foreach (var item in set)
            {
                count++;
            }

            Assert.AreEqual(2, count);
        }

#if !NET40

        [TestMethod]
        public async Task Can_enumerate_set_async()
        {
            var data = new List<Blog> { new Blog(), new Blog() };

            var set = Substitute.For<DbSet<Blog>, IQueryable<Blog>, IDbAsyncEnumerable<Blog>>()
                                .SetupData(data);

            var count = 0;
            await set.ForEachAsync(b => count++);

            Assert.AreEqual(2, count);
        }

#endif

        [TestMethod]
        public void Can_use_linq_materializer_directly_on_set()
        {
            var data = new List<Blog> { new Blog(), new Blog() };

            var set = Substitute.For<DbSet<Blog>, IQueryable<Blog>, IDbAsyncEnumerable<Blog>>()
                                .SetupData(data);

            var result = set.ToList();

            Assert.AreEqual(2, result.Count);
        }

#if !NET40

        [TestMethod]
        public async Task Can_use_linq_materializer_directly_on_set_async()
        {
            var data = new List<Blog> { new Blog(), new Blog() };

            var set = Substitute.For<DbSet<Blog>, IQueryable<Blog>, IDbAsyncEnumerable<Blog>>()
                                .SetupData(data);

            var result = await set.ToListAsync();

            Assert.AreEqual(2, result.Count);
        }

#endif

        [TestMethod]
        public void Can_use_linq_opeartors()
        {
            var data = new List<Blog>
            {
                new Blog { BlogId = 1 },
                new Blog { BlogId = 2 },
                new Blog { BlogId = 3}
            };

            var set = Substitute.For<DbSet<Blog>, IQueryable<Blog>, IDbAsyncEnumerable<Blog>>()
                                .SetupData(data);

            var result = set.Where(b => b.BlogId > 1)
                            .OrderByDescending(b => b.BlogId)
                            .ToList();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(3, result[0].BlogId);
            Assert.AreEqual(2, result[1].BlogId);
        }

#if !NET40

        [TestMethod]
        public async Task Can_use_linq_opeartors_async()
        {
            var data = new List<Blog>
            {
                new Blog { BlogId = 1 },
                new Blog { BlogId = 2 },
                new Blog { BlogId = 3}
            };

            var set = Substitute.For<DbSet<Blog>, IQueryable<Blog>, IDbAsyncEnumerable<Blog>>()
                                .SetupData(data);

            var result = await set.Where(b => b.BlogId > 1)
                                  .OrderByDescending(b => b.BlogId)
                                  .ToListAsync();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(3, result[0].BlogId);
            Assert.AreEqual(2, result[1].BlogId);
        }

#endif

        [TestMethod]
        public void Can_use_include_directly_on_set()
        {
            var data = new List<Blog> { new Blog(), new Blog() };

            var set = Substitute.For<DbSet<Blog>, IQueryable<Blog>, IDbAsyncEnumerable<Blog>>()
                                .SetupData(data);

            var result = set.Include(b => b.Posts)
                            .ToList();

            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void Can_use_include_after_linq_operator()
        {
            var data = new List<Blog> { new Blog(), new Blog() };

            var set = Substitute.For<DbSet<Blog>, IQueryable<Blog>, IDbAsyncEnumerable<Blog>>()
                .SetupData(data);

            var result = set.OrderBy(b => b.BlogId)
                            .Include(b => b.Posts)
                            .ToList();

            Assert.AreEqual(2, result.Count);
        }

        public class Blog
        {
            public int BlogId { get; set; }

            public string Url { get; set; }

            public List<Post> Posts { get; set; }
        }

        public class Post
        {
            public int PostId { get; set; }

            public string Title { get; set; }

            public string Content { get; set; }

            public int BlogId { get; set; }

            public Blog Blog { get; set; }
        }
    }
}