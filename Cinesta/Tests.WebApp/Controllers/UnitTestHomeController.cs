using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApp.Controllers;
using Xunit;
using Xunit.Abstractions;
using Assert = Xunit.Assert;

// EXAMPLE
//TODO: REMOVE AFTER IMPLEMENTATION OF OWN TESTING

namespace Tests.WebApp.Controllers;

public class UnitTestHomeController
{
    private readonly HomeController _homeController;

    private readonly ITestOutputHelper _testOutputHelper;
    //private readonly AppDbContext _context;

    public UnitTestHomeController(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;

        //set up mock db - inmemory
        //var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        //optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        //_context = new AppDbContext(optionsBuilder.Options);

        //_context.Database.EnsureDeleted();
        //_context.Database.EnsureCreated();

        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<HomeController>();

        _homeController = new HomeController(logger);
    }

    [Fact]
    public void IndexAction_ReturnsNoVm()
    {
        var result = _homeController.Index() as ViewResult;
        _testOutputHelper.WriteLine(result?.ToString());
        Assert.NotNull(result);
        Assert.Null(result!.Model);
    }
}