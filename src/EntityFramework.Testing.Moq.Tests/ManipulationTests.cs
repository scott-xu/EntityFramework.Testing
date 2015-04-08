using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EntityFramework.Testing.Moq.Tests
{
    public class ManipulationTests
    {
        [Fact]
        public void Can_remove_set()
        {
            var blog = new Blog();
            var data = new List<Blog> { blog };

            var set = new Mock<DbSet<Blog>>()
                .SetupData(data);

            set.Object.Remove(blog);

            var result = set.Object.ToList();

            Assert.Equal(0, result.Count);
        }

        [Fact]
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

            Assert.Equal(1, result.Count);
        }

        [Fact]
        public void Can_add_set()
        {
            var blog = new Blog();
            var data = new List<Blog> { };

            var set = new Mock<DbSet<Blog>>()
                .SetupData(data);

            set.Object.Add(blog);

            var result = set.Object.ToList();

            Assert.Equal(1, result.Count);
        }

        [Fact]
        public void Can_addRange_sets()
        {
            var data = new List<Blog> { new Blog(), new Blog() };

            var set = new Mock<DbSet<Blog>>()
                .SetupData(new List<Blog> { new Blog() });

            set.Object.AddRange(data);

            var result = set.Object.ToList();

            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void Can_toList_twice()
        {
            var set = new Mock<DbSet<Blog>>()
                .SetupData(new List<Blog> { new Blog() });

            var result = set.Object.ToList();

            var result2 = set.Object.ToList();

            Assert.Equal(1, result2.Count);
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
