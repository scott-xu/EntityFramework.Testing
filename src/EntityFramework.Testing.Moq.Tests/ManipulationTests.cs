using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFramework.Testing.Moq.Tests
{
    [TestClass]
    public class ManipulationTests
    {
        [TestMethod]
        public void Can_remove_set()
        {
            var blog = new Blog();
            var data = new List<Blog> { blog };

            var set = new Mock<DbSet<Blog>>()
                .SetupData(data);

            set.Object.Remove(blog);

            var result = set.Object.ToList();

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void Can_removeRange_sets()
        {
            var blog = new Blog();
            var blog2 = new Blog();
            var range = new List<Blog> { blog, blog2 };
            var data = new List<Blog> { blog, blog2, new Blog() };

            var set = new Mock<DbSet<Blog>>()
                .SetupData(data);

            set.Object.RemoveRange(range);

            var result = set.Object.ToList();

            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void Can_add_set()
        {
            var blog = new Blog();
            var data = new List<Blog> { };

            var set = new Mock<DbSet<Blog>>()
                .SetupData(data);

            set.Object.Add(blog);

            var result = set.Object.ToList();

            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void Can_addRange_sets()
        {
            var data = new List<Blog> { new Blog(), new Blog() };

            var set = new Mock<DbSet<Blog>>()
                .SetupData(new List<Blog> { new Blog() });

            set.Object.AddRange(data);

            var result = set.Object.ToList();

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
