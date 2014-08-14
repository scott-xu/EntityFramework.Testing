## EntityFramework.Testing
EntityFramework.Testing provides an implementation of `DbAsyncQueryProvider` that can be used when testing a component that uses async queries with EntityFramework. You can read more about how to use these components at http://msdn.com/data/dn314429#async.

The project is cut from EntityFrameworks' [source code](http://entityframework.codeplex.com/SourceControl/latest#test/EntityFramework/FunctionalTests/TestDoubles/). Some changes are made to be compliance with StyleCop/CodeAnalysis

## EntityFramework.Testing.Moq
EntityFramework.Testing.Moq provides a helpful extension method to mock EntityFramework's DbSets using Moq. 

For example, given the following controller.

```
public class BlogsController : Controller
{
    private readonly BloggingContext db;

    public BlogsController(BloggingContext context)
    {
        db = context;
    }

    public async Task<ViewResult> Index()
    {
        var query = db.Blogs.OrderBy(b => b.Name);

        return View(await query.ToListAsync());
    }
}
```

You can write a unit test against an mock context as follows. `SetupData` extension method is part of EntityFramework.Testing.Moq.

```
[TestMethod]
public async Task Index_returns_blogs_ordered_by_name()
{
    // Create some test data
    var data = new List<Blog>
    {
        new Blog{ Name = "BBB" },
        new Blog{ Name = "CCC" },
        new Blog{ Name = "AAA" }
    };

    // Create a mock set and context
    var set = new Mock<DbSet<Blog>>()
        .SetupData(data);

    var context = new Mock<BloggingContext>();
    context.Setup(c => c.Blogs).Returns(set.Object);

    // Create a BlogsController and invoke the Index action
    var controller = new BlogsController(context.Object);
    var result = await controller.Index();

    // Check the results
    var blogs = (List<Blog>)result.Model;
    Assert.AreEqual(3, blogs.Count());
    Assert.AreEqual("AAA", blogs[0].Name);
    Assert.AreEqual("BBB", blogs[1].Name);
    Assert.AreEqual("CCC", blogs[2].Name);
}
```
