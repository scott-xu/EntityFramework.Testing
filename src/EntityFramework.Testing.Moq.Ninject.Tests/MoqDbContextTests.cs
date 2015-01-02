namespace EntityFramework.Testing.Moq.Ninject.Tests
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using global::Moq;
    using global::Ninject;
    using global::Ninject.MockingKernel;
    using global::Ninject.MockingKernel.Moq;
    using EntityFramework.Testing.Ninject.Tests;
    using Xunit;

    public class MoqDbContextTests : DbContextTests
    {

        [Fact]
        public override void Can_setup_dbset()
        {
            using (var kernel = this.CreateMockingKernel() as MoqMockingKernel)
            {
                var blogs = new List<Blog> { new Blog(), new Blog() };
                kernel.GetMock<DbSet<Blog>>().SetupData(blogs);

                var db = kernel.Get<BlogDbContext>();

                Assert.Equal(2, db.Blogs.Count());
            }
        }

        protected override MockingKernel CreateMockingKernel()
        {
            var kernel = new MoqMockingKernel();
            kernel.Load(new EntityFrameworkTestingMoqModule());
            return kernel;
        }
    }
}
