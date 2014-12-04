using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EntityFramework.Testing.NSubstitute.Tests
{
    [TestClass]
    public class ManipulationTests
    {
        [TestMethod]
        public void Can_remove_set()
        {
            var blog = new Blog();
            var data = new List<Blog> { blog };

            var set = Substitute.For<DbSet<Blog>, IQueryable<Blog>>()
                                .SetupData(data);

            set.Remove(blog);

            var result = set.ToList();

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void Can_removeRange_sets()
        {
            var blog = new Blog();
            var blog2 = new Blog();
            var range = new List<Blog> { blog, blog2 };
            var data = new List<Blog> { blog, blog2, new Blog() };

            var set = Substitute.For<DbSet<Blog>, IQueryable<Blog>>()
                                .SetupData(data);

            ((DbSet<Blog>)set).RemoveRange(range);

            var result = set.ToList();

            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void Can_add_set()
        {
            var blog = new Blog();
            var data = new List<Blog> { };

            var set = Substitute.For<DbSet<Blog>, IQueryable<Blog>>()
                                .SetupData(data);

            set.Add(blog);

            var result = set.ToList();

            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void Can_addRange_sets()
        {
            var data = new List<Blog> { new Blog(), new Blog() };

            var set = Substitute.For<DbSet<Blog>, IQueryable<Blog>>()
                                .SetupData(new List<Blog> { new Blog() });

            ((DbSet<Blog>)set).AddRange(data);

            var result = set.ToList();

            Assert.AreEqual(3, result.Count);
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