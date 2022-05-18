using App.DAL.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApp.Areas.Authorized.Controllers;
using Xunit;
using Xunit.Abstractions;
using Assert = NUnit.Framework.Assert;

namespace Tests.WebApp.Controllers;

public class UnitTestAgeRatingsController
{
    private readonly AgeRatingsController _ageRatingsController;
    private readonly ITestOutputHelper _testOutputHelper;

    public UnitTestAgeRatingsController(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;

        //set up mock db - inmemory
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());

        var context = new AppDbContext(optionsBuilder.Options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<AgeRatingsController>();

        _ageRatingsController = new AgeRatingsController(context, logger);
    }

    [Fact]
    public async Task IndexAction_ReturnsEmptyVm()
    {
        var result = await _ageRatingsController.Index() as ViewResult;
        _testOutputHelper.WriteLine(result?.ToString());
        Assert.NotNull(result);
        Assert.Null(result!.Model);
    }
}