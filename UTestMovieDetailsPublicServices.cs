using App.BLL;
using App.Contracts.BLL;
using App.Contracts.Public;
using App.DAL.EF;
using App.Public;
using App.Public.DTO.v1;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApp.ApiControllers;
using Xunit;
using Xunit.Abstractions;
using Assert = Xunit.Assert;

namespace UnitTests.WebApp.UnitTest;

public class UnitTestMovieDetailsPublicServices
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly MovieDetailsController _movieDetailsController;
    private readonly DbContextOptionsBuilder<AppDbContext> _optionsBuilder;
    private const string Culture = "en-Gb";

    public UnitTestMovieDetailsPublicServices(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
 
        _optionsBuilder.UseInMemoryDatabase("InMemoryDb")
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            .EnableSensitiveDataLogging();

        var context = new AppDbContext(_optionsBuilder.Options);
        context.Database.EnsureDeleted();
         context.Database.EnsureCreated();

         var appBll = GetBll(context);
         var appPublic = GetPublic(appBll);
         
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<MovieDetailsController>();

        _movieDetailsController = new MovieDetailsController(@appPublic, appBll, logger);
    }

    private IAppBll GetBll(AppDbContext context)
    {
        var mockMapper = new MapperConfiguration(config =>
        {
            config.AddProfile<App.DAL.EF.AutoMapperConfig>();
            config.AddProfile<App.BLL.AutoMapperConfig>();
        });
        var mapper = mockMapper.CreateMapper();
        var uow = new AppUOW(context, mapper);
        return new AppBll(uow, mapper);
    }

    private IAppPublic GetPublic(IAppBll appBll)
    {
        var mockMapper = new MapperConfiguration(config =>
        {
            config.AddProfile<App.BLL.AutoMapperConfig>();
            config.AddProfile<App.Public.AutoMapperConfig>();
        });

        var mapper = mockMapper.CreateMapper();
        return new AppPublic(appBll, mapper);
    }

// USUAL SERVICES
    
    [Fact]
    public async Task GetMovieDetailsReturnsEmptyIEnumerable()
    {
        var result = await _movieDetailsController.GetMovieDetails(Culture);
        _testOutputHelper.WriteLine(result?.ToString());
        Assert.NotNull(result);
        Assert.Empty(result);
        
    }

    [Fact]
    public async Task PostMovieDetailsReturnsCreatedObject()
    {
        var movieDetails = new MovieDetails
        {
            PosterUri = "posterUri"
        }
    }

    // [Fact]
    // public async Task IndexAction_ReturnsVmWithCorrectAddedData()
    // {
    //     var id = Guid.NewGuid();
    //
    //     var testAgeRating = new AgeRating
    //     {
    //         Id = id,
    //         Naming = "PERE",
    //         AllowedAge = 0
    //     };
    //
    //     _bll.AgeRating.Add(testAgeRating);
    //     await _bll.SaveChangesAsync();
    //     
    //     var result = await _ageRatingsController.Index() as ViewResult;
    //     _testOutputHelper.WriteLine(result?.ToString());
    //     
    //     Assert.NotNull(result);
    //     Assert.NotEmpty((result!.Model as IEnumerable)!);
    //
    //     var model = result.Model! as IEnumerable<AgeRating>;
    //     var ageRatings = model!.ToList();
    //     Assert.Single(ageRatings);
    //     Assert.Equal("PERE", ageRatings.First().Naming);
    // }
    //
    // [Fact]
    // public async Task DetailsAction_ReturnsVmWithCorrectData()
    // {
    //
    //     var id = Guid.NewGuid();
    //
    //     var testAgeRating = new AgeRating
    //     {
    //         Id = id,
    //         Naming = "PERE",
    //         AllowedAge = 0
    //     };
    //
    //     _bll.AgeRating.Add(testAgeRating);
    //     await _bll.SaveChangesAsync();
    //     
    //
    //     var result = await _ageRatingsController.Details(id) as ViewResult;
    //     _testOutputHelper.WriteLine(result?.ToString());
    //     
    //     Assert.NotNull(result);
    //     var model = result!.Model as AgeRating;
    //
    //     Assert.NotNull(model);
    //     Assert.Equal("PERE", model!.Naming);
    //     Assert.Equal(0, model.AllowedAge);
    //     
    // }
    //
    // [Fact]
    // public async Task EditAction_ReturnsVmWithCorrectData()
    // {
    //
    //     var id = Guid.NewGuid();
    //
    //     var testAgeRating = new AgeRating
    //     {
    //         Id = id,
    //         Naming = "PERE",
    //         AllowedAge = 0
    //     };
    //
    //
    //     _bll.AgeRating.Add(testAgeRating);
    //     await _bll.SaveChangesAsync();
    //     
    //     
    //     var result = await _ageRatingsController.Edit(id) as ViewResult;
    //     _testOutputHelper.WriteLine(result?.ToString());
    //     
    //     Assert.NotNull(result);
    //     var model = result!.Model as AgeRating;
    //
    //     Assert.NotNull(model);
    //     Assert.Equal("PERE", model!.Naming);
    //     Assert.Equal(0, model.AllowedAge);
    //     
    // }
    //
    // [Fact]
    // public async Task DeleteAction_RedirectsToIndexAfterDeletion()
    // {
    //     var id = Guid.NewGuid();
    //
    //     var testAgeRating = new AgeRating
    //     {
    //         Id = id,
    //         Naming = "PERE",
    //         AllowedAge = 0
    //     };
    //
    //
    //     _bll.AgeRating.Add(testAgeRating);
    //     await _bll.SaveChangesAsync();
    //     
    //     
    //     await _ageRatingsController.DeleteConfirmed(id);
    //     var result = await _ageRatingsController.Index() as ViewResult;
    //     _testOutputHelper.WriteLine(result?.ToString());
    //     Assert.NotNull(result);
    //     Assert.Empty((result!.Model as IEnumerable)!);
    //
    //     
    //
    // }
}