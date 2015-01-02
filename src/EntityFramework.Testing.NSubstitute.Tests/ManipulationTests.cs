//-----------------------------------------------------------------------------------------------------
// <copyright file="ManipulationTests.cs" company="Justin Yoo">
//   Copyright (c) 2014 Justin Yoo.
// </copyright>
//-----------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using NSubstitute;
using Xunit;

namespace EntityFramework.Testing.NSubstitute.Tests
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Allowed test class to be left without being documented.")]
    public class ManipulationTests
    {
        [Fact]
        public void Can_remove_set()
        {
            var blog = new Blog();
            var data = new List<Blog> { blog, new Blog() };

            var set = this.GetSubstituteDbSet().SetupData(data);

            set.Remove(blog);

            var result = set.ToList();

            Assert.Equal(1, result.Count);
        }

        [Fact]
        public void Can_removeRange_sets()
        {
            var blog = new Blog();
            var blog2 = new Blog();
            var range = new List<Blog> { blog, blog2 };
            var data = new List<Blog> { blog, blog2, new Blog() };

            var set = this.GetSubstituteDbSet().SetupData(data);

            set.RemoveRange(range);

            var result = set.ToList();

            Assert.Equal(1, result.Count);
        }

        [Fact]
        public void Can_add_set()
        {
            var blog = new Blog();
            var data = new List<Blog> { };

            var set = this.GetSubstituteDbSet().SetupData(data);

            set.Add(blog);

            var result = set.ToList();

            Assert.Equal(1, result.Count);
        }

        [Fact]
        public void Can_addRange_sets()
        {
            var data = new List<Blog> { new Blog(), new Blog() };

            var set = this.GetSubstituteDbSet().SetupData(new List<Blog> { new Blog() });

            set.AddRange(data);

            var result = set.ToList();

            Assert.Equal(3, result.Count);
        }

        private DbSet<Blog> GetSubstituteDbSet()
        {
#if NET40
            return Substitute.For<DbSet<Blog>, IQueryable<Blog>>();
#else
            return Substitute.For<DbSet<Blog>, IQueryable<Blog>, IDbAsyncEnumerable<Blog>>();
#endif
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