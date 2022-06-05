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

namespace UnitTests;

public class UnitTestMovieDetailsPublicServices
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly IAppPublic _public;
    private MovieDetailsController _movieDetailsController;
    private const string Culture = "en-Gb";

    private AgeRating _ageRating;
    private MovieType _movieType;

    public UnitTestMovieDetailsPublicServices(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
 
        optionsBuilder.UseInMemoryDatabase("InMemoryDb")
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            .EnableSensitiveDataLogging();

        var context = new AppDbContext(optionsBuilder.Options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        var appBll = GetBll(context);
        _public = GetPublic(appBll);

        using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<MovieDetailsController>();

        _movieDetailsController = new MovieDetailsController(_public, appBll, logger);
        _movieDetailsController.ControllerContext = new ControllerContext();
        
        //Additional Data
        _ageRating = new AgeRating
        {
            Naming = "naming",
            AllowedAge = 0,
        };
        
        _movieType = new MovieType
        {
            Naming = "Movie"
        };
    }

    private static IAppBll GetBll(AppDbContext context)
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

    private static IAppPublic GetPublic(IAppBll appBll)
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
        _public.AgeRating.Add(_ageRating);
        _public.MovieType.Add(_movieType);
        await _public.SaveChangesAsync();

        var returnedAgeRatings = await _public.AgeRating.GetAllAsync();
        var returnedMovieTypes = await _public.MovieType.GetAllAsync();

        var ageRatingId = returnedAgeRatings.First().Id;
        var movieTypeId = returnedMovieTypes.First().Id;

        var movieDetails = new MovieDetails
        {
            PosterUri = "posterUri",
            Title = "title",
            Released = new DateTime(),
            Description = "description",
            AgeRatingId = ageRatingId,
            MovieTypeId = movieTypeId
        };

        var result = await _movieDetailsController.PostMovieDetails(movieDetails);
        var response = result.Result;
        var value = result.Value;
    }
}